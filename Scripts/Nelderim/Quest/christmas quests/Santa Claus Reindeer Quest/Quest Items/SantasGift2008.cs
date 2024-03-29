/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/30/2005
 * Time: 3:51 PM
 * Santas Gift
 * 
 */

namespace Server.Items
{
	public class SantasGift2008 : Item
	{
		[Constructable]
		public SantasGift2008() : base(0x20D4)
		{
			Stackable = false;
			Weight = 3.0;
			Name = "Prezent od Pana";
			Hue = 1151;
			LootType = LootType.Blessed;
		}

		public SantasGift2008(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060662, "Od \t Renifera Pana");
		}

		public override void OnDoubleClick(Mobile from)
		{
			Effects.PlaySound(from, from.Map, 0x83);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
