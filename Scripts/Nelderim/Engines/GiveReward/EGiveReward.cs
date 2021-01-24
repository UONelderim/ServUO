//******************************************************
// Name: EGiveReward
// Desc: Written by Eclipse
//******************************************************
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Accounting;
using Server.Commands;

namespace Server.Gumps
{
	public class GiveRewardGump : BaseListGump
	{
		public static void Initialize()
		{
			CommandSystem.Register( "EGiveReward", AccessLevel.Administrator, new CommandEventHandler( EGiveReward_OnCommand ) );
		}

		[Usage( "EGiveReward" )]
		[Description( "" )]
		private static void EGiveReward_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new GiveRewardGump() );
		}

		public class RewardEntry
		{
			public Type m_type;
			public int m_amount;

			public RewardEntry(Type type, int amount)
			{
				m_type = type;
				m_amount = amount;
			}
		}

		public override string GumpName{ get{ return "Give Reward"; } }
		public override Version VerNum{ get{ return new Version(1,0,2); } }
		public override bool UseButton1{ get{ return true; } }
		private bool m_bUseRandom;
		private int m_iRandomNum = 1;
		private int m_iRandomRangeMax = 7;

		public GiveRewardGump() : this( new ArrayList(), 0, false, 0, 0 )
		{
		}

		public GiveRewardGump(ArrayList alList, int loc, bool bUseRandom, int iRandomNum, int iRandomRangeMax) : base(alList, loc, "")
		{
			m_bUseRandom = bUseRandom;
			m_iRandomNum = iRandomNum;
			m_iRandomRangeMax = iRandomRangeMax;
			Init();
		}

		public override void Init()
		{
			base.Init();
			this.AddLabel(130, 526, 0, @"Add Reward");
			this.AddLabel(347, 526, 0, @"Amount");
			this.AddBackground(346, 545, 50, 28, 9350);
			this.AddBackground(127, 545, 216, 28, 9350);
			this.AddTextEntry(132, 549, 200, 20, 0, 11, @"");
			this.AddTextEntry(350, 549, 40, 20, 0, 13, @"");
			this.AddButton(399, 551, 4023, 4024, 10, GumpButtonType.Reply, 0);

			this.AddBackground(549, 235, 221, 346, 9200);

			this.AddLabel(612, 260, 0, @"Give out rewards");
			this.AddButton(574, 260, 4008, 4009, 12, GumpButtonType.Reply, 0);

			this.AddLabel(612, 290, 0, @"Add To Your Backpack");
			this.AddButton(574, 290, 4008, 4009, 14, GumpButtonType.Reply, 0);

			this.AddLabel(612, 320, 0, @"Give Random Item");
			this.AddCheck(574, 320, 210, 211, m_bUseRandom, 15);

			this.AddLabel(574, 350, 0, @"Amount   Range 1-");

			this.AddBackground(574, 370, 48, 28, 9350);
			this.AddTextEntry(579, 374, 35, 20, 0, 16, m_iRandomNum.ToString());

			this.AddBackground(634, 370, 48, 28, 9350);
			this.AddTextEntry(637, 374, 35, 20, 0, 17, m_iRandomRangeMax.ToString());

		}

		public override string OnPopulateStringList(object obj, int loc)
		{
			if(obj is RewardEntry)
				return string.Format( "Type: {0} Amount: {1}<BR>", ((RewardEntry)obj).m_type.Name, ((RewardEntry)obj).m_amount.ToString() );
			return "";
		}

		private void GiveRewards()
		{
			foreach ( Account a in Accounts.GetAccounts() )
			{
				for (int i=0 ;i<5 ;i++ )
				{
					if( a[i] != null && a[i].BankBox != null)
					{
						a[i].BankBox.DropItem( CreateRewardContainer() );
						break;
					}
				}
			}
		}

		private Container CreateRewardContainer()
		{
			Container rewardCont = new GiftBox();

			if(m_bUseRandom)
			{
				for(int i=0;i<m_iRandomNum;i++)
				{
					int iArrayLoc = m_iRandomRangeMax;
					if(m_iRandomRangeMax > m_alList.Count)
						m_iRandomRangeMax = m_alList.Count;
					AddRewardToContainer( rewardCont, (RewardEntry) m_alList[Utility.Random(m_iRandomRangeMax)] );
				}

				for(int i=m_iRandomRangeMax;i<m_alList.Count;i++)
					AddRewardToContainer( rewardCont, (RewardEntry) m_alList[i] );
			}
			else
			{
				foreach( RewardEntry entry in m_alList )
					AddRewardToContainer( rewardCont, entry );
			}
			return rewardCont;
		}

		private void AddRewardToContainer( Container cont, RewardEntry entry )
		{
			object obj = Activator.CreateInstance( entry.m_type );
			if(obj != null && obj is Item && cont != null)
			{
				if( ((Item)obj).Stackable && entry.m_amount > 1 )
					((Item)obj).Amount = entry.m_amount;

				cont.DropItem( ((Item)obj) );
			}
		}

		public override void OnButton1Click( Mobile from, int pos )
		{
			m_alList.RemoveAt(pos);
		}

		public override void OnRequestGump(Mobile from, ArrayList alList, int loc)
		{
			from.SendGump(new GiveRewardGump(m_alList, loc, m_bUseRandom, m_iRandomNum, m_iRandomRangeMax));
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			m_bUseRandom = info.IsSwitched( 15 );

			try
			{
				m_iRandomNum = int.Parse( ((TextRelay)info.GetTextEntry( 16 )).Text );
			}
			catch
			{
				m_iRandomNum = 0;
			}

			try
			{
				m_iRandomRangeMax = int.Parse( ((TextRelay)info.GetTextEntry( 17 )).Text );
			}
			catch
			{
				m_iRandomRangeMax = 0;
			}


			switch ( info.ButtonID )
			{
				case 14:
					if(state.Mobile.Backpack != null)
					{
						state.Mobile.SendMessage("Adding reward to your backpack...");
						state.Mobile.Backpack.DropItem( CreateRewardContainer() );
						state.Mobile.SendMessage("Done...");
					}
					break;
				case 12:
					state.Mobile.SendMessage("Giving out rewards...");
					GiveRewards();
					state.Mobile.SendMessage("Done...");
					break;
				case 10:
					Type RewardType = ScriptCompiler.FindTypeByName( ((TextRelay)info.GetTextEntry( 11 )).Text );

					int RewardAmount = 1;

					try
					{
						RewardAmount = int.Parse( ((TextRelay)info.GetTextEntry( 13 )).Text );
					}
					catch{}

					if(RewardType != null && RewardType.IsSubclassOf( typeof(Item) ) )
						m_alList.Add(new RewardEntry(RewardType,RewardAmount));
					else
						state.Mobile.SendMessage("That is not an Item.");
					break;
				default:
					base.OnResponse( state, info );
					return;
			}

			OnRequestGump(state.Mobile, m_alList, m_iLoc);
		}
	}
}