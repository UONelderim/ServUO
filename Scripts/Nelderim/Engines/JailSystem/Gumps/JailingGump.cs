// 05.07.07 :: troyan :: zabezpieczenie przed crashem

using System;

using Server;
using Server.Gumps;
using Server.Accounting;

namespace Arya.Jail
{
	/// <summary>
	/// This is the gump finalizing the jailing
	/// </summary>
	public class JailingGump : Gump
	{
		private const int LabelHue = 0x480;

		/// <summary>
		/// The account of the player being jailed
		/// </summary>
		private Account m_Account;
		/// <summary>
		/// The mobile being jailed
		/// </summary>
		private Mobile m_Offender;
		/// <summary>
		/// The reason of the jailing
		/// </summary>
		private string m_Reason;
		/// <summary>
		/// The additional comment
		/// </summary>
		private string m_Comment;
		/// <summary>
		/// AutoRelease check
		/// </summary>
		private bool m_AutoRelease;
		/// <summary>
		/// Jail the full account
		/// </summary>
		private bool m_FullJail;
		/// <summary>
		/// Days of the sentence
		/// </summary>
		private int m_Days;
		/// <summary>
		/// Hours of the sentence
		/// </summary>
		private int m_Hours;
		
		private JailType m_Jail;

		public JailingGump( Mobile offender, Account account, string reason , JailType jail ) : this( offender, account, reason, jail ,  true, 1, 0, false, null )
		{
		}

		public JailingGump( Mobile offender, Account account, string reason, JailType jail,  bool autorelease, int days, int hours, bool fulljail, string comment ) : base( 100, 100 )
		{
			m_Offender = offender;
			m_Account = account;
			m_Reason = reason;
			m_AutoRelease = autorelease;
			m_Days = days;
			m_Hours = hours;
			m_FullJail = fulljail;
			m_Comment = comment;
			m_Jail = jail;

			if ( m_Offender == null ) // Force full jail if jailing an account
			{
				m_FullJail = true;
			}

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);

			this.AddBackground(0, 0, 419, 240, 9250);
			this.AddAlphaRegion(15, 15, 390, 210);

			// Auto Release: check 0
			this.AddCheck(20, 15, 9721, 9724, m_AutoRelease, 0 );
			this.AddLabel(50, 20, LabelHue, @"Autouwolnienie w:");

			// Days: text 0
			this.AddLabel(230, 20, LabelHue, @"Dni");
			this.AddImageTiled(159, 19, 62, 22, 5154);
			this.AddAlphaRegion(160, 20, 60, 20);
			this.AddTextEntry(175, 20, 30, 20, LabelHue, 0, m_Days.ToString() );

			// Hours: text 1
			this.AddLabel(340, 20, LabelHue, @"Godziny");
			this.AddImageTiled(269, 19, 62, 22, 5154);
			this.AddAlphaRegion(270, 20, 60, 20);
			this.AddTextEntry(285, 20, 30, 20, LabelHue, 1, m_Hours.ToString() );

			// Full Jail: check 1
			if ( m_Offender != null )
			{
				this.AddCheck(20, 45, 9721, 9724, m_FullJail, 1);
			}
			else
			{
				this.AddImage( 20, 45, 9724, 0 );
			}

			this.AddLabel(50, 50, LabelHue, @"Wsadz wszystkie postacie na koncie do wiezienia");

			// Comments: text 2
			this.AddLabel(25, 80, LabelHue, @"Dodaj komentarz dotyczacy tego zamkniecia");
			this.AddImageTiled(24, 99, 372, 92, 5154);
			this.AddAlphaRegion(25, 100, 370, 90);
			this.AddTextEntry(25, 100, 370, 90, LabelHue, 2, m_Comment != null ? m_Comment : "" );

			// Cancel: button 0
			this.AddButton(30, 200, 4002, 4003, 0, GumpButtonType.Reply, 0);
			this.AddLabel(70, 200, LabelHue, @"Anuluj");

			// Commit: button 1
			this.AddButton(240, 200, 4023, 4024, 1, GumpButtonType.Reply, 0);
			this.AddLabel(280, 200, LabelHue, @"Wsadz do Wiezienia");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			try
			{
				// Calculate days and hours first
				try
				{
					if ( info.TextEntries[ 0 ].Text != null )
					{
						m_Days = (int) uint.Parse( info.TextEntries[ 0 ].Text );
					}
				}
				catch  {}
	
				try
				{
					if ( info.TextEntries[ 1 ].Text != null )
					{
						m_Hours = (int) uint.Parse( info.TextEntries[ 1 ].Text );
					}
				}
				catch {}
	
				// Comment
				if ( info.TextEntries[ 2 ].Text != null )
				{
					m_Comment = info.TextEntries[ 2 ].Text;
				}
	
				m_AutoRelease = false;
				m_FullJail = false;
	
				// Switches
				foreach( int check in info.Switches )
				{
					switch( check )
					{
						case 0 : // AutoRelease
							m_AutoRelease = true;
							break;
						case 1: // FullJail
							m_FullJail = true;
							break;
					}
				}
	
				if ( m_Offender == null )
				{
					m_FullJail = true;
				}
	
				switch ( info.ButtonID )
				{
					case 0: // Cancel jailing
	
						JailSystem.CancelJail( m_Offender, sender.Mobile );
						sender.Mobile.SendMessage( "You have canceled the jailing of {0} and brought them to your location", m_Offender.Name );
						return;
	
					case 1: // Commit jailing
	
						TimeSpan duration = new TimeSpan( m_Days, m_Hours, 0, 0, 0 );
	
						if ( m_AutoRelease && duration == TimeSpan.Zero  )
						{
							sender.Mobile.SendMessage( "Zaznaczajac opcje autowypuszczenie musisz podac prawidlowy czas trawania kary!" );
							sender.Mobile.SendGump( new JailingGump( m_Offender, m_Account, m_Reason, m_Jail , m_AutoRelease, m_Days, m_Hours, m_FullJail, m_Comment ) );
							return;
						}
						if( m_Comment == null || m_Comment.Length == 0 )
						{
							sender.Mobile.SendMessage( "Musisz dodac komentarz do tego zamkniecia , czyli za co dokladnie i szczegolowy opis!" );
							sender.Mobile.SendGump( new JailingGump( m_Offender, m_Account, m_Reason, m_Jail , m_AutoRelease, m_Days, m_Hours, m_FullJail, m_Comment ) );
							return;
						}
						
	
						JailSystem.CommitJailing( m_Offender, m_Account, sender.Mobile, m_Reason, m_AutoRelease, duration, m_FullJail, m_Comment , m_Jail );
						break;
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				sender.Mobile.SendMessage( "Wystapil niespodziewany blad systemu!" );
			}
		}
	}
}
