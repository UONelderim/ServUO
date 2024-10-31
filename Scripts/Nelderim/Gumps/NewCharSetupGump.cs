using System;
using System.Linq;
using Server.Gumps;
using Server.Misc;
using Server.Network;

namespace Server.Nelderim.Gumps
{
	public class NewCharSetupGump : Gump
	{
		private const int MAX_TOTAL_STATS = 80;
		private const int MAX_SINGLE_STAT = 60;
		private const int MIN_SINGLE_STAT = 10;

		private enum NewCharPage
		{
			Stat,
			Skill,
			Summary
		}

		private struct NewCharInfo
		{
			public int NewStr;
			public int NewDex;
			public int NewInt;
			public readonly int[] Skills;

			public NewCharInfo()
			{
				NewStr = 0;
				NewDex = 0;
				NewInt = 0;
				Skills = [-1, -1, -1];
			}
		};

		public static void Initialize()
		{
			EventSink.Login += e => Check(e.Mobile);
		}

		public static void Check(Mobile m)
		{
			if (m.Name.StartsWith(RenameGump.DEFAULT_PREFIX))
			{
				m.Frozen = true;
				m.SendGump(new RenameGump());
			}
			else if (m.RawStr <= 10 && m.RawDex <= 10 && m.RawInt <= 10) //Not initialized
			{
				m.Frozen = true;
				m.SendGump(new NewCharSetupGump());
			}
		}

		private NewCharPage _Page;
		private string _Status;
		private NewCharInfo _Info;

		public NewCharSetupGump() : this(NewCharPage.Stat, "", new NewCharInfo())
		{
		}

		private NewCharSetupGump(NewCharPage page, string status, NewCharInfo info) : base(50, 50)
		{
			_Page = page;
			_Status = status;
			_Info = info;

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			switch (page)
			{
				case NewCharPage.Stat:
					AddStatPage();
					break;
				case NewCharPage.Skill:
					AddSkillPage();
					break;
				case NewCharPage.Summary:
					AddSummaryPage();
					break;
			}
		}

		private void AddStatPage()
		{
			AddBackground(50, 50, 437, 215, 2620);
			AddLabel(180, 67, 1160, "Wybor poczatkowych statsytk");
			AddLabel(84, 106, 0x20, _Status);
			AddLabel(84, 146, 1152, "Sila:");
			AddLabel(208, 146, 1152, "Zrecznosc:");
			AddLabel(348, 146, 1152, "Inteligencja:");
			AddImageTiled( 114, 146, 20, 20, 0xBBC );
			AddTextEntry(114, 146, 50, 20, 1359, 0, _Info.NewStr > 0 ? _Info.NewStr.ToString() : "");
			AddImageTiled( 272, 146, 20, 20, 0xBBC );
			AddTextEntry(272, 146, 50, 20, 1359, 1, _Info.NewDex > 0 ? _Info.NewDex.ToString() : "");
			AddImageTiled( 420, 146, 20, 20, 0xBBC );
			AddTextEntry(420, 146, 50, 20, 1359, 2, _Info.NewInt > 0 ? _Info.NewInt.ToString() : "");
			AddButton(400, 232, 12009, 12010, 2, GumpButtonType.Reply, 0);
			AddLabel(170, 200, 1152, $"* Suma powinna wynosic {MAX_TOTAL_STATS} *");
		}

		private void AddSkillPage()
		{
			AddBackground(50, 50, 437, 215, 2620);
			AddLabel(180, 67, 1160, "Wybor poczatkowych umiejetnosci");
			AddLabel(84, 96, 0x20, _Status);

			AddButton(75, 121, 0x4D6, 0x4D7, 11, GumpButtonType.Reply, 0);
			AddLabel(70, 156, 1152, GetSkillName(_Info.Skills[0]));
			AddButton(215, 121, 0x4D9, 0x4DA, 12, GumpButtonType.Reply, 0);
			AddLabel(210, 156, 1152, GetSkillName(_Info.Skills[1]));
			AddButton(355, 121, 0x4DC, 0x4DD, 13, GumpButtonType.Reply, 0);
			AddLabel(350, 156, 1152, GetSkillName(_Info.Skills[2]));

			AddLabel(120, 200, 1152, $"* Wybrane umiejetnosci beda na poziomie 50.0 *");

			//Back,Continue
			AddButton(65, 232, 12015, 12016, 1, GumpButtonType.Reply, 0);
			AddButton(400, 232, 12009, 12010, 2, GumpButtonType.Reply, 0);
		}

		private string GetSkillName(int skillIndex)
		{
			if (skillIndex < 0 || skillIndex > SkillInfo.Table.Length)
			{
				return "Nie wybrano";
			}

			return SkillInfo.Table[skillIndex].Name;
		}

		private void AddSummaryPage()
		{
			AddBackground(50, 50, 437, 215, 2620);
			AddLabel(190, 67, 1160, "Podsumowanie postaci");
			AddLabel(114, 116, 1160, "Statystyki");
			AddLabel(114, 136, 1152, $"Sila: {_Info.NewStr}");
			AddLabel(114, 156, 1152, $"Zrecznosc: {_Info.NewDex}");
			AddLabel(114, 176, 1152, $"Inteligencja: {_Info.NewInt}");
			AddLabel(304, 116, 1160, "Umiejetnosci");
			AddLabel(304, 136, 1152, $"1. {GetSkillName(_Info.Skills[0])}");
			AddLabel(304, 156, 1152, $"2. {GetSkillName(_Info.Skills[1])}");
			AddLabel(304, 176, 1152, $"3. {GetSkillName(_Info.Skills[2])}");
			
			//Back,Continue
			AddButton(65, 232, 12015, 12016, 1, GumpButtonType.Reply, 0);
			AddButton(400, 232, 12000, 12001, 2, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			var m = sender.Mobile;
			if (_Page == NewCharPage.Stat)
			{
				if (!Int32.TryParse(info.GetTextEntry(0).Text, out var newStr))
				{
					Resend(m, _Page, "Nieprawodilowa wartosc sily.");
					return;
				}
				_Info.NewStr = newStr;
				
				if (!Int32.TryParse(info.GetTextEntry(1).Text, out var newDex))
				{
					Resend(m, _Page, "Nieprawodilowa wartosc zrecznosci.");
					return;
				}
				_Info.NewDex = newDex;
				
				if (!Int32.TryParse(info.GetTextEntry(2).Text, out var newInt))
				{
					Resend(m, _Page, "Nieprawodilowa wartosc inteligencji.");
					return;
				}
				_Info.NewInt = newInt;
				
				if (newStr is < MIN_SINGLE_STAT or > MAX_SINGLE_STAT)
				{
					Resend(m, _Page, $"Wartosc sily musi byc pomiedzy {MIN_SINGLE_STAT}-{MAX_SINGLE_STAT}");
					return;
				}
				if (newDex is < MIN_SINGLE_STAT or > MAX_SINGLE_STAT)
				{
					Resend(m, _Page,
						$"Wartosc zrecznosci musi byc pomiedzy {MIN_SINGLE_STAT}-{MAX_SINGLE_STAT}");
					return;
				}
				if (newInt is < MIN_SINGLE_STAT or > MAX_SINGLE_STAT)
				{
					Resend(m, _Page,
						$"Wartosc inteligencji musi byc pomiedzy {MIN_SINGLE_STAT}-{MAX_SINGLE_STAT}");
					return;
				}
				

				var sum = newStr + newDex + newInt;
				if (sum != MAX_TOTAL_STATS)
				{
					Resend(m, _Page, $"Niewlasciwa suma statystyk: {sum}");
					return;
				}

				Resend(m, info.ButtonID == 0 ? NewCharPage.Stat : NewCharPage.Skill);
				return;
			}

			if (_Page == NewCharPage.Skill)
			{
				if (info.ButtonID == 1)
				{
					Resend(m, NewCharPage.Stat);
					return;
				}

				if (info.ButtonID == 11)
				{
					SendSkillSelectGump(m, 0);
					return;
				}

				if (info.ButtonID == 12)
				{
					SendSkillSelectGump(m, 1);
					return;
				}

				if (info.ButtonID == 13)
				{
					SendSkillSelectGump(m, 2);
					return;
				}
				if (_Info.Skills.Any(s => s == -1))
				{
					Resend(m, _Page, "Musisz wybrac 3 umiejetnosci.");
					return;
				}

				Resend(m, info.ButtonID == 0 ? NewCharPage.Skill : NewCharPage.Summary);
			}

			if (_Page == NewCharPage.Summary)
			{
				if (info.ButtonID == 1)
				{
					Resend(m, NewCharPage.Skill);
					return;
				}

				if (info.ButtonID == 2)
				{
					m.InitStats(_Info.NewStr, _Info.NewDex, _Info.NewInt);
					for (var index = 0; index < 3; index++)
					{
						var infoSkill = _Info.Skills[index];
						var skill = m.Skills[infoSkill];

						if (skill != null)
						{
							skill.BaseFixedPoint = 500;
							CharacterCreation.AddSkillItems(skill.SkillName, m);
						}
					}

					m.Frozen = false;
					m.SendMessage(0x40, "Inicjalizacja postaci zakonczona");
					return;
				}
				Resend(m, _Page);
			}
		}

		private void Resend(Mobile m, NewCharPage page, string status = "")
		{
			m.CloseGump<NewCharSetupGump>();
			m.SendGump(new NewCharSetupGump(page, status, _Info));
		}

		private void SendSkillSelectGump(Mobile m, int index)
		{
			var config = new SkillSelectGump.SkillSelectConfiguration();
			config.CancelCallback = () => Resend(m, NewCharPage.Skill);
			config.DisabledSkills = _Info.Skills;
			var gump = new SkillSelectGump(
				s =>
				{
					_Info.Skills[index] = s.SkillID;
					Resend(m, NewCharPage.Skill);
				},
				config
			);
			m.SendGump(gump);
		}
	}
}
