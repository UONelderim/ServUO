using System;
using Server.Mobiles;
using System.Collections.Generic;
using System.Text.RegularExpressions ;
namespace Server
{
	/** 
	* Lute Notes
	**/
	public static class LuteNotes
	{		
		//notes
		//octive 1
		public const  int C1 = 0x404;
		public const  int CS1 = 0x407;
		public const  int D1 = 0x409;
		public const  int DS1 = 0x40C;
		public const  int E1 = 0x40E;
		public const  int F1 = 0x410;
		public const  int FS1 = 0x412;
		public const  int G1 = 0x414;
		public const  int GS1 = 0x416;
		public const  int A1 = 0x3FD;
		public const  int AS1 = 0x3FF;
		public const  int B1 = 0x401;
		//octive 2
		public const  int C2 = 0x405;
		public const  int CS2 = 0x408;
		public const  int D2 = 0x40A;
		public const  int DS2 = 0x40D;
		public const  int E2 = 0x40F;
		public const  int F2 = 0x411;
		public const  int FS2 = 0x413;
		public const  int G2 = 0x415;
		public const  int GS2 = 0x417;
		public const  int A2 = 0x3FE;
		public const  int AS2= 0x400;
		public const  int B2 = 0x402;
		//octive 3
		public const  int C3 = 0x406;
	}
	/**
	* Lap Harp Notes
	*/
	public static class LapHarpNotes
	{
		//notes
		//octive 1
		public const  int C1 = 0x3D0;
		public const  int CS1 = 0x3D3;
		public const  int D1 = 0x3D5;
		public const  int DS1 = 0x3D7;
		public const  int E1 = 0x3D9;
		public const  int F1 = 0x3DB;
		public const  int FS1 = 0x3DD;
		public const  int G1 = 0x3DF;
		public const  int GS1 = 0x3E1;
		public const  int A1 = 0x3CA;
		public const  int AS1 = 0x3CC;
		public const  int B1 = 0x3CE;
		//octive 2
		public const  int C2 = 0x3D1;
		public const  int CS2 = 0x3D4;
		public const  int D2 = 0x3D6;
		public const  int DS2 = 0x3D8;
		public const  int E2 = 0x3DA;
		public const  int F2 = 0x3DC;
		public const  int FS2 = 0x3DE;
		public const  int G2 = 0x3E0;
		public const  int GS2 = 0x3E2;
		public const  int A2 = 0x3CB;
		public const  int AS2= 0x3CD;
		public const  int B2 = 0x3CF;
		//octive 3
		public const  int C3 = 0x3D2;
	}
	/** 
	* Stand Harp Notes
	**/
	public static class StandHarpNotes
	{
		//notes
		//octive 1
		public const  int C1 = 0x049D;
		public const  int CS1 = 0x04A0;
		public const  int D1 = 0x04A2;
		public const  int DS1 = 0x04A4;		
		public const  int E1 = 0x04A6;
		public const  int F1 = 0x04A8;
		public const  int FS1 = 0x04AA;
		public const  int G1 = 0x04AC;
		public const  int GS1 = 0x04AE;
		public const  int A1 = 0x0497;
		public const  int AS1 = 0x0499;
		public const  int B1 = 0x049B;
		//octive 2
		public const  int C2 = 0x049E;
		public const  int CS2 = 0x04A1;
		public const  int D2 = 0x04A3;
		public const  int DS2 = 0x04A5;
		public const  int E2 = 0x04A7;
		public const  int F2 = 0x04A9;
		public const  int FS2 = 0x04AB;
		public const  int G2 = 0x04AD;
		public const  int GS2 = 0x04AF;
		public const  int A2 = 0x0498;
		public const  int AS2= 0x049A;
		public const  int B2 = 0x049C;
		//octive 3
		public const  int C3 = 0x049F;
	}
	public static class MusicComposer  
	{
		private const int NOTE_DELAY_MILI = 500;
		/**
		*	3 Arguments to play a song
		*	from - mobile playing/getting song sent to
		*   SongScript - the UO songstrip string
		*/
		public static  bool PlaySong(Mobile from, string SongScript){
			List<int>  MusicalNotes  = GetSongFromString(SongScript);
			if(MusicalNotes != null){
				PlaySingleNotesForSong(from, MusicalNotes);
				return true;
			}else{
				return false;
			}
		}
		/**
		*	Parses a song string, using regex, L stands for Lute, and H stands for Harp
		*/
		private static List<int> GetSongFromString(string data){
			List<int>  MusicalNotes = new List<int>();
			Regex regexObj = new Regex(@"""[^""\r\n]*""|'[^'\r\n]*'|[^,\r\n]*");
			Match matchResults = regexObj.Match(data);
			while (matchResults.Success){
				//make sure the string value is upper case, soe users can enter whatever they want
				switch(matchResults.Value.ToUpper()){
					//lute
					case "LC1":
						MusicalNotes.Add( (int)LuteNotes.C1 );	
					break;
					case "LCS1":
						MusicalNotes.Add( (int)LuteNotes.CS1 );
					break;
					case "LD1":
						MusicalNotes.Add( (int)LuteNotes.D1 );
					break;
					case "LDS1":
						MusicalNotes.Add( (int)LuteNotes.DS1 );
					break;
					case "LE1":
						MusicalNotes.Add( (int)LuteNotes.E1 );
					break;
					case "LF1":
						MusicalNotes.Add( (int)LuteNotes.F1);
					break;
					case "LFS1":
						MusicalNotes.Add( (int)LuteNotes.FS1 );
					break;
					case "LG1":
						MusicalNotes.Add( (int)LuteNotes.G1 );
					break;
					case "LGS1":
						MusicalNotes.Add( (int)LuteNotes.GS1 );
					break;
					case "LA1":
						MusicalNotes.Add( (int)LuteNotes.A1 );
					break;
					case "LAS1":
						MusicalNotes.Add( (int)LuteNotes.AS1);
					break;
					case "LB1":
						MusicalNotes.Add( (int)LuteNotes.B1 );				
					break;
					case "LC2":
						MusicalNotes.Add( (int)LuteNotes.C2 );				
					break;
					case "LCS2":
						MusicalNotes.Add( (int)LuteNotes.CS2 );
					break;
					case "LD2":
						MusicalNotes.Add( (int)LuteNotes.D2 );
					break;
					case "LDS2":
						MusicalNotes.Add( (int)LuteNotes.DS2);
					break;
					case "LE2":
						MusicalNotes.Add( (int)LuteNotes.E2 );
					break;
					case "LF2":
						MusicalNotes.Add( (int)LuteNotes.F2 );
					break;
					case "LFS2":
						MusicalNotes.Add( (int)LuteNotes.FS2 );
					break;
					case "LG2":
						MusicalNotes.Add( (int)LuteNotes.G2 );
					break;
					case "LGS2":
						MusicalNotes.Add( (int)LuteNotes.GS2 );
					break;
					case "LA2":
						MusicalNotes.Add( (int)LuteNotes.A2 );
					break;
					case "LAS2":
						MusicalNotes.Add( (int)LuteNotes.AS2 );
					break;
					case "LB2":
						MusicalNotes.Add( (int)LuteNotes.B2 );				
					break;
					case "LC3":					
						MusicalNotes.Add( (int)LuteNotes.C3 );
					break;
					//harp notes
					case "HC1":
						MusicalNotes.Add( (int)LapHarpNotes.C1 );	
					break;
					case "HCS1":
						MusicalNotes.Add( (int)LapHarpNotes.CS1 );
					break;
					case "HD1":
						MusicalNotes.Add( (int)LapHarpNotes.D1 );
					break;
					case "HDS1":
						MusicalNotes.Add( (int)LapHarpNotes.DS1 );
					break;
					case "HE1":
						MusicalNotes.Add( (int)LapHarpNotes.E1 );
					break;
					case "HF1":
						MusicalNotes.Add( (int)LapHarpNotes.F1 );
					break;
					case "HFS1":
						MusicalNotes.Add( (int)LapHarpNotes.FS1 );
					break;
					case "HG1":
						MusicalNotes.Add( (int)LapHarpNotes.G1 );
					break;
					case "HGS1":
						MusicalNotes.Add( (int)LapHarpNotes.GS1);
					break;
					case "HA1":
						MusicalNotes.Add( (int)LapHarpNotes.A1 );
					break;
					case "HAS1":
						MusicalNotes.Add( (int)LapHarpNotes.AS1 );
					break;
					case "HB1":
						MusicalNotes.Add( (int)LapHarpNotes.B1 );				
					break;
					case "HC2":
						MusicalNotes.Add( (int)LapHarpNotes.C2 );				
					break;
					case "HCS2":
						MusicalNotes.Add( (int)LapHarpNotes.CS2 );
					break;
					case "HD2":
						MusicalNotes.Add( (int)LapHarpNotes.D2 );
					break;
					case "HDS2":
						MusicalNotes.Add( (int)LapHarpNotes.DS2 );
					break;
					case "HE2":
						MusicalNotes.Add( (int)LapHarpNotes.E2 );
					break;
					case "HF2":
						MusicalNotes.Add( (int)LapHarpNotes.F2 );
					break;
					case "HFS2":
						MusicalNotes.Add( (int)LapHarpNotes.FS2 );
					break;
					case "HG2":
						MusicalNotes.Add( (int)LapHarpNotes.G2 );
					break;
					case "HGS2":
						MusicalNotes.Add( (int)LapHarpNotes.GS2 );
					break;
					case "HA2":
						MusicalNotes.Add( (int)LapHarpNotes.A2 );
					break;
					case "HAS2":
						MusicalNotes.Add( (int)LapHarpNotes.AS2 );
					break;
					case "HB2":
						MusicalNotes.Add( (int)LapHarpNotes.B2 );				
					break;
					case "HC3":					
						MusicalNotes.Add( (int)LapHarpNotes.C3 );
					break;
					//stand harp notes
					//M just mean, music I guess?
					case "MC1":
						MusicalNotes.Add( (int)StandHarpNotes.C1 );	
					break;
					case "MCS1":
						MusicalNotes.Add( (int)StandHarpNotes.CS1 );
					break;
					case "MD1":
						MusicalNotes.Add( (int)StandHarpNotes.D1 );
					break;
					case "MDS1":
						MusicalNotes.Add( (int)StandHarpNotes.DS1 );
					break;
					case "ME1":
						MusicalNotes.Add( (int)StandHarpNotes.E1 );
					break;
					case "MF1":
						MusicalNotes.Add( (int)StandHarpNotes.F1 );
					break;
					case "MFS1":
						MusicalNotes.Add( (int)StandHarpNotes.FS1 );
					break;
					case "MG1":
						MusicalNotes.Add( (int)StandHarpNotes.G1 );
					break;
					case "MGS1":
						MusicalNotes.Add( (int)StandHarpNotes.GS1);
					break;
					case "MA1":
						MusicalNotes.Add( (int)StandHarpNotes.A1 );
					break;
					case "MAS1":
						MusicalNotes.Add( (int)StandHarpNotes.AS1 );
					break;
					case "MB1":
						MusicalNotes.Add( (int)StandHarpNotes.B1 );				
					break;
					case "MC2":
						MusicalNotes.Add( (int)StandHarpNotes.C2 );				
					break;
					case "MCS2":
						MusicalNotes.Add( (int)StandHarpNotes.CS2 );
					break;
					case "MD2":
						MusicalNotes.Add( (int)StandHarpNotes.D2 );
					break;
					case "MDS2":
						MusicalNotes.Add( (int)StandHarpNotes.DS2 );
					break;
					case "ME2":
						MusicalNotes.Add( (int)StandHarpNotes.E2 );
					break;
					case "MF2":
						MusicalNotes.Add( (int)StandHarpNotes.F2 );
					break;
					case "MFS2":
						MusicalNotes.Add( (int)StandHarpNotes.FS2 );
					break;
					case "MG2":
						MusicalNotes.Add( (int)StandHarpNotes.G2 );
					break;
					case "MGS2":
						MusicalNotes.Add( (int)StandHarpNotes.GS2 );
					break;
					case "MA2":
						MusicalNotes.Add( (int)StandHarpNotes.A2 );
					break;
					case "MAS2":
						MusicalNotes.Add( (int)StandHarpNotes.AS2 );
					break;
					case "MB2":
						MusicalNotes.Add( (int)StandHarpNotes.B2 );				
					break;
					case "MC3":					
						MusicalNotes.Add( (int)StandHarpNotes.C3 );
					break;
					//special case if a break in music
					case "-":
						MusicalNotes.Add( (int)0x0);
					break;
					default:
					break;
				};
				//get next regex match
				matchResults = matchResults.NextMatch();
			}
			//finally return the music notes in something we can play
			if(MusicalNotes.Count > 0){
				return MusicalNotes;
			}else{
				return null;
			}
		}
		/**
		* Basically, it just creates a new callback, for every beat, and plays the note
		*/		
		private static void PlaySingleNotesForSong(Mobile sendMusicTo,List<int>  MusicalNotes ){
			//we have to do this with a callback, to not delay anything else
			TimerCallback callback = new TimerCallback( delegate( ) {
					if(MusicalNotes != null && sendMusicTo != null && sendMusicTo.Player){
						//if more than two beats remaining
						if(MusicalNotes.Count > 1){
							//this is the SPACER (nothing), if it's 0, no sound effects
							if( (int)MusicalNotes[0] != 0){
								sendMusicTo.PlaySound((int)MusicalNotes[0]);
							}
							MusicalNotes.RemoveAt(0);
							PlaySingleNotesForSong(sendMusicTo, MusicalNotes);
						}else if(MusicalNotes.Count == 1){
							//this is the SPACER (nothing), if it's 0, no sound effects
							if( (int)MusicalNotes[0] != 0){
								//is it play, or send sound, I believe sendSound sends to 1
								//mobile, while playsound sends to all mobiles
								sendMusicTo.PlaySound((int)MusicalNotes[0]);
							}
							MusicalNotes.RemoveAt(0);
						}
					}
			});
			//set delay of call
			Timer timer = Timer.DelayCall( TimeSpan.FromMilliseconds(NOTE_DELAY_MILI), callback	);		
		}
	}
}