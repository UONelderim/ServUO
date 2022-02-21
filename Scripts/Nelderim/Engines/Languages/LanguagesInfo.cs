#region References

using Server;

#endregion

namespace Nelderim
{
	class LanguagesInfo : NExtensionInfo
	{
		public LanguagesInfo()
		{
			LanguagesKnown = new KnownLanguages();
		}

		public KnownLanguages LanguagesKnown { get; set; }

		public Language LanguageSpeaking { get; set; }

		public override void Deserialize(GenericReader reader)
		{
			LanguagesKnown = new KnownLanguages((Language)reader.ReadInt());
			LanguageSpeaking = (Language)reader.ReadInt();

			if (LanguagesKnown == null) LanguagesKnown = new KnownLanguages();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)LanguagesKnown.Value);
			writer.Write((int)LanguageSpeaking);
		}
	}
}
