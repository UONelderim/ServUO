using Server;
using System;

namespace Scripts.Mythik.Systems.Achievements
{
	public class AchievementStatus
	{
		public int ID { get; set; }
		public int Progress { get; set; }
		public DateTime CompletedOn { get; set; }
		public bool Completed => CompletedOn != default;

		public AchievementStatus() { }

		public AchievementStatus(GenericReader reader)
		{
			Deserialize(reader);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); // version
			writer.Write(ID);
			writer.Write(Progress);
			writer.Write(CompletedOn);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			ID = reader.ReadInt();
			Progress = reader.ReadInt();
			CompletedOn = reader.ReadDateTime();
		}
	}
}
