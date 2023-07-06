#region References

using System;
using Server;

#endregion

namespace Nelderim
{
	public enum Language
	{
		Belkot = -1,
		Powszechny = 0,
		Krasnoludzki = 1,
		Elficki = 2,
		Drowi = 3,
		Jarlowy = 4,
		Demoniczny = 5,
		Orkowy = 6,
		Nieumarlych = 7,
	}

	[PropertyObject]
	public class KnownLanguages
	{
		private readonly ushort[] _languageValues = new ushort[8];

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Powszechny
		{
			get => this[Language.Powszechny];
			set => this[Language.Powszechny] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Krasnoludzki
		{
			get => this[Language.Krasnoludzki];
			set => this[Language.Krasnoludzki] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Elficki
		{
			get => this[Language.Elficki];
			set => this[Language.Elficki] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Drowi
		{
			get => this[Language.Drowi];
			set => this[Language.Drowi] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Jarlowy
		{
			get => this[Language.Jarlowy];
			set => this[Language.Jarlowy] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Demoniczny
		{
			get => this[Language.Demoniczny];
			set => this[Language.Demoniczny] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Orkowy
		{
			get => this[Language.Orkowy];
			set => this[Language.Orkowy] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Nieumarlych
		{
			get => this[Language.Nieumarlych];
			set => this[Language.Nieumarlych] = value;
		}

		public override string ToString()
		{
			return "...";
		}

		public ushort this[Language lang]
		{
			get
			{
				if (lang == Language.Belkot) return 0;
				return _languageValues[(int)lang];
			}
			set  
			{
				if(lang == Language.Belkot) return;
				_languageValues[(int)lang] = value;
			}
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write((int)0);
			writer.Write(_languageValues.Length);
			foreach (var value in _languageValues)
			{
				writer.Write(value);
			}
		}

		public void Deserialize(GenericReader reader)
		{
			reader.ReadInt(); //version
			var count = reader.ReadInt();
			for (int i = 0; i < count; i++)
			{
				_languageValues[i] = reader.ReadUShort();
			}
		}

		public void Clear()
		{
			foreach (Language lang in Enum.GetValues(typeof(Language)))
			{
				this[lang] = 0;
			}
		}
	}
}
