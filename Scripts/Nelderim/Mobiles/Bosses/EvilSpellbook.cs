using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("resztki niecnej ksiegi zaklec")]
	public class EvilSpellbook : BaseCreature
	{
		[Constructable]
		public EvilSpellbook() : base(AIType.AI_NecroMage, FightMode.Weakest, 10, 1, 0.1, 0.2)
		{
			Name = "niecna ksiega zaklec";
			Body = 985; // 0x3D9
			BaseSoundID = 466;

			SetStr(400, 500);
			SetDex(300, 350);
			SetInt(900, 950);

			SetHits(10000);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Cold, 100);
			SetDamageType(ResistanceType.Physical, 0);

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Poison, 60, 70);
			SetResistance(ResistanceType.Energy, 60, 70);
			SetResistance(ResistanceType.Cold, 60, 70);

			SetSkill(SkillName.EvalInt, 120.0, 130.0);
			SetSkill(SkillName.Magery, 115.0, 125.0);
			SetSkill(SkillName.Meditation, 105.0, 115.0);
			SetSkill(SkillName.SpiritSpeak, 200.0);
			SetSkill(SkillName.Necromancy, 112.6, 117.5);
			SetSkill(SkillName.MagicResist, 150.0, 175.0);
			SetSkill(SkillName.Tactics, 90.0, 95.0);
			SetSkill(SkillName.Wrestling, 120.0, 130.0);

			AddItem(new LightSource());
			
			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}


		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			ArtifactHelper.ArtifactDistribution(this);
		}

		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Lethal;
		public override Poison HitPoison => Poison.Lethal;
		public override double WeaponAbilityChance => 0.4;

		public EvilSpellbook(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
