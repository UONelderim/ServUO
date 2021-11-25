using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class BlessTarget : Target // Create our targeting class (which we derive from the base target class)
	{
		private BlessDeed m_Deed;

		public BlessTarget( BlessDeed deed ) : base( 1, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target ) // Override the protected OnTarget() for our feature
		{
			if ( m_Deed.Deleted )
				return;

			if ( target is Item )
			{
				Item item = (Item)target;

				if ( item.LootType == LootType.Blessed || item.BlessedFor == from || (Mobile.InsuranceEnabled && item.Insured) ) // Check if its already newbied (blessed)
				{
					from.SendLocalizedMessage( 1045113 ); // That item is already blessed
				}
				else if ( item.LootType != LootType.Regular )
				{
					from.SendLocalizedMessage( 1045114 ); // You can not bless that item
				}
				else
				{
					if( item.RootParent != from ) // Make sure its in their pack or they are wearing it
					{
						from.SendLocalizedMessage( 500509 ); // You cannot bless that object
					}
					else
					{
						item.LootType = LootType.Blessed;
						from.SendLocalizedMessage( 1010026 ); // You bless the item....

						m_Deed.Delete(); // Delete the bless deed
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 500509 ); // You cannot bless that object
			}
		}
	}

	public class BlessDeed : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public BlessDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			Name = "zwoj blogoslawienstwa";
			LootType = LootType.Blessed;
		}

		public BlessDeed( Serial serial ) : base( serial )
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage( 1005018 ); // What item would you like to bless? (Clothes Only)
				from.Target = new BlessTarget( this ); // Call our target
			 }
		}	
	}
}