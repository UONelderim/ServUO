#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Katrilla")]
	public class NKatrill : BaseCreature
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
		public NKatrill() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "Katrill - ksiaze demonow, pan mordu";
			Body = 10;
			Hue = 1570;
			BaseSoundID = 357;

			SetStr(986, 1185);
			SetDex(177, 255);
			SetInt(151, 250);

			SetHits(5000);

			SetDamage(33, 40);

			SetDamageType(ResistanceType.Physical, 25);
			SetDamageType(ResistanceType.Fire, 25);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 90);
			SetResistance(ResistanceType.Fire, 50, 60);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 90);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.Anatomy, 100.1, 110.0);
			SetSkill(SkillName.EvalInt, 70.1, 80.0);
			SetSkill(SkillName.Magery, 75.5, 80.0);
			SetSkill(SkillName.Meditation, 65.1, 70.0);
			SetSkill(SkillName.MagicResist, 100.5, 150.0);
			SetSkill(SkillName.Tactics, 110.1, 130.0);
			SetSkill(SkillName.Wrestling, 110.1, 130.0);

			Fame = 28000;
			Karma = -28000;

			VirtualArmor = 90;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.2;

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				corpse.DropItem(new NSerceKatrilla());

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

		public NKatrill(Serial serial) : base(serial)
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
