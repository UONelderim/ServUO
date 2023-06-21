using Nelderim;

namespace Server
{
	public partial class Mobile
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Language LanguageSpeaking
		{
			get => Languages.Get(this).LanguageSpeaking;
			set => Languages.Get(this).LanguageSpeaking = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public KnownLanguages LanguagesKnown
		{
			get => Languages.Get(this).LanguagesKnown;
			set => Languages.Get(this).LanguagesKnown = value;
		}
	}
}
