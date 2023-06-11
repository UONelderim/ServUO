using System.Collections.Generic;
using Nelderim;
using Server;

namespace Scripts.Mythik.Systems.Achievements
{
	public class AchievementInfo : NExtensionInfo
	{
		public Dictionary<int, AchievementStatus> Achievements = new Dictionary<int, AchievementStatus>();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.Write(Achievements.Count);
			foreach (var key in Achievements.Keys)
			{
				writer.Write(key);
				Achievements[key].Serialize(writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			reader.ReadInt(); //version
			var count = reader.ReadInt();
			Achievements = new Dictionary<int, AchievementStatus>(count);
			for (var i = 0; i < count; i++)
			{
				var key = reader.ReadInt();
				var value = new AchievementStatus(reader);
				Achievements[key] = value;
			}
		}
	}
}
