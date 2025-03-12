namespace Server.Mobiles
{
	public class UndershadowBeetle : Beetle
	{
		[Constructable]
		public UndershadowBeetle()
			: base("ogromny zuk")
		{
			double chance = Utility.RandomDouble() * 100;

			if (chance <= 0.1) 
				Hue = 2899;
			else if (chance <= 0.3)
				Hue = Hue = Utility.RandomList(1573, 2882);
			else if (chance <= 5)
				Hue = Utility.RandomList(1899, 1109);
			else if (chance <= 12)
				Hue = Utility.RandomList(1896, 1894);
			else if (chance <= 25)
				Hue = 1893;
			else
				Hue = 1891;
		}

		public UndershadowBeetle(Serial serial)
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
