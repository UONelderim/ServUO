#region References
using System;
using System.Collections;
using System.Collections.Generic;

using Server.Network;
#endregion

namespace Server
{
	public enum StatCode
	{
		Str,
		Dex,
		Int
	}

	public delegate TimeSpan SkillUseCallback(Mobile user);

	public enum SkillLock : byte
	{
		Up = 0,
		Down = 1,
		Locked = 2
	}

	public enum SkillName
	{
		Alchemy = 0,
		Anatomy = 1,
		AnimalLore = 2,
		ItemID = 3,
		ArmsLore = 4,
		Parry = 5,
		Begging = 6,
		Blacksmith = 7,
		Fletching = 8,
		Peacemaking = 9,
		Camping = 10,
		Carpentry = 11,
		Cartography = 12,
		Cooking = 13,
		DetectHidden = 14,
		Discordance = 15,
		EvalInt = 16,
		Healing = 17,
		Fishing = 18,
		Herbalism = 19,
		Herding = 20,
		Hiding = 21,
		Provocation = 22,
		Inscribe = 23,
		Lockpicking = 24,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Snooping = 28,
		Musicianship = 29,
		Poisoning = 30,
		Archery = 31,
		SpiritSpeak = 32,
		Stealing = 33,
		Tailoring = 34,
		AnimalTaming = 35,
		TasteID = 36,
		Tinkering = 37,
		Tracking = 38,
		Veterinary = 39,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Mining = 45,
		Meditation = 46,
		Stealth = 47,
		RemoveTrap = 48,
		Necromancy = 49,
		Focus = 50,
		Chivalry = 51,
		Bushido = 52,
		Ninjitsu = 53,
		Spellweaving = 54,
		Mysticism = 55,
		Imbuing = 56,
		Throwing = 57
	}

	[PropertyObject]
	public class Skill
	{
		private readonly Skills m_Owner;
		private readonly SkillInfo m_Info;
		private ushort m_Base;
		private ushort m_Cap;
		private SkillLock m_Lock;

		public override string ToString()
		{
			return String.Format("[{0}: {1}]", Name, Base);
		}

		public Skill(Skills owner, SkillInfo info, GenericReader reader)
		{
			m_Owner = owner;
			m_Info = info;

			int version = reader.ReadByte();

			switch (version)
			{
				case 0:
					{
						m_Base = reader.ReadUShort();
						m_Cap = reader.ReadUShort();
						m_Lock = (SkillLock)reader.ReadByte();

						break;
					}
				case 0xFF:
					{
						m_Base = 0;
						m_Cap = 1000;
						m_Lock = SkillLock.Up;

						break;
					}
				default:
					{
						if ((version & 0xC0) == 0x00)
						{
							if ((version & 0x1) != 0)
							{
								m_Base = reader.ReadUShort();
							}

							if ((version & 0x2) != 0)
							{
								m_Cap = reader.ReadUShort();
							}
							else
							{
								m_Cap = 1000;
							}

							if ((version & 0x4) != 0)
							{
								m_Lock = (SkillLock)reader.ReadByte();
							}

							if ((version & 0x8) != 0)
							{
								VolumeLearned = reader.ReadInt();
							}

							if ((version & 0x10) != 0)
							{
								NextGGSGain = reader.ReadDateTime();
							}
						}

						break;
					}
			}

			if (m_Lock < SkillLock.Up || m_Lock > SkillLock.Locked)
			{
				Console.WriteLine("Bad skill lock -> {0}.{1}", owner.Owner, m_Lock);
				m_Lock = SkillLock.Up;
			}
		}

		public Skill(Skills owner, SkillInfo info, int baseValue, int cap, SkillLock skillLock)
		{
			m_Owner = owner;
			m_Info = info;
			m_Base = (ushort)baseValue;
			m_Cap = (ushort)cap;
			m_Lock = skillLock;
		}

		public void SetLockNoRelay(SkillLock skillLock)
		{
			if (skillLock < SkillLock.Up || skillLock > SkillLock.Locked)
			{
				return;
			}

			m_Lock = skillLock;
		}

		public void Serialize(GenericWriter writer)
		{
			if (m_Base == 0 && m_Cap == 1000 && m_Lock == SkillLock.Up && VolumeLearned == 0 && NextGGSGain == DateTime.MinValue)
			{
				writer.Write((byte)0xFF); // default
			}
			else
			{
				var flags = 0x0;

				if (m_Base != 0)
				{
					flags |= 0x1;
				}

				if (m_Cap != 1000)
				{
					flags |= 0x2;
				}

				if (m_Lock != SkillLock.Up)
				{
					flags |= 0x4;
				}

				if (VolumeLearned != 0)
				{
					flags |= 0x8;
				}

				if (NextGGSGain != DateTime.MinValue)
				{
					flags |= 0x10;
				}

				writer.Write((byte)flags); // version

				if (m_Base != 0)
				{
					writer.Write((short)m_Base);
				}

				if (m_Cap != 1000)
				{
					writer.Write((short)m_Cap);
				}

				if (m_Lock != SkillLock.Up)
				{
					writer.Write((byte)m_Lock);
				}

				if (VolumeLearned != 0)
				{
					writer.Write(VolumeLearned);
				}

				if (NextGGSGain != DateTime.MinValue)
				{
					writer.Write(NextGGSGain);
				}
			}
		}

		public Skills Owner => m_Owner;

		public SkillName SkillName => (SkillName)m_Info.SkillID;

		public int SkillID => m_Info.SkillID;

		[CommandProperty(AccessLevel.Counselor)]
		public string Name => m_Info.Name;

		public SkillInfo Info => m_Info;

		[CommandProperty(AccessLevel.Counselor)]
		public SkillLock Lock => m_Lock;

		[CommandProperty(AccessLevel.Counselor)]
		public int VolumeLearned
		{
			get;
			set;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public DateTime NextGGSGain
		{
			get;
			set;
		}

		public int BaseFixedPoint
		{
			get => m_Base;
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value >= 0x10000)
				{
					value = 0xFFFF;
				}

				var sv = (ushort)value;

				int oldBase = m_Base;

				if (m_Base != sv)
				{
					m_Owner.Total = m_Owner.Total - m_Base + sv;

					m_Base = sv;

					m_Owner.OnSkillChange(this);

					var m = m_Owner.Owner;

					if (m != null)
					{
						m.OnSkillChange(SkillName, (double)oldBase / 10);
					}
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double Base { get => m_Base / 10.0; set => BaseFixedPoint = (int)(value * 10.0); }

		public int CapFixedPoint
		{
			get => m_Cap;
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value >= 0x10000)
				{
					value = 0xFFFF;
				}

				var sv = (ushort)value;

				if (m_Cap != sv)
				{
					m_Cap = sv;

					m_Owner.OnSkillChange(this);
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double Cap
		{
			get => m_Cap / 10.0;
			set
			{
				double old = m_Cap / 10;

				CapFixedPoint = (int)(value * 10.0);

				if (old != value && Owner.Owner != null)
				{
					EventSink.InvokeSkillCapChange(new SkillCapChangeEventArgs(Owner.Owner, this, old, value));
				}
			}
		}

		private static bool m_UseStatMods;

		public static bool UseStatMods { get => m_UseStatMods; set => m_UseStatMods = value; }

		public int Fixed => (int)(Value * 10);

		[CommandProperty(AccessLevel.Counselor)]
		public double Value
		{
			get
			{
				//There has to be this distinction between the racial values and not to account for gaining skills and these skills aren't displayed nor Totaled up.
				var value = NonRacialValue;

				var raceBonus = m_Owner.Owner.GetRacialSkillBonus(SkillName);

				if (raceBonus > value)
				{
					value = raceBonus;
				}

				return value;
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public double NonRacialValue
		{
			get
			{
				var baseValue = Base;
				var inv = 100.0 - baseValue;

				if (inv < 0.0)
				{
					inv = 0.0;
				}

				inv /= 100.0;

				var statsOffset = ((m_UseStatMods ? m_Owner.Owner.Str : m_Owner.Owner.RawStr) * m_Info.StrScale) 
					            + ((m_UseStatMods ? m_Owner.Owner.Dex : m_Owner.Owner.RawDex) * m_Info.DexScale) 
								+ ((m_UseStatMods ? m_Owner.Owner.Int : m_Owner.Owner.RawInt) * m_Info.IntScale);

				var statTotal = m_Info.StatTotal * inv;

				statsOffset *= inv;

				if (statsOffset > statTotal)
				{
					statsOffset = statTotal;
				}

				var value = baseValue + statsOffset;

				m_Owner.Owner.ValidateSkillMods();

				var mods = m_Owner.Owner.SkillMods;

				double bonusObey = 0.0, bonusNotObey = 0.0;

				for (var i = 0; i < mods.Count; ++i)
				{
					var mod = mods[i];

					if (mod.Skill == (SkillName)m_Info.SkillID)
					{
						if (mod.Relative)
						{
							if (mod.ObeyCap)
							{
								bonusObey += mod.Value;
							}
							else
							{
								bonusNotObey += mod.Value;
							}
						}
						else
						{
							bonusObey = 0.0;
							bonusNotObey = 0.0;
							value = mod.Value;

							break;
						}
					}
				}

				value += bonusNotObey;

				if (value < Cap)
				{
					value += bonusObey;

					if (value > Cap)
					{
						value = Cap;
					}
				}

				m_Owner.Owner.MutateSkill((SkillName)m_Info.SkillID, ref value);

				return value;
			}
		}

		public bool IsMastery => m_Info.IsMastery;

		public bool LearnMastery(int volume)
		{
			if (!IsMastery || HasLearnedVolume(volume))
				return false;

			VolumeLearned = volume;

			if (VolumeLearned > 3)
				VolumeLearned = 3;

			if (VolumeLearned < 0)
				VolumeLearned = 0;

			return true;
		}

		public bool HasLearnedVolume(int volume)
		{
			return VolumeLearned >= volume;
		}

		public bool HasLearnedMastery()
		{
			return VolumeLearned > 0;
		}

		public bool SetCurrent()
		{
			if (IsMastery)
			{
				m_Owner.CurrentMastery = (SkillName)m_Info.SkillID;
				return true;
			}

			return false;
		}

		public void Update()
		{
			m_Owner.OnSkillChange(this);
		}
	}

	public class SkillInfo
	{
		private readonly int m_SkillID;

		public SkillInfo(
			int skillID,
			string name,
			double strScale,
			double dexScale,
			double intScale,
			string title,
			SkillUseCallback callback,
			double strGain,
			double dexGain,
			double intGain,
			double gainFactor,
			StatCode primary,
			StatCode secondary,
			bool mastery = false,
			bool usewhilecasting = false)
		{
			Name = name;
			Title = title;
			m_SkillID = skillID;
			StrScale = strScale / 100.0;
			DexScale = dexScale / 100.0;
			IntScale = intScale / 100.0;
			Callback = callback;
			StrGain = strGain;
			DexGain = dexGain;
			IntGain = intGain;
			GainFactor = gainFactor;
			Primary = primary;
			Secondary = secondary;
			IsMastery = mastery;
			UseWhileCasting = usewhilecasting;

			StatTotal = strScale + dexScale + intScale;
		}

		public StatCode Primary { get; private set; }
		public StatCode Secondary { get; private set; }

		public SkillUseCallback Callback { get; set; }

		public int SkillID => m_SkillID;

		public string Name { get; set; }

		public string Title { get; set; }

		public double StrScale { get; set; }

		public double DexScale { get; set; }

		public double IntScale { get; set; }

		public double StatTotal { get; set; }

		public double StrGain { get; set; }

		public double DexGain { get; set; }

		public double IntGain { get; set; }

		public double GainFactor { get; set; }

		public bool IsMastery { get; set; }

		public bool UseWhileCasting { get; set; }

		public int Localization => 1044060 + SkillID;

		private static SkillInfo[] m_Table = new SkillInfo[58]
		{
			new SkillInfo(0, "Alchemia", 0.0, 5.0, 5.0, "Alchemik", null, 0.0, 0.5, 0.5, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(1, "Anatomia", 0.0, 0.0, 0.0, "Anatom", null, 0.15, 0.15, 0.7, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(2, "Wiedza o Zwierzetach", 0.0, 0.0, 0.0, "Zoolog", null, 0.0, 0.0, 1.0, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(3, "Identyfikacja", 0.0, 0.0, 0.0, "Rzeczoznawca", null, 0.0, 0.0, 1.0, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(4, "Wiedza o broni", 0.0, 0.0, 0.0, "Zbrojmistrz", null, 0.75, 0.15, 0.1, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(5, "Parowanie", 7.5, 2.5, 0.0, "Obronca", null, 0.75, 0.25, 0.0, 1.0, StatCode.Dex, StatCode.Str, true ),
			new SkillInfo(6, "Zebranie", 0.0, 0.0, 0.0, "Zebrak", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(7, "Kowalstwso", 10.0, 0.0, 0.0, "Kowal", null, 1.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(8, "Wyrabianie Lukow", 6.0, 16.0, 0.0, "Luczarz", null, 0.6, 1.6, 0.0, 1.0, StatCode.Dex, StatCode.Str),
			new SkillInfo(9, "Uspokajanie", 0.0, 0.0, 0.0, "Uspokajacz", null, 0.0, 0.0, 0.0, 1.0, StatCode.Int, StatCode.Dex, true ),
			new SkillInfo(10, "Obozowanie", 20.0, 15.0, 15.0, "Podroznik", null, 2.0, 1.5, 1.5, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(11, "Stolarstwo", 20.0, 5.0, 0.0, "Stolarz", null, 2.0, 0.5, 0.0, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(12, "Kartografia", 0.0, 7.5, 7.5, "Kartograf", null, 0.0, 0.75, 0.75, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(13, "Gotowanie", 0.0, 20.0, 30.0, "Szef kuchni", null, 0.0, 2.0, 3.0, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(14, "Wykrywanie", 0.0, 0.0, 0.0, "Zwiadowca", null, 0.0, 0.4, 0.6, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(15, "Dezorientacja", 0.0, 2.5, 2.5, "Dezorientator", null, 0.0, 0.25, 0.25, 1.0, StatCode.Dex, StatCode.Int, true ),
			new SkillInfo(16, "Madrosc", 0.0, 0.0, 0.0, "Medrzec", null, 0.0, 0.0, 1.0, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(17, "Leczenie", 6.0, 6.0, 8.0, "Lekarz", null, 0.6, 0.6, 0.8, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(18, "Rybactwo", 0.0, 0.0, 0.0, "Rybak", null, 0.5, 0.5, 0.0, 1.0, StatCode.Dex, StatCode.Str),
			new SkillInfo(19, "Zielarstwo", 0.0, 0.0, 0.0, "Zielarz", null, 0.0, 0.2, 0.8, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(20, "Pasterstwo", 16.25, 6.25, 2.5, "Pasterz", null, 1.625, 0.625, 0.25, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(21, "Ukrywanie", 0.0, 0.0, 0.0, "Cien", null, 0.0, 0.8, 0.2, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(22, "Prowakacja", 0.0, 4.5, 0.5, "Prowokator", null, 0.0, 0.45, 0.05, 1.0, StatCode.Int, StatCode.Dex, true ),
			new SkillInfo(23, "Inskrypcja", 0.0, 2.0, 8.0, "Skryba", null, 0.0, 0.2, 0.8, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(24, "Wlamywanie", 0.0, 25.0, 0.0, "Wlamywacz", null, 0.0, 2.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(25, "Magia", 0.0, 0.0, 15.0, "Mag", null, 0.0, 0.0, 1.5, 1.0, StatCode.Int, StatCode.Str, true ),
			new SkillInfo(26, "Odpornosc na magie", 0.0, 0.0, 0.0, "Odporny", null, 0.25, 0.25, 0.5, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(27, "Taktyka", 0.0, 0.0, 0.0, "Taktyk", null, 0.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(28, "Zagladanie", 0.0, 25.0, 0.0, "Szpieg", null, 0.0, 2.5, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(29, "Muzykalnosc", 0.0, 0.0, 0.0, "Bard", null, 0.0, 0.8, 0.2, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(30, "Zatruwanie", 0.0, 4.0, 16.0, "Morderca", null, 0.0, 0.4, 1.6, 1.0, StatCode.Int, StatCode.Dex, true ),
			new SkillInfo(31, "Lucznictwo", 2.5, 7.5, 0.0, "Lucznik", null, 0.25, 0.75, 0.0, 1.0, StatCode.Dex, StatCode.Str, true ),
			new SkillInfo(32, "Rozmowa z Duchami", 0.0, 0.0, 0.0, "Medium", null, 0.0, 0.0, 1.0, 1.0, StatCode.Int, StatCode.Str, false, true),
			new SkillInfo(33, "Okradanie", 0.0, 10.0, 0.0, "Kieszonkowiec", null, 0.0, 1.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(34, "Krawiectwo", 3.75, 16.25, 5.0, "Krawiec", null, 0.38, 1.63, 0.5, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(35, "Oswajanie", 14.0, 2.0, 4.0, "Oswajacz", null, 1.4, 0.2, 0.4, 1.0, StatCode.Str, StatCode.Int, true ),
			new SkillInfo(36, "Ocena Smakiem", 0.0, 0.0, 0.0, "Degustator", null, 0.2, 0.0, 0.8, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(37, "Majsterkowanie", 5.0, 2.0, 3.0, "Majster", null, 0.5, 0.2, 0.3, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(38, "Tropienie", 0.0, 12.5, 12.5, "Tropiciel", null, 0.0, 1.25, 1.25, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(39, "Weterynaria", 8.0, 4.0, 8.0, "Weterynarz", null, 0.8, 0.4, 0.8, 1.0, StatCode.Int, StatCode.Dex),
			new SkillInfo(40, "Walka Mieczami", 7.5, 2.5, 0.0, "Miecznik", null, 0.75, 0.25, 0.0, 1.0, StatCode.Str, StatCode.Dex, true ),
			new SkillInfo(41, "Walka Obuchami", 9.0, 1.0, 0.0, "Młotnik", null, 0.9, 0.1, 0.0, 1.0, StatCode.Str, StatCode.Dex, true ),
			new SkillInfo(42, "Szermierka", 4.5, 5.5, 0.0, "Szermierz", null, 0.45, 0.55, 0.0, 1.0, StatCode.Dex, StatCode.Str, true ),
			new SkillInfo(43, "Walka Piesciami", 9.0, 1.0, 0.0, "Piesciarz", null, 0.9, 0.1, 0.0, 1.0, StatCode.Str, StatCode.Dex, true ),
			new SkillInfo(44, "Drawlstwo", 20.0, 0.0, 0.0, "Drwal", null, 2.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(45, "Gornictwo", 20.0, 0.0, 0.0, "Gornik", null, 2.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Dex),
			new SkillInfo(46, "Medytacja", 0.0, 0.0, 0.0, "Stoik", null, 0.0, 0.0, 0.0, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(47, "Skradanie", 0.0, 0.0, 0.0, "Lotr", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(48, "Rozbrajanie", 0.0, 0.0, 0.0, "Rozbrajacz", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(49, "Nekromancja", 0.0, 0.0, 0.0, "Nekromanta", null, 0.0, 0.0, 0.0, 1.0, StatCode.Int, StatCode.Str, true ),
			new SkillInfo(50, "Koncentracja", 0.0, 0.0, 0.0, "Skupiony", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Int),
			new SkillInfo(51, "Rycerstwo", 0.0, 0.0, 0.0, "Paladyn", null, 0.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Int, true ),
			new SkillInfo(52, "Tradycja Wojenna", 0.0, 0.0, 0.0, "Wojownik", null, 0.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Int, true ),
			new SkillInfo(53, "Skrytobojstwo", 0.0, 0.0, 0.0, "Ninja", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Int, true ),
			new SkillInfo(54, "Druidyzm", 0.0, 0.0, 0.0, "Druid", null, 0.0, 0.0, 0.0, 1.0, StatCode.Int, StatCode.Str, true),
			new SkillInfo(55, "Mistycyzm", 0.0, 0.0, 0.0, "Mistyk", null, 0.0, 0.0, 0.0, 1.0, StatCode.Str, StatCode.Int, true ),
			new SkillInfo(56, "Imbuing", 0.0, 0.0, 0.0, "Artificer", null, 0.0, 0.0, 0.0, 1.0, StatCode.Int, StatCode.Str),
			new SkillInfo(57, "Rzucanie", 0.0, 0.0, 0.0, "Rzucacz", null, 0.0, 0.0, 0.0, 1.0, StatCode.Dex, StatCode.Str, true ),
		};

		public static SkillInfo[] Table
		{
			get => m_Table; set
			{
				if (m_Table == value)
				{
					return;
				}

				var si = UseStatInfluences;

				if (si)
				{
					DisableStatInfluences();
				}

				m_Table = value;

				if (!si)
				{
					DisableStatInfluences();
				}
			}
		}

		private static double[,] m_CachedStatInfluences;

		public static bool UseStatInfluences
		{
			get => Config.Get("Gains.UseStatInfluences", false);
			set
			{
				Config.Set("Gains.UseStatInfluences", value);

				Invalidate();
			}
		}

		static SkillInfo()
		{
			Core.OnExpansionChanged += Invalidate;

			Invalidate();
		}

		public static void Invalidate()
		{
			if (UseStatInfluences)
			{
				EnableStatInfluences();
			}
			else
			{
				DisableStatInfluences();
			}
		}

		private static void DisableStatInfluences()
		{
			if (m_CachedStatInfluences != null)
			{
				return;
			}

			var table = Table;

			m_CachedStatInfluences = new double[table.Length, 4];

			for (var i = 0; i < table.Length; ++i)
			{
				var info = table[i];

				m_CachedStatInfluences[i, 0] = info.StrScale;
				m_CachedStatInfluences[i, 1] = info.DexScale;
				m_CachedStatInfluences[i, 2] = info.IntScale;
				m_CachedStatInfluences[i, 3] = info.StatTotal;

				info.StrScale = 0.0;
				info.DexScale = 0.0;
				info.IntScale = 0.0;
				info.StatTotal = 0.0;
			}
		}

		private static void EnableStatInfluences()
		{
			if (m_CachedStatInfluences == null)
			{
				return;
			}

			var table = Table;

			for (var i = 0; i < table.Length; ++i)
			{
				var info = table[i];

				info.StrScale = m_CachedStatInfluences[i, 0];
				info.DexScale = m_CachedStatInfluences[i, 1];
				info.IntScale = m_CachedStatInfluences[i, 2];
				info.StatTotal = m_CachedStatInfluences[i, 3];
			}

			m_CachedStatInfluences = null;
		}
	}

	[PropertyObject]
	public class Skills : IEnumerable<Skill>
	{
		private readonly Mobile m_Owner;
		private readonly Skill[] m_Skills;
		private int m_Total, m_Cap;
		private Skill m_Highest;

		#region Skill Getters & Setters
		[CommandProperty(AccessLevel.Counselor)]
		public Skill Alchemy { get => this[SkillName.Alchemy]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Anatomy { get => this[SkillName.Anatomy]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill AnimalLore { get => this[SkillName.AnimalLore]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill ItemID { get => this[SkillName.ItemID]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill ArmsLore { get => this[SkillName.ArmsLore]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Parry { get => this[SkillName.Parry]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Begging { get => this[SkillName.Begging]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Blacksmith { get => this[SkillName.Blacksmith]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Fletching { get => this[SkillName.Fletching]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Peacemaking { get => this[SkillName.Peacemaking]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Camping { get => this[SkillName.Camping]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Carpentry { get => this[SkillName.Carpentry]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Cartography { get => this[SkillName.Cartography]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Cooking { get => this[SkillName.Cooking]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill DetectHidden { get => this[SkillName.DetectHidden]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Discordance { get => this[SkillName.Discordance]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill EvalInt { get => this[SkillName.EvalInt]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Healing { get => this[SkillName.Healing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Fishing { get => this[SkillName.Fishing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Herbalism { get => this[SkillName.Herbalism]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Herding { get => this[SkillName.Herding]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Hiding { get => this[SkillName.Hiding]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Provocation { get => this[SkillName.Provocation]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Inscribe { get => this[SkillName.Inscribe]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Lockpicking { get => this[SkillName.Lockpicking]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Magery { get => this[SkillName.Magery]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill MagicResist { get => this[SkillName.MagicResist]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Tactics { get => this[SkillName.Tactics]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Snooping { get => this[SkillName.Snooping]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Musicianship { get => this[SkillName.Musicianship]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Poisoning { get => this[SkillName.Poisoning]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Archery { get => this[SkillName.Archery]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill SpiritSpeak { get => this[SkillName.SpiritSpeak]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Stealing { get => this[SkillName.Stealing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Tailoring { get => this[SkillName.Tailoring]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill AnimalTaming { get => this[SkillName.AnimalTaming]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill TasteID { get => this[SkillName.TasteID]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Tinkering { get => this[SkillName.Tinkering]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Tracking { get => this[SkillName.Tracking]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Veterinary { get => this[SkillName.Veterinary]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Swords { get => this[SkillName.Swords]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Macing { get => this[SkillName.Macing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Fencing { get => this[SkillName.Fencing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Wrestling { get => this[SkillName.Wrestling]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Lumberjacking { get => this[SkillName.Lumberjacking]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Mining { get => this[SkillName.Mining]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Meditation { get => this[SkillName.Meditation]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Stealth { get => this[SkillName.Stealth]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill RemoveTrap { get => this[SkillName.RemoveTrap]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Necromancy { get => this[SkillName.Necromancy]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Focus { get => this[SkillName.Focus]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Chivalry { get => this[SkillName.Chivalry]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Bushido { get => this[SkillName.Bushido]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Ninjitsu { get => this[SkillName.Ninjitsu]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Spellweaving { get => this[SkillName.Spellweaving]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Mysticism { get => this[SkillName.Mysticism]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Imbuing { get => this[SkillName.Imbuing]; set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Throwing { get => this[SkillName.Throwing]; set { } }
		#endregion

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public int Cap { get => m_Cap; set => m_Cap = value; }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public SkillName CurrentMastery
		{
			get;
			set;
		}

		public int Total { get => m_Total; set => m_Total = value; }

		public Mobile Owner => m_Owner;

		public int Length => m_Skills.Length;

		public Skill this[SkillName name] => this[(int)name];

		public Skill this[int skillID]
		{
			get
			{
				if (skillID < 0 || skillID >= m_Skills.Length)
				{
					return null;
				}

				var sk = m_Skills[skillID];

				if (sk == null)
				{
					m_Skills[skillID] = sk = new Skill(this, SkillInfo.Table[skillID], 0, 1000, SkillLock.Up);
				}

				return sk;
			}
		}

		public override string ToString()
		{
			return "...";
		}

		public static bool UseSkill(Mobile from, SkillName name)
		{
			return UseSkill(from, (int)name);
		}

		public static bool UseSkill(Mobile from, int skillID)
		{
			if (!from.CheckAlive())
			{
				return false;
			}
			else if (!from.Region.OnSkillUse(from, skillID))
			{
				return false;
			}
			else if (!from.AllowSkillUse((SkillName)skillID))
			{
				return false;
			}

			if (skillID >= 0 && skillID < SkillInfo.Table.Length)
			{
				var info = SkillInfo.Table[skillID];

				if (info.Callback != null)
				{
					if (Core.TickCount - from.NextSkillTime >= 0 && (info.UseWhileCasting || from.Spell == null))
					{
						from.DisruptiveAction();

						from.NextSkillTime = Core.TickCount + (int)info.Callback(from).TotalMilliseconds;

						return true;
					}
					else
					{
						from.SendSkillMessage();
					}
				}
				else
				{
					from.SendLocalizedMessage(500014); // That skill cannot be used directly.
				}
			}

			return false;
		}

		public Skill Highest
		{
			get
			{
				if (m_Highest == null)
				{
					Skill highest = null;
					var value = Int32.MinValue;

					for (var i = 0; i < m_Skills.Length; ++i)
					{
						var sk = m_Skills[i];

						if (sk != null && sk.BaseFixedPoint > value)
						{
							value = sk.BaseFixedPoint;
							highest = sk;
						}
					}

					if (highest == null && m_Skills.Length > 0)
					{
						highest = this[0];
					}

					m_Highest = highest;
				}

				return m_Highest;
			}
		}

		public void Serialize(GenericWriter writer)
		{
			m_Total = 0;

			writer.Write(4); // version

			writer.Write((int)CurrentMastery);

			writer.Write(m_Cap);
			writer.Write(m_Skills.Length);

			for (var i = 0; i < m_Skills.Length; ++i)
			{
				var sk = m_Skills[i];

				if (sk == null)
				{
					writer.Write((byte)0xFF);
				}
				else
				{
					sk.Serialize(writer);
					m_Total += sk.BaseFixedPoint;
				}
			}
		}

		public Skills(Mobile owner)
		{
			m_Owner = owner;
			m_Cap = Config.Get("PlayerCaps.TotalSkillCap", 7000);

			var info = SkillInfo.Table;

			m_Skills = new Skill[info.Length];

			//for ( int i = 0; i < info.Length; ++i )
			//	m_Skills[i] = new Skill( this, info[i], 0, 1000, SkillLock.Up );
		}

		public Skills(Mobile owner, GenericReader reader)
		{
			m_Owner = owner;

			var version = reader.ReadInt();

			switch (version)
			{
				case 4:
				CurrentMastery = (SkillName)reader.ReadInt();
				goto case 3;
				case 3:
				case 2:
					{
						m_Cap = reader.ReadInt();

						goto case 1;
					}
				case 1:
					{
						if (version < 2)
						{
							m_Cap = 7000;
						}

						if (version < 3)
						{
							/*m_Total =*/
							reader.ReadInt();
						}

						var info = SkillInfo.Table;

						m_Skills = new Skill[info.Length];

						var count = reader.ReadInt();

						for (var i = 0; i < count; ++i)
						{
							if (i < info.Length)
							{
								var sk = new Skill(this, info[i], reader);

								if (sk.BaseFixedPoint != 0 || sk.CapFixedPoint != 1000 || sk.Lock != SkillLock.Up || sk.VolumeLearned != 0)
								{
									m_Skills[i] = sk;
									m_Total += sk.BaseFixedPoint;
								}
							}
							else
							{
								new Skill(this, null, reader);
							}
						}

						//for ( int i = count; i < info.Length; ++i )
						//	m_Skills[i] = new Skill( this, info[i], 0, 1000, SkillLock.Up );

						break;
					}
				case 0:
					{
						reader.ReadInt();

						goto case 1;
					}
			}
		}

		public void OnSkillChange(Skill skill)
		{
			if (skill == m_Highest) // could be downgrading the skill, force a recalc
			{
				m_Highest = null;
			}
			else if (m_Highest != null && skill.BaseFixedPoint > m_Highest.BaseFixedPoint)
			{
				m_Highest = skill;
			}

			m_Owner.OnSkillInvalidated(skill);

			var ns = m_Owner.NetState;

			if (ns != null)
			{
				ns.Send(new SkillChange(skill));

				m_Owner.Delta(MobileDelta.Skills);
				m_Owner.ProcessDelta();
			}
		}

		public IEnumerator<Skill> GetEnumerator()
		{
			for (var i = 0; i < m_Skills.Length; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
