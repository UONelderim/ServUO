using System;

namespace Server.Items
{
	public abstract class BasePotrawka : Item
	{
		public virtual int Bonus { get { return 0; } }
		public virtual StatType Type { get { return StatType.Int; } }
		public virtual TimeSpan Duration { get { return TimeSpan.FromMinutes(10.0); } }
		public virtual TimeSpan Cooldown { get { return TimeSpan.FromMinutes(60.0); } }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BasePotrawka(int hue) : base(0x284F)
		{
			Hue = hue;
		}

		public BasePotrawka(Serial serial) : base(serial)
		{
		}

		public virtual bool Apply(Mobile from)
		{
			bool applied = Spells.SpellHelper.AddStatOffset(from, Type, Bonus, TimeSpan.FromMinutes(10.0));


			if ( !applied )
				from.SendLocalizedMessage(502173); // You are already under a similar effect.

			return applied;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if ( !IsChildOf(from.Backpack) )
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if ( Apply(from) )
			{
				from.FixedEffect(0x375A, 10, 15);
				from.PlaySound(0x1E7);
				Delete();
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
		public override int Bonus { get { return 5; } }
		public override StatType Type { get { return StatType.Int; } }


		[Constructable]
		public Potrawka() : base(0x284F)
		{
			Stackable = false;
			Name = "pożywne klopsiki";
			Hue = 51;
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
		public override int Bonus { get { return 5; } }
		public override StatType Type { get { return StatType.Dex; } }


		[Constructable]
		public PysznaPotrawka() : base(0x284F)
		{
			Stackable = false;
			Name = "tłuste klopsiki";
			Hue = 39;
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
		public override int Bonus { get { return 5; } }
		public override StatType Type { get { return StatType.Str; } }


		[Constructable]
		public PotrawkaBle() : base(0x284F)
		{
			Stackable = false;
			Name = "klopsiki z dynią";
			Hue = 11;
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