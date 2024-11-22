#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server.Items
{
	class AddPackDeed : Item
	{
		// braz, bialy, pospolity
		private static readonly IDictionary<int, int> _horseBodyToHue = new Dictionary<int, int>
		{
			{ 0xCC, 1048 }, { 0xE2, 1001 }, { 0xE4, 0 }, { 0xC8, 0 }
		};

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null)
				return;
			if(!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1070021); // Musisz miec juki w swoim plecaku aby ich uzyc
				return;
			}
			from.SendLocalizedMessage(1070022); // Wskaz zwierze do ktorego chcesz dodac pakunki.
			from.BeginTarget(2, false, TargetFlags.None, OnTarget);
		}

		public void OnTarget(Mobile from, object targeted)
		{
			if (from == null || targeted is not BaseCreature target)
				return;
			
			var packAnimalType = targeted switch
			{
				Horse _ => typeof(PackHorse),
				Llama _ => typeof(PackLlama),
				RidableLlama _ => typeof(PackLlama),
				_ => null
			};
			
			if(packAnimalType == null)
			{
				from.SendLocalizedMessage(1070023); //"Mozesz wskazac jedynie konia albo lame."
				return;
			}
			if (target.ControlMaster != from)
			{
				from.SendLocalizedMessage(1070024); // "Zwierze musi byc Twoje!"
				return;
			}

			var gump = new GeneralConfirmGump();
			gump.OnContinue += (_, _) => OnConfirm(from, target, packAnimalType);
			gump.Text = "Czy jestes pewien ze chcesz zalozyc juki na to zwierze?";
			from.SendGump(gump);
		}

		public void OnConfirm(Mobile from, BaseCreature target, Type packAnimalType)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1070021); // Musisz miec juki w swoim plecaku aby ich uzyc
			else if (!target.Alive)
				from.SendLocalizedMessage(1070025); // "Nie mozesz zalozyc pakunkow na zwloki"
			else if (target.ControlMaster != from)
				from.SendLocalizedMessage(1070026); // "Nie mozesz utrzymac tego zwierzecia w miejscu"
			else if (!from.InRange(target, 2))
				from.SendLocalizedMessage(1070027); // "To zwierze jest za daleko"
			else if (target is BaseMount mount && mount.Rider != null)
				from.SendLocalizedMessage(1070028); // "Ktos siedzi na Twoim wierzchowcu."
			else
			{
				var packAnimal = (BaseCreature)Activator.CreateInstance(packAnimalType);

				// Kazde zwierze juczne ma prawo do swojego ciala, wlasnego koloru skury i dostepu do skrzynki bankowej
				string[] fieldsToSkip = { "Body", "BodyValue", "RawBody", "Hue", "RawHue", "Hidden", "Bankbox" };

				target.CantWalk = true;
				packAnimal.Hidden = true;

				if (target is Horse)
					packAnimal.Hue = _horseBodyToHue[target.BodyValue];

				foreach (var prop in typeof(BaseCreature).GetProperties())
				{
					if (fieldsToSkip.Contains(prop.Name))
						continue;
					
					if (prop.CanWrite && prop.GetValue(target, null) != prop.GetValue(packAnimal, null))
					{
						prop.SetValue(packAnimal, prop.GetValue(target, null), null);
					}
				}

				target.Delete();
				packAnimal.Hidden = false;
				packAnimal.CantWalk = false;

				from.SendLocalizedMessage(1070029); // "Zalozyles juki"

				Delete();
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		[Constructable]
		public AddPackDeed()
			: base(0x2AA3)
		{
			Name = "Juki";
			Weight = 10.0;
			Light = LightType.Empty;
		}

		public AddPackDeed(Serial serial)
			: base(serial)
		{
		}
	}
}
