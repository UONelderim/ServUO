/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/30/2005
 * Time: 5:32 PM
 * 
 * Santas Coal
 */
using System;
using Server;

namespace Server.Items
{
	public class SantasCoal : Item
	{
		[Constructable]
		public SantasCoal() : this( 1 )
		{
		}

		[Constructable]
		public SantasCoal( int amount ) : base( 0x1366 )
		{
			Name = "Kawalek magicznego wegielksa";
			LootType = LootType.Blessed;
			Hue = 962;
			Stackable = false;
			Weight = 5.0;
			Amount = amount;
		}
		 public override void GetProperties( ObjectPropertyList list )
	         {
	  	    base.GetProperties( list );

		    list.Add( 1041052 ); 
    	     }
		
        public override void OnDoubleClick( Mobile from )
        {

        	if ( !IsChildOf (from.Backpack))
        		{
				from.SendMessage( "Aby złożyć życzenie i zmiażdżyć węgiel rękoma, musi być w twoim plecaku." );
        	    }
        	else
        	{
        	
        	Effects.PlaySound( from, from.Map, 0x2E3 );
        	if( Utility.Random( 100 ) < 100 ) 
			       
         			switch ( Utility.Random( 3 ) )
			{ 

				case 0: from.AddToBackpack( new Diamond(Utility.RandomList(   6 ,  7 , 8 , 8 , 9 , 10 ) ) );
				{
				from.SendMessage( "Używasz całej swojej brutalnej siły - i zamieniasz Magiczny Węgiel w Diamenty." );
				}
					break;
				case 1: from.AddToBackpack( new SantasElvenRobe() ); 
				{
				from.SendMessage( "Obiecujesz być dobry w przyszłym roku - a Magiczny Węgiel słyszy Twoje słowa i daje Ci prezent." );
				}
				    break;
				case 2: from.AddToBackpack( new SantasFancyElvenRobe() ); 
				{
				from.SendMessage( "Obiecujesz, że będziesz EKSTRA dobry w przyszłym roku - a Magiczny Węgiel usłyszy Twoje słowa i da Ci prezent." );
				}
					break;	
			}			
				this.Delete();
        	}
		}

		public SantasCoal( Serial serial ) : base( serial )
		{
		}

		//public override Item Dupe( int amount )
		//{
			//return base.Dupe( new SantasCoal( amount ), amount );
		//}

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
