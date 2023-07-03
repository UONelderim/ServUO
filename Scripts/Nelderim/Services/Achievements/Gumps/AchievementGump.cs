using Server.Gumps;
using System.Collections.Generic;
using Server.Network;
using System;
using System.Linq;

namespace Scripts.Mythik.Systems.Achievements
{
	class AchievementGump : Gump
	{
		private int m_curTotal;
		private Dictionary<int, AchievementStatus> m_curAchieves;

		public AchievementGump(Dictionary<int, AchievementStatus> achieves, int total, int category = 1) : base(25, 25)
		{
			m_curAchieves = achieves;
			m_curTotal = total;
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(11, 15, 758, 575, 2600);
			AddBackground(57, 92, 666, 478, 9250);
			AddBackground(321, 104, 386, 453, 9270);
			AddBackground(72, 104, 245, 453, 9270);
			AddBackground(72, 34, 635, 53, 9270);
			AddBackground(327, 0, 133, 41, 9200);
			AddLabel(292, 52, 68, "System Osiagniec");
			AddLabel(360, 11, 82, $"{total} Punktow");
			AddBackground(341, 522, 353, 26, 9200);

			int cnt = 0;
			if (AchievementSystem.Categories.TryGetValue(category, out var reqCat))
			{
				Console.WriteLine("Couldnt find Achievement category: " + category);
				reqCat = AchievementSystem.Categories[0];
			}

			foreach (var cat in AchievementSystem.Categories.Values)
			{
				var x = 90;
				var bgID = 9200;

				if (cat.Parent != 0 && cat.ID != reqCat.ID && cat.Parent != reqCat.ID && cat.Parent != reqCat.Parent)
					continue;
				if (cat.Parent != 0)
					x += 20;
				if (cat.ID == category)
					bgID = 5120;

				AddBackground(x, 123 + (cnt * 31), 18810 / x, 25, bgID);
				if (cat.ID == category) // selected
					AddImage(x + 12, 129 + (cnt * 31), 1210);
				else
					AddButton(x + 12, 129 + (cnt * 31), 1209, 1210, (int)(5000 + cat.ID), GumpButtonType.Reply, 0);
				AddLabel(x + 32, 125 + (cnt * 31), 0, cat.Name);
				cnt++;
			}

			cnt = 0;
			foreach (var ac in AchievementSystem.Definitions.Values.Where(ac => ac.CategoryID == category))
			{
				if (ac.PreReq != null)
				{
					if (!achieves.ContainsKey(ac.PreReq.ID))
						continue;
					if (achieves[ac.PreReq.ID].Completed)
						continue;
				}

				if (achieves.TryGetValue(ac.ID, out var achieve))
				{
					AddAchieve(ac, cnt, achieve);
				}
				else
				{
					if (ac.Secret)
						continue;
					AddAchieve(ac, cnt, null);
				}

				cnt++;
			}
		}

		private void AddAchieve(BaseAchievement ac, int i, AchievementStatus acheiveData)
		{
			var index = i % 4;
			if (index == 0)
			{
				AddButton(658, 524, 4005, 4006, 0, GumpButtonType.Page, (i / 4) + 1);
				AddPage((i / 4) + 1);
				AddLabel(484, 526, 32, "Strona " + ((i / 4) + 1));
				AddButton(345, 524, 4014, 4015, 0, GumpButtonType.Page, i / 4);
			}

			var bg = 9350;
			if (acheiveData.Completed)
				bg = 9300;
			AddBackground(340, 122 + (index * 100), 347, 97, bg);
			AddLabel(414, 131 + (index * 100), 49, ac.Title);
			if (ac.ItemIcon > 0)
				AddItem(357, 147 + (index * 100), ac.ItemIcon);
			AddImageTiled(416, 203 + (index * 100), 95, 9, 9750);

			var step = 95.0 / ac.CompletionTotal;
			var progress = 0;
			if (acheiveData.Completed)
				progress = acheiveData.Progress;

			AddImageTiled(416, 203 + (index * 100), (int)(progress * step), 9, 9752);
			AddHtml(413, 152 + (index * 100), 194, 47, ac.Desc, (bool)true, (bool)true);
			if (acheiveData.Completed)
				AddLabel(566, 127 + (index * 100), 32, acheiveData.CompletedOn.ToShortDateString());

			if (ac.CompletionTotal > 1)
				AddLabel(522, 196 + (index * 100), 0, progress + @" / " + ac.CompletionTotal);

			AddBackground(628, 149 + (index * 100), 48, 48, 9200);
			AddLabel(648, 163 + (index * 100), 32, ac.Points.ToString());
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 0)
				return;
			var btn = info.ButtonID - 5000;
			sender.Mobile.SendGump(new AchievementGump(m_curAchieves, m_curTotal, btn));
		}
	}
}
