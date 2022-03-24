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
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			LanguagesKnown = new KnownLanguages((Language)reader.ReadInt());
			LanguageSpeaking = (Language)reader.ReadInt();

			if (LanguagesKnown == null) LanguagesKnown = new KnownLanguages();
		}
	}
}
