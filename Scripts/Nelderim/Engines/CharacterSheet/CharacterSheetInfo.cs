#region References

using System;
using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim
{
	public enum EventFrequency
	{
		F30M,
		F60M,
		F90M,
		FLonger
	}

	public enum CrestSize
	{
		Small,
		Medium,
		Large
	}

	class CharacterSheetInfo : NExtensionInfo
	{
		public int Crest { get; set; }

		public CrestSize CrestSize { get; set; }

		public string AppearanceAndCharacteristic { get; set; }

		public string HistoryAndProfession { get; set; }

		public bool AttendenceInEvents { get; set; }

		public EventFrequency EventFrequencyAttendence { get; set; }

		public int QuestPoints { get; set; }

		public DateTime LastQuestPointsTime { get; set; }
		
		public SortedSet<QuestPointsHistoryEntry> QuestPointsHistory { get; set; } = new SortedSet<QuestPointsHistoryEntry>();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)1 ); //version
			writer.Write(Crest);
			writer.Write((int)CrestSize);
			writer.Write(AppearanceAndCharacteristic);
			writer.Write(HistoryAndProfession);
			writer.Write(AttendenceInEvents);
			writer.Write((int)EventFrequencyAttendence);
			writer.Write(QuestPoints);
			writer.Write(LastQuestPointsTime);
			writer.Write(QuestPointsHistory.Count);
			foreach (var questPointsHistoryEntry in QuestPointsHistory)
			{
				questPointsHistoryEntry.Serialize(writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			Crest = reader.ReadInt();
			CrestSize = (CrestSize)reader.ReadInt();
			AppearanceAndCharacteristic = reader.ReadString();
			HistoryAndProfession = reader.ReadString();
			AttendenceInEvents = reader.ReadBool();
			EventFrequencyAttendence = (EventFrequency)reader.ReadInt();
			QuestPoints = reader.ReadInt();
			LastQuestPointsTime = reader.ReadDateTime();
			if (version > 0)
			{
				var questPointsHistoryEntryCount = reader.ReadInt();
				QuestPointsHistory = new SortedSet<QuestPointsHistoryEntry>();
				for (int i = 0; i < questPointsHistoryEntryCount; i++)
				{
					QuestPointsHistory.Add(new QuestPointsHistoryEntry(reader));
				}
			}
		}
	}
}
