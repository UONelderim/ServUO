#region References

using System;

#endregion

namespace Server.Items
{
	public class BookBag : Bag
	{
		[Constructable]
		public BookBag() : this(1)
		{
			Movable = true;
			Hue = 0x489;
			Name = "worek z ksiegami";
		}

		[Constructable]
		public BookBag(int amount)
		{
			DropItem(new Spellbook(UInt64.MaxValue));
			DropItem(new NecromancerSpellbook(0xFFFF));
			DropItem(new BookOfChivalry(0x3FF));
		}


		public BookBag(Serial serial) : base(serial)
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
