namespace Server.Mobiles
{
	public class ForestBeetle : Beetle
	{
		[Constructable]
		public ForestBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.1)
				Hue = 1158;
			else if (chance <= 0.3)
				Hue = 1395;
			else if (chance <= 5)
				Hue = Utility.RandomList(2448, 2981);
			else if (chance <= 12)
				Hue = Utility.RandomList(1178, 2906);
			else if (chance <= 25)
				Hue = Utility.RandomList(1397, 1315, 2120);
			else
				Hue = 0;
		}

		public ForestBeetle(Serial serial)
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
