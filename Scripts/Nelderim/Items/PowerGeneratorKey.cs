using System;
using Server;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PowerGeneratorKey : Item
	{
		private double SuccessChance => 0.85;
		private bool DestroyKeyOnFailure => false;
		private int MaxRange => 3;

		private string FailureMsg
		{
			get
			{
				if (DestroyKeyOnFailure)
					return "Doszlo do spiecia, klucz sie przepalil!";
				else
					return "Doszlo do spiecia, sproboj ponownie.";
			}
		}

		[Constructable]
		public PowerGeneratorKey() : base(0x32F8)
		{
			Weight = 1.0;
			Name = "Klucz deszyfrujacy";
			Label1 = "*klucz pasuje do panelu kontrolnego generatora mocy*";
		}

		public void UseOn(Mobile from, ControlPanel panel)
		{
			if (Utility.RandomDouble() < SuccessChance)
			{
				from.SendMessage("Klucz zadzialal!");

				panel.Solve(from);

				Consume();
			}
			else
			{
				DoDamage(from);

				if (DestroyKeyOnFailure)
				{
					if (DestroyKeyOnFailure)
					{
						Consume();
					}
				}
			}
		}

		public void DoDamage(Mobile to)
		{
			to.Send(new UnicodeMessage(Serial, ItemID, MessageType.Regular, 0x3B2, 3, "", "", FailureMsg));
			to.BoltEffect(0);
			to.LocalOverheadMessage(MessageType.Regular, 0xC9, true, "* Twoje cialo drga w wyniku porazenia pradem *");
			to.NonlocalOverheadMessage(MessageType.Regular, 0xC9, true, string.Format("* {0} ma skurcze spowodowane porazeniem pradem *", to.Name));

			AOS.Damage(to, to, 60, 0, 0, 0, 0, 100);
		}

			public PowerGeneratorKey(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendMessage("Ten klucz jest poza zasiegiem.");
				return;
			}

			from.Target = new GeneratorTarget(this);

			from.SendMessage("Wskaz panel kontrolny generatora mocy");
		}

		private class GeneratorTarget : Target
		{
			PowerGeneratorKey m_Key;
			public GeneratorTarget(PowerGeneratorKey key) : base(key.MaxRange, false, TargetFlags.None)
			{
				m_Key = key;

				CheckLOS = false;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is ControlPanel)
				{
					m_Key.UseOn(from, (ControlPanel)targeted);
				}
				else
				{
					from.SendMessage("To nie jest panel kontrolny generatora mocy.");
				}
			}
		}
	}
}
