using Nelderim;
using Server.Network;

namespace Server
{
	public partial class Mobile
	{
		public virtual bool UseLanguages => false;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public Language LanguageSpeaking
		{
			get
			{
				var lang = Languages.Get(this).LanguageSpeaking;
				if (LanguagesKnown[lang] == 0)
				{
					LanguageSpeaking = Nelderim.Language.Belkot;
				}
				return lang;
			}
			set => Languages.Get(this).LanguageSpeaking = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public KnownLanguages LanguagesKnown
		{
			get => Languages.Get(this).LanguagesKnown;
			set => Languages.Get(this).LanguagesKnown = value;
		}
		
		public void NPublicOverheadMessage(MessageType type, int hue, bool ascii, string text, bool noLineOfSight)
		{
			if (m_Map != null)
			{
				var translationResult = Translate.Apply(text, LanguageSpeaking);
				Packet p;
				
				var eable = m_Map.GetClientsInRange(m_Location);

				foreach (var state in eable)
				{
					var mobile = state.Mobile;

					if (mobile != null && mobile.CanSee(this) && (noLineOfSight || mobile.InLOS(this)))
					{
						var translated = ShouldTranslate(type) ? Translate.Combine(translationResult, this, mobile) : text;
						if (ascii)
						{
							p = new AsciiMessage(Serial, Body, type, hue, 3, Name, translated);
						}
						else
						{
							p = new UnicodeMessage(Serial, Body, type, hue, 3, m_Language, Name, translated);
						}

						p.Acquire();
						state.Send(p);
						Packet.Release(p);
					}
				}
				eable.Free();
			}
		}

		public bool ShouldTranslate(MessageType type)
		{
			return type == MessageType.Regular || type == MessageType.Whisper || type == MessageType.Yell;
		}
	}
}
