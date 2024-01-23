using System;

namespace Server.Items
{
	public abstract class BasePotrawka : Item
	{
		public override double DefaultWeight => 1.0;
		public virtual int Bonus => 10;
		public virtual StatType Type => StatType.Str;

		public BasePotrawka(int hue) : base(0x284F)
		{
			Hue = hue;
		}

		public BasePotrawka(Serial serial) : base(serial)
		{
		}

		public virtual bool Apply(Mobile from)
		{
			var modName = $"[Potrawka] {Type}";
			if (from.GetStatMod(modName) != null)
			{
				from.SendLocalizedMessage(
					1062927); // You have eaten one of these recently and eating another would provide no benefit.
				return false;
			}

			from.AddStatMod(new StatMod(Type, modName, Bonus, TimeSpan.FromMinutes(5.0)));
			return true;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (Apply(from))
			{
				from.PlaySound(0x1EE);
				Consume();
			}
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

	public class Potrawka : BasePotrawka
	{
		public override StatType Type => StatType.Str;

		[Constructable]
		public Potrawka() : base(0x284F)
		{
			Stackable = true;
			Name = "pożywne klopsiki";
			Hue = 51;
			Label1 = "sprawia, ze stajesz sie silniejszy";
		}

		public Potrawka(Serial serial) : base(serial)
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

	public class PysznaPotrawka : BasePotrawka
	{
		public override StatType Type => StatType.Dex;

		[Constructable]
		public PysznaPotrawka() : base(0x284F)
		{
			Stackable = true;
			Name = "tłuste klopsiki";
			Hue = 39;
			Label1 = "zwieksza Twoja zrecznosc";
		}

		public PysznaPotrawka(Serial serial) : base(serial)
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

	public class PotrawkaBle : BasePotrawka
	{
		public override StatType Type => StatType.Int;

		[Constructable]
		public PotrawkaBle() : base(0x284F)
		{
			Stackable = true;
			Name = "klopsiki z dynią";
			Hue = 11;
			Label1 = "wzmaga prace umyslu";
		}

		public PotrawkaBle(Serial serial) : base(serial)
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
