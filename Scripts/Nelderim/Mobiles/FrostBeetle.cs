namespace Server.Mobiles
{
	public class FrostBeetle : Beetle
	{
		[Constructable]
		public FrostBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.5) 
				Hue = 2470;
			else if (chance <= 2) 
				Hue = Utility.RandomList(1073, 1790, 1077);
			else if (chance <= 10)
				Hue = Utility.RandomList(2498, 2461, 2460);
			else if (chance <= 25)
				Hue = Utility.RandomList(1150, 1153);
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
