using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;


namespace Server.Items
{
	public class RewardScroll : Item
	{
		private double m_Value;
		private int m_Class;
		private int m_Repeat;
		private int m_TotalGold;
		private Dictionary<Item, int> m_Given;

		private static List<NReward> m_Rewards;

		private static void BuildRewardsList()
		{
			m_Rewards = new List<NReward>();

			m_Rewards.Add(new PowerScrollReward(5, 4000));
			m_Rewards.Add(new PowerScrollReward(10, 30000));
			m_Rewards.Add(new PowerScrollReward(15, 240000));
			m_Rewards.Add(new PowerScrollReward(20, 960000));
			m_Rewards.Add(new PowderOfTranslocationReward(50000));
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
			BuildRewardsList();
		}

		public RewardScroll(int rewardClass) : this(rewardClass, 0)
		{
		}

		public RewardScroll(int rewardClass, int repeat) : base(0x14F0)
		{
			Weight = 0.1;

			LootType = LootType.Blessed;
			m_Class = Utility.Clamp(rewardClass, 1, 16);

			m_Value = (int)(250 * Math.Pow(2, 16 - m_Class));
			m_Given = new Dictionary<Item, int>();
			if (repeat == 0)
			{
				if (m_Class >= 1 && m_Class <= 9)
					m_Repeat = 6;
				else
					m_Repeat = 1;
			}
			else
				m_Repeat = repeat;

			base.Hue = m_Class <= 2 ? 1935 : m_Class <= 6 ? 0x8a5 : m_Class <= 11 ? 2401 : 0x972;
		}

		public RewardScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Repeat);
			writer.Write((int)m_Class);
			writer.Write((int)m_Value);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					m_Repeat = reader.ReadInt();
					m_Class = reader.ReadInt();
					m_Value = reader.ReadInt();
					break;
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public double Value => m_Value;

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public int Class
		{
			get => m_Class;
			set
			{
				m_Class = Utility.Clamp(value, 1, 16);
				m_Value = (int)(250 * Math.Pow(2, 16 - m_Class));
				Hue = m_Class == 1 ? 1935 : m_Class <= 6 ? 0x8a5 : m_Class <= 11 ? 2401 : 0x972;
				Delta(ItemDelta.Properties);
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public int Repeat => m_Repeat;

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(505595, m_Class.ToString()); // zwoj nagrody klasy ~1_VALUE~
			list.Add(505903, m_Repeat.ToString()); // Mozliwe jest ~1_VALUE~ losowac nagrod.
		}

		public override void OnSingleClick(Mobile from)
		{
			LabelTo(from, 505595, m_Class.ToString()); // // zwoj nagrody o wartosci ~1_VALUE~ centarow
		}

		public void Use(Mobile m, bool firstStage)
		{
			var from = m;

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
					var log = from.AccessLevel + " " + CommandLogging.Format(from) + " used Reward Scroll " +
					          CommandLogging.Format(this) + " of value " + m_Value + " [RewardScroll]";
					CommandLogging.WriteLine(from, log);
					CommandLogging.WriteLine(from, "[RewardScroll] LabelOfCreator = " + LabelOfCreator);

					if (m_Given == null)
						m_Given = new Dictionary<Item, int>();
					Generate(from);

					if (m_TotalGold > 0)
					{
						Item money;
						if (m_TotalGold < 5000)
							money = new Gold(m_TotalGold);
						else
							money = new BankCheck(m_TotalGold);
						money.LabelOfCreator = (string)CommandLogging.Format(from);

						m_Given.Add(money, m_TotalGold);
						from.Backpack.DropItem(money);
						m_TotalGold = 0;
					}


					from.SendLocalizedMessage(505596); // Nagroda zmaterializwoala sie w plecaku.

					from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
					from.PlaySound(0x1E0);

					if (m_Repeat > 1)
						from.SendGump(new InternalRepeatGump(this, m_Given));
					else
						Delete();
				}
			}
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public override void OnDoubleClick(Mobile from)
		{
			Use(from, true);
		}

		private void Generate(Mobile from)
		{
			var TotalRewardValue = 0;
			var InternalValue = m_Value;

			var totalValue = 0;
			var rewards = new List<NReward>();

			// generujemy liste dostepnych prezentow, znaczy takich, ktorych wartosc nie przekracza dostepnego zlota
			for (var i = 0; i < m_Rewards.Count && m_Rewards[i].Value <= m_Value; i++)
			{
				rewards.Add(m_Rewards[i]);
				totalValue += m_Rewards[i].Value;
			}

			while (m_Value > 0)
			{
				// losujemy nagrode
				var rnd = Utility.Random(totalValue);
				NReward reward = null;

				for (var i = 0;
				     i < rewards.Count && rnd >= 0 && (rewards[i]).Value <= InternalValue - TotalRewardValue;
				     i++)
				{
					reward = rewards[i];
					rnd -= reward.Value;
				}

				// sprawdzamy, czy ta nagroda wpada do koszyka
				// jesli tak, to przydzielamy
				// jak nie to dalej

				if (reward != null && Utility.RandomDouble() < 50.0 / reward.Value)
				{
					TotalRewardValue += reward.Value;

					var rewardItem = reward.Generate(from);

					rewardItem.LabelOfCreator = LabelOfCreator;
					rewardItem.Label3 = "generated from " + CommandLogging.Format(this);
					m_Given.Add(rewardItem, rewardItem.Amount);
					from.Backpack.DropItem(rewardItem);
				}


				m_Value -= 50;
			}

			// nowa korekcja wartosci		
			if (TotalRewardValue > InternalValue)
			{
				var diff = (int)(TotalRewardValue - InternalValue);
				TotalRewardValue -= m_TotalGold;
				m_TotalGold -= diff;

				if (m_TotalGold < 0)
					m_TotalGold = 0;

				TotalRewardValue += m_TotalGold;
			}
			else if (TotalRewardValue < InternalValue)
			{
				var diff = (int)(InternalValue - TotalRewardValue);
				TotalRewardValue -= m_TotalGold;
				m_TotalGold += diff;
				TotalRewardValue += m_TotalGold;
			}
		}

		public class InternalGump : Gump
		{
			private Mobile m_Mobile;
			private RewardScroll m_Scroll;

			public InternalGump(Mobile mobile, RewardScroll scroll) : base(25, 50)
			{
				m_Mobile = mobile;
				m_Scroll = scroll;

				AddPage(0);

				AddBackground(25, 10, 420, 200, 5054);

				AddImageTiled(33, 20, 401, 181, 2624);
				AddAlphaRegion(33, 20, 401, 181);


				if (m_Scroll.Repeat > 1)
				{
					AddHtmlLocalized(40,
							48,
							387,
							100,
							505904,
							true,
							true); /* <B>Zwój nagrody tej klasy umo¿liwia wielokrotne losowanie nagród.</B><BR><BR>
							        * Nim zaakceptujesz wylosowany zestaw nagród obejrzyj je. Odrzucajac to losowanie,
							        * masz mozliwosc ponownego uzycia zwoju. Przeniesienie jakiejkolwiek z nagrod poza
							        * plecak, lub uzycie jej, spowoduje brak mozliwoœci ponownego losowania. */
				}
				else
				{
					AddHtmlLocalized(40,
							48,
							387,
							100,
							505597,
							true,
							true); /* Uzycie zwoju spowoduje materializacje w plecaku nagrody o wartoœci zblizonej
							        * do wartosci zapisanej na zwoju.<BR><BR>
							        * Nagroda ta z pewnoscia <B>bedzie ciezsza i zajmie w plecaku wiecej miejsca</B>
							        * niz sam zwój, zatem wykorzystaj go w miejscu, gdzie mo¿esz zlozyc uzyskane nagrody.<BR><BR>
							        * Zwoj jest poblogoslawiony, na okaziciela i bezterminowy, zatem nie ma poœpiechu.*/
				}

				AddHtmlLocalized(125, 148, 200, 20, 1049478, 0xFFFFFF, false, false); // Do you wish to use this scroll?

				AddButton(100, 172, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtmlLocalized(135, 172, 120, 20, 1046362, 0xFFFFFF, false, false); // Yes

				AddButton(275, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtmlLocalized(310, 172, 120, 20, 1046363, 0xFFFFFF, false, false); // No

				AddHtml(40,
					20,
					260,
					20,
					String.Format("<basefont color=#FFFFFF>Zwój nagrody klasy {0}</basefont>", m_Scroll.Class),
					false,
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
			private RewardScroll m_Scroll;
			private int m_Class;
			private int m_Repeat;
			private Dictionary<Item, int> m_Given;

			public InternalRepeatGump(RewardScroll scroll, Dictionary<Item, int> items) : base(25, 50)
			{
				m_Scroll = scroll;
				m_Class = m_Scroll.Class;
				m_Repeat = m_Scroll.Repeat;
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

				AddHtml(40,
					20,
					260,
					20,
					String.Format("<basefont color=#FFFFFF>Zwój nagrody klasy {0}</basefont>", m_Class),
					false,
					false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				var from = state.Mobile;

				if (info.ButtonID == 0)
				{
					if (ConsumeAllRewards(m_Given, from))
					{
						var rs = new RewardScroll(m_Class, m_Repeat - 1);
						from.Backpack.DropItem(rs);
						from.PlaySound(0x1FF);
					}
					else
						from.SendLocalizedMessage(505906);
				}
				else
					from.SendLocalizedMessage(505907);
			}

			private bool ConsumeAllRewards(Dictionary<Item, int> rewards, Mobile from)
			{
				if (rewards.All(kvp =>
				    {
					    var found = from.Backpack.Items.Find(item => item.Equals(kvp.Key));
					    return found switch
					    {
						    BankCheck check => kvp.Value == check.Worth,
						    Item item => kvp.Value == item.Amount,
						    _ => false
					    };
				    }))
				{
					foreach (var keyValuePair in rewards) keyValuePair.Key.Delete();

					return true;
				}

				return false;
			}
		}
	}
}
