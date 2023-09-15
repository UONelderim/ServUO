using System.Collections.Generic;
using Server;

namespace Nelderim.Achievements
{
	public class AchievementsInfo : NExtensionInfo
	{
		public Dictionary<Achievement, AchievementStatus> Achievements = new ();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.Write(Achievements.Count);
			foreach (var key in Achievements.Keys)
			{
				writer.Write(key.Id);
				Achievements[key].Serialize(writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			reader.ReadInt(); //version
			var count = reader.ReadInt();
			Achievements = new Dictionary<Achievement, AchievementStatus>(count);
			for (var i = 0; i < count; i++)
			{
				var key = reader.ReadInt();
				var value = new AchievementStatus();
				value.Deserialize(reader);
				Achievements[AchievementSystem.Achievements[key]] = value;
			}
		}
	}
}
