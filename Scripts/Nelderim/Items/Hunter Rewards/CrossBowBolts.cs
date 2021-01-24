using System;
using Server;
using Server.Items;


namespace Server.Items
{
	public class CrossBowBolts : Item
	{
		[Constructable]
		public CrossBowBolts() : this( null )
		{
		}
		
		[Constructable]
		public CrossBowBolts( String name ) : base( 0x1BFD )
		{
			Name = name;
			Weight = 5.0;
		}
		
		public CrossBowBolts( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0); //Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
		}
	}
}
