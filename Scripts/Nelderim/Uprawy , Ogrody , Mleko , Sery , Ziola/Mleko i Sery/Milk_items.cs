using System;

namespace Server.Items
{
	public abstract class MilkBottle : Item
	{
		private Mobile m_Poisoner;
		private Poison m_Poison;
		private int m_FillFactor;
		
		public virtual Item EmptyItem{ get { return null; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner
		{
			get { return m_Poisoner; }
			set { m_Poisoner = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int FillFactor
		{
			get { return m_FillFactor; }
			set { m_FillFactor = value; }
		}
		
		public MilkBottle( int itemID ) : base( itemID )
		{
			this.FillFactor = 4;
		}
		
		public MilkBottle( Serial serial ) : base( serial )
		{
		}
		
		public void Boire( Mobile from )
		{
			if ( soif( from, m_FillFactor ) )
			{
				// Play a random "Boire" sound
				from.PlaySound( Utility.Random( 0x30, 2 ) );
				
				if ( from.Body.IsHuman && !from.Mounted )
					from.Animate( 34, 5, 1, true, false, 0 );
				
				if ( m_Poison != null )
					from.ApplyPoison( m_Poisoner, m_Poison );
				
				this.Consume();
				
				Item item = EmptyItem;
				
				if ( item != null )
					from.AddToBackpack( item );
			}
		}
		
		static public bool soif( Mobile from, int fillFactor )
		{
			if ( from.Thirst >= 20 )
			{
				from.SendMessage( "Nie mozesz juz nic wypic!" );
				return false;
			}
			
			int iThirst = from.Thirst + fillFactor;
			if ( iThirst >= 20 )
			{
				from.Thirst = 20;
				from.SendMessage( "Jestes juz pelen!" );
			}
			else
			{
				from.Thirst = iThirst;
				
				if ( iThirst < 5 )
					from.SendMessage( "Wypiles mleko ale wciaz jestes bardzo spragniony." );
				else if ( iThirst < 10 )
					from.SendMessage( "Wypiles mleko i poczules sie troche lepiej." );
				else if ( iThirst < 15 )
					from.SendMessage( "Po wypiciu mleka czujesz znacznie mniejsze pragnienie." );
				else
					from.SendMessage( "Po wypiciu mleka czujesz sie pelen." );
			}
			
			return true;
		}
		
		
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;
			
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
				Boire( from );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 1 ); // version
			
			writer.Write( m_Poisoner );
			
			Poison.Serialize( m_Poison, writer );
			writer.Write( m_FillFactor );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 1:
				{
					m_Poisoner = reader.ReadMobile();

					goto case 0;
				}
				case 0:
				{
					m_Poison = Poison.Deserialize( reader );
					m_FillFactor = reader.ReadInt();
					break;
				}
			}
		}
	}
	public class BottleCowMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleCowMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Butelka krowiego mleka";
		}
		
		public BottleCowMilk( Serial serial ) : base( serial )
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
	public class BottleGoatMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleGoatMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Butelka koziego mleka";
		}
		
		public BottleGoatMilk( Serial serial ) : base( serial )
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
	public class BottleSheepMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleSheepMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Butelka owczego mleka";
		}
		
		public BottleSheepMilk( Serial serial ) : base( serial )
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


/* ***************************** Cheese ******************************** */



	// fromage de vache
	
	public class FromageDeVache : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "You can only cut up one wheel at a time." );
				return;
			}

			base.ScissorHelper( from, new FromageDeVacheWedge(), 1 );

			from.AddToBackpack( new FromageDeVacheWedgeSmall() );

			from.SendMessage( "You cut a wedge out of the wheel." );
		}

		[Constructable]
		public FromageDeVache() : this( 1 )
		{
		}

		[Constructable]
		public FromageDeVache( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "Emmental (krowi ser)";
			this.Hue = 0x481;
		}

		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeVache(), amount );
		//}
		
		public FromageDeVache( Serial serial ) : base( serial )
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

	public class FromageDeVacheWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			base.ScissorHelper( from, new FromageDeVacheWedgeSmall(), 3 );
			from.SendMessage( "You cut the wheel into 3 wedges." );
		}

		[Constructable]
		public FromageDeVacheWedge() : this( 1 )
		{
		}

		[Constructable]
		public FromageDeVacheWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Emmental (krowi ser)";
			this.Hue = 0x481;
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeVacheWedge(), amount );
		//}

		public FromageDeVacheWedge( Serial serial ) : base( serial )
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

	public class FromageDeVacheWedgeSmall : Food
	{
		[Constructable]
		public FromageDeVacheWedgeSmall() : this( 1 )
		{
		}

		[Constructable]
		public FromageDeVacheWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "Emmental (krowi ser)";
			this.Hue = 0x481;
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeVacheWedgeSmall(), amount );
		//}

		public FromageDeVacheWedgeSmall( Serial serial ) : base( serial )
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



	// fromage de Brebis
	
	public class FromageDeBrebis : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "You can only cut up one wheel at a time." );
				return;
			}

			base.ScissorHelper( from, new FromageDeBrebisWedge(), 1 );

			from.AddToBackpack( new FromageDeBrebisWedgeSmall() );

			from.SendMessage( "You cut a wedge out of the wheel." );
		}

		[Constructable]
		public FromageDeBrebis() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeBrebis( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "Perail de Brebis (owczy ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeBrebis( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeBrebis(), amount );
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

	public class FromageDeBrebisWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			base.ScissorHelper( from, new FromageDeBrebisWedgeSmall(), 3 );
			from.SendMessage( "You cut the wheel into 3 wedges." );
		}

		[Constructable]
		public FromageDeBrebisWedge() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeBrebisWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Perail de Brebis (owczy ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeBrebisWedge( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeBrebisWedge(), amount );
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

	public class FromageDeBrebisWedgeSmall : Food
	{
		[Constructable]
		public FromageDeBrebisWedgeSmall() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeBrebisWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "Perail de Brebis (owczy ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeBrebisWedgeSmall( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeBrebisWedgeSmall(), amount );
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



	// fromage de Chevre
	
	public class FromageDeChevre : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "You can only cut up one wheel at a time." );
				return;
			}

			base.ScissorHelper( from, new FromageDeChevreWedge(), 1 );

			from.AddToBackpack( new FromageDeChevreWedgeSmall() );

			from.SendMessage( "You cut a wedge out of the wheel." );
		}

		[Constructable]
		public FromageDeChevre() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeChevre( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "Chevreton du Bourbonnais (kozi ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeChevre( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeChevre(), amount );
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

	public class FromageDeChevreWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			base.ScissorHelper( from, new FromageDeChevreWedgeSmall(), 3 );
			from.SendMessage( "You cut the wheel into 3 wedges." );
		}

		[Constructable]
		public FromageDeChevreWedge() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeChevreWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Chevreton du Bourbonnais (kozi ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeChevreWedge( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeChevreWedge(), amount );
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

	public class FromageDeChevreWedgeSmall : Food
	{
		[Constructable]
		public FromageDeChevreWedgeSmall() : this( 1 )
		{
		}
		
		[Constructable]
		public FromageDeChevreWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "Chevreton du Bourbonnais (kozi ser)";
			this.Hue = 0x481;
		}
		
		public FromageDeChevreWedgeSmall( Serial serial ) : base( serial )
		{
		}
		
		//public override Item Dupe( int amount )
		//{
		//	return base.Dupe( new FromageDeChevreWedgeSmall(), amount );
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
