#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki azatotha - wladcy demonow")]
	public class WladcaDemonow : BaseCreature
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override bool CanRummageCorpses => true;
		public override Poison PoisonImmune => Poison.Lethal;

		[Constructable]
		public WladcaDemonow() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "azatoth - wladca demonow ";
			Body = 0x310;
			Hue = 1070;
			BaseSoundID = 357;

			SetStr(1286, 1385);
			SetDex(210, 265);
			SetInt(800, 900);

			SetMana(8000);
			SetHits(18000);

			SetDamage(26, 35);

			SetDamageType(ResistanceType.Fire, 25);
			SetDamageType(ResistanceType.Energy, 75);
			SetDamageType(ResistanceType.Physical, 0);

			SetResistance(ResistanceType.Physical, 90, 100);
			SetResistance(ResistanceType.Fire, 60, 80);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 90, 100);
			SetResistance(ResistanceType.Energy, 90, 100);

			SetSkill(SkillName.Anatomy, 25.1, 50.0);
			SetSkill(SkillName.EvalInt, 110);
			SetSkill(SkillName.Magery, 115.5, 130.0);
			SetSkill(SkillName.Meditation, 105.1, 120.0);
			SetSkill(SkillName.MagicResist, 120.5, 130.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			AddItem(new LightSource());

			Fame = 34000;
			Karma = -34000;

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				corpse.DropItem(new Bloodspawn());
				corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public WladcaDemonow(Serial serial) : base(serial)
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
