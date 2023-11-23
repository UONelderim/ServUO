#region References

using System;
using System.Collections.Generic;
using Nelderim;
using Nelderim.Gains;
using Server.Accounting;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.SkillHandlers;

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
			get
			{
				Account acct = Account as Account;

				if (acct != null)
				{
					return acct.QuestPoints;
				}

				return 0;
			}
			set
			{
				Account acct = Account as Account;

				if (acct != null)
				{
					acct.QuestPoints = value;
				}
			}
		}


		[CommandProperty(AccessLevel.Seer)]
		public DateTime LastQuestPointsTime
		{
			get
			{
				Account acct = Account as Account;

				if (acct != null)
				{
					return acct.LastQuestPointsTime;
				}

				return DateTime.MinValue;
			}
			set
			{
				Account acct = Account as Account;

				if (acct != null)
				{
					acct.LastQuestPointsTime = value;
				}
			}
		}

		public SortedSet<QuestPointsHistoryEntry> QuestPointsHistory
		{
			get
			{
				Account acct = Account as Account;

				if (acct != null)
				{
					return acct.QuestPointsHistory;
				}

				return new SortedSet<QuestPointsHistoryEntry>();
			}
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

		[CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextHunterBulkOrder
        {
            get
            {
                return BulkOrderSystem.GetNextBulkOrder(BODType.Hunter, this);
            }
            set
            {
                BulkOrderSystem.SetNextBulkOrder(BODType.Hunter, this, value);
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

		public override bool UseLanguages => true;
		
		
		
		public int NGetInsuranceCost(Item item)
		{
			int imbueWeight = Imbuing.GetTotalWeight(item, -1, false, false);
			int cost = 600; // this handles old items, set items, etc

			if (item is IVvVItem vItem && vItem.IsVvVItem)
				cost = 800;
			else if (imbueWeight > 0)
				cost = Math.Min(800, Math.Max(10, imbueWeight)) * 6;
			else if (GenericBuyInfo.BuyPrices.ContainsKey(item.GetType()))
				cost = Math.Min(800, Math.Max(10, GenericBuyInfo.BuyPrices[item.GetType()]));
			else if (item.LootType == LootType.Newbied)
				cost = 10;

			NegativeAttributes negAttrs = RunicReforging.GetNegativeAttributes(item);

			if (negAttrs != null && negAttrs.Prized > 0)
				cost *= 2;

			if (Region != null)
				cost = (int)(cost * Region.InsuranceMultiplier);

			return cost;
		}
	}
}
