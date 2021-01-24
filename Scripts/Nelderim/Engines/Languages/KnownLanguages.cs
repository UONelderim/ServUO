using Server;
using System;
using System.Collections.Generic;

namespace Nelderim
{
	[Flags]
	public enum Language
	{
		Powszechny = 0x00,
		Krasnoludzki = 0x01,
		Elficki = 0x02,
		Drowi = 0x04,
		Jarlowy = 0x08,
		Demoniczny = 0x10,
		Orkowy = 0x20,
		Nieumarlych = 0x40,
		Belkot = 0x80
	}

	[PropertyObject]
	public class KnownLanguages
	{
		private Language m_Languages;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Krasnoludzki {
			get { return Get(Language.Krasnoludzki); }
			set { Set(Language.Krasnoludzki, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Elficki {
			get { return Get(Language.Elficki); }
			set { Set(Language.Elficki, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Drowi {
			get { return Get(Language.Drowi); }
			set { Set(Language.Drowi, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Jarlowy {
			get { return Get(Language.Jarlowy); }
			set { Set(Language.Jarlowy, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Demoniczny {
			get { return Get(Language.Demoniczny); }
			set { Set(Language.Demoniczny, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Orkowy {
			get { return Get(Language.Orkowy); }
			set { Set(Language.Orkowy, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Nieumarlych {
			get { return Get(Language.Nieumarlych); }
			set { Set(Language.Nieumarlych, value); }
		}

		public bool Get(Language lang) {
			return m_Languages.HasFlag(lang);
		}

		public void Set(Language lang, bool value) {
			if (value)
				m_Languages |= lang;
			else
				m_Languages &= ~lang;
        }

        public KnownLanguages() : this( Language.Powszechny )
        {
        }

        public KnownLanguages(Language languages) {
			m_Languages = languages;
		}

		public Language Value {
			get { return m_Languages; }
		}

		public List<Language> List {
			get {
				List<Language> result = new List<Language>();
				foreach (Language lang in Enum.GetValues(typeof(Language)))
					if (Get(lang)) 
						result.Add(lang);

				return result;
			}
		}
		public override string ToString() {
			return "...";
		}

		public static implicit operator KnownLanguages( Language languages )
		{
			return new KnownLanguages( languages );
		}
	}
}
