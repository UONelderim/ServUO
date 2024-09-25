#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Delotha")]
	public class NDeloth : BaseCreature
	{
		public override bool BardImmune { get { return true; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		[Constructable]
		public NDeloth() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "Deloth  - ksiaze demonow, pan mroku";
			Body = 38;
			BaseSoundID = 357;

			SetStr(886, 985);
			SetDex(210, 265);
			SetInt(521, 550);

			SetMana(5000);
			SetHits(3000);

			SetDamage(22, 29);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Energy, 60);

			SetResistance(ResistanceType.Physical, 90);
			SetResistance(ResistanceType.Fire, 50, 60);
			SetResistance(ResistanceType.Cold, 90);
			SetResistance(ResistanceType.Poison, 50, 60);
			SetResistance(ResistanceType.Energy, 90);

			SetSkill(SkillName.Anatomy, 25.1, 50.0);
			SetSkill(SkillName.EvalInt, 140.1, 160.0);
			SetSkill(SkillName.Magery, 140.5, 160.0);
			SetSkill(SkillName.Meditation, 125.1, 150.0);
			SetSkill(SkillName.MagicResist, 140.5, 160.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 28000;
			Karma = -28000;

			VirtualArmor = 90;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				corpse.DropItem(new NSerceDelotha());

				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.50)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
		}

		public NDeloth(Serial serial) : base(serial)
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
