
namespace Server.Mobiles
{
[CorpseName( "zwloki konia bojowego" )]
	public class WarHorseA : NBaseWarHorse
	{
		[Constructable]
		public WarHorseA() : this( "kon bojowy" )
		{
			SetHits( 250 );
		}

		[Constructable]
		public WarHorseA( string name ) : base( name, 0x78, 0x3EAF )
		{
			SetResistance( ResistanceType.Fire, 55, 65 );
		}
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Equine; } }

		public WarHorseA( Serial serial ) : base( serial )
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