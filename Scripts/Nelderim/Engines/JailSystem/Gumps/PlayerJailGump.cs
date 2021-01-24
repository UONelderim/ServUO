// 05.06.27 :: troyan :: zabezpieczenie przed crashem
// 06.01.05 :: troyan :: wyrzucenie polskich znakow

using System;

using Server;
using Server.Gumps;

namespace Arya.Jail
{
	/// <summary>
	/// This gump is sent to the player with the details of their sentence
	/// </summary>
	public class PlayerJailGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;

		private JailEntry m_Jail;
		

		public PlayerJailGump( JailEntry jail ) : base( 100, 100 )
		{
			m_Jail = jail;
			if( jail == null )
				return;
			MakeGump();
		}

		private void MakeGump()
		{
			try
			{
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;
	
				this.AddPage(0);
				this.AddBackground(0, 0, 430, 155, 9250);
	
				this.AddImageTiled(15, 15, 400, 20, 5154);
				this.AddAlphaRegion(15, 15, 400, 20);
				this.AddLabel(20, 15, LabelHue, @"Zostales uwieziony przez:");
				this.AddLabelCropped( 180, 15, 235, 20, RedHue, string.Format( "{0}", m_Jail.JailedBy.Name ) );
	
				this.AddAlphaRegion(15, 35, 400, 20);
				this.AddLabel(20, 35, LabelHue, @"Przewinienie:");
				this.AddLabelCropped(100, 35, 315, 20, GreenHue, m_Jail.Reason);
	
				this.AddImageTiled(15, 55, 400, 20, 5154);
				this.AddAlphaRegion(15, 55, 400, 20);
				this.AddLabel(20, 55, LabelHue, @"Czas trwania:");
	
				if ( m_Jail.AutoRelease )
				{
					this.AddLabel(120, 55, GreenHue, string.Format( "{0} {1} i {2} godzin{3}", m_Jail.Duration.Days, m_Jail.Duration.Days == 1 ? "dzien" : "dni " , m_Jail.Duration.Hours , m_Jail.Duration.Hours <= 1 ? "a" : "y"  ) );
				}
				else
				{
					this.AddLabel(120, 55, GreenHue, "Nieokreslony" );
				}
	
				this.AddAlphaRegion(15, 75, 400, 40);
				if ( m_Jail.AutoRelease )
				{
					TimeSpan left = m_Jail.EndTime - DateTime.Now;
					if ( left > TimeSpan.Zero )
					{
						
						AddLabel( 20, 75, LabelHue, String.Format("Do konca kary pozostalo {0} {1}, {2} godzin{3} i {4} minut{5}.", left.Days, left.Days == 1 ? "dzien" : "dni" , left.Hours,left.Hours <= 1 ? "a" : "y", left.Minutes , left.Minutes == 1 ? "a" : "y" ) );
					}
					else
					{
						AddLabel( 20, 75, LabelHue, @"Kara minela. Zaraz zostaniesz wypuszczony." );
					}
				}
				this.AddButton(15, 120, 4017, 4018, 0, GumpButtonType.Reply, 0);
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
	}
}
