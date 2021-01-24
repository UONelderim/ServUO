using System;
using Server.Accounting;
using Server.Mobiles;
using Server.Spells;
using Arya.Jail;
using Server.Nelderim;
using Server.Spells.Necromancy;
using Server.Spells.Eighth;
using System.Collections.Generic;
using System.Xml;

namespace Server.Regions
{
	public class NelderimRegion : BaseRegion
	{
		private static List<Mobile> m_FirstWarning;
		private static List<Mobile> m_SecondWarning;
		private static ViolationsTimer m_Timer;
		private bool m_Allowed;
		protected bool m_SeasonOverride;
		protected SeasonList m_Season;
		
		public bool SeasonOverride { get { return m_SeasonOverride; } }
		public SeasonList Season { get { return m_Season; } }
			
		public static void Initialize()
		{
			m_FirstWarning = new List<Mobile>();
			m_SecondWarning = new List<Mobile>();
			m_Timer = new ViolationsTimer();
			m_Timer.Start();
		}
		
        // 20.08.2012 :: zombie
		public NelderimRegion( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
            string allowedAttrName = "allowed";
            m_Allowed = xml.HasAttribute( allowedAttrName ) && XmlConvert.ToInt32( xml.GetAttribute( allowedAttrName ) ) == 0 ? false : true;
		}
        // 
			
		public static List<Mobile> FirstWarning
		{
			get
			{
				return m_FirstWarning;
			}
		}
		
		public static List<Mobile> SecondWarning
		{
			get
			{
				return m_SecondWarning;
			}
		}
			
		private static bool Violator( Mobile m )
		{
			return FirstWarning.Contains( m ) || SecondWarning.Contains( m );
		}
		
		private static void Disviolate( Mobile m )
		{
			FirstWarning.Remove( m );
			SecondWarning.Remove( m );
		}
		
		public override void OnEnter( Mobile m )
		{ 	
			#region Seasons
			//if ( m.Player )
			//{
			//	if ( !m_SeasonOverride )
			//		m.Send( new SeasonChange( Map.Season ) );
			//	else
			//	{
			//		m.Send( new SeasonChange( ( int ) Season ) );
			//	}
				
			//	m.CheckLightLevels( true );
			//}
			#endregion
			
			#region zakazane regiony
			
			if ( m.Player && m.AccessLevel == AccessLevel.Player )
			{
				bool violator = Violator( m );
				
				if ( !m_Allowed )
				{
					m.SendLocalizedMessage( 505616, "", 0x25 );
					if ( !violator ) FirstWarning.Add( m );
				}
				else if ( violator )
				{
					m.SendLocalizedMessage( 505617, "", 167 );
					
					Disviolate( m );
				}
			}
			
			#endregion
			
			#region Zakaz summonow
			
			try
			{
				// zakaz wprowadzania summonow i innych
				if ( m is BaseCreature )
				{
					BaseCreature bc = m as BaseCreature;
					
					if ( bc.Controlled || bc.Summoned )
					{
						if ( RegionsEngine.PetIsBanned( this.Name, bc ) )
						{
						   Mobile owner = bc.Summoned ? bc.SummonMaster : bc.ControlMaster;

                           if ( owner != null )
                           {
                               if ( owner is PlayerMobile && owner.AccessLevel == AccessLevel.Player && owner.Player && owner.Kills < 5 )
                               {
                                   owner.SendLocalizedMessage( 505618, "", 0x25 );
                                   new BannedPetTimer( bc ).Start();
                               }
                           }
						}
					}	
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( e.ToString() );
			}
			#endregion

			#region Zakaz magicznych transofmracji
			try
			{
				if ( m is PlayerMobile && m.AccessLevel == AccessLevel.Player )
				{
					TransformContext transformContext = TransformationSpellHelper.GetContext( m );
					if ( transformContext != null && RegionsEngine.CastIsBanned( this.Name, (Spell)transformContext.Spell ) && !(transformContext.Spell is VampiricEmbraceSpell))
					{

						m.Criminal = (m.Kills < 5) ? true : false;
						m.SendLocalizedMessage( 505619, "", 0x25 );
					}
				}
			}
			catch (Exception e )
			{
				Console.WriteLine( e );
			}
            #endregion
        }

        public override void OnExit( Mobile m )
		{ 
		}
		
		public override void OnSpellCast(Mobile m, ISpell s)
		{
			// Sprawdza czy dana szkola magii, lub zaklecie nie sa zakazane w regionie
			if ( m.AccessLevel == AccessLevel.Player && RegionsEngine.CastIsBanned( this.Name, s as Spell ) )
			{
				m.Criminal = ( m.Kills < 5 ) ? true : false;
				m.SendLocalizedMessage( 505619, "", 0x25 );
			}			
			else if ( m.AccessLevel == AccessLevel.Player 
			         && ( s is SummonFamiliarSpell || s is AirElementalSpell || s is EarthElementalSpell
			        	|| s is FireElementalSpell || s is SummonDaemonSpell || s is WaterElementalSpell ) )
			{
				if ( RegionsEngine.PetIsBanned( this.Name, s as Spell ) )
				{
					m.Criminal = ( m.Kills < 5 ) ? true : false;
					m.SendLocalizedMessage( 505620, "", 0x25 );
				}
			}
			
			base.OnSpellCast( m, s );
		}

		private class BannedPetTimer : Timer
		{		
			private BaseCreature m_Pet;
			
			public BannedPetTimer( BaseCreature bc ) : base( TimeSpan.FromSeconds( 30 ) )
			{
				m_Pet = bc;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				try
				{
					if ( !m_Pet.Deleted )
					{
						if ( m_Pet.Controlled || m_Pet.Summoned )
						{
							if ( RegionsEngine.PetIsBanned( m_Pet.Region.Name, m_Pet ) )
							{
								Mobile owner = m_Pet.Summoned ? m_Pet.SummonMaster : m_Pet.ControlMaster;
								    	
								if ( owner != null )
									if ( owner is PlayerMobile && owner.AccessLevel == AccessLevel.Player && owner.Player )
							    	{
										owner.Criminal = ( owner.Kills < 5 ) ? true : false;
							    		owner.SendLocalizedMessage( 505621, "", 0x25 );
							    	}
							}	
						}
					}
				}
				catch ( Exception e )
				{
					Console.WriteLine( e.ToString() );
				}
			}
		}
		
		private class ViolationsTimer : Timer
		{		
			public ViolationsTimer() : base( TimeSpan.FromMinutes( 0 ), TimeSpan.FromMinutes( 1 ) )
			{
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				for ( int i = 0; i < NelderimRegion.SecondWarning.Count; i++)
				{
					Mobile m = ( Mobile ) NelderimRegion.SecondWarning[i];
					
					if ( !( m.Region is NelderimRegion ) )
					{
						m.SendLocalizedMessage( 505617, "", 167 );
						continue;
					}
					
					Console.WriteLine( "{0} laduje w wiezieniu za naruszenie zakazanych regionow!", m.Name );
					
					if ( JailSystem.CanBeJailed( m ) )
					{
						m.SendLocalizedMessage( 505622, "", 0x25 );
						JailSystem.CommitJailing( m, m.Account as Account, m, "Wkroczenie na zakazany teren", true, 
						              TimeSpan.FromHours( 1 ), true, 
						              "Automatyczne wiezienie za wkroczenie na zakazany teren -> X: " + m.X.ToString() 
						              + " Y: " + m.Y.ToString(), JailSystem.m_Jail[ 0 ] );
						
					}
				}
				
				NelderimRegion.SecondWarning.Clear();
				
				for ( int i = 0; i < NelderimRegion.FirstWarning.Count; i++)
				{
					Mobile m = ( Mobile ) NelderimRegion.FirstWarning[i];
					
					if ( !( m.Region is NelderimRegion ) )
					{
						m.SendLocalizedMessage( 505617, "", 167 );
						continue;
					}
					
					m.SendLocalizedMessage( 505623, "", 0x25 );
					NelderimRegion.SecondWarning.Add( m );
				}
				
				NelderimRegion.FirstWarning.Clear();
			}
		}
	}
}
