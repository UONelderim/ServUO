using System;
using Server;

namespace Nelderim
{
	public class QuestPointsHistoryEntry : IComparable<QuestPointsHistoryEntry>
	{
		public DateTime DateTime { get; set; }

		public string GameMaster { get; set; }

		public int Points { get; set; }

		public string Reason { get; set; }
		
		public string CharName { get; set; }

		public QuestPointsHistoryEntry(DateTime dateTime, string gameMaster, int points, string reason, string charName)
		{
			DateTime = dateTime;
			GameMaster = gameMaster;
			Points = points;
			Reason = reason;
			CharName = charName;
		}

		public QuestPointsHistoryEntry( GenericReader reader)
		{
			int version = reader.ReadInt();
			DateTime = reader.ReadDateTime();
			GameMaster = reader.ReadString();
			Points = reader.ReadInt();
			Reason = reader.ReadString();
		}


		public int CompareTo(QuestPointsHistoryEntry other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			return DateTime.CompareTo(other.DateTime);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.Write(DateTime);
			writer.Write(GameMaster);
			writer.Write(Points);
			writer.Write(Reason);
		}
	}
}
