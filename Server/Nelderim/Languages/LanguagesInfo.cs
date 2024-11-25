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
			writer.Write( (int)1 ); //version
			LanguagesKnown.Serialize(writer);
			writer.Write((int)LanguageSpeaking);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			switch (version)
			{
				case 1: 
					LanguagesKnown.Deserialize(reader);
					LanguageSpeaking = (NLanguage)reader.ReadInt();
					break;
				case 0: //Remove me once we migrated :)
					var oldLanguagesKnown = reader.ReadInt();
					if((oldLanguagesKnown & 0x01) != 0)
					{
						LanguagesKnown.Krasnoludzki = 1000;
					}
					if((oldLanguagesKnown & 0x02) != 0)
					{
						LanguagesKnown.Elficki = 1000;
					}
					if ((oldLanguagesKnown & 0x04) != 0)
					{
						LanguagesKnown.Drowi = 1000;
					}
					if ((oldLanguagesKnown & 0x08) != 0)
					{
						LanguagesKnown.Jarlowy = 1000;
					}
					if ((oldLanguagesKnown & 0x10) != 0)
					{
						LanguagesKnown.Demoniczny = 1000;
					}
					if ((oldLanguagesKnown & 0x20) != 0)
					{
						LanguagesKnown.Orkowy = 1000;
					}
					if ((oldLanguagesKnown & 0x40) != 0)
					{
						LanguagesKnown.Nieumarlych = 1000;
					}
					LanguagesKnown.Powszechny = 1000;
					var langSpeaking = reader.ReadInt();
					LanguageSpeaking = langSpeaking switch
					{
						0x00 => NLanguage.Powszechny,
						0x01 => NLanguage.Krasnoludzki,
						0x02 => NLanguage.Elficki,
						0x04 => NLanguage.Drowi,
						0x08 => NLanguage.Jarlowy,
						0x10 => NLanguage.Demoniczny,
						0x20 => NLanguage.Orkowy,
						0x40 => NLanguage.Nieumarlych,
						_ => NLanguage.Belkot
					};
					break;
			}
		}
	}
}
