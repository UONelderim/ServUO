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

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override bool AlwaysMurderer
        {
            get { return true; }
        }

        public override bool AutoDispel
        {
            get { return true; }
        }

        public override double AutoDispelChance
        {
            get { return 1.0; }
        }

        public override bool BardImmune
        {
            get { return !Core.SE; }
        }

        public override bool Unprovokable
        {
            get { return Core.SE; }
        }

        public override bool Uncalmable
        {
            get { return Core.SE; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Deadly; }
        }

        public override bool ShowFameTitle
        {
            get { return false; }
        }

        public override bool ClickTitle
        {
            get { return false; }
        }

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