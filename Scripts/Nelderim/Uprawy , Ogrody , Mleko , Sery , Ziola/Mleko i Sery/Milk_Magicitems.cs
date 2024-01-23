#region References

using System;
using Server.Spells;

#endregion

namespace Server.Items
{
	public abstract class BaseMagicCheese : Item
	{
		public virtual int Bonus => 0;
		public virtual StatType Type => StatType.Str;

		public BaseMagicCheese(int hue) : base(0x97E)
		{
			Weight = 1.0;

			if (Utility.RandomBool())
				Hue = Utility.RandomList(0x135, 0xcd, 0x38, 0x3b, 0x42, 0x4f, 0x11e, 0x60, 0x317, 0x10, 0x136, 0x1f9,
					0x1a, 0xeb, 0x86, 0x2e, 0x0497, 0x0481);
			else
				Hue = 3 + (Utility.Random(20) * 5);
		}

		public BaseMagicCheese(Serial serial) : base(serial)
		{
		}

		public virtual bool Apply(Mobile from)
		{
			var modName = $"[Ser] {Type}";
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

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class FromageDeChevreMagic : BaseMagicCheese
	{
		public override int Bonus => 5;
		public override StatType Type => StatType.Str;

		[Constructable]
		public FromageDeChevreMagic() : base(151)
		{
			Name = "crottin de Chavignol (magiczny owczy ser)";
		}

		public FromageDeChevreMagic(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class FromageDeVacheMagic : BaseMagicCheese
	{
		public override int Bonus => 5;
		public override StatType Type => StatType.Int;

		[Constructable]
		public FromageDeVacheMagic() : base(151)
		{
			Name = "Maroille (magiczny krowi ser)";
		}

		public FromageDeVacheMagic(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class FromageDeBrebisMagic : BaseMagicCheese
	{
		public override int Bonus => 5;
		public override StatType Type => StatType.Dex;


		[Constructable]
		public FromageDeBrebisMagic() : base(151)
		{
			Name = "Roquefort (magiczny owczy ser)";
		}

		public FromageDeBrebisMagic(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
