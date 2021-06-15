using System; 
using System.Collections;
using Server.Network; 
using Server.Mobiles; 
using Server.Items; 
using Server.Gumps;

namespace Server.Items.Crops 
{ 
	public class OnionSeed : BaseCrop 
	{ 
		// return true to allow planting on Dirt Item (ItemID 0x32C9)
		// See CropHelper.cs for other overriddable types
		public override bool CanGrowGarden{ get{ return true; } }
		
		[Constructable]
		public OnionSeed() : this( 1 )
		{
		}

		[Constructable]
		public OnionSeed( int amount ) : base( 0xF27 )
		{
			Stackable = true; 
			Weight = .5; 
			Hue = 0x5E2; 

			Movable = true; 
			
			Amount = amount;
			Name = "sadzonka cebuli"; 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Nie mozesz uprawiac roslin na wierzchowcu!." ); 
				return; 
			}

			Point3D m_pnt = from.Location;
			Map m_map = from.Map;

			if ( !IsChildOf( from.Backpack ) ) 
			{ 
				from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it. 
				return; 
			} 

			else if ( !CropHelper.CheckCanGrow( this, m_map, m_pnt.X, m_pnt.Y ) )
			{
				from.SendMessage( "Ta sadzonka nie urosnie tutaj." ); 
				return; 
			}
			
			//check for BaseCrop on this tile
			ArrayList cropshere = CropHelper.CheckCrop( m_pnt, m_map, 0 );
			if ( cropshere.Count > 0 )
			{
				from.SendMessage( "Tutaj juz cos rosnie." ); 
				return;
			}

			//check for over planting prohibt if 4 maybe 3 neighboring crops
			ArrayList cropsnear = CropHelper.CheckCrop( m_pnt, m_map, 1 );
			if ( ( cropsnear.Count > 3 ) || (( cropsnear.Count == 3 ) && Utility.RandomBool() ) )
			{
				from.SendMessage( "W tym miejscu jest zbyt wiele roslin." ); 
				return;
			}

			if ( this.BumpZ ) ++m_pnt.Z;

			if ( !from.Mounted )
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

			from.SendMessage("Zasadziles rosline"); 
			this.Consume(); 
			Item item = new OnionSeedling( from ); 
			item.Location = m_pnt; 
			item.Map = m_map; 
			
		} 

		public OnionSeed( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 


	public class OnionSeedling : BaseCrop 
	{ 
		private static Mobile m_sower;
		public Timer thisTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[Constructable] 
		public OnionSeedling( Mobile sower ) : base( 0xC68 )
		{ 
			Movable = false; 
			Name = "sadzonka cebuli"; 
			m_sower = sower;
			
			init( this );
		} 

		public static void init( OnionSeedling plant )
		{
			plant.thisTimer = new CropHelper.GrowTimer( plant, typeof(OnionCrop), plant.Sower ); 
			plant.thisTimer.Start(); 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Nie mozesz tego zebrac bedac na wierzchowcu." ); 
				return; 
			}

			if ( ( Utility.RandomDouble() <= .25 ) && !( m_sower.AccessLevel > AccessLevel.Counselor ) ) 
			{ //25% Chance
				from.SendMessage( "Wyrwales rosline z korzeniami." ); 
				thisTimer.Stop();
				this.Delete();
			}
			else from.SendMessage( "Ta roslina jest za mloda." ); 
		}

		public OnionSeedling( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			m_sower = reader.ReadMobile();

			init( this );
		} 
	} 

	public class OnionCrop : BaseCrop 
	{ 
		private const int max = 6;
		private int fullGraphic;
		private int pickedGraphic;
		private DateTime lastpicked;

		private Mobile m_sower;
		private int m_yield;

		public Timer regrowTimer;

		private DateTime m_lastvisit;

		[CommandProperty( AccessLevel.GameMaster )] 
		public DateTime LastSowerVisit{ get{ return m_lastvisit; } }

		[CommandProperty( AccessLevel.GameMaster )] // debuging
		public bool Growing{ get{ return regrowTimer.Running; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Yield{ get{ return m_yield; } set{ m_yield = value; } }

		public int Capacity{ get{ return max; } }
		public int FullGraphic{ get{ return fullGraphic; } set{ fullGraphic = value; } }
		public int PickGraphic{ get{ return pickedGraphic; } set{ pickedGraphic = value; } }
		public DateTime LastPick{ get{ return lastpicked; } set{ lastpicked = value; } }
		
		[Constructable] 
		public OnionCrop( Mobile sower ) : base( 0xC69 ) 
		{ 
			Movable = false; 
			Name = "cebula"; 

			m_sower = sower;
			m_lastvisit = DateTime.Now;

			init( this, false );
		}

		public static void init ( OnionCrop plant, bool full )
		{
			plant.PickGraphic = ( 0xC69 );
			plant.FullGraphic = ( 0xC6F );

			plant.LastPick = DateTime.Now;
			plant.regrowTimer = new CropTimer( plant );

			if ( full )
			{
				plant.Yield = plant.Capacity;
				((Item)plant).ItemID = plant.FullGraphic;
			}
			else
			{
				plant.Yield = 0;
				((Item)plant).ItemID = plant.PickGraphic;
				plant.regrowTimer.Start();
			}
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( m_sower == null || m_sower.Deleted ) 
				m_sower = from;

			if ( from != m_sower ) 
			{ 
			from.SendMessage( "To nie jest twoja roslina !" ); 
                        from.Criminal = true; 
			}

			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Nie mozesz tego zebrac bedac na wierzchowcu." ); 
				return; 
			}

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;
			
				if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					if ( m_yield < 1 )
					{
						from.SendMessage( "Nie ma tu nic do zebrania." ); 

						if ( PlayerCanDestroy && !( m_sower.AccessLevel > AccessLevel.Counselor ) )
						{  
						  from.SendMessage( "Wyrwales rosline z korzeniami." ); 
						  this.Delete();
						}
					}
					else //check sower
					{
						from.Direction = from.GetDirectionTo( this );

						from.Animate( from.Mounted ? 29:32, 5, 1, true, false, 0 ); 

						if ( from == m_sower ) 
						{
							m_lastvisit = DateTime.Now;
						}

						int pick = Utility.Random( m_yield + 1 );
			                         
			                        if ( pick == 0 )
						{
							from.SendMessage( "Wyrwales rosline z korzeniami." ); 
						        this.Delete();
						}
					
						m_yield -= pick;
						from.SendMessage( "Zebrales {0} cebuli{1}!", pick, ( pick == 1 ? "" : "" ) ); 

						((Item)this).ItemID = pickedGraphic;

                                                if ( pick > 0 )
						{
						Onion crop = new Onion( pick ); 
						from.AddToBackpack( crop );
                                                }

                                                if ( pick == 5 )
						{
							from.SendMessage( "Zebrales troche nasion." ); 							
						        from.AddToBackpack( new OnionSeed() );
							return;
						}

						if ( !regrowTimer.Running )
						{
							regrowTimer.Start();
						}
					}
				} 
				else 
				{ 
					from.SendMessage( "Jestes za daleko." ); 
				} 
			}
		} 

		private class CropTimer : Timer
		{
			private OnionCrop i_plant;

			public CropTimer( OnionCrop plant ) : base( TimeSpan.FromSeconds( 600 ), TimeSpan.FromSeconds( 15 ) )
			{
				Priority = TimerPriority.OneSecond;
				i_plant = plant;
			}

			protected override void OnTick()
			{
				if ( ( i_plant != null ) && ( !i_plant.Deleted ) )
				{
					int current = i_plant.Yield;

					if ( ++current >= i_plant.Capacity )
					{
						current = i_plant.Capacity;
						((Item)i_plant).ItemID = i_plant.FullGraphic;
						Stop();
					}
					else if ( current <= 0 )
						current = 1;

					i_plant.Yield = current;
					//i_plant.PublicOverheadMessage( MessageType.Regular, 0x22, false, string.Format( "{0}", current )); 
				}
				else Stop();
			}
		}

		public OnionCrop( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); 
			writer.Write( m_lastvisit );
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			switch ( version )
			{
				case 1:
				{
					m_lastvisit = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_sower = reader.ReadMobile();
					break;
				}
			}

			if ( version == 0 ) 
				m_lastvisit = DateTime.Now;

			init( this, true );
		} 
	} 
} 
