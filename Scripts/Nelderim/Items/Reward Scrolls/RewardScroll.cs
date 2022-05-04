#region References

using System;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class RewardScroll : Item
	{
		private int m_Class;
		private int m_TotalGold;

		private static List<NReward> m_Rewards;

		private static void BuildRewardsList()
		{
			m_Rewards = new List<NReward>();

			m_Rewards.Add(new PowerScrollReward(5, 4000));
			m_Rewards.Add(new PowerScrollReward(10, 30000));
			m_Rewards.Add(new PowerScrollReward(15, 240000));
			m_Rewards.Add(new PowerScrollReward(20, 960000));
			m_Rewards.Add(new PowderOfTranslocationReward(50000));
			//m_Rewards.Add(new SilverReward(1000));
			m_Rewards.Add(new BallOfSummoningReward(10000));
			m_Rewards.Add(new PowderOfTemperamentReward(20000));
			m_Rewards.Add(new DedicatedPowerScrollReward(5, 10000));
			m_Rewards.Add(new DedicatedPowerScrollReward(10, 80000));
			m_Rewards.Add(new DedicatedPowerScrollReward(15, 640000));
			m_Rewards.Add(new DedicatedPowerScrollReward(20, 2560000));
			m_Rewards.Add(new MinorArtifactScrollReward(false, 150000));
			m_Rewards.Add(new MinorArtifactScrollReward(true, 400000));
			m_Rewards.Add(new MajorArtifactScrollReward(false, 1500000));
			m_Rewards.Add(new MajorArtifactScrollReward(true, 4000000));

			m_Rewards.Sort();
		}

		public static void Initialize()
		{
			CommandSystem.Register("Reward", AccessLevel.Counselor, Reward_OnCommand);
			BuildRewardsList();
		}

		[Usage("Reward [klasa 1-16")]
		[Description("Nagradza cel scrollem nagrody o wartosci odpowiedniej do klasy.")]
		private static void Reward_OnCommand(CommandEventArgs arg)
		{
			if (arg.Length < 1)
			{
				arg.Mobile.SendMessage("Reward [klasa 1-16]");
				return;
			}

			int sc = Utility.Clamp(arg.GetInt32(0), 1, 16);
			sc = arg.Mobile.AccessLevel == AccessLevel.Administrator ? sc :
				arg.Mobile.AccessLevel == AccessLevel.Seer && sc < 3 ? 3 :
				arg.Mobile.AccessLevel == AccessLevel.GameMaster && sc < 7 ? 7 :
				arg.Mobile.AccessLevel == AccessLevel.Counselor && sc < 12 ? 12 : sc;

			arg.Mobile.Target = new InternalTarget(sc);
		}

		[Constructable]
		public RewardScroll() : this(0)
		{
		}

		public RewardScroll(int rewardClass) : this(rewardClass, 0)
		{
		}

		public RewardScroll(int rewardClass, int repeat) : base(0x14F0)
		{
			base.Weight = 0.1;

			LootType = LootType.Blessed;
			m_Class = Utility.Clamp(rewardClass, 1, 16);

			Value = (int)(250 * Math.Pow(2, 16 - m_Class));
			GivenRewards = new ArrayList();
			if (repeat == 0)
			{
				if (m_Class >= 1 && m_Class <= 9)
					Repeat = 6;
				else
					Repeat = 1;
			}
			else
				Repeat = repeat;

			base.Hue = m_Class <= 2 ? 2139 : m_Class <= 6 ? 2213 : m_Class <= 11 ? 2401 : 2418;
		}

		public RewardScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(Repeat);
			writer.Write(m_Class);
			writer.Write((int)Value);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					Repeat = reader.ReadInt();
					m_Class = reader.ReadInt();
					Value = reader.ReadInt();
					break;
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public double Value { get; private set; }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public int Class
		{
			get
			{
				return m_Class;
			}
			set
			{
				m_Class = Utility.Clamp(value, 1, 16);
				Value = (int)(250 * Math.Pow(2, 16 - m_Class));
				Hue = m_Class == 1 ? 1935 : m_Class <= 6 ? 0x8a5 : m_Class <= 11 ? 2401 : 0x972;
				Delta(ItemDelta.Properties);
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public int Repeat { get; private set; }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public ArrayList GivenRewards { get; private set; }

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(505595, m_Class.ToString()); // zwoj nagrody klasy ~1_VALUE~
			list.Add(505903, Repeat.ToString()); // Mozliwe jest ~1_VALUE~ losowac nagrod.
		}

		public override void OnAosSingleClick(Mobile from)
		{
			base.LabelTo(from, 505595, m_Class.ToString()); // // zwoj nagrody o wartosci ~1_VALUE~ centarow
		}

		public void Use(Mobile m, bool firstStage)
		{
			Mobile from = m;

			if (Deleted || !(from is PlayerMobile) || !from.Alive || from == null)
				return;

			if (IsChildOf(from.Backpack))
			{
				if (firstStage)
				{
					from.CloseGump(typeof(InternalGump));
					from.SendGump(new InternalGump(from, this));
				}
				else
				{
					string log = from.AccessLevel + " " + CommandLogging.Format(from) + " used Reward Scroll " +
					             CommandLogging.Format(this) + " of value " + Value + " [RewardScroll]";
					CommandLogging.WriteLine(from, log);
					CommandLogging.WriteLine(from, "[RewardScroll] LabelOfCreator = " + ModifiedBy);

					Generate(from);

					while (m_TotalGold > 0)
					{
						Item money;

						if (m_TotalGold < 5000)
						{
							money = new Gold(m_TotalGold);
							m_TotalGold = 0;
						}
						else if (m_TotalGold < 500000)
						{
							money = new BankCheck(m_TotalGold);
							m_TotalGold = 0;
						}
						else
						{
							money = new BankCheck(500000);
							m_TotalGold -= 500000;
						}

						money.ModifiedBy = (string)CommandLogging.Format(from);
						if (GivenRewards == null)
							GivenRewards = new ArrayList();
						GivenRewards.Add(money);
						from.Backpack.DropItem(money);
					}


					from.SendLocalizedMessage(505596); // Nagroda zmaterializwoala sie w plecaku.

					from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
					from.PlaySound(0x1E0);

					if (Repeat > 1)
					{
						from.SendGump(new InternalRepeatGump(this, GivenRewards));
					}
					else
						Delete();
				}
			}
			else
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			Use(from, true);
		}

		private void Generate(Mobile from)
		{
			int TotalRewardValue = 0;
			double InternalValue = Value;

			int totalValue = 0;
			ArrayList rewards = new ArrayList();

			// generujemy liste dostepnych prezentow, znaczy takich, ktorych wartosc nie przekracza dostepnego zlota
			for (int i = 0; i < m_Rewards.Count && m_Rewards[i].Value <= Value; i++)
			{
				rewards.Add(m_Rewards[i]);
				totalValue += m_Rewards[i].Value;
			}

			while (Value > 0)
			{
				// losujemy nagrode
				int rnd = Utility.Random(totalValue);
				NReward reward = null;

				for (int i = 0;
				     i < rewards.Count && rnd >= 0 &&
				     (rewards[i] as NReward).Value <= (InternalValue - TotalRewardValue);
				     i++)
				{
					reward = rewards[i] as NReward;
					rnd -= reward.Value;
				}

				// sprawdzamy, czy ta nagroda wpada do koszyka
				// jesli tak, to przydzielamy
				// jak nie to dalej

				if (reward != null && Utility.RandomDouble() < 50.0 / reward.Value)
				{
					TotalRewardValue += reward.Value;

					Item rewardItem = reward.Generate(from);

					rewardItem.ModifiedBy = this.ModifiedBy;
					rewardItem.Label3 = "generated from " + CommandLogging.Format(this);
					if (GivenRewards == null)
						GivenRewards = new ArrayList();
					GivenRewards.Add(rewardItem);
					from.Backpack.DropItem(rewardItem);
				}


				Value -= 50;
			}

			// nowa korekcja wartosci		
			if (TotalRewardValue > InternalValue)
			{
				int diff = (int)(TotalRewardValue - InternalValue);
				TotalRewardValue -= m_TotalGold;
				m_TotalGold -= diff;

				if (m_TotalGold < 0)
					m_TotalGold = 0;

				TotalRewardValue += m_TotalGold;
			}
			else if (TotalRewardValue < InternalValue)
			{
				int diff = (int)(InternalValue - TotalRewardValue);
				TotalRewardValue -= m_TotalGold;
				m_TotalGold += diff;
				TotalRewardValue += m_TotalGold;
			}
		}

		public class InternalGump : Gump
		{
			private readonly Mobile m_Mobile;
			private readonly RewardScroll m_Scroll;

			public InternalGump(Mobile mobile, RewardScroll scroll) : base(25, 50)
			{
				m_Mobile = mobile;
				m_Scroll = scroll;

				AddPage(0);

				AddBackground(25, 10, 420, 200, 5054);

				AddImageTiled(33, 20, 401, 181, 2624);
				AddAlphaRegion(33, 20, 401, 181);


				if (m_Scroll.Repeat > 1)
					AddHtmlLocalized(40, 48, 387, 100, 505904, true,
						true); /* <B>Zwój nagrody tej klasy umo¿liwia wielokrotne losowanie nagród.</B><BR><BR>
                                                                     * Nim zaakceptujesz wylosowany zestaw nagród obejrzyj je. Odrzucajac to losowanie,
                                                                     * masz mozliwosc ponownego uzycia zwoju. Przeniesienie jakiejkolwiek z nagrod poza
                                                                     * plecak, lub uzycie jej, spowoduje brak mozliwoœci ponownego losowania. */
				else
					AddHtmlLocalized(40, 48, 387, 100, 505597, true,
						true); /* Uzycie zwoju spowoduje materializacje w plecaku nagrody o wartoœci zblizonej 
				                                                             * do wartosci zapisanej na zwoju.<BR><BR>
				                                                             * Nagroda ta z pewnoscia <B>bedzie ciezsza i zajmie w plecaku wiecej miejsca</B> 
				                                                             * niz sam zwój, zatem wykorzystaj go w miejscu, gdzie mo¿esz zlozyc uzyskane nagrody.<BR><BR>
				                                                             * Zwoj jest poblogoslawiony, na okaziciela i bezterminowy, zatem nie ma poœpiechu.*/

				AddHtmlLocalized(125, 148, 200, 20, 1049478, 0xFFFFFF, false, false); // Do you wish to use this scroll?

				AddButton(100, 172, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtmlLocalized(135, 172, 120, 20, 1046362, 0xFFFFFF, false, false); // Yes

				AddButton(275, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtmlLocalized(310, 172, 120, 20, 1046363, 0xFFFFFF, false, false); // No

				AddHtml(40, 20, 260, 20,
					String.Format("<basefont color=#FFFFFF>Zwój nagrody klasy {0}</basefont>", m_Scroll.Class), false,
					false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				if (info.ButtonID == 1)
					m_Scroll.Use(m_Mobile, false);
			}
		}

		public class InternalRepeatGump : Gump
		{
			private readonly RewardScroll m_Scroll;
			private readonly int m_Class;
			private readonly int m_Repeat;
			private readonly ArrayList m_Given;

			public InternalRepeatGump(RewardScroll scroll, ArrayList items) : base(25, 50)
			{
				m_Scroll = scroll;
				m_Class = m_Scroll.Class;
				m_Repeat = m_Scroll.Repeat;
				m_Given = new ArrayList();
				m_Given = items;
				m_Scroll.Delete();

				AddPage(0);

				AddBackground(25, 10, 420, 200, 5054);

				AddImageTiled(33, 20, 401, 181, 2624);
				AddAlphaRegion(33, 20, 401, 181);


				AddHtmlLocalized(40, 48, 387, 100, 505905, true, true); // Czy akceptujesz wylosowany zestaw nagród?

				AddButton(100, 172, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtmlLocalized(135, 172, 120, 20, 1046362, 0xFFFFFF, false, false); // Yes

				AddButton(275, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtmlLocalized(310, 172, 120, 20, 1046363, 0xFFFFFF, false, false); // No

				AddHtml(40, 20, 260, 20,
					String.Format("<basefont color=#FFFFFF>Zwój nagrody klasy {0}</basefont>", m_Class), false, false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				Mobile from = state.Mobile;

				if (info.ButtonID == 0)
				{
					if (ConsumeAllRewards(m_Given, from))
					{
						RewardScroll rs = new RewardScroll(m_Class, m_Repeat - 1);
						from.Backpack.DropItem(rs);
						from.PlaySound(0x1FF);
					}
					else
						from.SendLocalizedMessage(505906);
				}
				else
					from.SendLocalizedMessage(505907);
			}

			private bool ConsumeAllRewards(ArrayList Rewards, Mobile from)
			{
				ArrayList items = new ArrayList();
				ArrayList m_rewards = new ArrayList();
				m_rewards = Rewards;

				foreach (Item item in from.Backpack.Items)
				{
					foreach (Item rew in m_rewards)
					{
						if (item == rew)
						{
							items.Add(item);
						}
					}
				}

				if (m_rewards.Count == items.Count)
				{
					foreach (Item item in items)
						item.Delete();
					return true;
				}

				return false;
			}
		}


		public class InternalTarget : Target
		{
			readonly int m_Class;

			public InternalTarget(int sc) : base(-1, false, TargetFlags.None)
			{
				m_Class = sc;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is PlayerMobile)
				{
					PlayerMobile pm = targeted as PlayerMobile;
					RewardScroll rs = new RewardScroll(m_Class);

					rs.ModifiedBy = (string)CommandLogging.Format(from);

					if (pm != null && pm.Alive && pm.Backpack != null && pm.Backpack.TryDropItem(from, rs, false))
					{
						from.SendLocalizedMessage(505599);
						pm.SendLocalizedMessage(505600);

						string log = from.AccessLevel + " " + CommandLogging.Format(from);
						log += " gave RewardScroll of class [" + m_Class + "] to " + CommandLogging.Format(targeted);
						log += " [RewardScroll]";
						CommandLogging.WriteLine(from, log);
						CommandLogging.WriteLine(pm, log);
					}
					else
						from.SendLocalizedMessage(505601);
				}
			}
		}
	}
}
