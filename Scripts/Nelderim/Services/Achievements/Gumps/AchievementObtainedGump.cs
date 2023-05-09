using Server;
using Server.Gumps;

namespace Scripts.Mythik.Systems.Achievements.Gumps
{
	class AchievementObtainedGump : Gump
	{
		public AchievementObtainedGump(BaseAchievement ach) : base(470, 389)
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(39, 38, 350, 100, 9270);
			AddAlphaRegion(48, 45, 332, 86);

			var bounds = ItemBounds.Table[ach.ItemIcon];
			if (ach.ItemIcon > 0)
				AddItem(80 - bounds.Width / 2 - bounds.X, (30 - bounds.Height / 2 - bounds.Y) + 60, ach.ItemIcon);
			AddLabel(121, 55, 49, ach.Title);
			AddHtml(120, 80, 167, 42, ach.Desc, true, true);
			AddLabel(275, 51, 61, @"UKONCZONO");
			AddBackground(320, 72, 44, 47, 9200);
			AddLabel(337, 87, 0, ach.Points.ToString());
		}
	}
}
