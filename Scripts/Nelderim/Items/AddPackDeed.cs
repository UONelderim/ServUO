#region References

using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

#endregion

// szczaw :: 2013.01.14 :: przeniesienie tekstow do cliloca
namespace Server.Items
{
	class AddPackDeed : Item
	{
		const int cliloc = 1070020;

		// braz, bialy, pospolity
		private static readonly IDictionary<int, int> _horseBodyToHue = new Dictionary<int, int>
		{
			{ 0xCC, 1048 }, { 0xE2, 1001 }, { 0xE4, 0 }, { 0xC8, 0 }
		};

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || !this.IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(cliloc + 1); // Musisz miec juki w swoim plecaku aby ich uzyc
				return;
			}

			from.SendLocalizedMessage(cliloc + 2); // Wskaz zwierze do ktorego chcesz dodac pakunki.
			from.BeginTarget(2, false, TargetFlags.None, OnTarget);
		}

		public void OnTarget(Mobile from, object targeted)
		{
			BaseCreature target = targeted as BaseCreature;
			Type packAnimalType;

			if (from == null || target == null)
				return;
			if (targeted is Horse)
				packAnimalType = typeof(PackHorse);
			else if (targeted is Llama || target is RidableLlama)
				packAnimalType = typeof(PackLlama);
			else
			{
				from.SendLocalizedMessage(cliloc + 3); //"Mozesz wskazac jedynie konia albo lame."
				return;
			}

			if (target.ControlMaster != from)
			{
				from.SendLocalizedMessage(cliloc + 4); // "Zwierze musi byc Twoje!"
				return;
			}

			var gump = new GeneralConfirmGump();
			gump.OnContinue += (s, i) => this.OnConfirm(from, target, packAnimalType);
			gump.Text = "Czy jestes pewien ze chcesz zalozyc juki na to zwierze?";

			from.SendGump(gump);
		}

		public void OnConfirm(Mobile from, BaseCreature target, Type packAnimalType)
		{
			// Czy nic sie nie zmienilo?
			if (!this.IsChildOf(from.Backpack))
				from.SendLocalizedMessage(cliloc + 1); // Musisz miec juki w swoim plecaku aby ich uzyc
			else if (!target.Alive)
				from.SendLocalizedMessage(cliloc + 5); // "Nie mozesz zalozyc pakunkow na zwloki"
			else if (target.ControlMaster != from)
				from.SendLocalizedMessage(cliloc + 6); // "Nie mozesz utrzymac tego zwierzecia w miejscu"
			else if (!from.InRange(target, 2))
				from.SendLocalizedMessage(cliloc + 7); // "To zwierze jest za daleko"
			else if (!(target is Llama) && ((BaseMount)target).Rider != null)
				from.SendLocalizedMessage(cliloc + 8); // "Ktos siedzi na Twoim wierzchowcu."
			else
			{
				BaseCreature packAnimal = (BaseCreature)Activator.CreateInstance(packAnimalType);

				// Kazde zwierze juczne ma prawo do swojego ciala, wlasnego koloru skury i dostepu do skrzynki bankowej
				string[] fieldsToSkip = { "Body", "BodyValue", "Hue", "Hidden", "Bankbox" };

				target.CantWalk = true;
				packAnimal.Hidden = true;

				if (target is Horse)
					packAnimal.Hue = _horseBodyToHue[target.BodyValue];

				foreach (var prop in typeof(BaseCreature).GetProperties())
				{
					// musi byc mozliwosc pisania do properties...
					// ... i nie chce przepisywac tych samych wartosci, bo settery ustawiaja jakies default value czy cos...
					if (!Contains(fieldsToSkip, prop.Name))
					{
						if (prop.CanWrite && prop.GetValue(target, null) != prop.GetValue(packAnimal, null))
						{
							prop.SetValue(packAnimal, prop.GetValue(target, null), null);
						}
					}
				}

				target.Delete();
				packAnimal.Hidden = false;
				packAnimal.CantWalk = false;

				from.SendLocalizedMessage(cliloc + 9); // "Zalozyles juki"

				this.Delete();
			}
		}

		private bool Contains(string[] array, string str)
		{
			foreach (string s in array)
			{
				if (s.Equals(str))
				{
					return true;
				}
			}

			return false;
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
