/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/30/2005
 * Time: 7:09 AM
 * HolidayBell20052005
 * 
 */

namespace Server.Items
{
	public class SantaHolidayBell : Item
	{
		public int offset;

		[Constructable]
		public SantaHolidayBell() : base(0x1c12)
		{
			Stackable = false;
			Weight = 1.0;
			Name = "Swiateczny dzwon 2021";
			Hue = Utility.RandomBirdHue();
			LootType = LootType.Blessed;
			offset = Utility.Random(0, 10);
		}

		public SantaHolidayBell(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1007149 + offset);
		}

		public override void OnDoubleClick(Mobile from)
		{
			Effects.PlaySound(from, from.Map, 0x505);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(offset);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			offset = reader.ReadInt();
		}
	}
}
