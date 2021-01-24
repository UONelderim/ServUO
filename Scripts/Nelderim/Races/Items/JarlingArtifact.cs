using Server.Gumps;

namespace Server.Items
{
	public class JarlingArtifact : RaceChoiceItem
	{
	    public JarlingArtifact( Serial serial ) : base( serial )
        {
        }

        [Constructable]
        public JarlingArtifact() : base( 0x13FB )
        {
            Name = "Artefakt Jarlingow";
			Hue = 56;
            m_Race = Race.NJarling;
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
