#region References

using System;
using Server.Gumps;

#endregion

namespace Server.Mobiles
{
	public class PlayerAnatomyGump : BaseGump
	{
		private readonly int _Label = 0xF424E5;

		public PlayerMobile Player { get; private set; }

		public PlayerAnatomyGump(PlayerMobile pm, PlayerMobile targ)
			: base(pm, 250)
		{
			Player = targ;
		}

		public override void AddGumpLayout()
		{
			AddPage(0);
			AddBackground(0, 24, 310, 285, 0x24A4); // 285 was 325
			AddHtml(47, 32, 210, 18, ColorAndCenter("#000080", Player.TitleName), false, false); //Or just .name?

			AddButton(140, 0, 0x82D, 0x82D, 0, GumpButtonType.Reply, 0);

			AddImage(40, 62, 0x82B);
			AddImage(40, 258, 0x82B);

			AddPage(1);

			AddImage(28, 76, 0x826);
			AddHtmlLocalized(47, 74, 160, 18, 1049593, 0xC8, false, false); // Attributes

			AddHtmlLocalized(53, 92, 160, 18, 1049578, _Label, false, false); // Hits
			AddHtml(180, 92, 75, 18, FormatAttributes(Player.Hits, Player.HitsMax), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1049579, _Label, false, false); // Stamina
			AddHtml(180, 110, 75, 18, FormatAttributes(Player.Stam, Player.StamMax), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1049580, _Label, false, false); // Mana
			AddHtml(180, 128, 75, 18, FormatAttributes(Player.Mana, Player.ManaMax), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1028335, _Label, false, false); // Strength
			AddHtml(180, 146, 75, 18, FormatStat(Player.Str), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 3000113, _Label, false, false); // Dexterity
			AddHtml(180, 164, 75, 18, FormatStat(Player.Dex), false, false);

			AddHtmlLocalized(53, 182, 160, 18, 3000112, _Label, false, false); // Intelligence
			AddHtml(180, 182, 75, 18, FormatStat(Player.Int), false, false);
		}


		private static string FormatAttributes(int cur, int max)
		{
			if (max == 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0}/{1}</div>", cur, max);
		}

		private static string FormatStat(int val)
		{
			if (val == 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0}</div>", val);
		}
	}
}
