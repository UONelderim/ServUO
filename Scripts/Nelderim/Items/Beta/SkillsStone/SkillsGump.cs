// ID 0000042
// ID 0000070

#region References

using System;
using System.Collections;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class PvPEditSkillGump : Gump
	{
		public static bool OldStyle = PropsConfig.OldStyle;

		public static int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static int GumpOffsetY = PropsConfig.GumpOffsetY;

		public static int TextHue = PropsConfig.TextHue;
		public static int TextOffsetX = PropsConfig.TextOffsetX;

		public static int OffsetGumpID = PropsConfig.OffsetGumpID;
		public static int HeaderGumpID = PropsConfig.HeaderGumpID;
		public static int EntryGumpID = PropsConfig.EntryGumpID;
		public static int BackGumpID = PropsConfig.BackGumpID;
		public static int SetGumpID = PropsConfig.SetGumpID;

		public static int SetWidth = PropsConfig.SetWidth;
		public static int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public static int SetButtonID1 = PropsConfig.SetButtonID1;
		public static int SetButtonID2 = PropsConfig.SetButtonID2;

		public static int PrevWidth = PropsConfig.PrevWidth;
		public static int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public static int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public static int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public static int NextWidth = PropsConfig.NextWidth;
		public static int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public static int NextButtonID1 = PropsConfig.NextButtonID1;
		public static int NextButtonID2 = PropsConfig.NextButtonID2;

		public static int OffsetSize = PropsConfig.OffsetSize;

		public static int EntryHeight = PropsConfig.EntryHeight;
		public static int BorderSize = PropsConfig.BorderSize;

		private static readonly int EntryWidth = 160;

		private static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
		private static readonly int TotalHeight = OffsetSize + (2 * (EntryHeight + OffsetSize));

		private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
		private static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;

		private readonly Mobile m_From;
		private readonly Mobile m_Target;
		private readonly Skill m_Skill;

		private readonly PvPSkillsGumpGroup m_Selected;

		public void UpdateSkillsToUse()
		{
			double SkillsCap = m_From.Skills.Cap / 10;
			SkillsToUse = SkillsCap - SkillsTotal;
		}

		public double SkillsToUse { get; set; }

		public double SkillsTotal
		{
			get { return m_From.SkillsTotal / 10; }
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			double SkillsCap = m_From.Skills.Cap / 10;
			UpdateSkillsToUse();

			if (info.ButtonID == 1)
			{
				try
				{
					TextRelay text = info.GetTextEntry(0);
					if (text != null)
					{
						double SkillAmount = Convert.ToDouble(text.Text);

						// ID 0000070
						if (SkillAmount > m_Skill.Cap)
							SkillAmount = m_Skill.Cap;

						if (SkillsTotal <= SkillsCap &&
						    (SkillAmount <= SkillsToUse + m_Skill.Base || SkillAmount <= m_Skill.Base))
						{
							m_Skill.Base = SkillAmount;
							UpdateSkillsToUse();
							m_From.PlaySound(0x411);
							m_From.FixedEffect(0x377A, 10, 16);
							if (SkillsToUse == 0)
								m_From.SendMessage(0x87, "Wykorzystales swoj limit umiejetnosci");
							else
								m_From.SendMessage(0x87, "Pozostalo Ci {0} punktow umiejetnosci", SkillsToUse);
						}
						else
						{
							m_Skill.Base = SkillsToUse;
							m_From.SendMessage(0x87, "Wykorzystales swoj limit umiejetnosci");
						}
					}

					m_From.SendGump(new PvPSkillsGump(m_From, m_Target, m_Selected));
				}
				catch
				{
					m_From.SendMessage("Niepoprawny format. Oczekiwano ###.#");
					m_From.SendGump(new PvPEditSkillGump(m_From, m_Target, m_Skill, m_Selected));
				}
			}
			else
			{
				m_From.SendGump(new PvPSkillsGump(m_From, m_Target, m_Selected));
			}
		}

		public PvPEditSkillGump(Mobile from, Mobile target, Skill skill, PvPSkillsGumpGroup selected)
			: base(GumpOffsetX, GumpOffsetY)
		{
			m_From = from;
			m_Target = target;
			m_Skill = skill;
			m_Selected = selected;

			string initialText = m_Skill.Base.ToString("F1");

			AddPage(0);

			AddBackground(0, 0, BackWidth, BackHeight, BackGumpID);
			AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), TotalHeight,
				OffsetGumpID);

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, skill.Name);
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
			{
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);
			}

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddTextEntry(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, 0, initialText);
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
			{
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);
			}

			AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 1, GumpButtonType.Reply, 0);
		}
	}

	public class PvPSkillsGump : Gump
	{
		public static bool OldStyle = PropsConfig.OldStyle;

		public static int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static int GumpOffsetY = PropsConfig.GumpOffsetY;

		public static int TextHue = PropsConfig.TextHue;
		public static int TextOffsetX = PropsConfig.TextOffsetX;

		public static int OffsetGumpID = PropsConfig.OffsetGumpID;
		public static int HeaderGumpID = PropsConfig.HeaderGumpID;
		public static int EntryGumpID = PropsConfig.EntryGumpID;
		public static int BackGumpID = PropsConfig.BackGumpID;
		public static int SetGumpID = PropsConfig.SetGumpID;

		public static int SetWidth = PropsConfig.SetWidth;
		public static int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public static int SetButtonID1 = PropsConfig.SetButtonID1;
		public static int SetButtonID2 = PropsConfig.SetButtonID2;

		public static int PrevWidth = PropsConfig.PrevWidth;
		public static int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public static int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public static int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public static int NextWidth = PropsConfig.NextWidth;
		public static int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public static int NextButtonID1 = PropsConfig.NextButtonID1;
		public static int NextButtonID2 = PropsConfig.NextButtonID2;

		public static int OffsetSize = PropsConfig.OffsetSize;

		public static int EntryHeight = PropsConfig.EntryHeight;
		public static int BorderSize = PropsConfig.BorderSize;

		private static readonly int NameWidth = 107;
		private static readonly int ValueWidth = 128;

		private static readonly int EntryCount = 15;

		private static readonly int TotalWidth =
			OffsetSize + NameWidth + OffsetSize + ValueWidth + OffsetSize + SetWidth + OffsetSize;

		private static int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

		private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;

		private static readonly int IndentWidth = 12;

		private readonly Mobile m_From;
		private readonly Mobile m_Target;

		private readonly PvPSkillsGumpGroup[] m_Groups;
		private readonly PvPSkillsGumpGroup m_Selected;

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonID = info.ButtonID - 1;

			int index = buttonID / 3;
			int type = buttonID % 3;

			switch (type)
			{
				case 0:
				{
					if (index >= 0 && index < m_Groups.Length)
					{
						PvPSkillsGumpGroup newSelection = m_Groups[index];

						if (m_Selected != newSelection)
						{
							m_From.SendGump(new PvPSkillsGump(m_From, m_Target, newSelection));
						}
						else
						{
							m_From.SendGump(new PvPSkillsGump(m_From, m_Target, null));
						}
					}

					break;
				}
				case 1:
				{
					if (m_Selected != null && index >= 0 && index < m_Selected.Skills.Length)
					{
						Skill sk = m_Target.Skills[m_Selected.Skills[index]];

						if (sk != null)
						{
							m_From.SendGump(new PvPEditSkillGump(m_From, m_Target, sk, m_Selected));
						}
						else
						{
							m_From.SendGump(new PvPSkillsGump(m_From, m_Target, m_Selected));
						}
					}

					break;
				}
				case 2:
				{
					if (m_Selected != null && index >= 0 && index < m_Selected.Skills.Length)
					{
						Skill sk = m_Target.Skills[m_Selected.Skills[index]];

						if (sk != null)
						{
							switch (sk.Lock)
							{
								case SkillLock.Up:
									sk.SetLockNoRelay(SkillLock.Down);
									sk.Update();
									break;
								case SkillLock.Down:
									sk.SetLockNoRelay(SkillLock.Locked);
									sk.Update();
									break;
								case SkillLock.Locked:
									sk.SetLockNoRelay(SkillLock.Up);
									sk.Update();
									break;
							}


							m_From.SendGump(new PvPSkillsGump(m_From, m_Target, m_Selected));
						}
					}

					break;
				}
			}
		}

		public int GetButtonID(int type, int index)
		{
			return 1 + (index * 3) + type;
		}

		public PvPSkillsGump(Mobile from, Mobile target)
			: this(from, target, null)
		{
		}

		public PvPSkillsGump(Mobile from, Mobile target, PvPSkillsGumpGroup selected)
			: base(GumpOffsetX, GumpOffsetY)
		{
			m_From = from;
			m_Target = target;

			m_Groups = PvPSkillsGumpGroup.Groups;
			m_Selected = selected;

			int count = m_Groups.Length;

			if (selected != null)
			{
				count += selected.Skills.Length;
			}

			int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

			AddPage(0);

			AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
			AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight,
				OffsetGumpID);

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) -
			                 (OldStyle ? SetWidth + OffsetSize : 0);

			if (OldStyle)
			{
				AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
			}
			else
			{
				AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);
			}

			x += PrevWidth + OffsetSize;

			if (!OldStyle)
			{
				AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0),
					EntryHeight, HeaderGumpID);
			}

			x += emptyWidth + OffsetSize;

			if (!OldStyle)
			{
				AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);
			}

			for (int i = 0; i < m_Groups.Length; ++i)
			{
				x = BorderSize + OffsetSize;
				y += EntryHeight + OffsetSize;

				PvPSkillsGumpGroup group = m_Groups[i];

				AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

				if (group == selected)
				{
					AddButton(x + PrevOffsetX, y + PrevOffsetY, 0x15E2, 0x15E6, GetButtonID(0, i), GumpButtonType.Reply,
						0);
				}
				else
				{
					AddButton(x + PrevOffsetX, y + PrevOffsetY, 0x15E1, 0x15E5, GetButtonID(0, i), GumpButtonType.Reply,
						0);
				}

				x += PrevWidth + OffsetSize;

				x -= (OldStyle ? OffsetSize : 0);

				AddImageTiled(x, y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID);
				AddLabel(x + TextOffsetX, y, TextHue, group.Name);

				x += emptyWidth + (OldStyle ? OffsetSize * 2 : 0);
				x += OffsetSize;

				if (SetGumpID != 0)
				{
					AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);
				}

				if (group == selected)
				{
					int indentMaskX = BorderSize;
					int indentMaskY = y + EntryHeight + OffsetSize;

					for (int j = 0; j < group.Skills.Length; ++j)
					{
						Skill sk = target.Skills[group.Skills[j]];

						x = BorderSize + OffsetSize;
						y += EntryHeight + OffsetSize;

						x += OffsetSize;
						x += IndentWidth;

						AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

						AddButton(x + PrevOffsetX, y + PrevOffsetY, 0x15E1, 0x15E5, GetButtonID(1, j),
							GumpButtonType.Reply, 0);

						x += PrevWidth + OffsetSize;

						x -= (OldStyle ? OffsetSize : 0);

						AddImageTiled(x, y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0) - OffsetSize - IndentWidth,
							EntryHeight, EntryGumpID);
						AddLabel(x + TextOffsetX, y, TextHue, sk == null ? "(null)" : sk.Name);

						x += emptyWidth + (OldStyle ? OffsetSize * 2 : 0) - OffsetSize - IndentWidth;
						x += OffsetSize;

						if (SetGumpID != 0)
						{
							AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);
						}

						if (sk != null)
						{
							int buttonID1, buttonID2;
							int xOffset, yOffset;

							switch (sk.Lock)
							{
								default:
								case SkillLock.Up:
									buttonID1 = 0x983;
									buttonID2 = 0x983;
									xOffset = 6;
									yOffset = 4;
									break;
								case SkillLock.Down:
									buttonID1 = 0x985;
									buttonID2 = 0x985;
									xOffset = 6;
									yOffset = 4;
									break;
								case SkillLock.Locked:
									buttonID1 = 0x82C;
									buttonID2 = 0x82C;
									xOffset = 5;
									yOffset = 2;
									break;
							}

							AddButton(x + xOffset, y + yOffset, buttonID1, buttonID2, GetButtonID(2, j),
								GumpButtonType.Reply, 0);

							y += 1;
							x -= OffsetSize;
							x -= 1;
							x -= 50;

							AddImageTiled(x, y, 50, EntryHeight - 2, OffsetGumpID);

							x += 1;
							y += 1;

							AddImageTiled(x, y, 48, EntryHeight - 4, EntryGumpID);

							AddLabelCropped(x + TextOffsetX, y - 1, 48 - TextOffsetX, EntryHeight - 3, TextHue,
								sk.Base.ToString("F1"));

							y -= 2;
						}
					}

					AddImageTiled(indentMaskX, indentMaskY, IndentWidth + OffsetSize,
						(group.Skills.Length * (EntryHeight + OffsetSize)) -
						(i < (m_Groups.Length - 1) ? OffsetSize : 0), BackGumpID + 4);
				}
			}
		}
	}

	public class PvPSkillsGumpGroup
	{
		public string Name { get; }

		public SkillName[] Skills { get; }

		public PvPSkillsGumpGroup(string name, SkillName[] skills)
		{
			Name = name;
			Skills = skills;

			Array.Sort(Skills, new SkillNameComparer());
		}

		private class SkillNameComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				SkillName a = (SkillName)x;
				SkillName b = (SkillName)y;

				string aName = SkillInfo.Table[(int)a].Name;
				string bName = SkillInfo.Table[(int)b].Name;

				return aName.CompareTo(bName);
			}
		}

		public static PvPSkillsGumpGroup[] Groups { get; } =
		{
			new PvPSkillsGumpGroup("Crafting",
				new[]
				{
					SkillName.Alchemy, SkillName.Blacksmith, SkillName.Cartography, SkillName.Carpentry,
					SkillName.Cooking, SkillName.Fletching, SkillName.Inscribe, SkillName.Tailoring,
					SkillName.Tinkering
				}),
			new PvPSkillsGumpGroup("Bardic",
				new[]
				{
					SkillName.Discordance, SkillName.Musicianship, SkillName.Peacemaking, SkillName.Provocation
				}),
			new PvPSkillsGumpGroup("Magical",
				new[]
				{
					SkillName.Chivalry, SkillName.EvalInt, SkillName.Magery, SkillName.MagicResist,
					SkillName.Meditation, SkillName.Necromancy, SkillName.SpiritSpeak, SkillName.Ninjitsu,
					SkillName.Bushido
				}),
			new PvPSkillsGumpGroup("Miscellaneous",
				new[]
				{
					SkillName.Camping, SkillName.Fishing, SkillName.Focus, SkillName.Healing, SkillName.Herding,
					SkillName.Lockpicking, SkillName.Lumberjacking, SkillName.Mining, SkillName.Snooping,
					SkillName.Veterinary
				}),
			new PvPSkillsGumpGroup("Combat Ratings",
				new[]
				{
					SkillName.Archery, SkillName.Fencing, SkillName.Macing, SkillName.Parry, SkillName.Swords,
					SkillName.Tactics, SkillName.Wrestling
				}),
			new PvPSkillsGumpGroup("Actions",
				new[]
				{
					SkillName.AnimalTaming, SkillName.Begging, SkillName.DetectHidden, SkillName.Hiding,
					SkillName.RemoveTrap, SkillName.Poisoning, SkillName.Stealing, SkillName.Stealth,
					SkillName.Tracking
				}),
			new PvPSkillsGumpGroup("Lore & Knowledge",
				new[]
				{
					SkillName.Anatomy, SkillName.AnimalLore, SkillName.ArmsLore, SkillName.Herbalism,
					SkillName.ItemID, SkillName.TasteID
				})
		};
	}
}
