using Server.Gumps;
using System.Collections.Generic;
using Server.Network;
using System;
using System.Linq;
using Server.Mobiles;

namespace Nelderim.Achievements.Gumps
{
	class AchievementGump : Gump
	{
		private PlayerMobile _target;

		public AchievementGump(PlayerMobile pm, int category = 1) : base(25, 25)
		{
			_target = pm;
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
			AddLabel(360, 11, 82, $"{_target.AchievementPoints} Punktow");
			AddBackground(341, 522, 353, 26, 9200);

			int cnt = 0;
			if (!AchievementSystem.Categories.TryGetValue(category, out var reqCat))
			{
				Console.WriteLine("Couldnt find Achievement category: " + category);
				reqCat = AchievementSystem.Categories.Values.First();
			}

			foreach (var cur in AchievementSystem.Categories.Values)
			{
				var x = 90;
				var bgID = 9200;

				if (cur.Parent != null && cur.Id != reqCat.Id && cur.Parent != reqCat && cur.Parent != reqCat?.Parent)
					continue;
				if (cur.Parent != null)
					x += 20;
				if (cur.Id == category)
					bgID = 5120;

				AddBackground(x, 123 + (cnt * 31), 18810 / x, 25, bgID);
				if (cur.Id == category) // selected
					AddImage(x + 12, 129 + (cnt * 31), 1210);
				else
					AddButton(x + 12, 129 + (cnt * 31), 1209, 1210, (int)(5000 + cur.Id), GumpButtonType.Reply, 0);
				AddLabel(x + 32, 125 + (cnt * 31), 0, cur.Name);
				cnt++;
			}

			cnt = 0;
			foreach (var ac in AchievementSystem.Achievements.Values.Where(ac => ac.Category.Id == category))
			{
				if (ac.PreReq != null)
				{
					if (!_target.Achievements.ContainsKey(ac.PreReq))
						continue;
					if (_target.Achievements[ac.PreReq].Completed)
						continue;
				}

				if (_target.Achievements.TryGetValue(ac, out var achieve))
				{
					AddAchievement(ac, cnt, achieve);
				}
				else
				{
					if (ac.Secret)
						continue;
					AddAchievement(ac, cnt, null);
				}

				cnt++;
			}
		}

		private void AddAchievement(Achievement achievement, int i, AchievementStatus status)
		{
			if (status == null)
			{
				status = AchievementStatus.Empty;
			}
			var index = i % 4;
			if (index == 0)
			{
				AddButton(658, 524, 4005, 4006, 0, GumpButtonType.Page, (i / 4) + 1);
				AddPage((i / 4) + 1);
				AddLabel(484, 526, 32, "Strona " + ((i / 4) + 1));
				AddButton(345, 524, 4014, 4015, 0, GumpButtonType.Page, i / 4);
			}

			AddBackground(340, 122 + (index * 100), 347, 97, status.Completed ? 9300 : 9350);
			AddLabel(414, 131 + (index * 100), 49, achievement.Name);
			if (achievement.Icon > 0)
				AddItem(357, 147 + (index * 100), achievement.Icon);
			AddImageTiled(416, 203 + (index * 100), 95, 9, 9750);

			var step = 95.0 / achievement.Goal.Amount;
			var progress = Math.Min(achievement.Goal.GetProgress(_target), achievement.Goal.Amount);

			AddImageTiled(416, 203 + (index * 100), (int)(progress * step), 9, 9752);
			AddHtml(413, 152 + (index * 100), 194, 47, achievement.Description, true, true);
			if (status.Completed)
				AddLabel(566, 127 + (index * 100), 32, status.CompletedOn.ToShortDateString());

			AddLabel(522, 196 + (index * 100), 0, $"{progress} / {achievement.Goal.Amount}");

			AddBackground(628, 149 + (index * 100), 48, 48, 9200);
			AddLabel(648, 163 + (index * 100), 32, achievement.Points.ToString());
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 0)
				return;
			var btn = info.ButtonID - 5000;
			sender.Mobile.SendGump(new AchievementGump(_target, btn));
		}
	}
}
