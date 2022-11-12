#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Nelderim
{
	public static class Translate
	{
		public static void Initialize()
		{
			// Register our speech handler
			EventSink.Speech += EventSink_Speech;
		}

		private static void EventSink_Speech(SpeechEventArgs args)
		{
			PlayerMobile from = args.Mobile as PlayerMobile;
			string mySpeech = args.Speech;
			if (from == null || mySpeech == null || args.Type == MessageType.Emote ||
			    from.LanguageSpeaking == Language.Powszechny)
			{
				return;
			}

			args.Blocked = true;
			int tileLength = 15;

			switch (args.Type)
			{
				case MessageType.Yell:
					tileLength = 18;
					break;
				case MessageType.Whisper:
					tileLength = 1;
					break;
			}

			foreach (Mobile m in from.Map.GetMobilesInRange(from.Location, tileLength))
			{
				if (m is PlayerMobile pm)
				{
					SayTo(from, pm, mySpeech);
				}
				else
				{
					m.OnSpeech(args);
				}
			}
		}
		
		private static void SayTo(PlayerMobile from, PlayerMobile to, string text)
		{
			from.RevealingAction();
			
			if (from == to || to.KnowsLanguage(from.LanguageSpeaking) )
			{
				from.SayTo(to, $"[{from.LanguageSpeaking.ToString()}] {text} ");
			}
			else
			{
				from.SayTo(to, CommonToForeign(text, from.LanguageSpeaking));
			}
		}

		public static void SayPublic(PlayerMobile from, string text)
		{
			foreach (Mobile m in from.Map.GetMobilesInRange(from.Location, 18))
			{
				if (m.Player)
				{
					SayTo(from, m as PlayerMobile, text);
				}
			}
		}
		
		public static bool KnowsLanguage(this Mobile m, Language lang)
		{
			if (m is PlayerMobile pm)
			{
				pm.LanguagesKnown.Get(lang);
			}

			return false;
		}

		public static String CommonToForeign(String speech, Language lang)
		{
			return lang switch
			{
				Language.Krasnoludzki => TranslateUsingDict(speech, LanguagesDictionary.Krasnoludzki),
				Language.Elficki => TranslateUsingDict(speech, LanguagesDictionary.Elficki),
				Language.Drowi => TranslateUsingDict(speech, LanguagesDictionary.Drowi),
				Language.Jarlowy => TranslateUsingDict(speech, LanguagesDictionary.Jarlowy),
				Language.Demoniczny => TranslateUsingWordsList(speech, LanguagesDictionary.Demoniczny),
				Language.Orkowy => TranslateUsingDict(speech, LanguagesDictionary.Orkowy),
				Language.Nieumarlych => TranslateUsingSentencesList(LanguagesDictionary.Nieumarlych),
				Language.Belkot => TranslateUsingWordsList(speech, LanguagesDictionary.Belkot),
				_ => speech
			};
		}

		private static readonly Random random = new Random();

		public static string RandomWord(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyz";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static string TranslateUsingDict(string speech, Dictionary<string, string> dict)
		{
			string translatedWord;
			StringBuilder sb = new StringBuilder(speech.Length);
			foreach (string word in speech.Split(' '))
			{
				if (word.StartsWith("*"))
				{
					translatedWord = word.Substring(1);
				}
				else if (dict.ContainsKey(word.ToLower()))
				{
					translatedWord = dict[word.ToLower()];
				}
				else
				{
					translatedWord = dict.ElementAt(Math.Abs(word.GetHashCode()) % dict.Count).Value;
				}

				if (translatedWord.Length > 0 && word.Length > 0 && Char.IsUpper(word[0]))
				{
					char upperChar = Char.ToUpper(translatedWord[0]);
					sb.Append(upperChar);
					sb.Append(translatedWord.Substring(1));
				}
				else
				{
					sb.Append(translatedWord);
				}

				sb.Append(" ");
			}

			return sb.ToString();
			;
		}

		public static string TranslateUsingWordsList(string speech, List<string> list)
		{
			string translatedWord;
			StringBuilder sb = new StringBuilder(speech.Length);
			foreach (string word in speech.Split(' '))
			{
				if (word.StartsWith("*"))
				{
					sb.Append(word);
				}
				else
				{
					translatedWord = list[Math.Abs(word.GetHashCode()) % list.Count];
					if (translatedWord.Length > 0 && word.Length > 0 && Char.IsUpper(word[0]))
					{
						char upperChar = Char.ToUpper(translatedWord[0]);
						sb.Append(upperChar);
						sb.Append(translatedWord.Substring(1));
					}
					else
					{
						sb.Append(translatedWord);
					}
				}

				sb.Append(" ");
			}

			return sb.ToString();
			;
		}

		public static string TranslateUsingSentencesList(List<string> list)
		{
			return list[random.Next(list.Count)];
		}
	}
}
