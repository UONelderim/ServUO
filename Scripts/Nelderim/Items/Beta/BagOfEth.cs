#region References

using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class BagOfEth : Bag
	{
		[Constructable]
		public BagOfEth() : this(12)
		{
			Movable = true;
			Hue = 1;
			Name = "worek z Etherealami";
		}

		[Constructable]
		public BagOfEth(int amount)
		{
			DropItem(new EtherealLlama());
			DropItem(new EtherealHorse());
			DropItem(new EtherealOstard());
			DropItem(new EtherealKirin());
			DropItem(new EtherealUnicorn());
			DropItem(new EtherealRidgeback());
			DropItem(new EtherealSwampDragon());
		}

		public BagOfEth(Serial serial) : base(serial)
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
