#region References

using System;
using System.Collections.Generic;
using Nelderim;
using Nelderim.Gains;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		// Gainy
		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double GainFactor
		{
			get => Gains.Get(this).GainFactor;
			set => Gains.Get(this).GainFactor = value;
		}
		
		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public DateTime GainBoostEndTime
		{
			get => Gains.Get(this).GainBoostEndTime;
			set => Gains.Get(this).GainBoostEndTime = value;
		}
		
		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public bool GainDebug
		{
			get => Gains.Get(this).GainDebug;
			set => Gains.Get(this).GainDebug = value;
		}
		
		public DateTime LastMacroCheck { get; set; }

		public bool GainsDebugEnabled { get; set; }

		// SUS
		private HashSet<Mobile> m_Seers;

		public void StartTracking(Mobile seer)
		{
			if (m_Seers == null)
				m_Seers = new HashSet<Mobile>();

			m_Seers.Add(seer);
		}

		public void StopTracking(Mobile seer)
		{
			if (m_Seers != null)
				m_Seers.Remove(seer);
		}

		// Karta postaci
		[CommandProperty(AccessLevel.Seer)]
		public int QuestPoints
		{
			get => CharacterSheet.Get(this).QuestPoints;
			set
			{
				CharacterSheet.Get(this).QuestPoints = value;
				LastQuestPointsTime = DateTime.Now;
			}
		}


		[CommandProperty(AccessLevel.Seer)]
		public DateTime LastQuestPointsTime
		{
			get => CharacterSheet.Get(this).LastQuestPointsTime;
			set => CharacterSheet.Get(this).LastQuestPointsTime = value;
		}

		// Languages
		[CommandProperty(AccessLevel.GameMaster)]
		public Language LanguageSpeaking
		{
			get => Languages.Get(this).LanguageSpeaking;
			set => Languages.Get(this).LanguageSpeaking = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public KnownLanguages LanguagesKnown
		{
			get => Languages.Get(this).LanguagesKnown;
			set => Languages.Get(this).LanguagesKnown = value;
		}

		// Grab

		public Container GrabContainer { get; set; }

		// Possess
		public Mobile m_PossessMob;
		public Mobile m_PossessStorageMob;

		// Nelderim disguise kit

		public Race RaceMod { get; set; } = null;

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
				Delta(MobileDelta.Hue);
			}
		}

		// Traps

		[CommandProperty(AccessLevel.GameMaster)]
		public int TrapsActive { get; set; } = 0;

		// Nowy detect hidden

		public Timer PassiveDetectHiddenTimer { get; set; }

		public DateTime NextPassiveDetectHiddenCheck { get; set; }

		//Hunters Bulki

		private DateTime m_NextHunterBulkOrder;

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan NextHunterBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextHunterBulkOrder - DateTime.Now;

				if (ts < TimeSpan.Zero)
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

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Noticed { set; get; }

		private RessurectProtectionTimer m_ResTimer;

		private class RessurectProtectionTimer : Timer
		{
			private readonly PlayerMobile m_Player;

			public RessurectProtectionTimer(PlayerMobile player) : base(TimeSpan.FromSeconds(5.0))
			{
				m_Player = player;
			}

			protected override void OnTick()
			{
				m_Player.Blessed = false;
			}
		}

		private void StartResurrectTimer()
		{
			Blessed = true;
			m_ResTimer = new RessurectProtectionTimer(this);
			m_ResTimer.Start();
		}

		private void StopResurrectTimer()
		{
			if (m_ResTimer != null && m_ResTimer.Running)
			{
				Blessed = false;
				m_ResTimer.Stop();
			}
		}

		public TimeSpan LongTermElapse => m_LongTermElapse;
	}
}
