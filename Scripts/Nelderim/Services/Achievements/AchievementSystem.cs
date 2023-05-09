using Server;
using System;
using System.Collections.Generic;
using Nelderim;
using Server.Mobiles;
using Server.Commands;
using Scripts.Mythik.Systems.Achievements.Gumps;

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

		internal static void SetAchievementStatus(PlayerMobile player, BaseAchievement achievement, int progress)
		{
			var achieves = player.Achievements;

			if (achieves.ContainsKey(achievement.ID))
			{
				if (achieves[achievement.ID].Progress >= achievement.CompletionTotal)
					return;
				achieves[achievement.ID].Progress += progress;
			}
			else
			{
				achieves.Add(achievement.ID, new AchievementStatus { Progress = progress });
			}

			if (achieves[achievement.ID].Progress >= achievement.CompletionTotal)
			{
				player.SendGump(new AchievementObtainedGump(achievement));
				achieves[achievement.ID].CompletedOn = DateTime.UtcNow;

				player.AchievementPoints += achievement.Points;

				if (achievement.RewardItems == null || achievement.RewardItems.Length <= 0) return;
				
				try
				{
					player.SendAsciiMessage("Otrzymales nagrode za zdobycie tego osiagniecia!");
					var item = (Item)Activator.CreateInstance(achievement.RewardItems[0]);
					player.AddToBackpack(item);
				}
				catch(Exception e)
				{
					Console.WriteLine("Exception in achievement system");
					Console.WriteLine(e);
				}
			}
		}
	}
}
