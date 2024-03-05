#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Wladcy Jeziora Lawy")]
	public class WladcaJezioraLawy : BaseCreature
	{
		[Constructable]
		public WladcaJezioraLawy() : base(AIType.AI_Mage, FightMode.Strongest, 11, 1, 0.2, 0.4)
		{
			Name = "Wladca Jeziora Lawy";
			Body = 0x311;
			Hue = 1358;
			BaseSoundID = 0x300;

			SetStr(1050);
			SetDex(150);
			SetInt(800);

			SetHits(10000);

			SetDamage(30, 40);

			SetResistance(ResistanceType.Physical, 50, 60);
			SetResistance(ResistanceType.Fire, 50, 60);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.DetectHidden, 80.0);
			SetSkill(SkillName.EvalInt, 100.0);
			SetSkill(SkillName.Magery, 100.0);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.MagicResist, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Wrestling, 120.0);
			SetSkill(SkillName.Swords, 120.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 55;

			SetWeaponAbility(WeaponAbility.CrushingBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved)
			{
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.60)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			// 07.01.2013 :: szczaw :: usuniecie PackGold
			//PackGold(1000, 2000 );
			AddLoot(LootPack.UltraRich);
			AddLoot(LootPack.FilthyRich);
		}

		public override bool BardImmune { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override double AttackMasterChance { get { return 0.10; } }
		public override double SwitchTargetChance { get { return 0.10; } }
		public override double DispelDifficulty { get { return 120.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public WladcaJezioraLawy(Serial serial) : base(serial)
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
