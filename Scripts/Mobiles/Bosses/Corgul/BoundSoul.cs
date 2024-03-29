namespace Server.Mobiles
{
    public class BoundSoul : BaseCreature
    {
        public override bool AlwaysMurderer => true;

        public BoundSoul()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "zakleta dusza";
            Body = 0x3CA;
            Hue = 0x4001;

            SetStr(150, 180);
            SetDex(120, 150);
            SetInt(20, 40);

            SetHits(600, 620);

            SetDamage(17, 22);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Poison, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);

            Fame = 5000;
            Karma = -5000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
        }

        public override bool OnBeforeDeath()
        {
            if (Region.IsPartOf<Regions.CorgulRegion>())
            {
                CorgulTheSoulBinder.CheckDropSOT(this);
            }

            return base.OnBeforeDeath();
        }

        public BoundSoul(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel => 3;

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
