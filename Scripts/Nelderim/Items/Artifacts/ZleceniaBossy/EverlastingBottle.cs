using System;

namespace Server.Items
{
	public class EverlastingBottle : Item
	{
		public override double DefaultWeight => 1.0;

		[Constructable]
		public EverlastingBottle() : base(0xF0E)
		{
			Hue = 2121;
			Name = "Wieczna Butelka";
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (from.Thirst > 20)
			{
				from.SendMessage("Nie jestes spragniony");
				return;
			}
			if (from.BeginAction(typeof(EverlastingBottle)))
			{
				from.Thirst = 20;
				from.SendMessage("Wypijasz zawartosc butelki, a ta magicznie zaczyna sie napelniac.");
				from.PlaySound(Utility.RandomList(0x30, 0x2D6));
				Timer.DelayCall(TimeSpan.FromMinutes(1), () => from.EndAction(typeof(EverlastingBottle)));
			}
			else
				from.SendMessage("Musisz odczekac chwile przed ponownym uzyciem.");
		}

		public EverlastingBottle(Serial serial) : base(serial)
		{
		}


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}


		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
