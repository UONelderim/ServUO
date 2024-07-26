#region References

using System;
using Server;

#endregion

namespace Nelderim
{
	public enum NLanguage
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
			get => this[NLanguage.Powszechny];
			set => this[NLanguage.Powszechny] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Krasnoludzki
		{
			get => this[NLanguage.Krasnoludzki];
			set => this[NLanguage.Krasnoludzki] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Elficki
		{
			get => this[NLanguage.Elficki];
			set => this[NLanguage.Elficki] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Drowi
		{
			get => this[NLanguage.Drowi];
			set => this[NLanguage.Drowi] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Jarlowy
		{
			get => this[NLanguage.Jarlowy];
			set => this[NLanguage.Jarlowy] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Demoniczny
		{
			get => this[NLanguage.Demoniczny];
			set => this[NLanguage.Demoniczny] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Orkowy
		{
			get => this[NLanguage.Orkowy];
			set => this[NLanguage.Orkowy] = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ushort Nieumarlych
		{
			get => this[NLanguage.Nieumarlych];
			set => this[NLanguage.Nieumarlych] = value;
		}

		public override string ToString()
		{
			return "...";
		}

		public ushort this[NLanguage lang]
		{
			get
			{
				if (lang == NLanguage.Belkot) return 0;
				return _languageValues[(int)lang];
			}
			set  
			{
				if(lang == NLanguage.Belkot) return;
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
			foreach (NLanguage lang in Enum.GetValues(typeof(NLanguage)))
			{
				this[lang] = 0;
			}
		}
	}
}
