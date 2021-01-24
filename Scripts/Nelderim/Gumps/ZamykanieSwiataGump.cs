using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
	public class ZamykanieSwiataGump : Gump
	{
		public ZamykanieSwiataGump() : base( 200, 200 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 310, 100, 9270);
			this.AddLabel(35, 23, 138, @"Zasniecie swiata nastapi za 2 Minuty!");
			this.AddLabel(40, 50, 255,  @"Prosimy wszystkich o rozlaczenie sie");
		}
	}
}