using Server;
using System;

namespace Nelderim.Achievements
{
	public class AchievementStatus
	{
		public static AchievementStatus Empty = new();
		public int Id { get; set; }
		public DateTime CompletedOn { get; set; }
		public bool Completed => CompletedOn != default;

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); // version
			writer.Write(Id);
			writer.Write(CompletedOn);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			Id = reader.ReadInt();
			CompletedOn = reader.ReadDateTime();
		}
	}
}
