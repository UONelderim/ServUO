using System;

using Server;
using Server.Gumps;

namespace Arya.Chess
{
	/// <summary>
	/// This gump is used to start the game
	/// </summary>
	public class StartGameGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;

		private ChessGame m_Game;
		private Mobile m_User;
		private bool m_IsOwner;
		private bool m_AllowSpectators;

		public StartGameGump( Mobile m, ChessGame game, bool isOwner, bool allowSpectators ) : base( 200, 200 )
		{
			m_Game = game;
			m_User = m;
			m_IsOwner = isOwner;
			m_AllowSpectators = allowSpectators;

			m_User.CloseGump( typeof( StartGameGump ) );

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			int height = 75;

			if ( m_IsOwner )
				height = 110;

			this.AddBackground(0, 0, 300, height, 9250);
			this.AddImageTiled(0, 0, 300, height, 9304);
			this.AddImageTiled(1, 1, 298, height - 2, 9274);
			this.AddAlphaRegion(1, 1, 298, height - 2);

			if ( m_IsOwner )
			{
				if ( m_Game.Guest == null )
					this.AddLabel(10, 5, GreenHue, @"Rozpoczecie nowej gry");
				else
					this.AddLabel(10, 5, GreenHue, @"Czekam, az Twoj partner do gry zaakceptuje wyzwanie");

				this.AddImageTiled(10, 25, 280, 1, 9304);

				// Bring again target : 1
				if ( m_Game.Guest == null )
				{
					this.AddButton(15, 30, 5601, 5605, 1, GumpButtonType.Reply, 0);
					this.AddLabel(35, 28, LabelHue, @"Wybierz przeciwnika...");
				}

				// Cancel : 0
				this.AddButton(15, 50, 5601, 5605, 2, GumpButtonType.Reply, 0);
				this.AddLabel(35, 48, LabelHue, @"Anuluj");

				int bid = m_AllowSpectators ? 2153 : 2151;
				this.AddButton( 10, 75, bid, bid, 3, GumpButtonType.Reply, 0 );
				this.AddLabel( 45, 80, LabelHue, "Czy pozwolic widzom ogladac co sie dzieje na szachownicy?" );
			}
			else
			{
				this.AddLabel(10, 5, GreenHue, string.Format( "Graj z {0}?", m_Game.Owner.Name ) );
				this.AddImageTiled(10, 25, 280, 1, 9304);

				// Accept : 1
				this.AddButton(15, 30, 5601, 5605, 1, GumpButtonType.Reply, 0);
				this.AddLabel(35, 28, LabelHue, @"Akceptuj");

				// Refuse : 0
				this.AddButton(15, 50, 5601, 5605, 2, GumpButtonType.Reply, 0);
				this.AddLabel(35, 48, LabelHue, @"Odmow");
			}
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( m_IsOwner )
			{
				if ( info.ButtonID == 3 )
				{
					// Switch the allow spectators flag
					m_Game.AllowSpectators = !m_AllowSpectators;
					sender.Mobile.SendGump( new StartGameGump( sender.Mobile, m_Game, m_IsOwner, !m_AllowSpectators ) );
				}
				else if ( info.ButtonID == 2 )
				{
					m_Game.CancelGameStart( sender.Mobile );
				}
				else if ( info.ButtonID == 1 )
				{
					sender.Mobile.Target = new ChessTarget( m_Game, sender.Mobile, "Z kim chcesz zagrac?",
						new ChessTargetCallback( m_Game.ChooseOpponent ) );
					
					sender.Mobile.SendGump( new StartGameGump( sender.Mobile, m_Game, m_IsOwner, m_AllowSpectators ) );
				}
			}
			else
			{
				if ( info.ButtonID == 2 )
				{
					m_Game.CancelGameStart( sender.Mobile );
				}
				else if ( info.ButtonID == 1 )
				{
					m_Game.AcceptGame( sender.Mobile );
				}
			}
		}
	}
}