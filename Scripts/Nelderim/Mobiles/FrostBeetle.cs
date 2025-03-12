namespace Server.Mobiles
{
	public class FrostBeetle : Beetle
	{
		[Constructable]
		public FrostBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.1)
				Hue = 2470;
			else if (chance <= 0.3)
				Hue = 1073;
			else if (chance <= 5)
				Hue = Utility.RandomList(1790, 1077);
			else if (chance <= 12)
				Hue = Utility.RandomList(2498, 2461);
			else if (chance <= 25)
				Hue = Utility.RandomList(1150, 1153, 2460);
			else
				Hue = 0;
		}

		public FrostBeetle(Serial serial)
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
