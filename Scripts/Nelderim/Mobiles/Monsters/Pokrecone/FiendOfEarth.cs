namespace Server.Mobiles
{
    public class FiendOfEarth : BaseCreature
    {
        [Constructable]
        public FiendOfEarth() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Demon Ziemi";
            Body = 14;
            BaseSoundID = 227;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(4200);
            SetStam(102, 300);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);
        }

        public override bool AlwaysMurderer => true;
        public override bool AutoDispel => false;
        public override double AutoDispelChance => 1.0;
        public override bool BardImmune => false;
        public override bool Unprovokable => true;
        public override bool Uncalmable => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override bool ShowFameTitle => false;
        public override bool ClickTitle => false;

        public FiendOfEarth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
