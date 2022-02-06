
namespace Server.Mobiles
{
[CorpseName( "zwloki konia bojowego" )]
	public class WarHorseB : NBaseWarHorse
	{
		[Constructable]
		public WarHorseB() : this( "kon bojowy" )
		{
			SetHits( 250 );
		}

		[Constructable]
		public WarHorseB( string name ) : base( name, 0x77, 0x3EB1 )
		{
			SetResistance( ResistanceType.Cold, 55, 65 );
		}

		public override PackInstinct PackInstinct{ get{ return PackInstinct.Equine; } }

		public WarHorseB( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
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