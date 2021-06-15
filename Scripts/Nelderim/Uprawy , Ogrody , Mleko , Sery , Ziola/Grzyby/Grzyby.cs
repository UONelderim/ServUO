using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{

        public class Muchomor1 : Item
	{
		[Constructable]
		public Muchomor1() : base( 0x0D16 )
		{
			this.Movable = false;
		        this.Name = "Muchomor";
                        this.Weight = 1.0;
                        this.Hue = 25;	
		}

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Lekko trujacy" );	
                }

                public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive || from.Mounted ) return;
                                
                                if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					
				from.Direction = from.GetDirectionTo( this );
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

				from.SendMessage("Zebrales muchomora."); 
				this.Delete(); 

				from.AddToBackpack( new MuchomorA() );
				}	
                }
         
		public Muchomor1(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

        public class Muchomor2 : Item
	{
		[Constructable]
		public Muchomor2() : base( 0x0D16 )
		{
			this.Movable = false;
		        this.Name = "Muchomor";
                        this.Weight = 1.0;	
                        this.Hue = 20;
		}

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Przecietnie trujacy" );	
                }

                public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive || from.Mounted ) return;
                                
                                if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					
				from.Direction = from.GetDirectionTo( this );
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

				from.SendMessage("Zebrales przecietnie trujacego muchomora."); 
				this.Delete(); 

				from.AddToBackpack( new MuchomorB() );
				}	
                }
          
		public Muchomor2(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

        public class Muchomor3 : Item
	{
                [Constructable]
		public Muchomor3() : base( 0x0D16 )
		{
			this.Movable = false;
		        this.Name = "Muchomor";
                        this.Weight = 1.0;
                        this.Hue = 18;
		}

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Silnie trujacy" );	
                }

                public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive || from.Mounted ) return;
                                
                                if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					
				from.Direction = from.GetDirectionTo( this );
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

				from.SendMessage("Zebrales silnie trujacego muchomora."); 
				this.Delete(); 

				from.AddToBackpack( new MuchomorC() );
				}	
                }

		public Muchomor3(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

        public class Muchomor4 : Item
	{
                [Constructable]
		public Muchomor4() : base( 0x0D16 )
		{
			this.Movable = false;
		        this.Name = "Muchomor";
                        this.Weight = 1.0;
                        this.Hue = 15;
		}

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Smiertelnie trujacy" );	
                }

                public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive || from.Mounted ) return;
                                
                                if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					
				from.Direction = from.GetDirectionTo( this );
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

				from.SendMessage("Zebrales smiertelnie trujacego muchomora."); 
				this.Delete(); 

				from.AddToBackpack( new MuchomorD() );
				}	
                }

		public Muchomor4(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}


        public class MuchomorA : Food
	{
		[Constructable]
		public MuchomorA() : this( 1 )
		{
		}

		[Constructable]
		public MuchomorA( int amount ) : base( 0x0D16 )
		{
			this.Name = "Muchomor";
                        this.Weight = 1.0;
			this.FillFactor = 1;
		        this.Poison = Poison.Lesser;
                        this.Hue = 25;
                        this.Amount = amount;
                        this.Stackable = true;
                }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Lekko trujacy" );	
                }
  
                public MuchomorA( Serial serial ) : base( serial )
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


        public class MuchomorB : Food
	{
		[Constructable]
		public MuchomorB() : this( 1 )
		{
		}

		[Constructable]
		public MuchomorB( int amount ) : base( 0x0D16 )
		{
                        this.Name = "Muchomor";
                        this.Weight = 1.0;
			this.FillFactor = 1;  
		        this.Poison = Poison.Regular;
                        this.Hue = 20;
                        this.Amount = amount;
                        this.Stackable = true;  
                }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "Przecietnie trujacy" );
                }

		public MuchomorB( Serial serial ) : base( serial )
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


        public class MuchomorC : Food
	{
		[Constructable]
		public MuchomorC() : this( 1 )
		{
		}

		[Constructable]
		public MuchomorC( int amount ) : base( 0x0D16 )
		{
			this.Name = "Muchomor";
                        this.Weight = 1.0;
			this.FillFactor = 1;  
		        this.Poison = Poison.Greater;
                        this.Hue = 18;
                        this.Amount = amount;
                        this.Stackable = true;
                }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
                
		base.AddNameProperty( list );
		list.Add( "Silnie trujacy" );
                }

		public MuchomorC( Serial serial ) : base( serial )
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

        public class MuchomorD : Food
	{
		[Constructable]
		public MuchomorD() : this( 1 )
		{
		}

		[Constructable]
		public MuchomorD( int amount ) : base( 0x0D16 )
		{
			this.Name = "Muchomor";
                        this.Weight = 1.0;
			this.FillFactor = 1;  
		        this.Poison = Poison.Deadly;
                        this.Hue = 15;
                        this.Amount = amount;
                        this.Stackable = true;   
                }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
                
		base.AddNameProperty( list );
		list.Add( "Smiertelnie trujacy" );
                }

		public MuchomorD( Serial serial ) : base( serial )
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
