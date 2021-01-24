using System;

namespace Server.Items
{
	public class NSerceDzahhara : Item
	{
		//private static TimeSpan m_DDT = TimeSpan.FromMinutes( 1440.0 ); // Czas Rozpadu w minutach

		[Constructable]
		public NSerceDzahhara() : base( 0x1CED )
		{
			//Timer.DelayCall( m_DDT, new TimerCallback( Delete) ); 
			Name= "Spowite Cierpieniem Serce Wielkiego Ksiecia Demonow Dzahhara";
			Weight = 1.0;
			Hue = 0x8A5;
			LootType = LootType.Cursed;
		}

		public NSerceDzahhara( Serial serial ) : base( serial )
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

			int version = reader.ReadInt();
		}
	}
}
