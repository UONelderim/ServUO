#region References

using Server;

#endregion

namespace Nelderim
{
	public class LanguagesInfo : NExtensionInfo
	{
		public KnownLanguages LanguagesKnown { get; set; } = new();

		public NLanguage LanguageSpeaking { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			LanguagesKnown.Serialize(writer);
			writer.Write((int)LanguageSpeaking);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			LanguagesKnown.Deserialize(reader);
			LanguageSpeaking = (NLanguage)reader.ReadInt();
		}
	}
}
