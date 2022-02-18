#region References

using Server.Gumps;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.SicknessSys.Gumps
{
	public class PowerGump : Gump
	{
		private readonly VirusCell Cell;

		public PowerGump(PlayerMobile pm, VirusCell cell, int beat, int x = 25, int y = 25, bool IsBeat = false) :
			base(x, y)
		{
			if (pm != null)
			{
				pm.CloseGump(typeof(PowerGump));

				Cell = cell;

				if (SicknessHelper.IsNight(cell.PM))
					cell.Hue = 1175;
				else
					cell.Hue = 1157;

				Closable = false;
				Dragable = false;

				int VeinHue = 1;
				int LabelHue = 1;

				if (IsBeat)
				{
					pm.PlaySound(0x24C);

					VeinHue = beat;

					if (cell.InDebug)
						cell.SendDebugInfo();

					if (cell.Stage == 1)
						LabelHue = 37;
					else if (cell.Stage == 2)
						LabelHue = 53;
					else
						LabelHue = 1153;

					if (cell.Power < 1 || cell.Level > 99)
					{
						if (!SicknessHelper.IsSpecialVirus(cell))
						{
							if (cell.Stage == 1)
								AddImage(x + 1, y + 2, 1643, 37);
							else if (cell.Stage == 2)
								AddImage(x + 1, y + 2, 1643, 53);
							else
								AddImage(x + 1, y + 2, 1643, 1153);
						}
						else if (SicknessHelper.IsNight(pm))
						{
							if (cell.Illness == IllnessType.Vampirism)
								AddImage(x + 1, y + 2, 1643, 1174);
							else if (cell.Stage == 2)
								AddImage(x + 1, y + 2, 1643, 1176);
						}
					}
				}

				AddImage(x, y, 105, beat);

				AddImage(x - 12, y + 15, 30063, VeinHue);

				int Mod41 = GetMod41(cell);

				if (cell.Level < 10)
					AddLabel(x + 29 + Mod41, y + 16, LabelHue, "" + cell.Level);
				else if (cell.Level < 100)
					AddLabel(x + 25 + Mod41, y + 16, LabelHue, "" + cell.Level);
				else
					AddLabel(x + 21 + Mod41, y + 16, LabelHue, "" + cell.Level);

				AddButton(x + 25, y + 35, 1625, 1626, 0, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			Cell.IsMovingGump = true;

			from.Target = new ScreenTarget(Cell);
		}

		private static int GetMod41(VirusCell cell)
		{
			int Mod = 0;

			if (cell.Level.ToString().Contains("1"))
			{
				foreach (char letter in cell.Level.ToString())
				{
					if (letter == '1')
					{
						Mod++;
					}
				}
			}

			return Mod;
		}
	}
}
