namespace Server.Mobiles
{
	public class DesertBeetle : Beetle
	{
		[Constructable]
		public DesertBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.1) 
				Hue = 2977;
			else if (chance <= 0.3)
				Hue = 2978;
			else if (chance <= 5)
				Hue = Utility.RandomList(2898, 2897);
			else if (chance <= 12)
				Hue = Utility.RandomList(1359, 2089);
			else if (chance <= 25)
				Hue = Utility.RandomList(1491, 1182, 1556);
			else
				Hue = 0;
		}

		public DesertBeetle(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
