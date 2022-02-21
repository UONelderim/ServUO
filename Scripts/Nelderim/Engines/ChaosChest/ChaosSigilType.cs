#region References

using System;
using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Engines.ChaosChest
{
	[Flags]
	public enum ChaosSigilType
	{
		NONE = 0x00,
		Natury = 0x01,
		Morlokow = 0x02,
		Smierci = 0x04,
		Krysztalow = 0x08,
		Ognia = 0x10,
		Swiatla = 0x20,
		Licza = 0x40,
		ALL = 0x7F // Have to be sum of all others!!!
	}

	[PropertyObject]
	public class ChaosSigils
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Natury
		{
			get { return Get(ChaosSigilType.Natury); }
			set { Set(ChaosSigilType.Natury, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Morlokow
		{
			get { return Get(ChaosSigilType.Morlokow); }
			set { Set(ChaosSigilType.Morlokow, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Smierci
		{
			get { return Get(ChaosSigilType.Smierci); }
			set { Set(ChaosSigilType.Smierci, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Krysztalowa
		{
			get { return Get(ChaosSigilType.Krysztalow); }
			set { Set(ChaosSigilType.Krysztalow, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Ognia
		{
			get { return Get(ChaosSigilType.Ognia); }
			set { Set(ChaosSigilType.Ognia, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Swiatla
		{
			get { return Get(ChaosSigilType.Swiatla); }
			set { Set(ChaosSigilType.Swiatla, value); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Licza
		{
			get { return Get(ChaosSigilType.Licza); }
			set { Set(ChaosSigilType.Licza, value); }
		}

		public bool Get(ChaosSigilType chaosSigilType)
		{
			return Value.HasFlag(chaosSigilType);
		}

		public void Set(ChaosSigilType chaosSigilType, bool value)
		{
			if (value)
				Value |= chaosSigilType;
			else
				Value &= ~chaosSigilType;
		}

		public ChaosSigilType Value { get; private set; }

		public ChaosSigils() : this(0)
		{
		}

		public ChaosSigils(ChaosSigilType flags)
		{
			Value = flags;
		}

		public List<ChaosSigilType> List
		{
			get
			{
				List<ChaosSigilType> result = new List<ChaosSigilType>();
				foreach (ChaosSigilType type in Enum.GetValues(typeof(ChaosSigilType)))
					if (Get(type))
						result.Add(type);

				return result;
			}
		}

		public override string ToString()
		{
			return "...";
		}

		public static implicit operator ChaosSigils(ChaosSigilType flags)
		{
			return new ChaosSigils(flags);
		}
	}
}
