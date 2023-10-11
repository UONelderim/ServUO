using System.Collections.Generic;
using Server;
using Nelderim.Achievements.Gumps;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;

namespace Nelderim.Achievements
{
	public partial class AchievementSystem : NExtension<AchievementsInfo>
	{
		public static string ModuleName = "Achievements";

		public static AchievementRegistry<AchievementCategory> CategoryRegistry = new("achievementCategories");
		public static AchievementRegistry<Achievement> AchievementRegistry = new("achievements");
		
		public static Dictionary<int, Achievement> Achievements => AchievementRegistry.Entries;
		public static Dictionary<int, AchievementCategory> Categories => CategoryRegistry.Entries;

		
		public static void Initialize()
		{
			CategoryRegistry.Load();
			AchievementRegistry.Load();

			RegisterAchievements();

			CommandSystem.Register("achievements", AccessLevel.Player, AchievementsCommand);
			EventSink.WorldSave += Save;
			Load(ModuleName);

			CategoryRegistry.Save();
			AchievementRegistry.Save();
		}

		private static AchievementCategory Register(AchievementCategory category)
		{
			return CategoryRegistry.Register(category);
		}

		private static Achievement Register(Achievement achievement)
		{
			return AchievementRegistry.Register(achievement);
		}
		
		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
		
		private static void AchievementsCommand(CommandEventArgs e)
		{
			var pm = e.Mobile as PlayerMobile;
			if (pm == null) return;
			if (pm.AccessLevel <= AccessLevel.Counselor)
			{
				pm.SendGump(new AchievementGump(pm));
			}
			else
			{
				pm.BeginTarget(8, false, TargetFlags.None, (from, target) =>
					{
						if (target is PlayerMobile targetPm)
							from.SendGump(new AchievementGump(targetPm));
					}
				);
			}
		}
	}
}
