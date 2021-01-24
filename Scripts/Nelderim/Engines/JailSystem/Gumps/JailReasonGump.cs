// 05.06.22 :: troyan :: naprawa bledu z crashem serwera przy [jail account

using System;
using Server;
using Server.Gumps;
using Server.Accounting;

namespace Arya.Jail
{
	/// <summary>
	/// First gump in the jailing sequence
	/// </summary>
	public class JailReasonGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int RedHue = 0x20;
		private const int GreenHue = 0x40;

		/// <summary>
		/// The player being jailed
		/// </summary>
		private Mobile m_Offender;

		/// <summary>
		/// The account of the player being jailed
		/// </summary>
		private Account m_Account;

		/// <summary>
		/// Defines the 5 common reasons supported by the gump
		/// </summary>
		
		private int m_JailChoice;
		private string m_Text;
		
		
		public JailReasonGump( Mobile offender , int jailint ) : this ( offender , jailint , null )
		{
		}
		
		public JailReasonGump( Mobile offender, int jailint , string text ) : base( 100, 100 )
		{
			m_Text = text;
			m_Offender = offender;
			m_JailChoice = jailint;
			if ( m_Offender.Account != null )
			{
				m_Account = offender.Account as Account;
			}

			MakeGump();
		}
		public JailReasonGump( Account account , int jailint ) : this( account , jailint , null )
		{
		}
		
		public JailReasonGump( Account account , int jailint , string text ) : base( 100, 100 )
		{
			m_Offender = null;
			m_Text = text;
			m_JailChoice = jailint;
			m_Account = account;

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 290, 380, 9250);
			this.AddAlphaRegion(15, 15, 260, 350);
			this.AddLabel(170, 20, RedHue, @"Ustawienia");
			
			// Account name
			this.AddLabel( 30, 40, LabelHue, "Konto:" );

			if ( m_Account != null )
			{
				this.AddLabel( 100, 40, GreenHue, m_Account.Username );
			}
			else
			{
				this.AddLabel( 100, 40, GreenHue, "N/A" );
			}

			// Player name
			this.AddLabel(30, 60, LabelHue, "Gracz:" );

			if ( m_Offender != null )
			{
				this.AddLabel(100, 60, GreenHue, String.Format( "{0} ( {1} )",m_Offender.Name , m_Offender.Race ) );
			}
			else
			{
				this.AddLabel( 100, 60, GreenHue, "Wszystkie postacie" );
			}

			this.AddLabel(30, 85, RedHue, @"Wybierz wiezienie:");

			for ( int i = 0; i < JailSystem.m_Jail.Length && i < 5 ; i++ )
			{
				AddLabel( 70, 110 + i * 30, LabelHue, JailSystem.m_Jail[i].Name );
				if( ( i + 1 ) == m_JailChoice)
				{
					AddButton( 30, 110 + i * 30, 4006, 4007, i + 1, GumpButtonType.Reply, 0 );
				}
				else
				{
					AddButton( 30, 110 + i * 30, 4005, 4007, i + 1, GumpButtonType.Reply, 0 );
				}
			}

			this.AddLabel(30, 260, RedHue, @"Przewinienie:");
			this.AddImageTiled( 34, 279, 232, 22, 9384 );
			this.AddAlphaRegion(35, 280, 230, 20);
			this.AddTextEntry(35, 280, 230, 20, LabelHue, 0, m_Text == null ? "" : m_Text );
			

			this.AddLabel(70, 320, LabelHue, @"Wsadz do wiezienia");		
			this.AddButton(30, 320, 4008, 4010, 6, GumpButtonType.Reply, 0); // Cancel
			
			this.AddLabel(70, 340, LabelHue, @"Anuluj");	
			this.AddButton(30, 340, 4002, 4003, 0, GumpButtonType.Reply, 0); // Cancel
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			try
			{
			
				string reason = null;
				switch ( info.ButtonID )
				{
					case 0: // Cancel the jailing
	
						if ( m_Offender != null )
						{
							JailSystem.CancelJail( m_Offender, sender.Mobile );
						}
						else if ( m_Account != null )
						{
							sender.Mobile.SendMessage( "Anulowales uwiezienie postaci z konta {0}.", m_Account.Username );
						}
	
						return;
	
					case 1: 
					case 2:
					case 3:
					case 4:
					case 5:
						{
							if ( m_Offender != null )
								sender.Mobile.SendGump( new JailReasonGump( m_Offender, info.ButtonID , info.TextEntries[ 0 ].Text ) );
							else 
								sender.Mobile.SendGump( new JailReasonGump( m_Account, info.ButtonID , info.TextEntries[ 0 ].Text ) );
							break;
						}
					case 6: // Other, check for text
	
						if ( info.TextEntries[ 0 ].Text != null && info.TextEntries[ 0 ].Text.Length > 0 && m_JailChoice != 0 )
						{
							reason = info.TextEntries[ 0 ].Text;	
							JailType jail = JailSystem.m_Jail[	m_JailChoice -1 ];
									
							sender.Mobile.SendGump( new JailingGump( m_Offender, m_Account, reason , jail ) );
						}
						else
						{
							sender.Mobile.SendMessage( "Musisz wybrac wiezienie i uzupelnic pole przewinienie!" );
	
							if ( m_Offender != null )
							{
								sender.Mobile.SendGump( new JailReasonGump( m_Offender , m_JailChoice , info.TextEntries[ 0 ].Text ) );
							}
							else
							{
								sender.Mobile.SendGump( new JailReasonGump( m_Account , m_JailChoice , info.TextEntries[ 0 ].Text ) );
							}
						}
						break;
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
				sender.Mobile.SendMessage("Blad systemu wieziennictwa!");
			}
		}
	}
}
