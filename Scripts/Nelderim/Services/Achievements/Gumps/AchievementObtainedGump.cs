using Server;
using Server.Gumps;

namespace Nelderim.Achievements.Gumps
{
	class AchievementObtainedGump : Gump
	{
		public AchievementObtainedGump(Achievement ach) : base(470, 389)
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(39, 38, 350, 100, 9270);
			AddAlphaRegion(48, 45, 332, 86);

			var bounds = ItemBounds.Table[ach.Icon];
			if (ach.Icon > 0)
				AddItem(80 - bounds.Width / 2 - bounds.X, (30 - bounds.Height / 2 - bounds.Y) + 60, ach.Icon);
			AddLabel(121, 55, 49, ach.Name);
			AddHtml(120, 80, 167, 42, ach.Description, true, true);
			AddLabel(275, 51, 61, @"UKONCZONO");
			AddBackground(320, 72, 44, 47, 9200);
			AddLabel(337, 87, 0, ach.Points.ToString());
		}
	}
}
