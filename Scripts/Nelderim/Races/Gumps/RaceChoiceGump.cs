using System;
using Server;
using Server.Network;

namespace Server.Gumps
{
	public class RaceChoiceGump : Gump
    {
        protected Mobile m_From;
        protected Race m_Race;

        protected int m_SkinHue;
        protected int m_HairHue;
        protected HairItemID m_HairItemID;
        protected FacialHairItemID m_FacialHairItemID;

        public enum ButtonID
        {
            Cancel,
            Apply
        }

        public string Color( string text )
        {
            return Color( text, 0xFFFFFF );
        }

        public string Color( string text, int color )
        {
            return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
        }

        public string Center( string text )
        {
            return String.Format( "<CENTER>{0}</CENTER>", text );
        }

        public int GetGumpID( HairItemID hid )
        {
            int offset = m_From.Female ? 10000 : 0;

            switch ( hid )
            {
                case HairItemID.Short:          return 50700 + offset;
                case HairItemID.Long:           return 50701 + offset;
                case HairItemID.PonyTail:       return 50702 + offset;
                case HairItemID.Mohawk:         return 50703 + offset;
                case HairItemID.Pageboy:        return 50710 + offset;
                case HairItemID.Buns:           return 50712 + offset;
                case HairItemID.Afro:           return 50900 + offset;
                case HairItemID.Receeding:      return 50901 + offset;
                case HairItemID.TwoPigTails:    return 50902 + offset;
                case HairItemID.Krisna:         return 50715 + offset;
                default:                        return 0;
            }
        }

        public int GetGumpID( FacialHairItemID fhid )
        {
            switch ( fhid )
            {
                case FacialHairItemID.LongBeard:        return 50801;
                case FacialHairItemID.ShortBeard:       return 50802;
                case FacialHairItemID.Goatee:           return 50800;
                case FacialHairItemID.Mustache:         return 50808;
                case FacialHairItemID.MediumShortBeard: return 50904;
                case FacialHairItemID.MediumLongBeard:  return 50905;
                case FacialHairItemID.Vandyke:          return 50906;
                default:                                return 0;
            }
        }

        public virtual bool AllowAppearanceChange()
		{
			return true;
		}
		
		public virtual void DrawBackground()
		{
            AddBackground( 0, 0, 605, 570, 9200 );
            AddImageTiled( 5, 5, 595, 560, 9274 );
            AddLabel( 20, 10, 930, String.Format("WYBOR RASY NELDERIM - {0}", m_Race.GetName( Cases.Mianownik, false ).ToUpper() ) );
			
            AddHtml( 390, 257, 203, 25, Color( Center( "RASA " + m_Race.GetName( Cases.Dopelniacz, true ).ToUpper() ), 0x333333 ), true, false );
            AddHtmlLocalized( 388, 282, 205, 255, m_Race.DescNumber, (bool)true, (bool)true ); // opis
			
            AddButton( 457, 540, 0xef, 0xf0, (int)ButtonID.Apply, GumpButtonType.Reply, 0 );
            AddButton( 527, 540, 242, 243, (int)ButtonID.Cancel, GumpButtonType.Reply, 0 );
		}
		
		public void DrawPaperdoll()
		{
            AddImageTiled( 376, 5, 217, 251, 1800 ); // paperdoll

            // Body
            if ( m_From.Female )
                AddImage( 400, 20, 0xd, m_SkinHue - 1 );
            else
                AddImage( 400, 20, 0xc, m_SkinHue - 1 );

            // Hair
            if( m_HairItemID != HairItemID.None )
                AddImage( 400, 18, GetGumpID( m_HairItemID ), m_HairHue - 1 );
            // FacialHair
            if ( m_FacialHairItemID != FacialHairItemID.None )
                AddImage( 400, 20, GetGumpID( m_FacialHairItemID ), m_HairHue - 1 );
		}
		
		public void DrawChoices()
		{
            AddHtml( 15, 40, 360, 28, Center( "Kolor skory" ), true, false ); // Kolor skory

            int x = 15;
            int y = 80;

            for ( int i = 101; i <= m_Race.SkinHues.Length + 100; i++ )
            {
                if ( x > 370 )
                {
                    x = 15;
                    y = y + 25;
                }

                AddButton( x, y, 0x56f7, 0x56f7, i, GumpButtonType.Reply, 0 );
                AddImage( x, y, 0x56f7, m_Race.SkinHues[ i - 101 ] - 1 );
                x = x + 20;
            }

            AddHtml( 15, y + 25, 360, 28, Center( "Kolor wlosow" ), true, false ); // Kolor wlosow

            y = y + 60;
            x = 15;

            for ( int i = 201; i <= m_Race.HairHues.Length + 200; i++ )
            {
                if ( x > 370 )
                {
                    x = 15;
                    y = y + 25;
                }

                AddButton( x, y, 0x56f7, 0x56f7, i, GumpButtonType.Reply, 0 );
                AddImage( x, y, 0x56f7, m_Race.HairHues[ i - 201 ] - 1 );
                x = x + 20;
            }

            AddHtml( 15, y + 25, 360, 28, Center( "Fryzura" ), true, false ); // Fryzura
            y = y + 60;
            x = 15;

            HairItemID[] hairStyles = m_From.Female ? m_Race.FemaleHairStyles : m_Race.MaleHairStyles;
            for ( int i = 301; i <= ( hairStyles.Length + 300 ); i++ )
            {
                if ( x > 300 )
                {
                    x = 15;
                    y = y + 50;
                }

                AddButton( x, y, 0xd0, 0xd1, i, GumpButtonType.Reply, 0 );

                HairItemID hid = hairStyles[i - 301];
                int gumpID = GetGumpID( hid );

                if ( gumpID == 0 )
                    AddHtml( x + 30, y, 50, 50, Color( "Brak" ), false, false );
                else
                    AddImage( x - 50, y - 50, gumpID );

                x = x + 80;
            }

            AddHtml( 15, y + 35, 360, 28, Center( "Zarost" ), true, false ); // Zarost
            y = y + 70;
            x = 15;

            FacialHairItemID[] facialHairStyles = m_From.Female ? new FacialHairItemID[0] : m_Race.FacialHairStyles;
            for ( int i = 401; i <= ( facialHairStyles.Length + 400 ); i++ )
            {
                if ( x > 300 )
                {
                    x = 15;
                    y = y + 50;
                }

                AddButton( x, y, 0xd0, 0xd1, i, GumpButtonType.Reply, 0 );

                FacialHairItemID fhid = facialHairStyles[i - 401];
                int gumpID = GetGumpID( fhid );

                if ( gumpID == 0 )
                    AddHtml( x + 30, y, 50, 50, Color( "Brak" ), false, false );
                else
                    AddImage( x - 50, y - 50, gumpID );

                x = x + 80;
            }
		}
		
		public RaceChoiceGump( Mobile from, Race race ) : this( from, race, race.SkinHues[0], race.HairHues[0], HairItemID.None, FacialHairItemID.None )
        {
        }

        public RaceChoiceGump( Mobile from, Race race, int skinHue, int hairHue, HairItemID hairItemID, FacialHairItemID facialHairItemID ) : base( 40, 40 )
        {
            m_From = from;

			if (!AllowAppearanceChange())
			{
                from.SendLocalizedMessage( 501704 ); // You cannot disguise yourself while incognitoed.
				return;
			}			
			
            m_Race = race;

            m_SkinHue = skinHue;
            m_HairHue = hairHue;
            m_HairItemID = hairItemID;
            m_FacialHairItemID = facialHairItemID;

            from.CloseGump( typeof( RaceChoiceGump ) );	// TODO: czy zamknie tez gump klasy pochodnej?

            Closable = true;
            Dragable = true;
            Resizable = false;

			AddPage( 0 );
			
			DrawBackground();

			DrawPaperdoll();

			DrawChoices();
        }

        public static void Initialize()
        {
            //EventSink.Movement += new MovementEventHandler( EventSink_Movement_DGumpClose );
        }

        private static void EventSink_Movement_DGumpClose( MovementEventArgs args )
        {
            Mobile m = args.Mobile;
            m.CloseGump( typeof( RaceChoiceGump ) );
        }
		
		public virtual void ApplyAppearance(Race race, int skinHue, int hairHue, int hairID, int facialHairID)
		{
            m_From.Race          = race;
            m_From.HairItemID    = hairID;
            m_From.HairHue       = hairHue;
            m_From.Hue           = skinHue;
            if(! m_From.Female )
                m_From.FacialHairItemID = facialHairID;
            m_From.FacialHairHue = m_HairHue;

            m_From.SendMessage( 0x20, "Od teraz jestes {0}.", m_Race.GetName( Cases.Narzednik ) );
		}
		
		public virtual void ExtendedResponse(RelayInfo info)
		{
            m_From.SendGump(new RaceChoiceGump(m_From, m_Race, m_SkinHue, m_HairHue, m_HairItemID, m_FacialHairItemID));            
		}

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            if ( info.ButtonID == (int)ButtonID.Cancel )
                return;
            else
				
			if ( info.ButtonID == (int)ButtonID.Apply )
            {
				ApplyAppearance(m_Race, (int)m_SkinHue, m_HairHue, (int)m_HairItemID, (int)m_FacialHairItemID);
                return;
            }
            else if ( info.ButtonID < 200 )
                m_SkinHue = m_Race.ClipSkinHue( m_Race.SkinHues[ info.ButtonID - 101 ] );
            else if ( info.ButtonID < 300 )
                m_HairHue = m_Race.ClipHairHue( m_Race.HairHues[ info.ButtonID - 201 ] );
            else if ( info.ButtonID < 400 )
            {
                HairItemID[] hs = m_From.Female ? m_Race.FemaleHairStyles : m_Race.MaleHairStyles;
                m_HairItemID = hs[ info.ButtonID - 301 ];
            }
            else if ( info.ButtonID < 500 )
                m_FacialHairItemID = m_Race.FacialHairStyles[ info.ButtonID - 401 ];

			ExtendedResponse(info);
        }
    }
}
