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
					break;
			}
			
			LanguageSpeaking = (NLanguage)reader.ReadInt();
		}
	}
}
