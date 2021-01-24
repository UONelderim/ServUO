using System;
using System.Collections;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Necromancy;
using Server.Spells.Chivalry;
using Server.Spells.Bushido;
using Server.Spells.Ninjitsu;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.Tournament;
using Server.Spells.Fifth;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Regions
{
	public class NArenaRegion : Region
	{
		#region Subclasses
		
		public class FightTimer : Timer
		{		
			Mobile m_Target;
			
			public FightTimer( Mobile target, int span ) : base( TimeSpan.FromMinutes( span ) )
			{
				m_Target = target;
				Priority = TimerPriority.FiftyMS;
				//Console.WriteLine( "building Timer - tick {0}", DateTime.Now.TimeOfDay );
			}

			protected override void OnTick()
			{
				//Console.WriteLine( "Killing Timer - tock {0}", DateTime.Now.TimeOfDay );
				                  
				if ( m_Target.Region is NArenaRegion && ( m_Target.Region as NArenaRegion ).IsFighter( m_Target ) )
				{
					m_Target.SendLocalizedMessage( 505256 ); // Uplynal wykupiony czas na arenie.
					( m_Target.Region as NArenaRegion ).EndFight( m_Target, EOFReason.EndOfTime );
				}
			}
		}
		
		public class AfterFightBlockTimer : Timer
		{		
			Mobile m_Target;
			
			public AfterFightBlockTimer( Mobile target ) : base( TimeSpan.FromSeconds( 5 ) )
			{
				NArenaRegion.Protection.Add( target );
				m_Target = target;
				m_Target.Frozen = true;
				// 5-sekundowa blokada bezpieczenstwa. Anuluj wszystkie agresywne akcje!
				m_Target.SendLocalizedMessage( 505257, "", 38 );
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				m_Target.Frozen = false;
				m_Target.SendLocalizedMessage( 505258, "", 167 ); // Blokada bezpieczenstwa zniesiona.
				NArenaRegion.Protection.Remove( m_Target );
			}
		}
		
		public class BeforeFightBlockTimer : Timer
		{		
			Mobile m_Target;
			
			public BeforeFightBlockTimer( Mobile target ) : base( TimeSpan.FromSeconds( 5 ) )
			{
				m_Target = target;
				m_Target.Frozen = true;
				// 5-sekundowa blokada startowa. Gotuj sie!
				m_Target.SendLocalizedMessage( 505259, "", 38 );
				Priority = TimerPriority.EveryTick;
			}

			protected override void OnTick()
			{
				m_Target.Frozen = false;
				m_Target.SendLocalizedMessage( 505260, "", 167 ); // Blokada zniesiona. WALKA!!!
			}
		}
		
		public class Fighter
		{
			private Mobile m_Mobile;
			private FightTimer m_Timer;
			private FightType m_Fight; 
			private DateTime m_FightStart;
			private NArenaRegion m_Owner;
			
			public Mobile Foe { get { return m_Mobile; } }
			public DateTime Start { get { return m_FightStart; } }
			public FightType Fight { get { return m_Fight; } }
			public NArenaRegion Arena { get { return m_Owner; } }
			public FightTimer Timer { get { return m_Timer; } }
			
			public Fighter( NArenaRegion a, Mobile m, FightType ft )
			{
				m_Owner = a;
				m_Mobile = m;
				m_Fight = ft;
				m_Timer = new FightTimer( m, ( int ) ft );
				m_FightStart = DateTime.Now;
				m_Timer.Start();
			}
		}
		
		#endregion
		#region Fields & Properties
		
		private ArenaTrainer m_Owner;
		private ArrayList m_Fighters;
		private static ArrayList m_Protection;
		public ArenaTrainer Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}
		public ArrayList Fighters
		{
			get { return m_Fighters; }
		}
		public static ArrayList Protection
		{
			get
			{
				if ( m_Protection == null )
					m_Protection = new ArrayList();
				
				return m_Protection;
			}
			set
			{
				m_Protection = value;
			}
		}
		
		#endregion
		#region Constructors, Initialization and Serialization
				
		public NArenaRegion( string name, Map map, Rectangle3D[] area, ArenaTrainer owner ) : base( owner.ArenaName, owner.Map, 100, area )
		{
			m_Owner = owner;
			m_Fighters = new ArrayList();
			DefaultLogoutDelay = TimeSpan.FromSeconds( 30 );
		}

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Logout += new LogoutEventHandler( OnLogout );
		}
		
		#endregion
		#region Overriden Region Methods
		
		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}
		
		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = 6;
		}
		
		public override bool OnBeginSpellCast( Mobile m, ISpell s )
		{
			FightType ft = FightType.None;
			
			if ( m.AccessLevel >= AccessLevel.Counselor )
				return base.OnBeginSpellCast( m, s );
			
			if ( IsFighter( m ) && ( ( m_Owner.NoMagery && !( s is PaladinSpell ) && !( s is NecromancerSpell ) && !( s is SamuraiSpell ) && !( s is NinjaSpell ) )  // 07.02.28 :: emfor
			                        || ( s is PaladinSpell && m_Owner.NoChivalry ) || ( s is NecromancerSpell && m_Owner.NoNecro ) ) )
			{
				m.SendLocalizedMessage( 505261 ); // Zasady walki zabraniaja stosowania tej magii!
				return false;
			}
			else if ( m_Owner.NoHiding && s is InvisibilitySpell && IsFighter( m, out ft ) )
			{
				m.SendLocalizedMessage( 505262 ); // "Zasady walki zabraniaja uzycia tego!!!"
				
				if ( ( int ) ft < ( int ) FightType.ShortTraining )
				{
					m_Owner.Say( 505263 ); // Zasady walki zabraniaja ukrywania sie!
					
					EndFight( m, EOFReason.Defeat );
									
					Mobile op = ( m_Fighters[ 0 ] as Fighter ).Foe;
									
					if ( op != null && op.Alive )								
						EndFight( op, EOFReason.Victory );
				}
				
				return false;	
			}
			else if ( s is GateTravelSpell || s is RecallSpell || s is MarkSpell )
			{
				m.SendLocalizedMessage( 505264 ); // You cannot cast that spell here.
				return false;
			}
			else 
			{
				return base.OnBeginSpellCast( m, s );
			}
		}
		
		public override bool OnDoubleClick(Mobile m, object o)
		{
			if ( m.AccessLevel > AccessLevel.Player )
				return true;
			
			if ( m_Owner.NoAlchemy && o is BasePotion )
			{
				m.SendLocalizedMessage( 505262 ); // "Zasady walki zabraniaja uzycia tego!!!"
				return false;
			}
			
			if ( m_Owner.NoHealing && o is Bandage )
			{
				m.SendLocalizedMessage( 505262 ); // "Zasady walki zabraniaja uzycia tego!!!"
				return false;
			}
			
			if ( m_Owner.NoMounts && ( o is BaseMount || o is EtherealMount ) )
			{
				m.SendLocalizedMessage( 505265 ); // Zasady walki zabraniaja dosiadania wierzchowcow!!!
				return false;
			}
			
			return base.OnDoubleClick( m, o );
		}
		
		public override bool OnSkillUse( Mobile m, int Skill )
		{
			FightType ft = FightType.None;
			
			if ( m.AccessLevel > AccessLevel.Counselor )
				return true;
			
			if ( m_Owner.NoHiding && Skill == ( int ) SkillName.Hiding && IsFighter( m, out ft ) )
			{
				m.SendLocalizedMessage( 505262 ); // "Zasady walki zabraniaja uzycia tego!!!"
				
				if ( ( int ) ft < ( int ) FightType.ShortTraining )
				{
					m_Owner.Say( 505263 ); // Zasady walki zabraniaja ukrywania sie!
					
					EndFight( m, EOFReason.Defeat );
									
					Mobile op = ( m_Fighters[ 0 ] as Fighter ).Foe;
									
					if ( op != null && op.Alive )								
						EndFight( op, EOFReason.Victory );
				}
				
				return false;	
			}
			
			if ( m_Owner.NoNecro && Skill == ( int ) SkillName.SpiritSpeak && IsFighter( m ) )
			{
				m.SendLocalizedMessage( 505266 ); // Zasady walki zabraniaja uzycia tej umiejetnosci!!!
				
				return false;
			}
			
			if ( Skill == ( int ) SkillName.Stealing || Skill == ( int ) SkillName.Snooping && IsFighter( m ) )
			{
				m.SendLocalizedMessage( 505266 ); // Zasady walki zabraniaja uzycia tej umiejetnosci!!!
				
				m_Owner.Say( 505267 ); //Zasady areny zabraniaja okradania przeciwnikow!
					
				if ( ( int ) ft < ( int ) FightType.ShortTraining )
				{
					EndFight( m, EOFReason.Defeat );
									
					Mobile op = ( m_Fighters[ 0 ] as Fighter ).Foe;
									
					if ( op != null && op.Alive )								
						EndFight( op, EOFReason.Victory );
				}
				else
					Extort( m );
								
				return false;	
			}
			
			return true;
		}

		public static void OnLogin( LoginEventArgs e )
		{
            Mobile m = e.Mobile;
            Region region = m.Region;

            if ( region is NArenaRegion )
                ( (NArenaRegion)region ).Extort( m );
		}
		
		public static void OnLogout( LogoutEventArgs e )
		{
			e.Mobile.Region.OnExit( e.Mobile );
		}
		
		public override void OnEnter( Mobile m )
		{
			try
			{
				if ( IsFighter( m ) )
				{
					#region Mounts
					
					if ( m_Owner.NoMounts && m.Mounted && m.Mount != null )
					{
						m.SendLocalizedMessage( 505265 ); // Zasady walki zabraniaja dosiadania wierzchowcow!!!
						
						if ( m.Mount is EtherealMount )
						{
							EtherealMount mount = m.Mount as EtherealMount;
							
							mount.UnmountMe();
						}
						else if ( m.Mount is BaseCreature )
						{
							BaseCreature bc = m.Mount as BaseCreature;
							
							( bc as BaseMount ).Rider = null;
							bc.ControlOrder = OrderType.Stay;
							bc.ControlTarget = null;
								
							Extort( bc as Mobile );
						}
					}
					
					#endregion
					#region Controlled Creatures
					
					//Najpierw sprawdzamy zasady wprowadzania kontrolowancow
					if ( m is BaseCreature && !( m is BaseMount ) )
					{
						BaseCreature bc = m as BaseCreature;
						Mobile cm = bc.ControlMaster;
						
						if ( m_Owner.NoFamiliars && m is BaseFamiliar )
						{
							if ( cm != null )
								cm.SendLocalizedMessage( 505268 ); // Zasady areny nie pozwalaja wprowadzic na nia tej istoty.
							
							NArenaRegion.Unsummon( m );
						}
						else if ( m_Owner.NoSummons && NArenaRegion.IsSummon( m ) )
						{
							cm = bc.SummonMaster;
								
							if ( cm != null )
								cm.SendLocalizedMessage( 505268 );
							
							NArenaRegion.Unsummon( m );	
						}
						else if ( m_Owner.NoControlledSummons && NArenaRegion.IsControlledSummon( m ) )
						{
							if ( cm != null )
								cm.SendLocalizedMessage( 505268 );
							
							NArenaRegion.Unsummon( m );	
						}
						else if ( m_Owner.NoControls && !( NArenaRegion.IsControlledSummon( m ) || NArenaRegion.IsSummon( m )
						                                 || m is BaseFamiliar ) )
						{
							if ( cm != null )
								cm.SendLocalizedMessage( 505268 );
							
							bc.ControlOrder = OrderType.Stay;
							bc.ControlTarget = null;
								
							Extort( m );
						}
					}
					#endregion
					#region Notoriety actualization
					
					foreach( Mobile mobile in GetPlayers() )
					{
						if ( mobile == m )
							continue;
						
						if ( IsFighter( mobile ) )
						{
							LookAgain( mobile );
							
							if ( m.NetState != null ) 
								m.NetState.Send( new MobileMoving( mobile, Notoriety.Compute( m, mobile ) ) );
						}
					}
					
					foreach( Mobile mobile in GetMobiles() )
					{
						if ( mobile == m )
							continue;
						
						if ( IsFighter( mobile ) )
						{
							NetState ns = m.NetState;
							
							if ( ns != null ) 
								ns.Send( new MobileMoving( mobile, Notoriety.Compute( m, mobile ) ) );
						}
					}
					
					#endregion
					
					LookAgain( m );
					m.SendLocalizedMessage( 505269, m_Owner.ArenaName ); // Wkraczasz na arene ~1_NAME~.
				}
				else if ( m_Owner.NoEnter && !( m is BaseNelderimGuard ) )
					Extort ( m );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				Extort ( m );
			}
		}
		
        /*
		// 07.05.12 :: emfor :: start
		public override void OnMobileAdd( Mobile m )
		{
      if( m is BaseMount && m_Owner.NoControls )
      {
          BaseCreature bc = m as BaseCreature;
          
          Mobile pm = bc.ControlMaster;
          pm.SendMessage("Twoje zwierze zostalo usuniete z areny!");
          
          bc.ControlOrder = OrderType.Stay;
					bc.ControlTarget = null;
          
          ArenaRegion.Extort( m );
      }
    }
    // 07.05.12 :: emfor :: end
    */

		public override void OnExit( Mobile m )
		{
			try
			{
				if ( IsFighter( m ) )
				{
					if ( m is PlayerMobile )
						EndFight( m, EOFReason.Exit );
					else
						ClearAgressors( m );
					
					#region Notoriety actualization
					
					foreach( Mobile mobile in GetMobiles() )
					{
						if ( mobile == m )
							continue;
								
						if ( IsFighter( mobile ) && m.NetState != null )
							m.NetState.Send( new MobileMoving( mobile, Notoriety.Compute( m, mobile ) ) );
					}
					
					foreach( Mobile mobile in GetPlayers() )
					{
						if ( mobile == m )
							continue;
						
						if ( IsFighter( mobile ) )
						{
							LookAgain( mobile );
							
							if ( m.NetState != null )
								m.NetState.Send( new MobileMoving( mobile, Notoriety.Compute( m, mobile ) ) );
							
							if ( mobile.NetState != null )
								mobile.NetState.Send( new MobileMoving( m, Notoriety.Compute( mobile, m ) ) );
						}
					}
					
					#endregion
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				m_Owner.RestartArena();
			}
		}

		public override void OnDeath( Mobile m )
		{
			FightType ft = FightType.None;
			
			try
			{
				if ( IsFighter( m, out ft ) )
				{
					if ( m is BaseCreature && !( m is BaseFamiliar || NArenaRegion.IsSummon( m ) || NArenaRegion.IsControlledSummon( m ) ) )
					{
						NArenaRegion.Rejuvenate( m, false );
						NArenaRegion.Teleport( m, m_Owner.EndOfFightPoint );
						ClearAgressors( m );
						( m as BaseCreature ).ControlOrder = OrderType.Stay;
						( m as BaseCreature ).ControlTarget = null;
						
						if ( ft != FightType.ShortTraining && ft != FightType.LongTraining )
							m.Frozen = true;
					}
					else
					{
						switch ( ft )
						{
							#region Training
							case FightType.ShortTraining:
							case FightType.LongTraining:
								{
									NArenaRegion.Rejuvenate( m, false );
									NArenaRegion.RejuvenateEffect( m );
									m.SendLocalizedMessage( 505270, "", 38 ); // smierc nie dosiega wojownikow areny!
									break;
								}
							#endregion
							#region Duels
							case FightType.ShortDuel:
							case FightType.LongDuel:
							case FightType.Tournament:
								{
									EndFight( m, EOFReason.Defeat );
									
									Mobile op = ( m_Fighters[ 0 ] as Fighter ).Foe;
									
									if ( op != null && op.Alive )								
										EndFight( op, EOFReason.Victory );
									
									break;
								}
							#endregion
						}
					}
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				m_Owner.RestartArena();
			}
			
			base.OnDeath( m );
		}
		
		#endregion
		#region Arena Methods
		
		public static bool Protected( Mobile m )
		{
			return Protection.Contains( m );
		}
		
		public bool AddFighter( Mobile m, FightType ft )
		{
			try
			{
				RemoveFighter( m );
				m_Fighters.Add( new Fighter( this, m, ft ) );
			}
			catch
			{
				return false;
			}
			                 
			return true;
		}
		
		public bool AddFighter( Mobile blue, Mobile red, FightType ft )
		{
			try
			{
				RemoveFighter( blue );
				RemoveFighter( red );
				
				m_Fighters.Add( new Fighter( this, blue, ft ) );
				m_Fighters.Add( new Fighter( this, red, ft ) );
				
				Point3D oldBlueLocation = blue.Location;
				Point3D oldRedLocation = red.Location;
				Map map = blue.Map;
				
				new BeforeFightBlockTimer( blue ).Start();
				new BeforeFightBlockTimer( red ).Start();
				
				NArenaRegion.Teleport( blue, m_Owner.CornerBlue );
				NArenaRegion.Teleport( red, m_Owner.CornerRed );
				
				#region Pets
				#region Blue
				
				IPooledEnumerable eable = map.GetMobilesInRange( oldBlueLocation, 8 );
				ArrayList toMove = new ArrayList();
				ArrayList toUnsummon = new ArrayList();
	
				foreach ( Mobile mob in eable )
				{
					if ( mob is BaseFamiliar || NArenaRegion.IsSummon( mob ) || NArenaRegion.IsControlledSummon( mob ) )
						toUnsummon.Add( mob );
					else if ( NArenaRegion.IsControlled( mob, blue ) && !Owner.NoControls )
						toMove.Add( mob );
				}
				
				foreach ( Mobile mob in toMove )
					mob.MoveToWorld( blue.Location, blue.Map );
				
				eable.Free();
				toMove.Clear();
			
				#endregion
				#region Red
				
				eable = map.GetMobilesInRange( oldRedLocation, 8 );
	
				foreach ( Mobile mob in eable )
				{
					if ( mob is BaseFamiliar || NArenaRegion.IsSummon( mob ) || NArenaRegion.IsControlledSummon( mob ) )
						toUnsummon.Add( mob );
					else if ( NArenaRegion.IsControlled( mob, red ) && !Owner.NoControls )
						toMove.Add( mob );
				}
				
				foreach ( Mobile mob in toMove )
					mob.MoveToWorld( red.Location, red.Map );
				
				for ( int i = toUnsummon.Count - 1; i >= 0; i-- )
					NArenaRegion.Unsummon( toUnsummon[ i ] as Mobile );
				
				eable.Free();
				
				#endregion
				#endregion
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				return false;
			}
			
			return true;
		}	
		
		public bool AddFighter( TournamentFight tf )
		{
			try
			{
				Mobile blue = tf.Blue;
				Mobile red = tf.Red;
				
				RemoveFighter( blue );
				RemoveFighter( red );
				
				m_Fighters.Add( new Fighter( this, blue, FightType.Tournament ) );
				m_Fighters.Add( new Fighter( this, red, FightType.Tournament ) );
				
				new BeforeFightBlockTimer( blue ).Start();
				new BeforeFightBlockTimer( red ).Start();
				
				Point3D oldBlueLocation = blue.Location;
				Point3D oldRedLocation = red.Location;
				Map map = blue.Map;
				
				NArenaRegion.Teleport( blue, m_Owner.CornerBlue );
				NArenaRegion.Teleport( red, m_Owner.CornerRed );
				
				
				#region Pets
				#region Blue
				
				IPooledEnumerable eable = map.GetMobilesInRange( oldBlueLocation, 8 );
				ArrayList toMove = new ArrayList();
				ArrayList toUnsummon = new ArrayList();
	
				foreach ( Mobile mob in eable )
				{
					if ( mob is BaseFamiliar || NArenaRegion.IsSummon( mob ) || NArenaRegion.IsControlledSummon( mob ) )
						toUnsummon.Add( mob );
					else if ( NArenaRegion.IsControlled( mob, blue ) )
						toMove.Add( mob );
				}
				
				foreach ( Mobile mob in toMove )
					mob.MoveToWorld( blue.Location, blue.Map );
				
				eable.Free();
				toMove.Clear();
			
				#endregion
				#region Red
				
				eable = map.GetMobilesInRange( oldRedLocation, 8 );
	
				foreach ( Mobile mob in eable )
				{
					if ( mob is BaseFamiliar || NArenaRegion.IsSummon( mob ) || NArenaRegion.IsControlledSummon( mob ) )
						toUnsummon.Add( mob );
					else if ( NArenaRegion.IsControlled( mob, blue ) )
						toMove.Add( mob );
				}
				
				foreach ( Mobile mob in toMove )
					mob.MoveToWorld( red.Location, red.Map );
				
				for ( int i = toUnsummon.Count - 1; i >= 0; i-- )
					NArenaRegion.Unsummon( toUnsummon[ i ] as Mobile );
				
				eable.Free();
				
				#endregion
				#endregion
			
				// LookAgain( red );
				// LookAgain( blue );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				return false;
			}
			
			return true;
		}
		
		public void EndFight( Mobile m )
		{
			EndFight( m, EOFReason.DefaultReason );
		}
		
		public void EndFight( Mobile m, EOFReason reason )
		{
			FightType ft = FightType.None;
				
			try
			{
				if ( m != null && IsFighter( m, out ft ) )
				{
					m.Combatant = null;
					m.Warmode = false;
					Target.Cancel( m );
					
					ClearAgressors( m );
					RemoveFighter( m );
					LookAgain( m );
					
					#region Followers
						
					if ( m.Followers > 0 )
					{
						ArrayList toDelete = new ArrayList();
						
						foreach ( Mobile mob in GetMobiles() )
						{
							if ( NArenaRegion.IsControlled( mob, m ) || NArenaRegion.IsSummoned( mob, m ) )
							{
								#region Dispell summonow
								if ( NArenaRegion.IsSummon( mob ) ||NArenaRegion.IsControlledSummon( mob ) )
									toDelete.Add( mob );
								else
								#endregion
								{
									( mob as BaseCreature ).ControlOrder = OrderType.Follow;
							   		( mob as BaseCreature ).ControlTarget = m;
								    	
							   		ClearAgressors( mob );
								}
							}
						}
						
						for ( int i = toDelete.Count - 1; i >= 0; i-- )
						{
							NArenaRegion.Unsummon( toDelete[i] as Mobile );
							toDelete.RemoveAt( i );
						}
						
						if ( m.Followers > 0 )
						{
							IPooledEnumerable eable = m_Owner.Map.GetMobilesInRange( m_Owner.EndOfFightPoint, 1 );
							
							foreach ( Mobile mob in eable )
							{
								if ( NArenaRegion.IsControlled( mob, m ) && mob.Frozen )
									mob.Frozen = false;
							}
						}
					}
					
					#endregion
					
					#region Reason is better then exit
					
					if ( ( int ) reason > ( int ) EOFReason.Exit )
					{
						NArenaRegion.Rejuvenate( m, true );
						new AfterFightBlockTimer( m ).Start();
						
						if ( reason != EOFReason.Victory )
						{
							m.SetLocation( Owner.EndOfFightPoint, true );
							
							#region Teleport Pets
							
							ArrayList move = new ArrayList();
		
							// nie powinno byc juz summonow, wiec je olewamy
							foreach ( Mobile mob in GetMobiles() )
							{
								if ( NArenaRegion.IsControlled( mob, m ) )
									move.Add( mob );
							}
				
							foreach ( Mobile mob in move )
								mob.MoveToWorld( m.Location, m.Map );
							
							#endregion	
							#region Efekt
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z), m.Map, 0x3728, 13);
						    Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 11), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 7), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 3), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3728, 13);
							m.PlaySound(0x228);
							#endregion
						}
						
						#region Fight result announcement
					
						if ( reason == EOFReason.Victory )
						{
							m.FixedParticles( 0x3779, 5, 20, 5002, EffectLayer.Head );
							m.PlaySound( 490 );
							m.SendLocalizedMessage( ( m.Female ) ? 505271 : 505272, "", 167 ); // Zwyciezylas! | Zwyciezyles!
							Owner.Say( 505273, m.Name ); // Zwyciezca jest ~1_NAME~!
						}
						else if ( reason == EOFReason.Defeat )
						{
							// "Zostalas pokonana!!!" : "Zostalas pokonany!!!"
							m.SendLocalizedMessage( ( m.Female ) ? 505274 : 505275, "", 167 );
							// ~1_NAME~ zostala haniebnie pokonana!!! : ~1_NAME~ zostal haniebnie pokonany!!!
							Owner.Say( m.Female ? 505276 : 505277, m.Name );
						}
						
						#endregion	
					}
					#endregion
					#region Coward!
					else if ( reason == EOFReason.Exit )
					{
						if ( ( int ) ft < ( int ) FightType.ShortTraining )
						{
							// "Zostalas pokonana!!!" : "Zostalas pokonany!!!"
							m.SendLocalizedMessage( ( m.Female ) ? 505274 : 505275, "", 167 );
							// ~1_NAME~ haniebnie uciekla z areny!!! : ~1_NAME~ haniebnie uciekl z areny!!!
							Owner.Say( m.Female ? 505278 : 505279, m.Name );
							
							Mobile op = ( m_Fighters[ 0 ] as Fighter ).Foe;
							
							if ( op != null && op.Alive )								
								EndFight( op, EOFReason.Victory );
						}
					}
					#endregion
					
					#region Tournament
					
					if ( ft == FightType.Tournament )
					{
						if ( reason == EOFReason.Victory )
							m_Owner.PassTournamentFightWinner( m );
						else if ( reason == EOFReason.EndOfTime )
							m_Owner.Tournament.PushFight( TournamentPushFight.Restart );
						else if ( reason == EOFReason.ForceRestart )
							m.SendLocalizedMessage( 505280 ); // Pojedynek zostanie powtorzony! Klepsydra na przygotowanie!
					}	
					
					#endregion
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				m_Owner.RestartArena();
			}
		}
		
		public bool IsFighter( Mobile m )
		{
			FightType ft;
			
			return IsFighter(  m, out ft );
		}
		
		public bool IsFighter( Mobile mob, out FightType ft )
		{
			Mobile m = mob;
			ft = FightType.None;
			
			if ( mob == null )
				return false;
			
			if ( mob is BaseCreature )
			{
				BaseCreature bc = mob as BaseCreature;
				
				if ( bc.Controlled && bc.ControlMaster != null )
					m = bc.ControlMaster;
				else if ( bc.Summoned && bc.SummonMaster != null )
					m = bc.SummonMaster;
				else 
					return false;					
			}
			
			try
			{
				foreach ( Fighter f in m_Fighters )
				{
					if ( f.Foe == m )
					{
						ft = f.Fight;
						return true;
					}
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			} 
			
			return false;
		}
		
		private bool RemoveFighter( Mobile m )
		{
			Fighter fight = null;
				
			try
			{				
				foreach ( Fighter f in m_Fighters )
				{
					if ( m == f.Foe )
					{
						f.Timer.Stop();
						fight = f;
						break;
					}
				}
				
				if ( fight != null )
				{
					m_Fighters.Remove( fight );
					
					m.Delta( MobileDelta.Noto );
					m.InvalidateProperties();
					Mobile.ProcessDeltaQueue();

					return true;
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
			
			return false;
		}
		
		private void ClearAgressors( Mobile m )
		{
			#region Revenants
			
			try
			{
				ArrayList rev = new ArrayList();
			
				foreach ( Mobile mobile in GetMobiles() )
				{
					if ( mobile != m && IsFighter( mobile ) && mobile is Revenant && ( mobile as Revenant ).ConstantFocus == m )
						rev.Add( mobile );
				}
						
				foreach ( Mobile mobile in rev )
					NArenaRegion.Unsummon( mobile );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
			
			#endregion
			#region Agressors
			
			try
			{
				List<AggressorInfo> list = m.Aggressors;
				
				for ( int i = list.Count - 1; i >= 0 ; i-- )
				{
					AggressorInfo info = (AggressorInfo)list[i];
			
					if ( IsFighter( info.Attacker ) && !info.Expired )
					{
						if ( info.Attacker.Combatant == m )
							info.Attacker.Combatant = null;
						
						List<AggressorInfo> list2 = info.Attacker.Aggressed;
						
						for ( int j = 0; j < list2.Count; j++ )
						{
							AggressorInfo info2 = (AggressorInfo)list2[j];
					
							if ( info2.Defender == m )
							{
								list2.RemoveAt( j );
								info2.Free();
							}
							
							list.RemoveAt( i );
							info.Free();
						}
					}
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
			
			#endregion
			#region Aggressed
			
			try
			{	
				List<AggressorInfo> list = m.Aggressed;
				
				for ( int i = list.Count - 1; i >= 0 ; i-- )
				{
					AggressorInfo info = (AggressorInfo)list[i];
	
					if ( IsFighter( info.Defender ) && !info.Expired )
					{
						if ( info.Defender.Combatant == m )
							info.Defender.Combatant = null;
						
						List<AggressorInfo> list2 = info.Defender.Aggressors;
						
						for ( int j = 0; j < list2.Count; j++ )
						{
							AggressorInfo info2 = (AggressorInfo)list2[j];
			
							if ( info2.Attacker == m )
							{
								list2.RemoveAt( j );
								info2.Free();	
							}
						}
						
						list.RemoveAt( i );
						info.Free();
					}
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
			
			#endregion
		}
		
		public void CloseBussiness()
		{
			try
			{
				for ( int i = 0; i < m_Fighters.Count; i++ )
				{
					Fighter f = m_Fighters[i] as Fighter;
					
					TimeSpan timeRemain = DateTime.Now - f.Start;
					
					int amount = ( int ) ( ( double ) timeRemain.Minutes / ( double ) f.Fight )
						* ( ( f.Fight > FightType.LongDuel ) ? 5 : 50 ) * 5 / 6;
					
					Banker.Deposit( f.Foe, amount );
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public bool Busy( bool hardBusy )
		{
			try
			{
				if ( hardBusy && m_Fighters.Count > 0 )
					return true;
				
				for ( int i = 0; i < m_Fighters.Count; i++ )
				{
					if ( ( ( Fighter ) m_Fighters[i] ).Fight < FightType.ShortTraining )
						return true;
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				return true;
			}
			
			return false;
		}

		private void Extort( Mobile m )
		{                       
			try
			{
				if ( m != null && m.AccessLevel == AccessLevel.Player )
				{
					// Hej! ~1_NAME~ nie masz prawa przebywac na arenie!
					this.Owner.Say( 505281, m.Name );
					// Nie masz prawa przebywac na arenie! Oplac wstep!
					m.SendLocalizedMessage( 505282 );
	            
					if ( this.Owner.NoEnter )
						NArenaRegion.Teleport( m, this.Owner.ExtortPoint );
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
	
		public void CleanArena( string message )
		{
			try
			{
				if ( message != null && GetMobileCount() > 0 )
					m_Owner.Say( message );
				
				ArrayList toExtort = new ArrayList();
				
				foreach ( Mobile m in GetMobiles() )
				{
					if ( !IsFighter( m ) && m.AccessLevel == AccessLevel.Player )
						toExtort.Add( m );
				}
				
				for ( int i = 0; i < toExtort.Count; i++ )
					NArenaRegion.Teleport( toExtort[ i ] as Mobile, m_Owner.ExtortPoint );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public void CleanArena( int message )
		{
			try
			{
				if ( GetMobileCount() > 0 )
					m_Owner.Say( message );
				
				ArrayList toExtort = new ArrayList();
				
				foreach ( Mobile m in GetMobiles() )
				{
					if ( !IsFighter( m ) && m.AccessLevel == AccessLevel.Player )
						toExtort.Add( m );
				}
				
				for ( int i = 0; i < toExtort.Count; i++ )
					NArenaRegion.Teleport( toExtort[ i ] as Mobile, m_Owner.ExtortPoint );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public static void Rejuvenate( Mobile m, bool full )
		{
			if ( m == null )
				return;
			
			if ( full )
			{
				m.Paralyzed = false;
				m.Poison = null;
				AnimalForm.RemoveContext( m, true );
				IncognitoSpell.StopTimer( m );
     			DisguiseTimers.StopTimer( m );
				CurseSpell.RemoveEffect( m );
				StrangleSpell.RemoveCurse( m );
				m.RemoveStatMod( String.Format( "[Magic] {0} Offset", StatType.Dex ) );
				m.RemoveStatMod( String.Format( "[Magic] {0} Offset", StatType.Int ) );
				m.RemoveStatMod( String.Format( "[Magic] {0} Offset", StatType.Str ) );
				m.Stam = m.StamMax;
				m.Mana = m.ManaMax;
			}
			
			m.Hits = m.HitsMax;
		}
		
		public static void RejuvenateEffect( Mobile m )
		{
			if ( m == null )
				return;
			
			m.FixedParticles( 0x3779, 5, 20, 5002, EffectLayer.Head );
			m.PlaySound( 490 );
		}
		
		public static void Teleport( Mobile m, Point3D loc )
		{
			if ( m == null )
				return;
			
			m.SetLocation( loc, true );
			
			#region Efekt
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z - 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z - 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 11), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 7), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 3), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3728, 13);
			m.PlaySound(0x228);
			#endregion
		}
		
		public static bool IsSummon( Mobile m )
		{
			return ( m is EnergyVortex || m is BladeSpirits  || m is Revenant );
		}
		
		public static bool IsControlledSummon( Mobile m )
		{
			return ( m is SummonedAirElemental || m is SummonedEarthElemental || m is SummonedFireElemental 
			        || m is SummonedWaterElemental || m is SummonedDaemon );
		}
		
		public static bool IsControlled( Mobile who )
		{
			BaseCreature bc = who as BaseCreature;
			
			return ( bc != null && bc.Controlled && bc.ControlMaster != null );
		}
		
		public static bool IsControlled( Mobile who, Mobile whom )
		{
			return IsControlled( who as BaseCreature, whom );
		}
		
		public static bool IsControlled( BaseCreature who, Mobile whom )
		{
			return ( who != null && who.Controlled && who.ControlMaster == whom );
		}
		
		public static bool IsSummoned( Mobile who )
		{
			BaseCreature bc = who as BaseCreature;
			
			return ( bc != null && bc.Summoned && bc.SummonMaster != null );
		}
		
		public static bool IsSummoned( Mobile who, Mobile whom )
		{
			return IsSummoned( who as BaseCreature, whom );
		}
		
		public static bool IsSummoned( BaseCreature who, Mobile whom )
		{
			return ( who != null && who.Summoned && who.SummonMaster == whom );
		}
		
		public static bool HasOwner( Mobile who )
		{
			return ( GetOwner( who as BaseCreature ) != null );
		}
		
		public static Mobile GetOwner( Mobile who )
		{
			return GetOwner( who as BaseCreature );
		}
		
		public static Mobile GetOwner( BaseCreature who )
		{
			if ( who == null )
				return null;
			
			return ( who.ControlMaster == null ) ? who.SummonMaster : who.ControlMaster;
		}
		
		public static void Unsummon( Mobile m )
		{
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 2100, 3, 5042, 0 );
			m.PlaySound( 0x201 );
			m.Delete();
		}
		
		public static bool CanSee( Mobile from, Mobile target )
		{
			// Console.WriteLine( "CanSee( {0}, {1} )", from.Name, target.Name );
			NArenaRegion fr = from.Region as NArenaRegion;
			bool fif = ( fr != null && fr.IsFighter( from ) );
						  
			if ( fif )
			{
				// Console.WriteLine( "{0} is fighter", from.Name );
				NArenaRegion tr = target.Region as NArenaRegion;
				bool tif = ( tr != null && tr.IsFighter( target ) );
				bool sameregion = ( fr == tr );
				
				if ( sameregion && tif )
				{
					// Console.WriteLine( "{0} is fighter too", target.Name );
					return true; 
				}
				else
				{
					// Console.WriteLine( "but {0} ins't", target.Name );
					return false;
				}
			}
			
			return true;
		}
		
		private void LookAgain( Mobile m )
		{
			try
			{
				if ( m == null || m.Map == null || m.NetState == null )
					return;
				
				IPooledEnumerable eable = m.Map.GetMobilesInRange( m.Location );
			
				foreach ( Mobile mob in eable )
				{
					if ( !m.CanSee( mob ) )
					{
						m.NetState.Send( mob.RemovePacket );
					}
					else
					{
						m.NetState.Send( new MobileIncoming( m, mob ) );
						m.NetState.Send( mob.OPLPacket );
					}
				}
	
				eable.Free();
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		#endregion
	}
}


	
