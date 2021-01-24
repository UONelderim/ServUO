using Server.Gumps;

namespace Server.Items
{
	public class ElfArtifact : RaceChoiceItem
	{
	    public ElfArtifact( Serial serial ) : base( serial )
        {
        }

        [Constructable]
        public ElfArtifact() : base( 0x2d20 )
        {
            Name = "Artefakt Elfow";
			Hue = 2486;
            m_Race = Race.NElf;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
            writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
    }
}
