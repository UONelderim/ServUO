// 08.03.03 :: juri :: utworzenie
// 08.03.18 :: juri :: zmiana nazwy z ValmOrbb na PomiotPajaka (Ilthar D'Orbb)

#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki pajaka")]
	public class PomiotPajaka : BaseCreature
	{
		[Constructable]
		public PomiotPajaka() : base(AIType.AI_Mage, FightMode.Weakest, 12, 1, 0.2, 0.4)
		{
			Name = "Ilthar D'Orbb";
			Body = 173;
			Hue = 1079;
			BaseSoundID = 1170;

			SetStr(402, 462);
			SetDex(118, 156);
			SetInt(212, 252);

			SetHits(348, 378);

			SetDamage(14, 16);

			SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Poison, 50);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 55, 60);
			SetResistance(ResistanceType.Fire, 45, 50);
			SetResistance(ResistanceType.Cold, 45, 50);
			SetResistance(ResistanceType.Poison, 95, 95);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.EvalInt, 50.1, 60.0);
			SetSkill(SkillName.Magery, 50.1, 60.0);
			SetSkill(SkillName.Meditation, 95.1, 110.0);
			SetSkill(SkillName.Poisoning, 120.1, 130.0);
			SetSkill(SkillName.MagicResist, 99.1, 100.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 92.5);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 93.9;

			SetWeaponAbility(WeaponAbility.MortalStrike);
			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 5);
			AddLoot(LootPack.MageryRegs, 10, 20);
		}

		public override bool AutoDispel => false;
		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool BardImmune { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }

		public PomiotPajaka(Serial serial) : base(serial)
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
