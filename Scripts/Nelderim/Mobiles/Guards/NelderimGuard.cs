/***************************************************************************
 *                              NelderimGuard.cs
 *                            -------------------
 * 							  Nelderim rel. Piencu 1.0
 * 							  http:\\nelderim.org
 *
 ***************************************************************************/

#region References

using System;
using Server.Items;
using Server.Nelderim;

#endregion

namespace Server.Mobiles
{
	public enum GuardType
	{
		StandardGuard,
		ArcherGuard,
		HeavyGuard,
		MageGuard,
		MountedGuard,
		EliteGuard,
		SpecialGuard
	}

	public enum WarFlag
	{
		None,
		White,
		Black,
		Red,
		Green,
		Blue
	}

	public class BaseNelderimGuard : BaseCreature
	{
		private string m_RegionName;
		private WarFlag m_Flag;
		private WarFlag m_Enemy;

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public string HomeRegionName
		{
			get => m_RegionName;

			set
			{
				m_RegionName = value;

				try
				{
					if (!RegionsEngine.MakeGuard(this, m_RegionName))
						m_RegionName = null;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					m_RegionName = null;
				}
			}
		}

		// 26.06.2012 :: zombie
		public override bool IsEnemy(Mobile m)
		{
			if (m == null)
				return false;

			// 07.11.2012 :: zombie :: tymczasowo
			if (m is BaseNelderimGuard)
				return false;
			// zombie

			if (m.Criminal || m.Kills >= 5)
				return true;

			if (WarOpponentFlag != WarFlag.None && WarOpponentFlag == (m as BaseNelderimGuard).WarSideFlag)
				return true;

			if (m is BaseCreature)
			{
				BaseCreature bc = m as BaseCreature;
				if (bc.AlwaysMurderer || (!bc.Controlled && bc.FightMode == FightMode.Closest))
					return true;

				if ((bc.Controlled && bc.ControlMaster != null &&
				     bc.ControlMaster.AccessLevel < AccessLevel.Counselor &&
				     (bc.ControlMaster.Criminal || bc.ControlMaster.Kills >= 5)) ||
				    (bc.Summoned && bc.SummonMaster != null && bc.SummonMaster.AccessLevel < AccessLevel.Counselor &&
				     (bc.SummonMaster.Criminal || bc.SummonMaster.Kills >= 5)))
					return true;
			}

			return false;
		}

		// zombie
		public override void CriminalAction(bool message)
		{
			// Straznik nigdy nie dostanie krima.
			// Byl problem, ze gdy straznik atakowal peta/summona gracza-krima to sam dostawal krima.s
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public WarFlag WarSideFlag
		{
			get => m_Flag;
			set
			{
				m_Flag = value;

				if (m_Flag != WarFlag.None && m_Flag == m_Enemy)
					m_Enemy = WarFlag.None;
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public WarFlag WarOpponentFlag
		{
			get => m_Enemy;
			set
			{
				m_Enemy = value;

				if (m_Enemy != WarFlag.None && m_Flag == m_Enemy)
					m_Flag = WarFlag.None;
			}
		}

		public BaseNelderimGuard(GuardType type) : this(type, FightMode.Criminal)
		{
		}

		public BaseNelderimGuard(GuardType type, FightMode fmode) : base(AIType.AI_Melee, fmode, 12, 1, 0.1, 0.4)
		{
			Type = type;
			m_RegionName = null;
			m_Flag = WarFlag.None;
			m_Enemy = WarFlag.None;

			switch (type)
			{
				case GuardType.MountedGuard:
					RangePerception = 16;
					ActiveSpeed = 0.05;
					AI = AIType.AI_Melee;
					PackGold(40, 80);
					break;
				case GuardType.ArcherGuard:
					RangePerception = 16;
					RangeFight = 6;
					ActiveSpeed = 0.2;
					AI = AIType.AI_Archer;
					PackGold(30, 90);
					break;
				case GuardType.EliteGuard:
					RangePerception = 18;
					ActiveSpeed = 0.05;
					PassiveSpeed = 0.2;
					AI = AIType.AI_Melee;
					PackGold(50, 100);
					break;
				case GuardType.SpecialGuard:
					RangePerception = 20;
					ActiveSpeed = 0.05;
					PassiveSpeed = 0.1;
					AI = AIType.AI_Melee;
					PackGold(60, 100);
					break;
				case GuardType.HeavyGuard:
					RangePerception = 16;
					ActiveSpeed = 0.05;
					PassiveSpeed = 0.1;
					AI = AIType.AI_Melee;
					PackGold(40, 80);
					break;
				case GuardType.MageGuard:
					RangePerception = 16;
					ActiveSpeed = 0.05;
					PassiveSpeed = 0.1;
					AI = AIType.AI_Mage;
					PackGold(40, 80);
					break;
				default:
					PackGold(20, 80);
					break;
			}

			Fame = 5000;
			Karma = 5000;

			SetWeaponAbility(WeaponAbility.Dismount);
			SetWeaponAbility(WeaponAbility.BleedAttack);

			new RaceTimer(this).Start();
		}

		public BaseNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override bool AutoDispel => true;
		public override bool Unprovokable => true;
		public override bool Uncalmable => true;
		public override bool BardImmune => true;
		public override Poison PoisonImmune => Poison.Greater;
		public override bool IsMonster => false;

		public override bool HandlesOnSpeech(Mobile from)
		{
			return true;
		}

		public override bool ShowFameTitle => false;

		public GuardType Type { get; }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2);

			// v 2
			writer.Write((int)m_Flag);
			writer.Write((int)m_Enemy);

			// v 1
			writer.Write(m_RegionName);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
				{
					m_Flag = (WarFlag)reader.ReadInt();
					m_Enemy = (WarFlag)reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					if (version < 2)
					{
						m_Flag = WarFlag.None;
						m_Enemy = WarFlag.None;
					}

					m_RegionName = reader.ReadString();
					break;
				}
				default:
				{
					if (version < 1)
						m_RegionName = null;
					break;
				}
			}
		}

		public override bool Move(Direction d)
		{
			if ((GetDistanceToSqrt(Home) > RangePerception * 6 )
			    &&  !(Home == new Point3D(0, 0, 0)))
			{
				DebugSay("I am to far");
				SetLocation(Home, false);
			}
			
			return base.Move(d);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Poor);
		}


		private class RaceTimer : Timer
		{
			private readonly BaseNelderimGuard m_Target;

			public RaceTimer(BaseNelderimGuard target) : base(TimeSpan.FromMilliseconds(250))
			{
				m_Target = target;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				try
				{
					if (!m_Target.Deleted)
						RegionsEngine.MakeGuard(m_Target);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
		}
	}

	[CorpseName("zwloki straznika")]
	public class StandardNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public StandardNelderimGuard() : base(GuardType.StandardGuard) { }

		public StandardNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class MageNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public MageNelderimGuard() : base(GuardType.MageGuard)
		{
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.Disarm);
		}

		public override double WeaponAbilityChance => 0.45;

		public MageNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class HeavyNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public HeavyNelderimGuard() : base(GuardType.HeavyGuard)
		{
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.45;

		public HeavyNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class MountedNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public MountedNelderimGuard() : base(GuardType.MountedGuard)
		{
			SetWeaponAbility(WeaponAbility.Disarm);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.45;

		public MountedNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class ArcherNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public ArcherNelderimGuard() : base(GuardType.ArcherGuard)
		{
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.ArmorIgnore);
		}

		public ArcherNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class EliteNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public EliteNelderimGuard() : base(GuardType.EliteGuard)
		{
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.5;

		public EliteNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[CorpseName("zwloki straznika")]
	public class SpecialNelderimGuard : BaseNelderimGuard
	{
		[Constructable]
		public SpecialNelderimGuard() : base(GuardType.SpecialGuard)
		{
			SetWeaponAbility(WeaponAbility.Disarm);
			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 1.0;

		public SpecialNelderimGuard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
