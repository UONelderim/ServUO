#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Dzahhara")]
	public class NDzahhar : BaseCreature
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
		public NDzahhar() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "Dzahhar  - ksiaze demonow, pan cierpienia";
			Body = 102;
			BaseSoundID = 357;

			SetStr(986, 1185);
			SetDex(177, 255);
			SetInt(451, 460);

			SetMana(1500);
			SetHits(4000);

			SetDamage(30, 35);

			SetDamageType(ResistanceType.Physical, 10);
			SetDamageType(ResistanceType.Fire, 25);
			SetDamageType(ResistanceType.Energy, 65);

			SetResistance(ResistanceType.Physical, 65, 80);
			SetResistance(ResistanceType.Fire, 90);
			SetResistance(ResistanceType.Cold, 90);
			SetResistance(ResistanceType.Poison, 90);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.Anatomy, 110.1, 120.0);
			SetSkill(SkillName.EvalInt, 110.1, 120.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.Meditation, 120.1, 130.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Tactics, 100.1, 120.0);
			SetSkill(SkillName.Wrestling, 100.1, 120.0);

			Fame = 28000;
			Karma = -28000;

			VirtualArmor = 90;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				corpse.Carved = true;

				corpse.DropItem(new NSerceDzahhara());

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

		public NDzahhar(Serial serial) : base(serial)
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
