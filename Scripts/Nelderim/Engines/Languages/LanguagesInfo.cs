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

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write((int)LanguagesKnown.Value);
			writer.Write((int)LanguageSpeaking);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			LanguagesKnown = new KnownLanguages((Language)reader.ReadInt());
			LanguageSpeaking = (Language)reader.ReadInt();

			LanguagesKnown ??= new KnownLanguages();
		}
	}
}
