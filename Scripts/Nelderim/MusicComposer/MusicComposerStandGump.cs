using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;
namespace Server.Gumps
{ 
    public class MusicComposerStandGump : Gump
    {
		private const int LEFT_BAR=25;
		private const int MAX_SONG_STRING_LENGTH = 512;
		public MusicComposerStandGump() : base( 0, 0){
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 320, 440, 9200);
			this.AddLabel(LEFT_BAR, 20, 0, @"Kompozytor muzyki");
			this.AddLabel(LEFT_BAR, 40, 0, @"Zrobiony przez PanThar i przetłumaczony przez: Maupishon");
			this.AddLabel(LEFT_BAR, 60,  32, @"Pierwsze litery: H=harfa, L=Lutnia, M=Stojąca Harfa");
			this.AddLabel(LEFT_BAR, 80,  32, @"A,B,C,D,E,F=Nuty, S=z krzyżykiem, 1,2=oktawa");
			this.AddLabel(LEFT_BAR, 100,  32, @"-=kazda nute oddzielaj przecinkiem");
			this.AddLabel(LEFT_BAR, 120,  32, @"Przyklad: HE1,-,HE2,LC2,-,LC1,HGS2,MG2");
			
			AddImageTiled(LEFT_BAR, 160, 271, 210, 0xA8E);
			AddTextEntry( LEFT_BAR+7, 160, 260, 190, 512, 0, @"");
			AddButton(LEFT_BAR, 400, 4005, 4006, 1, GumpButtonType.Reply, 3);
			AddButton(LEFT_BAR +180, 400, 4017, 4018, 2, GumpButtonType.Reply, 3);
    	} 
		/**
		*	Response to the gump submitt button(s). 
		* 	If it's valid, it will create a new song, even if there is nothing in it.
		*	we only check that it's not a null string
		*/
		public override void OnResponse( NetState state, RelayInfo info ){
			int btd = info.ButtonID;
			Mobile from = state.Mobile;
			if(info.ButtonID == 1  ){
				TextRelay entry = info.GetTextEntry(0);
				//parse out the text entry
				try{
							
					string Song =entry.Text;
					//if a player and alive
					if(!from.Player ){
						return;
					}else if(!from.Alive){
						from.SendMessage("Jak chcesz to zrobić, to najpierw odwiedź uzdrowiciela...");
						return;
					}else if(Song == ""){
						from.SendMessage("Wpisz swą pieśń by kontynuowac.");
						from.CloseGump(typeof(MusicComposerStandGump));
						//send gump to user
						from.SendGump(new MusicComposerStandGump());
						return;
					}else if(Song.Length >= MAX_SONG_STRING_LENGTH){
						from.SendMessage("Ta pieśń jest zbyt potężna.");
						return;
					}
					//let the composed song item take care of the rest, we succeeded in creating song/item
					from.AddToBackpack( new ComposedSong( Song, "" ) );
				}catch{
					from.SendMessage( "Problem z pisaniem piosenek? Poproś Bogów o pomoc." );			
				}				
			}else if(info.ButtonID == 2){
				from.SendMessage( "Opuściłeś kreator muzyki.");
			}
			else{
				state.Mobile.SendMessage( "TO NIELEGALNE! ZOSTAW TO NATYCHMIAST ŁOTRZE!");
			}
		}    
    }    
}