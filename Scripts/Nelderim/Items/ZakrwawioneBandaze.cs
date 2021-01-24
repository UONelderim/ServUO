using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using System.Collections.Generic;

namespace Server.Items
{
	public class ZakrwawioneBandaze : Item
	{ 
		[Constructable] 
		public ZakrwawioneBandaze() : this(1) 
		{ 
		} 

		[Constructable]
		public ZakrwawioneBandaze( int amount ) : base( 0xE20 )
		{ 
			Name = "Zakrwawione Bandaze";
			Stackable = true;
			Weight = 0.02;
			Amount = amount;
			Hue = 41;
		} 

		public ZakrwawioneBandaze (Serial serial) : base(serial)
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
			from.RevealingAction();
			if ( !from.Mounted )
			{
				if(this.Amount > 1)
				{
					from.SendLocalizedMessage( 1067047 );
				}
				else
				{
					from.SendLocalizedMessage( 1067048 );
				}
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 1067049 );
			}
		}

		private class InternalTarget : Target
		{
			private ZakrwawioneBandaze m_Bandaze;

			public InternalTarget( ZakrwawioneBandaze bandaze ) : base( 3, true, TargetFlags.Beneficial )
			{
				m_Bandaze = bandaze;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				StaticTarget a = targeted as StaticTarget;
				Item b = targeted as Item;
				LandTarget c = targeted as LandTarget;

				if ( m_Bandaze.Deleted )
					return;

				if ( targeted == a)
				{
					if ( (a.ItemID >= 5937 && a.ItemID <= 5978) || (a.ItemID >= 6038 && a.ItemID <= 6066) || (a.ItemID >= 6595 && a.ItemID <= 6636) || (a.ItemID >= 8093 && a.ItemID <= 8094) || (a.ItemID >= 8099 && a.ItemID <= 8138) || (a.ItemID >= 9299 && a.ItemID <= 9309) || (a.ItemID >= 13422 && a.ItemID <= 13525) || (a.ItemID >= 13549 && a.ItemID <= 13616) || (a.ItemID == 3707) || (a.ItemID >= 4088 && a.ItemID <= 4089) || (a.ItemID == 4104) || (a.ItemID == 5453) || (a.ItemID >= 5458 && a.ItemID <= 5460) || (a.ItemID == 5465) || (a.ItemID >= 2881 && a.ItemID <= 2884) )
					{
						int amount = m_Bandaze.Amount;
						from.AddToBackpack( new Bandage(amount) );
						if(m_Bandaze.Amount > 1)
						{	
							from.SendLocalizedMessage( 1067052, amount.ToString() );
						}
						else
						{	
							from.SendLocalizedMessage( 1067050 );
						}
						m_Bandaze.Amount = 1;
						m_Bandaze.Consume();
					}
					else
					{
						from.SendLocalizedMessage( 1067051 );
					}
				}
				else if ( targeted == b)
				{
					if ( (b.ItemID >= 5937 && b.ItemID <= 5978) || (b.ItemID >= 6038 && b.ItemID <= 6066) || (b.ItemID >= 6595 && b.ItemID <= 6636) || (b.ItemID >= 8099 && b.ItemID <= 8138) || (b.ItemID >= 9299 && b.ItemID <= 9309) || (b.ItemID >= 13422 && b.ItemID <= 13525) || (b.ItemID >= 13549 && b.ItemID <= 13616) || (b.ItemID == 3707) || (b.ItemID == 4104) || (b.ItemID == 5453) || (b.ItemID >= 5458 && b.ItemID <= 5460) || (b.ItemID == 5465) || (b.ItemID >= 2881 && b.ItemID <= 2884) )
					{
						int amount = m_Bandaze.Amount;
						from.AddToBackpack( new Bandage(amount) );
						if(m_Bandaze.Amount > 1)
						{	
							from.SendLocalizedMessage( 1067052, amount.ToString() );
						}
						else
						{	
							from.SendLocalizedMessage( 1067050 );
						}
						m_Bandaze.Amount = 1;
						m_Bandaze.Consume();
					}
					else if ( b is BaseBeverage )
					{
						BaseBeverage bev = b as BaseBeverage;

						if ( bev.Content == BeverageType.Water )
						{
							if ( bev.Quantity == 10 && m_Bandaze.Amount <= 100)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 91)
								{
									bev.Quantity = (bev.Quantity - 10);
								}
								else if(m_Bandaze.Amount >= 81)
								{
									bev.Quantity = (bev.Quantity - 9);
								}
								else if(m_Bandaze.Amount >= 71)
								{
									bev.Quantity = (bev.Quantity - 8);
								}
								else if(m_Bandaze.Amount >= 61)
								{
									bev.Quantity = (bev.Quantity - 7);
								}
								else if(m_Bandaze.Amount >= 51)
								{
									bev.Quantity = (bev.Quantity - 6);
								}
								else if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 9 && m_Bandaze.Amount <= 90)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 81)
								{
									bev.Quantity = (bev.Quantity - 9);
								}
								else if(m_Bandaze.Amount >= 71)
								{
									bev.Quantity = (bev.Quantity - 8);
								}
								else if(m_Bandaze.Amount >= 61)
								{
									bev.Quantity = (bev.Quantity - 7);
								}
								else if(m_Bandaze.Amount >= 51)
								{
									bev.Quantity = (bev.Quantity - 6);
								}
								else if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 8 && m_Bandaze.Amount <= 80)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 71)
								{
									bev.Quantity = (bev.Quantity - 8);
								}
								else if(m_Bandaze.Amount >= 61)
								{
									bev.Quantity = (bev.Quantity - 7);
								}
								else if(m_Bandaze.Amount >= 51)
								{
									bev.Quantity = (bev.Quantity - 6);
								}
								else if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 7 && m_Bandaze.Amount <= 70)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 61)
								{
									bev.Quantity = (bev.Quantity - 7);
								}
								else if(m_Bandaze.Amount >= 51)
								{
									bev.Quantity = (bev.Quantity - 6);
								}
								else if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 6 && m_Bandaze.Amount <= 60)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 51)
								{
									bev.Quantity = (bev.Quantity - 6);
								}
								else if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 5 && m_Bandaze.Amount <= 50)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 41)
								{
									bev.Quantity = (bev.Quantity - 5);
								}
								else if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 4 && m_Bandaze.Amount <= 40)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 31)
								{
									bev.Quantity = (bev.Quantity - 4);
								}
								else if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 3 && m_Bandaze.Amount <= 30)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 21)
								{
									bev.Quantity = (bev.Quantity - 3);
								}
								else if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 2 && m_Bandaze.Amount <= 20)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								if(m_Bandaze.Amount >= 11)
								{
									bev.Quantity = (bev.Quantity - 2);
								}
								else if(m_Bandaze.Amount >= 1)
								{
									bev.Quantity = (bev.Quantity - 1);
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else if ( bev.Quantity == 1 && m_Bandaze.Amount <= 10)
							{
								int amount = m_Bandaze.Amount;
								from.AddToBackpack( new Bandage(amount) );
								if(m_Bandaze.Amount > 1)
								{	
									from.SendLocalizedMessage( 1067052, amount.ToString() );
								}
								else
								{	
									from.SendLocalizedMessage( 1067050 );
								}
								from.SendMessage("Some of the water in the container has been depleted.");
								bev.Quantity = (bev.Quantity - 1);
								m_Bandaze.Amount = 1;
								m_Bandaze.Consume();
							}
							else
							{
								from.SendMessage("There isn't enough water in that to wash with.");
							}
						}
						else
						{
							from.SendLocalizedMessage( 1067051 );
						}
					}
					else
					{
						from.SendLocalizedMessage( 1067051 );
					}
				}
				else if ( targeted == c)
				{
					if ( (c.TileID >= 168 && c.TileID <= 171) || (c.TileID >= 310 && c.TileID <= 311) )
					{
						int amount = m_Bandaze.Amount;
						from.AddToBackpack( new Bandage(amount) );
						if(m_Bandaze.Amount > 1)
						{	
							from.SendLocalizedMessage( 1067052, amount.ToString() );
						}
						else
						{	
							from.SendLocalizedMessage( 1067050 );
						}
						m_Bandaze.Amount = 1;
						m_Bandaze.Consume();
					}
					else
					{
						from.SendLocalizedMessage( 1067051 );
					}
				}
				else
				{
					from.SendLocalizedMessage( 1067051 );
				}
			}
		}
	}
}