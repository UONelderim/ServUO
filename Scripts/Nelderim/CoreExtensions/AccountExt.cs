using System;
using System.Collections.Generic;
using System.Xml;
using Nelderim;
using Server.Nelderim;

namespace Server.Accounting
{
	public partial class Account
	{
		public int QuestPoints { get; set; }
		
		public DateTime LastQuestPointsTime { get; set; }
		
		public SortedSet<QuestPointsHistoryEntry> QuestPointsHistory { get; set; } = new SortedSet<QuestPointsHistoryEntry>();
		
		public Faction Faction { get; set; } = Faction.None;

		private void SaveNelderim(XmlElement account)
		{
			AppendNode(account, "questPoints", XmlConvert.ToString(QuestPoints));
			AppendNode(account, "lastQuestPointsTime", XmlConvert.ToString(LastQuestPointsTime));
			if (QuestPointsHistory?.Count > 0)
			{
				var xmlqph = AppendNode(account, "questPointsHistory", null, ("count", XmlConvert.ToString(QuestPointsHistory.Count)));

				foreach (var qph in QuestPointsHistory)
				{
					AppendNode(xmlqph, "entry", qph.Reason, 
						("gm", qph.GameMaster), 
						("dateTime", qph.DateTime.ToString()),
						("points", qph.Points.ToString()),
						("charName", qph.CharName)
						);
				}
			}
			AppendNode(account, "faction", Faction.ToString());
		}

		private void LoadNelderim(XmlElement node)
		{
			QuestPoints = Utility.GetXMLInt32(Utility.GetText(node["questPoints"], "0"), 0);
			LastQuestPointsTime = Utility.GetXMLDateTime(Utility.GetText(node["lastQuestPointsTime"], "0"), DateTime.MinValue);
			
			var xmlqph = node["questPointsHistory"];

			if (xmlqph == null)
			{
				return;
			}

			var elements = xmlqph.GetElementsByTagName("entry");
			
			foreach (XmlElement entry in elements)
			{
				var dateTime = Utility.GetXMLDateTime(Utility.GetAttribute(entry,"dateTime", ""), DateTime.MinValue);
				var gm = Utility.GetAttribute(entry,"gm", "");
				var points = Utility.GetXMLInt32(Utility.GetAttribute(entry,"points", "0"),0);
				var charName = Utility.GetAttribute(entry, "charName", "");
				var reason = Utility.GetText(entry, "");
				QuestPointsHistory.Add(new QuestPointsHistoryEntry(dateTime, gm, points, reason, charName));
			}
			
			Faction = Faction.Parse(Utility.GetText(node["faction"], Faction.None.ToString()));
		}
	}
}
