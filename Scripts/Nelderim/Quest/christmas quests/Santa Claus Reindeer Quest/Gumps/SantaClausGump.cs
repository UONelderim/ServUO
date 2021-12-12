/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/16/2005
 * Time: 8:20 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System; 
using Server;
using Server.Commands; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{ 
   public class SantaClausGump : Gump 
   { 
      public static void Initialize() 
      { 
         CommandSystem.Register( "SantaClausGump", AccessLevel.GameMaster, new CommandEventHandler( SantaClausGump_OnCommand ) ); 
      } 

      private static void SantaClausGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new SantaClausGump( e.Mobile ) ); 
      } 

      public SantaClausGump( Mobile owner ) : base( 50,50 ) 
      { 
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
		//	AddImageTiled(  54, 33, 369, 400, 2624 );
            AddBackground(85, 30, 329, 408, 3500);
            AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Zagubione renifery Pana" );
			

			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=WHITE>*Widzisz jak postać posmutniała...*<BR><BR>" +
"<BASEFONT COLOR=WHITE>Moje renifery zaginęły. Boję się, że mogło im się przytrafić coś strasznego.<BR>" +
"<BASEFONT COLOR=WHITE>Niedługo nadejdzie Dzień Pana i nie mam nikogo z moich zaufanych przyjaciół, który ciągnie moje sanie.<BR><BR>" +
"<BASEFONT COLOR=WHITE>Pomógłbyś temu wesołemu, staremu grubasowi i odnalazł mojego zaginionego renifera? Jestem pewien, że o nich słyszałeś…<BR>"+
"<BASEFONT COLOR=WHITE>Ich imiona to Dasher, Dancer, Prancer and Vixen. Comet, Cupid, Donner i Blitzen.<BR>" +
"<BASEFONT COLOR=WHITE>Nie zapomnij o  Rudolphie! Warto poszukać nieopodal Saew.<BR><BR>" +
"<BASEFONT COLOR=WHITE>Znajdź je i zwróć mi każdy z nich. Kiedy wrócisz, po prostu powiedz moje imię, aby zwrócić moją uwagę, ponieważ jestem BARDZO zajęty przygotowaniami do tego Dnia Pana.<BR>" +
"<BASEFONT COLOR=WHITE>Zapłacę ci za twoje wysiłki i jestem PEWNY, że moi przyjaciele oddadzą ci buty z kopyt, abyś mógł bezpiecznie wrócić do domu!<BR>" +
"<BASEFONT COLOR=WHITE>Oto książka, którą napisały moje elfy, aby poprowadzić cię w tym zadaniu.<BR>" +
"<BASEFONT COLOR=WHITE>A teraz musisz się ruszać, przyjacielu, bo mamy niewiele czasu do stracenia! Święta zbliżają się!<BR>" +
						     "</BODY>", false, true);
			
//			<BASEFONT COLOR=#7B6D20>			

			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
               //Cancel 
               from.SendMessage( "*...cięzko wzdycha....*" );
               break; 
            } 

         }
      }
   }
}
