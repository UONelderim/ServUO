using Server;
using System.Collections.Generic;
using Nelderim;
using Server.Mobiles;
using Server.Commands;

namespace Scripts.Mythik.Systems.Achievements
{
	public partial class AchievementSystem : NExtension<AchievementInfo>
	{
		public static string ModuleName = "Achievements";

		internal static Dictionary<int, BaseAchievement> Definitions = new Dictionary<int, BaseAchievement>();
		internal static Dictionary<int, AchievementCategory> Categories = new Dictionary<int, AchievementCategory>();

		public static void Initialize()
		{
			RegisterAchievements();
			CommandSystem.Register("achievements", AccessLevel.Player, OpenGumpCommand);
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}

		public static void OpenGump(Mobile from, Mobile target)
		{
			if (from == null || target == null) return;

			if (target is PlayerMobile player)
			{
				from.SendGump(new AchievementGump(player.Achievements, player.AchievementPoints));
			}
		}

		[Usage("osiagniecia"), Aliases("achievements")]
		[Description("Opens the Achievements gump")]
		private static void OpenGumpCommand(CommandEventArgs e)
		{
			OpenGump(e.Mobile, e.Mobile);
		}
	}
}
