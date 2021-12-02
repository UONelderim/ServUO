using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class OrangePetalsBrew : Item
	{
		public virtual TimeSpan Cooldown { get { return TimeSpan.FromMinutes(15.0); }  }
		

		public override double DefaultWeight
		{
			get { return 2.0; }
		}

		[Constructable]
		public OrangePetalsBrew() : this( 1 )
		{
		}

		[Constructable]
		public OrangePetalsBrew( int amount ) : base( 3850 )
		{
			Stackable = true;
			Hue = 0x2B;
			Amount = amount;
            Name = "Wywar Odpornosci na trucizny";
		}

		public OrangePetalsBrew( Serial serial ) : base( serial )
		{
		}

		public override bool CheckItemUse( Mobile from, Item item ) 
		{ 
			if ( item != this )
				return base.CheckItemUse( from, item );

			if ( from != this.RootParent )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
				return false;
			}

			return base.CheckItemUse( from, item );
		}

		public override void OnDoubleClick( Mobile from )
		{
			OrangePetalsBrewContext context = GetContext( from );

			if ( context != null )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061904 );
				return;
			}

			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061905 );
			from.PlaySound( 0x3B );

			Timer timer = new OrangePetalsBrewTimer( from );
			timer.Start();

			AddContext( from, new OrangePetalsBrewContext( timer ) );

			this.Consume();
		}

		private static Hashtable m_Table = new Hashtable();

		private static void AddContext( Mobile m, OrangePetalsBrewContext context )
		{
			m_Table[m] = context;
		}

		public static void RemoveContext( Mobile m )
		{
			OrangePetalsBrewContext context = GetContext( m );

			if ( context != null )
				RemoveContext( m, context );
		}

		private static void RemoveContext( Mobile m, OrangePetalsBrewContext context )
		{
			m_Table.Remove( m );

			context.Timer.Stop();
		}

		private static OrangePetalsBrewContext GetContext( Mobile m )
		{
			return ( m_Table[m] as OrangePetalsBrewContext );
		}

		public static bool UnderEffect( Mobile m )
		{
			return ( GetContext( m ) != null );
		}

		private class OrangePetalsBrewTimer : Timer
		{
			private Mobile m_Mobile;

			public OrangePetalsBrewTimer( Mobile from ) : base ( TimeSpan.FromMinutes( 2.0 ) )
			{
				m_Mobile = from;
			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Deleted )
				{
					m_Mobile.LocalOverheadMessage( MessageType.Regular, 0x3F, true,
						"* Efekt wywaru odpornosci na trucizne zanika *" );
				}

				RemoveContext( m_Mobile );
			}
		}

		private class OrangePetalsBrewContext
		{
			private Timer m_Timer;

			public Timer Timer{ get{ return m_Timer; } }

			public OrangePetalsBrewContext( Timer timer )
			{
				m_Timer = timer;
			}
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