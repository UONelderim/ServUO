using System;
using Server;
using Server.Items;

namespace Solaris.BoardGames
{
	//a bomberman obstacle defines the object types
	public class SpeedUpgrade : BombermanUpgrade
	{
		public SpeedUpgrade() : base( 0x2308, "WIEKSZA PREDKOSC" )
		{
			Hue = 1152;
		}
		
		//deserialize constructor
		public SpeedUpgrade( Serial serial ) : base( serial )
		{
		}
		
		protected override void Upgrade( Mobile m )
		{
			base.Upgrade( m );
			
			BombBag bag = (BombBag)m.Backpack.FindItemByType( typeof( BombBag ) );
			
			if( bag != null )
			{
				bag.SpeedBoost();
			}
		}
		
		
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}
