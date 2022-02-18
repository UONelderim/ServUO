namespace Server.Mobiles
{
	public class MinotaurSummon : Minotaur
	{
		public MinotaurSummon()
		{
		}

		public override bool DeleteCorpseOnDeath => true;

		public MinotaurSummon(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
		}
	}
}
