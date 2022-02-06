
namespace Server.Mobiles
{
[CorpseName( "zwloki konia bojowego" )]
	public class WarHorseD : NBaseWarHorse
	{
		[Constructable]
		public WarHorseD() : this( "kon bojowy" )
		{
			SetHits( 250 );
		}

		[Constructable]
		public WarHorseD( string name ) : base( name, 0x76, 0x3EB2 )
		{
			SetResistance( ResistanceType.Energy, 55, 65 );
		}

		public override PackInstinct PackInstinct{ get{ return PackInstinct.Equine; } }

		public WarHorseD( Serial serial ) : base( serial )
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