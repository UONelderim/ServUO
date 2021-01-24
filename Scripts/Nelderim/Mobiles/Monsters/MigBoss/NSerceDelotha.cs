using System;

namespace Server.Items
{
	public class NSerceDelotha : Item
	{
		//private static TimeSpan m_DDT = TimeSpan.FromMinutes( 1440.0 ); // czas rozpadu w minutach

		[Constructable]
		public NSerceDelotha() : base( 0x1CED )
		{
			//Timer.DelayCall( m_DDT, new TimerCallback( Delete) ); 
			Name= "Spowite Mrokiem Serce Wielkiego Ksiecia Demonow Delotha";
			Weight = 1.0;
			Hue = 0x455;
			LootType = LootType.Cursed;
		}

		public NSerceDelotha( Serial serial ) : base( serial )
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
