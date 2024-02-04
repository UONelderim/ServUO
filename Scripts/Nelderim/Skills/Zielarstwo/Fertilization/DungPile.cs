
using System;
using Server.Engines.Plants;
using Server.HuePickers;
using Server.Mobiles;

namespace Server.Items
{
	public class DungPile : Item
	{
		private TimeSpan DungLivetime => DefecationTimer.DefaultDefecationInterval - TimeSpan.FromSeconds(2);

		[Constructable]
		public DungPile() : base(0x913)
		{
			Hue = Utility.RandomList(2308, 2309, 2310, 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318);
			Name = "kupa lajna";

			Timer.DelayCall(DungLivetime, Delete);
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			AddNameProperty(list);
		}

		public override bool OnDragLift(Mobile from)
		{
			from.SendMessage("Nie chcesz tego dotykac... Przydalaby sie szufla.");
			return false;
		}

		public override bool Decays
		{
			get { return true; }
		}

		public DungPile(Serial serial)
			: base(serial)
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

			Timer.DelayCall(DungLivetime, Delete);
		}
	}
}



