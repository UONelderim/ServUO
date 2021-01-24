// Emote Gump - Ver: 1.5            02/05/2004 
// 
// Scripted by Zire for the Playground shard
//

using System;
using Server;
using Server.Gumps;
using Server.Commands;

namespace Emote.Gumps
{
    public enum EmotePage
    {
        P1,
        P2,
        P3,
        P4,
    }

    public class EmoteGump : Gump
    {
        private static string[] m_Strings = new string[]
		{
			"Aha", "Tryumf", "Oklaski", "Wydm. nosa", "Uklon", "Kaszlniecie", "Bekniecie", "Chrzakniecie", "Kaszel", 
			"Placz", "Omdlenie", "Pierdniecie", "Zaskoczenie", "Chichot", "Obrzydzenie", "Warkniecie", "Hej", 
			"Czkawka", "Niedowiary", "Pocalunek", "Smiech", "Protest", "Zdziwienie", "Zniechecenie", "Ups", 
			"Wymiotowanie", "Uderzenie", "Wrzask", "Uciszanie", "Ulga", "Spoliczkowanie", "Kichniecie", "Katar", 
			"Chrapanie", "Spluwanie", "Jezyk", "Tupanie", "Gwizdanie", "Nawolywanie", "Ziewanie", "Zadowolenie", 
			"Okrzyk"
		};

        public static void Initialize()
        {
            CommandSystem.Register( "emocje", AccessLevel.Player, new CommandEventHandler( Msg_OnCommand ) );
            CommandSystem.Register( "e", AccessLevel.Player, new CommandEventHandler( Msg_OnCommand ) );
        }

        [Usage( "Emocje [opcja]" )]
        [Description( "Emocje" )]
        private static void Msg_OnCommand( CommandEventArgs e )
        {
            if (e.Length >= 1)
            {
                for (int i = 0; i < m_Strings.Length; i++)
                {
                    if (m_Strings[i].ToLower() == e.ArgString.ToLower())
                    {
                        Emote( e.Mobile, i );
                        return;
                    }
                }
            }
            else
                e.Mobile.SendGump( new EmoteGump( e.Mobile, EmotePage.P1 ) );
        }

        private Mobile m_From;
        private EmotePage m_Page;

        private const int Blanco = 0xFFFFFF;
        private const int Azul = 0x8080FF;

        public void AddPageButton( int x, int y, int buttonID, string text, EmotePage page, params EmotePage[] subpage )
        {
            bool seleccionado = (m_Page == page);

            for (int i = 0; !seleccionado && i < subpage.Length; ++i)
                seleccionado = (m_Page == subpage[i]);

            AddButton( x, y - 1, seleccionado ? 4006 : 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
            AddHtml( x + 35, y, 200, 20, Color( text, seleccionado ? Azul : Blanco ), false, false );
        }

        public void AddButtonLabeled( int x, int y, int buttonID, string text )
        {
            AddButton( x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
            AddHtml( x + 35, y, 240, 20, Color( text, Blanco ), false, false );
        }

        public int GetButtonID( int type, int index )
        {
            return 1 + (index * 15) + type;
        }

        public string Color( string text, int color )
        {
            return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
        }

        public EmoteGump( Mobile from, EmotePage page )
            : base( 600, 50 )
        {
            from.CloseGump( typeof( EmoteGump ) );
            m_From = from;
            m_Page = page;
            Closable = true;
            Dragable = true;


            AddPage( 0 );
            AddBackground( 0, 65, 130, 360, 5054 );
            AddAlphaRegion( 10, 70, 110, 350 );
            AddImageTiled( 10, 70, 110, 20, 9354 );
            AddLabel( 13, 70, 200, "Lista emocji" );
            AddImage( 100, 0, 10410 );
            AddImage( 100, 305, 10412 );
            AddImage( 100, 150, 10411 );

            switch (page)
            {

                case EmotePage.P1:
                {
                    AddButtonLabeled( 10, 90, GetButtonID( 1, 1 ), "Aha" );
                    AddButtonLabeled( 10, 115, GetButtonID( 1, 2 ), "Tryumf" );
                    AddButtonLabeled( 10, 140, GetButtonID( 1, 3 ), "Oklaski" );
                    AddButtonLabeled( 10, 165, GetButtonID( 1, 4 ), "Wydm. nosa" );
                    AddButtonLabeled( 10, 190, GetButtonID( 1, 5 ), "Uklon" );
                    AddButtonLabeled( 10, 215, GetButtonID( 1, 6 ), "Kaszlniecie" );
                    AddButtonLabeled( 10, 240, GetButtonID( 1, 7 ), "Bekniecie" );
                    AddButtonLabeled( 10, 265, GetButtonID( 1, 8 ), "Chrzakniecie" );
                    AddButtonLabeled( 10, 290, GetButtonID( 1, 9 ), "Kaszel" );
                    AddButtonLabeled( 10, 315, GetButtonID( 1, 10 ), "Placz" );
                    AddButtonLabeled( 10, 340, GetButtonID( 1, 11 ), "Omdlenie" );
                    AddButtonLabeled( 10, 365, GetButtonID( 1, 12 ), "Pierdniecie" );
                    AddButton( 70, 380, 4502, 0504, GetButtonID( 0, 2 ), GumpButtonType.Reply, 0 );
                }
                break;

                case EmotePage.P2:
                {
                    AddButtonLabeled( 10, 90, GetButtonID( 1, 13 ), "Zaskoczenie" );
                    AddButtonLabeled( 10, 115, GetButtonID( 1, 14 ), "Chichot" );
                    AddButtonLabeled( 10, 140, GetButtonID( 1, 15 ), "Obrzydzenie" );
                    AddButtonLabeled( 10, 165, GetButtonID( 1, 16 ), "Warkniecie" );
                    AddButtonLabeled( 10, 190, GetButtonID( 1, 17 ), "Hej" );
                    AddButtonLabeled( 10, 215, GetButtonID( 1, 18 ), "Czkawka" );
                    AddButtonLabeled( 10, 240, GetButtonID( 1, 19 ), "Niedowiary" );
                    AddButtonLabeled( 10, 265, GetButtonID( 1, 20 ), "Pocalunek" );
                    AddButtonLabeled( 10, 290, GetButtonID( 1, 21 ), "Smiech" );
                    AddButtonLabeled( 10, 315, GetButtonID( 1, 22 ), "Protest" );
                    AddButtonLabeled( 10, 340, GetButtonID( 1, 23 ), "Zdziwienie" );
                    AddButtonLabeled( 10, 365, GetButtonID( 1, 24 ), "Zniechecenie" );
                    AddButton( 10, 380, 4506, 4508, GetButtonID( 0, 1 ), GumpButtonType.Reply, 0 );
                    AddButton( 70, 380, 4502, 0504, GetButtonID( 0, 3 ), GumpButtonType.Reply, 0 );
                    break;
                }

                case EmotePage.P3:
                {
                    AddButtonLabeled( 10, 90, GetButtonID( 1, 25 ), "Ups" );
                    AddButtonLabeled( 10, 115, GetButtonID( 1, 26 ), "Wymiotowanie" );
                    AddButtonLabeled( 10, 140, GetButtonID( 1, 27 ), "Uderzenie" );
                    AddButtonLabeled( 10, 165, GetButtonID( 1, 28 ), "Wrzask" );
                    AddButtonLabeled( 10, 190, GetButtonID( 1, 29 ), "Uciszanie" );
                    AddButtonLabeled( 10, 215, GetButtonID( 1, 30 ), "Ulga" );
                    AddButtonLabeled( 10, 240, GetButtonID( 1, 31 ), "Spoliczkowanie" );
                    AddButtonLabeled( 10, 265, GetButtonID( 1, 32 ), "Kichniecie" );
                    AddButtonLabeled( 10, 290, GetButtonID( 1, 33 ), "Katar" );
                    AddButtonLabeled( 10, 315, GetButtonID( 1, 34 ), "Chrapanie" );
                    AddButtonLabeled( 10, 340, GetButtonID( 1, 35 ), "Spluwanie" );
                    AddButtonLabeled( 10, 365, GetButtonID( 1, 36 ), "Jezyk" );
                    AddButton( 10, 380, 4506, 4508, GetButtonID( 0, 2 ), GumpButtonType.Reply, 0 );
                    AddButton( 70, 380, 4502, 0504, GetButtonID( 0, 4 ), GumpButtonType.Reply, 0 );
                    break;
                }

                case EmotePage.P4:
                {
                    AddButtonLabeled( 10, 90, GetButtonID( 1, 37 ), "Tupanie" );
                    AddButtonLabeled( 10, 115, GetButtonID( 1, 38 ), "Gwizdanie" );
                    AddButtonLabeled( 10, 140, GetButtonID( 1, 39 ), "Nawolywanie" );
                    AddButtonLabeled( 10, 165, GetButtonID( 1, 40 ), "Ziewanie" );
                    AddButtonLabeled( 10, 190, GetButtonID( 1, 41 ), "Zadowolenie" );
                    AddButtonLabeled( 10, 215, GetButtonID( 1, 42 ), "Okrzyk" );
                    AddButton( 10, 380, 4506, 4508, GetButtonID( 0, 3 ), GumpButtonType.Reply, 0 );
                    break;
                }
            }
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            int val = info.ButtonID - 1;

            if (val < 0)
                return;

            Mobile from = m_From;

            int type = val % 15;
            int index = val / 15;

            switch (type)
            {
                case 0:
                {
                    EmotePage page;

                    switch (index)
                    {
                        case 1: page = EmotePage.P1; break;
                        case 2: page = EmotePage.P2; break;
                        case 3: page = EmotePage.P3; break;
                        case 4: page = EmotePage.P4; break;
                        default: return;
                    }

                    from.SendGump( new EmoteGump( from, page ) );
                    break;
                }
                case 1:
                {
                    Emote( from, index - 1 );

                    EmotePage page;

                    if (index < 13)
                        page = EmotePage.P1;
                    else if (index < 25)
                        page = EmotePage.P2;
                    else if (index < 37)
                        page = EmotePage.P3;
                    else
                        page = EmotePage.P4;

                    from.SendGump( new EmoteGump( from, page ) );
                    break;
                }
            }
        }

        public static void Emote( Mobile from, int what )
        {
            from.RevealingAction();
            from.Emote( 505526 + what );

            switch (what + 1)
            {
                case 1: // *Kiwa glowa ze zrozumieniem*	
                {
                    if (from.Female)
                        from.PlaySound( 778 );
                    else
                        from.PlaySound( 1049 );
                    break;
                }
                case 2: // *Krzyczy triumfalnie*
                {
                    if (from.Female)
                        from.PlaySound( 779 );
                    else
                        from.PlaySound( 1050 );
                    break;
                }
                case 3: // *Klaszcze*
                {
                    if (from.Female)
                        from.PlaySound( 780 );
                    else
                        from.PlaySound( 1051 );
                    break;
                }
                case 4: // *Wydmuchuje nos w chusteczke*
                {
                    if (from.Female)
                        from.PlaySound( 781 );
                    else
                        from.PlaySound( 1052 );

                    if (!from.Mounted)
                        from.Animate( 34, 5, 1, true, false, 0 );

                    break;
                }
                case 5: // *Klania sie*
                {
                    if (!from.Mounted)
                        from.Animate( 32, 5, 1, true, false, 0 );

                    break;
                }

                case 6: // *Odkasluje*
                {
                    if (from.Female)
                        from.PlaySound( 786 );
                    else
                        from.PlaySound( 1057 );

                    break;
                }
                case 7: // *Beka*
                {
                    if (from.Female)
                        from.PlaySound( 782 );
                    else
                        from.PlaySound( 1053 );

                    if (!from.Mounted)
                        from.Animate( 33, 5, 1, true, false, 0 );

                    break;
                }
                case 8: // *Chrzaka*
                {
                    if (from.Female)
                        from.PlaySound( 784 );
                    else
                        from.PlaySound( 1055 );

                    if (!from.Mounted)
                        from.Animate( 33, 5, 1, true, false, 0 );

                    break;
                }
                case 9: // *Kaszle*
                {
                    if (from.Female)
                        from.PlaySound( 785 );
                    else
                        from.PlaySound( 1056 );

                    if (!from.Mounted)
                        from.Animate( 33, 5, 1, true, false, 0 );

                    break;
                }
                case 10: // "*Szlocha*"
                {
                    if (from.Female)
                        from.PlaySound( 787 );
                    else
                        from.PlaySound( 1058 );

                    if (!from.Mounted)
                        from.Animate( 34, 5, 1, true, false, 0 );

                    break;
                }
                case 11: // *Omdlewa* / *Mdleje*
                {
                    from.PlaySound( from.Female ? 791 : 1063 );
                    if (!from.Mounted) from.Animate( 22, 5, 1, true, false, 0 );
                    break;
                }
                case 12: // *Roztacza wkolo odor gorskiego trolla*
                {
                    from.PlaySound( from.Female ? 792 : 1064 );
                    break;
                }
                case 13: // *Okazuje bezgraniczne zaskoczenie*
                {
                    from.PlaySound( from.Female ? 793 : 1065 );
                    break;
                }
                case 14: // *Chichocze*
                {
                    from.PlaySound( from.Female ? 794 : 1066 );
                    break;
                }
                case 15: // *Okazuje obrzydzenie*
                {
                    from.PlaySound( from.Female ? 795 : 1067 );
                    if (!from.Mounted) from.Animate( 6, 5, 1, true, false, 0 );
                    break;
                }
                case 16: // *Powarkuje*
                {
                    from.PlaySound( from.Female ? 796 : 1068 );
                    if (!from.Mounted) from.Animate( 6, 5, 1, true, false, 0 );
                    break;
                }
                case 17: // *Wola*
                {
                    from.PlaySound( from.Female ? 797 : 1069 );
                    break;
                }
                case 18: // *Czka*
                {
                    from.PlaySound( from.Female ? 798 : 1070 );
                    break;
                }
                case 19: // *Przysluchuje sie z niedowierzaniem*
                {
                    from.PlaySound( from.Female ? 799 : 1071 );
                    break;
                }
                case 20: // *Caluje*
                {
                    from.PlaySound( 800 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 21: // *Smieje sie gromko*
                {
                    from.PlaySound( 801 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 22: // *Protestuje*
                {
                    from.PlaySound( 802 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 23: // *Dziwi sie*
                {
                    from.PlaySound( 803 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 24: // *Wzdycha ze zniecheceniem*
                {
                    from.PlaySound( 811 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 25: // *Usmiecha sie przepraszajaco*
                {
                    from.PlaySound( 812 + ((from.Female) ? 0 : 272) );
                    break;
                }
                case 26: // *Wymiotuje*
                {
                    from.PlaySound( 813 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 32, 5, 1, true, false, 0 );
                    break;
                }
                case 27: // *Uderza piescia*
                {
                    from.PlaySound( 315 );
                    if (!from.Mounted) from.Animate( 31, 5, 1, true, false, 0 );
                    break;
                }
                case 28: // *Wrzeszczy*
                {
                    from.PlaySound( 814 + ((from.Female) ? 0 : 274) );
                    break;
                }
                case 29: // *Ucisza*
                {
                    from.PlaySound( 815 + ((from.Female) ? 0 : 274) );
                    break;
                }
                case 30: // Wzdycha z ulga
                {
                    from.PlaySound( 816 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 6, 5, 1, true, false, 0 );
                    break;
                }
                case 31: // *Policzkuje*
                {
                    from.PlaySound( 948 );
                    if (!from.Mounted) from.Animate( 11, 5, 1, true, false, 0 );
                    break;
                }
                case 32: // *Kicha*
                {
                    from.PlaySound( 817 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 32, 5, 1, true, false, 0 );
                    break;
                }
                case 33: // *Pociaga nosem*
                {
                    from.PlaySound( 818 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 34, 5, 1, true, false, 0 );
                    break;
                }
                case 34: // *Chrapie*
                {
                    from.PlaySound( 819 + ((from.Female) ? 0 : 274) );
                    break;
                }
                case 35: // *Spluwa*
                {
                    from.PlaySound( 820 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 6, 5, 1, true, false, 0 );
                    break;
                }
                case 36: // *Wystawia jezyk*
                {
                    from.PlaySound( 792 );
                    break;
                }
                case 37: // *Niecierpliwi sie*
                {
                    from.PlaySound( 874 );
                    if (!from.Mounted) from.Animate( 38, 5, 1, true, false, 0 );
                    break;
                }
                case 38: // *Gwizdze*
                {
                    from.PlaySound( 821 + ((from.Female) ? 0 : 274) );
                    if (!from.Mounted) from.Animate( 5, 5, 1, true, false, 0 );
                    break;
                }
                case 39: // *Nawoluje*
                {
                    from.PlaySound( (from.Female) ? 783 : 1054 );
                    break;
                }
                case 40: // Ziewa
                {
                    from.PlaySound( (from.Female) ? 822 : 1096 );
                    if (!from.Mounted) from.Animate( 17, 5, 1, true, false, 0 );
                    break;
                }
                case 41: // *Okazuje zadowolenie*
                {
                    from.PlaySound( (from.Female) ? 823 : 1097 );
                    break;
                }
                case 42: // *Ryczy dziko*
                {
                    from.PlaySound( 1098 );
                    break;
                }
            }
        }
    }
}
