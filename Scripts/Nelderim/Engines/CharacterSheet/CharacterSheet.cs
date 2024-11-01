#region References

using System.Collections.Generic;
using Server;
using Server.Mobiles;

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

	class CharacterSheet() : NExtension<CharacterSheetInfo>("CharacterSheet")
	{
		public static new void Initialize()
		{
			Register(new CharacterSheet());
		}
	}
	
	class CharacterSheetInfo : NExtensionInfo
	{
		public int Crest { get; set; }

		public CrestSize CrestSize { get; set; }

		public string AppearanceAndCharacteristic { get; set; }

		public string HistoryAndProfession { get; set; }

		public bool AttendenceInEvents { get; set; }

		public EventFrequency EventFrequencyAttendence { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)2 ); //version
			writer.Write(Crest);
			writer.Write((int)CrestSize);
			writer.Write(AppearanceAndCharacteristic);
			writer.Write(HistoryAndProfession);
			writer.Write(AttendenceInEvents);
			writer.Write((int)EventFrequencyAttendence);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt(); //version
			Crest = reader.ReadInt();
			CrestSize = (CrestSize)reader.ReadInt();
			AppearanceAndCharacteristic = reader.ReadString();
			HistoryAndProfession = reader.ReadString();
			AttendenceInEvents = reader.ReadBool();
			EventFrequencyAttendence = (EventFrequency)reader.ReadInt();
			if (version < 2)
			{
				PlayerMobile pm = World.FindMobile(Serial) as PlayerMobile;
				var questPoints = reader.ReadInt();
				var lastQuestPointsTime = reader.ReadDateTime();
				if (pm != null)
				{
					pm.QuestPoints += questPoints;
					pm.LastQuestPointsTime = lastQuestPointsTime;
				}
				if (version > 0)
				{
					var questPointsHistoryEntryCount = reader.ReadInt();
					var questPointsHistory = new SortedSet<QuestPointsHistoryEntry>();
					for (int i = 0; i < questPointsHistoryEntryCount; i++)
					{
						var qphe = new QuestPointsHistoryEntry(reader);
						qphe.CharName = pm?.Name;
						questPointsHistory.Add(qphe);
					}
					pm?.QuestPointsHistory.UnionWith(questPointsHistory);
				}
			}
		}
	}
}
