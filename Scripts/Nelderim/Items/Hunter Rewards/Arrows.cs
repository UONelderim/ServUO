using System;
using Server;
using Server.Items;


namespace Server.Items
{
	public class Arrows : Item
	{
		[Constructable]
		public Arrows() : this( null )
		{
		}
		
		[Constructable]
		public Arrows( String name ) : base( 0xF41 )
		{
			Name = name;
			Weight = 5.0;
		}
		
		public Arrows( Serial serial ) : base( serial )
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
