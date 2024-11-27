#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki demona hordy")]
	public class CommonHordeDaemon : BaseCreature
	{
		[Constructable]
		public CommonHordeDaemon() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "demon hordy";
			Body = 776; //brak animacji w ServUO TODO: tymczasowo daje ten sam BodyBalue co w LesserHordeDaemon, to fix later: podegrac animacje
			BaseSoundID = 357;

			SetStr(316, 340);
			SetDex(131, 160);
			SetInt(111, 125);

			SetHits(510, 524);

			SetDamage(10, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 35, 45);
			SetResistance(ResistanceType.Cold, 35, 45);
			SetResistance(ResistanceType.Poison, 35, 45);
			SetResistance(ResistanceType.Energy, 35, 45);

			SetSkill(SkillName.MagicResist, 40.0);
			SetSkill(SkillName.Tactics, 60.1, 75.0);
			SetSkill(SkillName.Wrestling, 65.1, 70.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 38;

			AddItem(new LightSource());
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.05)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
		}

		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override bool BardImmune { get { return false; } }

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 194;
		}

		public CommonHordeDaemon(Serial serial) : base(serial)
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
