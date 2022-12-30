using System.Collections.Generic;
using Nelderim;
using Server.Mobiles;
using Server.Network;

namespace Server.SkillHandlers
{
	public partial class AnimalTaming
	{
		private static readonly List<string> _RegularTamingSpeech = new List<string> 
		{
			"Czy zostaniesz moim przyjacielem?",
			"Zawsze chcialem miec takiego towarzysza",
			"Nie boj sie",
			"Nie zrobie ci krzywdy",
			"Obiecuje dobrze sie toba opiekowac",
			"Bedziesz podrozowac ze mna, piekne stworzenie?",
			"Jezeli pojdziesz za mna, mozemy sie razem chronic",
			"Moge cie chronic przed niebezpieczenstwami swiata...",
			"To bedzie zaszczyt moc z toba przemierzac swiat",
			"Tutaj...",
			"Jakie mile stworzenie...",
			"Dobrze...",
			"Chodz tutaj..." 
		};
		
		private static readonly List<string> _DrowTamingSpeech = new List<string>
		{
			"Czy zostaniesz moim sluga?",
			"Przyda mi sie taki poddany",
			"Nie boj sie",
			"Nie uderze zbyt mocno",
			"Bede dobrze cie karmic",
			"Bedziesz podrozowac ze mna, dzikie stworzenie?",
			"Jezeli pojdziesz za mna, bedziesz moc mnie chronic",
			"Moge ci pokazac niebezpieczenstwa tego swiata...",
			"Bedziesz zaszczycony mogac ze mna przemierzac swiat",
			"Tutaj...",
			"Jakie spokojne stworzenie...",
			"Dobrze...",
			"Chodz tutaj..."
		};
		
		private static void TamerSpeech(Mobile tamer)
		{
			var wordList = tamer.Race == Race.NDrow ? _DrowTamingSpeech : _RegularTamingSpeech;
			var toSay = Utility.RandomList(wordList);
			if (tamer is PlayerMobile pm)
			{
				Translate.SayPublic(pm, toSay);
			}
			else
			{
				tamer.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, toSay);
			}
		}
	}
}
