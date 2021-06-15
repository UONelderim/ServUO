using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Wheat : Item
	{
		[Constructable]
		public Wheat() : this( 1 )
		{
		}
		
		[Constructable]
		public Wheat( int amount ) : base( 0x1EBD )
		{
			Name = "pszenica";
			//Amount = amount;
			Weight = 3.0;
			Stackable = true;
		}

		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new Wheat(), amount );
		//}

		public Wheat( Serial serial ) : base( serial )
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
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			from.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private Wheat m_Item;

			public InternalTarget( Wheat item ) : base( 1, false, TargetFlags.None )
			{
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Item.Deleted ) return;

				else if ( IsFlourMill(targeted) )
				{
					if(m_Item.Amount >= 1)
					{
						m_Item.Consume();
						from.SendMessage("Zrobiles make");
						from.AddToBackpack( new SackFlour() );
					}
					else
						from.SendMessage("Nie masz wiecej zborza");
				}
			}
		}
		
		public static bool IsFlourMill( object targeted )
		{
			int itemID;

			if ( targeted is Item )
				itemID = ((Item)targeted).ItemID & 0x3FFF;
			else if ( targeted is StaticTarget )
				itemID = ((StaticTarget)targeted).ItemID & 0x3FFF;
			else
				return false;

			if ( itemID >= 0x1883 && itemID <= 0x1893 )
				return true; // millstones
			else if ( itemID >= 0x1920 && itemID <= 0x1937 )
				return true; // flourmill

			return false;
		}
	}
}