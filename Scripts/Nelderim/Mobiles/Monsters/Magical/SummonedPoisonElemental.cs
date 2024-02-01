namespace Server.Mobiles
{
	[CorpseName( "zwloki zywiolaka trucizny" )]
	public class SummonedPoisonElemental : BaseCreature
	{
		[Constructable]
		public SummonedPoisonElemental () : base( AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4 )
		{
			Name = "zywiolak trucizny";
			Body = 162;
			BaseSoundID = 263;

			// staty i skille w przyblizeniu wzorowane na SummonedDaemon i EnergyVortex

			SetStr(200);
			SetDex(200);
			SetInt(100);

			SetHits( 200 );

			SetDamage( 14, 19 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Poison, 90 );

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 90);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 98.1, 99.0);
			SetSkill(SkillName.Poisoning, 90.1, 100.0);

			VirtualArmor = 70;

			ControlSlots = SummonedControlSlots;
		}

		public static int SummonedControlSlots => 4;

		public override bool BleedImmune => true;
		public override Poison PoisonImmune => Poison.Lethal;

		public override Poison HitPoison => Poison.Deadly;
		public override double HitPoisonChance => 0.75;

		public SummonedPoisonElemental( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
