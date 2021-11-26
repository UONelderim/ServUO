
namespace Server.Mobiles
{
[CorpseName( "zwloki konia bojowego" )]
	public class WarHorseC : NBaseWarHorse
	{
		[Constructable]
		public WarHorseC() : this( "kon bojowy" )
		{
			SetHits( 250 );
		}

		[Constructable]
		public WarHorseC( string name ) : base( name, 0x79, 0x3EB0 )
		{
			SetResistance( ResistanceType.Poison, 45, 55 );
		}

		public override PackInstinct PackInstinct{ get{ return PackInstinct.Equine; } }

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