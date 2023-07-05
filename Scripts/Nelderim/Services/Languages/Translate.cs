#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Network;

#endregion

namespace Nelderim
{
	public static class Translate
	{
		public static void Initialize()
		{
			EventSink.Speech += EventSink_Speech;
		}

		private static void EventSink_Speech(SpeechEventArgs args)
		{
			Mobile from = args.Mobile;
			string mySpeech = args.Speech;
			if (from == null || mySpeech == null || args.Type == MessageType.Emote)
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
				SayTo(from, m, mySpeech);
			}
		}
		
		private static void SayTo(Mobile from, Mobile to, string text)
		{
			from.RevealingAction();
			if (to.IsStaff() || from == to) 
				from.SayTo(to, $"[{from.LanguageSpeaking}] {text}");
			else
			{
				var translated = TryTranslate(from, to, text, from.LanguageSpeaking);
				if (to.LanguagesKnown[from.LanguageSpeaking] > 50)
				{
					translated = $"[{from.LanguageSpeaking}] {translated}";
				}

				from.SayTo(to, translated);
			}
		}

		private static string TryTranslate(Mobile from, Mobile to, string text, Language lang)
		{
			var translated = lang switch
			{
				Language.Krasnoludzki => TranslateUsingDict(text, LanguagesDictionary.Krasnoludzki),
				Language.Elficki => TranslateUsingDict(text, LanguagesDictionary.Elficki),
				Language.Drowi => TranslateUsingDict(text, LanguagesDictionary.Drowi),
				Language.Jarlowy => TranslateUsingDict(text, LanguagesDictionary.Jarlowy),
				Language.Demoniczny => TranslateUsingWordsList(text, LanguagesDictionary.Demoniczny),
				Language.Orkowy => TranslateUsingDict(text, LanguagesDictionary.Orkowy),
				Language.Nieumarlych => TranslateUsingSentencesList(LanguagesDictionary.Nieumarlych),
				Language.Powszechny => RandomWord(text),
				Language.Belkot => RandomWord(text),
				_ => text
			};
			
			return Mix(text, translated, from.LanguagesKnown[lang], to.LanguagesKnown[lang]);
		}

		public static string Mix(string original, string translated, ushort fromLangValue, ushort toLangValue)
		{
			StringBuilder sb = new StringBuilder();
			for (var i = 0; i < original.Length; i++)
			{
				if (fromLangValue > Utility.Random(1000)) sb.Append(original[i]);
				else sb.Append(translated[(int)(((float)i / original.Length) * translated.Length)]);
			}

			return sb.ToString();
		}
		
		public static void SayPublic(Mobile from, string text)
		{
			foreach (Mobile m in from.Map.GetMobilesInRange(from.Location, 18))
			{
				if (m.Player)
				{
					SayTo(from, m, text);
				}
			}
		}
		private static char[] _Alphabet = "abcdefghijklmnopqrstuvwxyz".ToArray();

		public static string RandomWord(string text)
		{
			var sb = new StringBuilder();
			for (var i = 0; i < text.Length; i++)
			{
				if (_Alphabet.Contains(Char.ToLower(text[i])))
				{
					var c = Utility.RandomList(_Alphabet);
					sb.Append(Char.IsUpper(text[i]) ? Char.ToUpper(c) : c);
				}
				else
				{
					sb.Append(text[i]);
				}
			}
			return sb.ToString();
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
		}

		public static string TranslateUsingSentencesList(List<string> list)
		{
			return list[Utility.Random(list.Count)];
		}
	}
}
