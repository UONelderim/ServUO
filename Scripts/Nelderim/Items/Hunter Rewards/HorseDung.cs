using System;
using Server;
using Server.Items;
using Server.Network;  


namespace Server.Items
{
	public class HorseDung : Item
	{
		[Constructable]
		public HorseDung() : this( null )
		{
		}
		
		[Constructable]
		public HorseDung( String name ) : base(Utility.RandomMinMax( 0xF3C, 0xF3B ))
		{
			Name = name;
			Weight = 1.0;
		}
		
		public override bool HandlesOnMovement{ get{ return false; } }

                public override void OnMovement( Mobile m, Point3D oldLocation )
                {
                     base.OnMovement( m, oldLocation );
        	
        	    if ( m.InRange( this, 0 ) )
        	    {
        		    m.SendMessage( "You stepped in horse dung!" );
        	    }
            }
		
		public HorseDung( Serial serial ) : base( serial )
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
