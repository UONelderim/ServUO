namespace Server.Mobiles
{
	public class SwampBeetle : Beetle
	{
		[Constructable]
		public SwampBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.1) 
				Hue = 2963;
			else if (chance <= 0.3)
				Hue = 1700;
			else if (chance <= 5)
				Hue = Utility.RandomList(1400, 1179);
			else if (chance <= 12)
				Hue = Utility.RandomList(1388, 2129);
			else if (chance <= 25)
				Hue = Utility.RandomList(2207, 2127, 2212);
			else
				Hue = 0;
		}

		public SwampBeetle(Serial serial)
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
