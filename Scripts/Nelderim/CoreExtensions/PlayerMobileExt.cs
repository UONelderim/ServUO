using Nelderim;
using Nelderim.Gains;
using Server.Commands;
using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public partial class PlayerMobile
    {
        // Gainy
        private DateTime m_LastMacroCheck;
        public DateTime LastMacroCheck
        {
            get => m_LastMacroCheck; 
            set => m_LastMacroCheck = value; 
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public DateTime LastPowerHour
        {
            get => Gains.Get( this ).LastPowerHour; 
            set => Gains.Get( this ).LastPowerHour = value; 
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool AllowPowerHour
        {
            get => !HasPowerHour && (DateTime.Now.DayOfYear > LastPowerHour.DayOfYear || DateTime.Now.Year != LastPowerHour.Year); 
        }

        [CommandProperty( AccessLevel.Counselor )]
        public bool HasPowerHour
        {
            get => DateTime.Now - LastPowerHour <= PowerHour.Duration; 
        }

        private bool m_GainsDebugEnabled;

        public bool GainsDebugEnabled
        {
            get { return m_GainsDebugEnabled; }
            set { m_GainsDebugEnabled = value; }
        }

        // SUS
        private HashSet<Mobile> m_Seers;

        public void StartTracking( Mobile seer )
        {
            if ( m_Seers == null )
                m_Seers = new HashSet<Mobile>();

            m_Seers.Add( seer );
        }

        public void StopTracking( Mobile seer )
        {
            if ( m_Seers != null )
                m_Seers.Remove( seer );
        }

        // Karta postaci
        [CommandProperty( AccessLevel.Seer )]
        public int QuestPoints
        {
            get => CharacterSheet.Get( this ).QuestPoints; 
            set
            {
                CharacterSheet.Get( this ).QuestPoints = value;
                LastQuestPointsTime = DateTime.Now;
            }
        }


        [CommandProperty( AccessLevel.Seer )]
        public DateTime LastQuestPointsTime
        {
            get => CharacterSheet.Get( this ).LastQuestPointsTime; 
            set => CharacterSheet.Get( this ).LastQuestPointsTime = value; 
        }

        // Languages
        [CommandProperty( AccessLevel.GameMaster )]
        public Language LanguageSpeaking
        {
            get => Languages.Get( this ).LanguageSpeaking; 
            set => Languages.Get( this ).LanguageSpeaking = value; 
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public KnownLanguages LanguagesKnown
        {
            get => Languages.Get( this ).LanguagesKnown; 
            set => Languages.Get( this ).LanguagesKnown = value; 
        }

        // Grab
        private Container m_GrabContainer;
        public Container GrabContainer
        {
            get { return m_GrabContainer; }
            set { m_GrabContainer = value; }
        }

        // Possess
        public Mobile m_PossessMob;
        public Mobile m_PossessStorageMob;

        // Nelderim disguise kit
        private Race m_RaceMod = null;
        public Race RaceMod
        {
            get { return m_RaceMod; }
            set { m_RaceMod = value; }
        }

        private int m_HueDisguise = -1;
        public int HueDisguise
        {
            get
            {
                return m_HueDisguise;
            }
            set
            {
                m_HueDisguise = value;
                Delta( MobileDelta.Hue );
            }
        }

        // Traps
        private int m_TrapsActive = 0;

        [CommandProperty( AccessLevel.GameMaster )]
        public int TrapsActive
        {
            get { return m_TrapsActive; }
            set { m_TrapsActive = value; }
        }

        // Nowy detect hidden
        private DateTime m_NextPassiveDetectHiddenCheck;
        private Timer m_PassiveDetectHiddenTimer;

        public Timer PassiveDetectHiddenTimer
        {
            get { return m_PassiveDetectHiddenTimer; }
            set { m_PassiveDetectHiddenTimer = value; }
        }

        public DateTime NextPassiveDetectHiddenCheck
        {
            get { return m_NextPassiveDetectHiddenCheck; }
            set { m_NextPassiveDetectHiddenCheck = value; }
        }

		//Hunters Bulki

		private DateTime m_NextHunterBulkOrder;

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan NextHunterBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextHunterBulkOrder - DateTime.Now;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try { m_NextHunterBulkOrder = DateTime.Now + value; }
				catch { }
			}
		}
		
		// Nietolerancja
		private bool m_IntolerateAction;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Noticed
		{
			set { m_IntolerateAction = value; }
			get { return m_IntolerateAction; }
		}
	}
}
