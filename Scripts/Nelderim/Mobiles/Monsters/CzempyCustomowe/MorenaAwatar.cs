#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("resztki awatara nekromantki")]
	public class MorenaAwatar : BaseCreature
	{
		public override bool BleedImmune => true;
		public override double SwitchTargetChance => 0.5;
		public override double AttackMasterChance => 0.5;

		private static readonly bool OverrideRules = true;

		private static readonly double m_AbilityChance = 0.45;

		private static readonly int m_ReturnTime = 240;

		private static readonly int m_MinTime = 10;
		private static readonly int m_MaxTime = 20;

		private DateTime m_NextAbilityTime;

		private DateTime m_NextReturnTime;

		private readonly List<Mobile> m_Minions;

		[Constructable]
		public MorenaAwatar() : base(AIType.AI_Necro, FightMode.Strongest, 12, 1, 0.2, 0.4)
		{
			Name = "Morena - Awatar";
			Female = true;
			Body = 0x191;
			BaseSoundID = 412;
			Kills = 50;

			AddItem(new HoodedShroudOfShadows(Utility.RandomNeutralHue()) { Movable = false });
			AddItem(new BlackStaff());

			SetStr(324, 458);
			SetDex(144, 172);
			SetInt(1449, 1568);

			SetHits(1120, 1190);

			SetDamage(25, 37);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Cold, 40);
			SetDamageType(ResistanceType.Energy, 40);

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 50, 70);
			SetResistance(ResistanceType.Poison, 50, 70);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.EvalInt, 120.1, 140.0);
			SetSkill(SkillName.Magery, 120.1, 140.0);
			SetSkill(SkillName.Necromancy, 120.1, 140.0);
			SetSkill(SkillName.SpiritSpeak, 120.1, 140.0);
			SetSkill(SkillName.Meditation, 100.1, 120.0);
			SetSkill(SkillName.Poisoning, 100.1, 120.0);
			SetSkill(SkillName.MagicResist, 200.1, 240.0);
			SetSkill(SkillName.Tactics, 90.1, 110.0);
			SetSkill(SkillName.Wrestling, 75.1, 110.0);

			PackItem(new GnarledStaff());

			SetWeaponAbility(WeaponAbility.BleedAttack);

			m_Minions = [];
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.50)
					corpse.DropItem(new Pumice());
			}

			base.OnCarve(from, corpse, with);
		}

		public override int GetIdleSound() => 0x19D;
		public override int GetAngerSound() => 0x175;
		public override int GetDeathSound() => 0x108;
		public override int GetAttackSound() => 0xE2;
		public override int GetHurtSound() => 0x28B;

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.ClericScrolls);
		}

		public override bool Unprovokable => true;
		public override Poison PoisonImmune => Poison.Lethal;
		public override int TreasureMapLevel => 5;

		public override bool CanPaperdollBeOpenedBy(Mobile from) => false;

		private void SpawnMinions()
		{
			if (m_Minions.Any(m => !m.Deleted && m.Alive)) return;

			m_Minions.Clear();

			if (Map == null) return;

			var isSkeleton = Utility.RandomBool();

			ShowMorphEffect();
			
			BodyMod = isSkeleton ? Utility.RandomList(50, 56) : 3;

			var minionsCount = Utility.RandomMinMax(5, 8);

			for (int i = 0; i < minionsCount; ++i)
			{
				BaseCreature minion = isSkeleton ? new Skeleton() : new Zombie();

				minion.Team = Team;

				bool validLocation = false;

				Point3D loc = Location;

				for (int j = 0; !validLocation && j < 5; ++j)
				{
					int x = X + Utility.Random(8) - 4;
					int y = Y + Utility.Random(8) - 4;
					int avgZ = Map.GetAverageZ(x, y);

					if (validLocation = Map.CanFit(x, y, Z, 16, false, false))
						loc = new Point3D(x, y, Z);
					else if (validLocation = Map.CanFit(x, y, avgZ, 16, false, false))
						loc = new Point3D(x, y, avgZ);
				}

				minion.MoveToWorld(loc, Map);
				minion.Combatant = Combatant;

				m_Minions.Add(minion);
			}

			m_NextReturnTime = DateTime.Now + TimeSpan.FromSeconds(m_ReturnTime);
		}

		private void PoisonAttack()
		{
			Combatant.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
			Combatant.PlaySound(0x474);

			if (Combatant is Mobile m)
				m.ApplyPoison(this, Poison.GetPoison(4));
		}

		public override void OnThink()
		{
			if (BodyMod != 0)
			{
				if (m_Minions.All(m => m.Deleted || !m.Alive) || DateTime.Now > m_NextReturnTime)
				{
					m_Minions.Clear();

					ShowMorphEffect();

					BodyMod = 0;
				}
			}

			if (!OverrideRules || Combatant == null)
			{
				base.OnThink();

				return;
			}

			if (DateTime.Now >= m_NextAbilityTime)
			{
				if (m_AbilityChance > Utility.RandomDouble())
				{
					if (Utility.RandomBool())
						PoisonAttack();
					else
						SpawnMinions();
				}

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(m_MinTime, m_MaxTime));
			}

			base.OnThink();
		}

		public void ShowMorphEffect()
		{
			Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
				0x3728, 8, 20, 5042);

			Effects.PlaySound(this, Map, 0x201);
		}

		public MorenaAwatar(Serial serial) : base(serial)
		{
			m_Minions = [];
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
