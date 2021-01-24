using System;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Gumps
{
	public class BaseListGump : Gump
	{
		protected int m_iLoc;
		protected static int iButtonShift = 1000000;
		public ArrayList m_alList;

		public virtual string GumpName{ get{ return ""; } }
		public virtual Version VerNum{ get{ return new Version(1,0,0); } }
		public virtual int BackGroundID{ get{ return 9200; } }
		public virtual int AmountToGet{ get{ return 20; } }

		public virtual bool UseButton1{ get{ return false; } }
		public virtual int Button1NormalID{ get{ return 3; } }
		public virtual int Button1PressedID{ get{ return 4; } }

		public virtual bool UseButton2{ get{ return false; } }
		public virtual int Button2NormalID{ get{ return 9702; } }
		public virtual int Button2PressedID{ get{ return 9703; } }

		public virtual bool UseFilter{ get{ return false; } }

		public string m_sFilter;

		public BaseListGump( ArrayList alList, int loc, string sFilter ) : base( 50, 50 )
		{
			m_iLoc = loc;
			m_alList = alList;
			m_sFilter = sFilter;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			if( m_iLoc >= m_alList.Count )
				m_iLoc = GetLastPageLoc();
		}

		public virtual void Init()
		{
			this.AddPage(0);
			this.AddBackground(30, 51, 518, 529, BackGroundID);
			this.AddBackground(74, 34, 425, 36, 9200);

			this.AddHtml( 81, 43, 409, 19, string.Format("<center>{0} V{1}</center>", GumpName, VerNum.ToString()), (bool)false, (bool)false);
			this.AddHtml( 111, 87, 413, 409, PopulateStringList(m_iLoc), (bool)true, (bool)false);

			this.AddButton(495, 537, 4017, 4018, (int)Buttons.Close, GumpButtonType.Reply, 0);
			this.AddImage(50, 501, 9012);

			if(UseFilter)
			{
				this.AddBackground(227, 543, 216, 28, 9350);
				this.AddLabel(230, 525, 0, @"Filter List");
				this.AddButton(444, 547, 4023, 4024, (int)Buttons.Find, GumpButtonType.Reply, 0);
				this.AddTextEntry(235, 547, 201, 20, 0, (int)Buttons.FilterEntry, m_sFilter);
			}

			CreateButtons( m_alList.Count );
		}

		public virtual void CreateButtons(int iTotalEnteries)
		{
			int iAmountofButtons = iTotalEnteries - m_iLoc;
			if(iAmountofButtons > AmountToGet)
				iAmountofButtons = AmountToGet;

			for(int i=0;i<iAmountofButtons;i++)
			{
				if(UseButton1)
					this.AddButton(64, 91+( 18 * i ), Button1NormalID, Button1PressedID, (i+m_iLoc)+(iButtonShift*2), GumpButtonType.Reply, 0);
				if(UseButton2)
					this.AddButton(88, 91+( 18 * i ), Button2NormalID, Button2PressedID, (i+m_iLoc)+iButtonShift, GumpButtonType.Reply, 0);
			}

			if( (m_iLoc + AmountToGet) < iTotalEnteries)
			{
				this.AddButton(458, 500, 5601, 5605, (int)Buttons.Next, GumpButtonType.Reply, 0);
				this.AddButton(478, 500, 9702, 9703, (int)Buttons.End, GumpButtonType.Reply, 0);
			}

			if(m_iLoc - AmountToGet >= 0)
			{
				this.AddButton(439, 500, 5603, 5607, (int)Buttons.Prev, GumpButtonType.Reply, 0);
				this.AddButton(419, 500, 9706, 9707, (int)Buttons.Beginning, GumpButtonType.Reply, 0);
			}
		}

		public string ShortenText( string str, int maxLenght )
		{
			if( str == null )
				return "";

			if( str.Length > maxLenght )
				return str.Substring(0, maxLenght-3)+"...";
			else return str;
		}

		public virtual string OnPopulateStringList(object obj, int loc)
		{
			return "";
		}

		private int GetLastPageLoc()
		{
			return ( (m_alList.Count - 1) /  AmountToGet) * AmountToGet;
		}

		protected bool CheckArrayAtLoc( int loc )
		{
			if( loc < m_alList.Count )
			{
				if( m_alList[loc] is Item && ( ((Item)m_alList[loc]) == null || ((Item)m_alList[loc]).Deleted ) )
				{
					m_alList.RemoveAt(loc);
					return false;
				}
				else if( m_alList[loc] is Mobile && ( ((Mobile)m_alList[loc]) == null || ((Mobile)m_alList[loc]).Deleted ) )
				{
					m_alList.RemoveAt(loc);
					return false;
				}
				else if( m_alList[loc] == null )
				{
					m_alList.RemoveAt(loc);
					return false;
				}
			}
			else return false;

			return true;
		}

		private void CheckArray()
		{
			for(int i=(m_alList.Count-1);i>=0;i--)
				CheckArrayAtLoc(i);
		}

		private string PopulateStringList(int loc)
		{
			string list = "";
			int iCounter = loc;

			// Check for null or deleted objects in the array
			CheckArray();

			for(; iCounter < m_alList.Count && iCounter-loc < AmountToGet ; iCounter++)
				list += OnPopulateStringList( m_alList[iCounter], iCounter );

			list += string.Format("<BR>{0}-{1} of {2}", loc, iCounter, m_alList.Count);
			return list;
		}

		public enum Buttons
		{
			Close,
			Next,
			Prev,
			End,
			Beginning,
			Find,
			FilterEntry,
		}

		public virtual void OnRequestGump(Mobile from, ArrayList alList, int loc)
		{
		}

		public virtual void OnButton1Click( Mobile from, int pos )
		{
		}

		public virtual void OnFilterButtonClick( Mobile from )
		{
		}

		public virtual void OnButton2Click( Mobile from, int pos )
		{
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			switch ( info.ButtonID )
			{
				case (int)Buttons.Find:
					m_sFilter = ((TextRelay)info.GetTextEntry( (int)Buttons.FilterEntry )).Text;
					this.OnFilterButtonClick( state.Mobile );
					return;
				case (int)Buttons.Close:
					return;
				case (int)Buttons.Next:
					OnRequestGump(state.Mobile, m_alList, m_iLoc + AmountToGet);
					return;
				case (int)Buttons.Prev:
					OnRequestGump(state.Mobile, m_alList, m_iLoc - AmountToGet);
					return;
				case (int)Buttons.Beginning:
					OnRequestGump(state.Mobile, m_alList, 0);
					return;
				case (int)Buttons.End:
					OnRequestGump(state.Mobile, m_alList, GetLastPageLoc());
					return;
				default:
					int iButton = info.ButtonID-iButtonShift;
					if( iButton >= iButtonShift )
						this.OnButton1Click( state.Mobile, iButton-iButtonShift );
					else if(iButton < iButtonShift)
						this.OnButton2Click( state.Mobile, iButton );
					break;
			}

			OnRequestGump(state.Mobile, m_alList, m_iLoc);
		}
	}
}