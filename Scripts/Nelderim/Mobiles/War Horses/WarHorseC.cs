
namespace Server.Mobiles
{
[CorpseName( "zwloki konia bojowego" )]
	public class WarHorseC : NBaseWarHorse
	{
		[Constructable]
		public WarHorseC() : this( "kon bojowy" )
		{
		}

		[Constructable]
		public WarHorseC( string name ) : base( name, 0x79, 0x3EB0 )
		{
			SetResistance( ResistanceType.Poison, 35, 45 );
		}

		public WarHorseC( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( !m_isVersion0 )
			{
				int version = reader.ReadInt();
			}
		}
	}
}