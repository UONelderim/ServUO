/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/30/2005
 * Time: 7:27 PM
 *  You can change the color of the throne deed on line 55 
 * Santas Chair Addon
 */

namespace Server.Items
{
	public class SantasChairAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SantasChairAddonDeed(); } }

		[Constructable]
		public SantasChairAddon()
		{
			AddComponent(new AddonComponent(0x1526), 0, 1, 0);
			AddComponent(new AddonComponent(0x1527), 0, 0, 0);
			Name = "Krzeslo Pana";
		}

		public SantasChairAddon(Serial serial) : base(serial)
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

	public class SantasChairAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SantasChairAddon(); } }


		[Constructable]
		public SantasChairAddonDeed()
		{
			Name = "Kontrakt na krzeslo pana ";
			LootType = LootType.Blessed;
			Hue = 32;
		}

		public SantasChairAddonDeed(Serial serial) : base(serial)
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
