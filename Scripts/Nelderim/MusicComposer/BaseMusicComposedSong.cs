using System; 
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
namespace Server.Items
{

	public abstract class BaseMusicComposedSong : Item 
	{
		//required musicanship skill to succeed at playing song.
		//this stores the song to be played
		private string Song = "";
		//the song name (optional)
		private string SongName = "";
		//how long to delay each action on the character for
		private const int ACTION_DELAY_SECONDS = 5;
		public BaseMusicComposedSong(string _Song, string _SongName) : base(  0x0EBE )	{
			this.Song = _Song;
			this.SongName = _SongName;
			this.Weight = 1.0;
			this.LootType = LootType.Blessed;
		}
		public BaseMusicComposedSong( Serial serial ) : base( serial ){
			this.Weight = 1.0;
		}
		//same as a regular song, except this is not pressed.
		public override void OnDoubleClick( Mobile from )
		{	
			PlaySong(from);
		}
		/**
		* Plays song it has stored, and performs action before doing it.
		*/
		protected void PlaySong(Mobile from){
			//if a player and alive
			if(!from.Player ){
				return;
			}else if(!from.Alive){
				from.SendMessage("Nie możesz tego zrobić będąc martwym.");
				return;
			}
			//finally, begin action
			if ( from.BeginAction( typeof( Item ) )  ){
				//create callback to end our action.
				TimerCallback callback = new TimerCallback( delegate( ) {
					from.EndAction( typeof( Item ) );
				});
				//set delay of call
				Timer timer = Timer.DelayCall( TimeSpan.FromSeconds(ACTION_DELAY_SECONDS), callback	);
				//check our musicianship skill
				if(BaseInstrument.CheckMusicianship(from) ){
					//let musicComposer take care of the rest
					MusicComposer.PlaySong(from, this.Song); //true just means its play, as opposed to "send" song		
				}else{
					from.SendMessage("Nie udało Ci się zagrac tej piosenki.");
				}
			}else{
				from.SendMessage("Zaczekaj by wykonać inną akcję.");
			}
		}
		public override string DefaultName	{
			get {
				if(SongName == ""){
					return "zapis nutowy"; 
				}else{
					return this.SongName + " i jej zapis nutowy";
				}
			}
		}
		public override void Serialize( GenericWriter writer ){
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( this.Song );
			writer.Write( this.SongName);
		}

		public override void Deserialize( GenericReader reader ){
			base.Deserialize( reader );
			int version = reader.ReadInt();
			this.Song = reader.ReadString();
			this.SongName = reader.ReadString();

			this.LootType = LootType.Blessed;
		}
	}
}