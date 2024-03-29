using Server.Gumps;
using Server.Mobiles;

namespace Server.Services.TownCryer
{
	public class TownCryerCityGump : BaseTownCryerGump
	{
		public TownCryerCityEntry Entry { get; private set; }

		public TownCryerCityGump(PlayerMobile pm, TownCrier cryer, TownCryerCityEntry entry)
			: base(pm, cryer)
		{
			Entry = entry;
		}

		public override void AddGumpLayout()
		{
			base.AddGumpLayout();

			AddHtml(57, 155, 724, 20, Entry?.Title ?? "Untitled", false, false);
			AddHtmlLocalized(57, 180, 724, 20, 1154760, Entry?.Author ?? "Unknown", 0, false, false); // By: ~1_NAME~

			AddHtml(57, 215, 724, 205, Entry?.Body ?? "", false, false);

			AddImage(85, 425, 0x5EF);
		}

		public override void OnResponse(RelayInfo info)
		{
			if (info.ButtonID == 0)
			{
				var gump = new TownCryerGump(User, Cryer)
				{
					Category = TownCryerGump.GumpCategory.City
				};
				_ = SendGump(gump);
			}
		}
	}
}
