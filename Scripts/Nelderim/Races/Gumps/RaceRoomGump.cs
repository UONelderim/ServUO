using System;
using Server;
using Server.Gumps;
using Server.Regions;

namespace Server.Gumps
{
    public class RaceRoomGump : Gump
    {
        public RaceRoomGump( Mobile from , RaceRoomType room ) : base( 0, 0 )
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(50, 35, 300, 300, 9200);
            this.AddAlphaRegion(60, 45, 279, 280);
            
            switch ( room )
            {
				case RaceRoomType.Czlowiek:
                {
                    this.AddHtmlLocalized( 73, 58, 252, 257, Race.NTamael.DescNumber, true, true);
                        
                    break;
                }				
				case RaceRoomType.Elf:
                {
                    this.AddHtmlLocalized( 73, 58, 252, 257, Race.NElf.DescNumber, true, true);
                        
                    break;
                }
				case RaceRoomType.Drow:
                {
                    this.AddHtmlLocalized( 73, 58, 252, 257, Race.NDrow.DescNumber, true, true);
                        
                    break;
                }
				case RaceRoomType.Krasnolud:
                {
                    this.AddHtmlLocalized( 73, 58, 252, 257, Race.NKrasnolud.DescNumber, true, true);
                        
                    break;
                }
				
                case RaceRoomType.Teleport:
                {
                    this.AddHtml( 73, 58, 252, 257, @"", true, true);
                    break;
                }
            }
        }
    }
}
