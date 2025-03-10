namespace Server.Mobiles
{
    [CorpseName("zwloki Spite")]
    public class Spite : Changeling
    {
        [Constructable]
        public Spite()
        {
            Hue = DefaultHue;

            SetStr(53, 214);
            SetDex(243, 367);
            SetInt(369, 586);

            SetHits(1013, 1052);
            SetStam(243, 367);
            SetMana(369, 586);

            SetDamage(14, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 85, 90);
            SetResistance(ResistanceType.Fire, 41, 46);
            SetResistance(ResistanceType.Cold, 40, 44);
            SetResistance(ResistanceType.Poison, 42, 46);
            SetResistance(ResistanceType.Energy, 45, 47);

            SetSkill(SkillName.Wrestling, 12.8, 16.7);
            SetSkill(SkillName.Tactics, 102.6, 131.0);
            SetSkill(SkillName.MagicResist, 141.2, 161.6);
            SetSkill(SkillName.Magery, 108.4, 119.2);
            SetSkill(SkillName.EvalInt, 108.4, 120.0);
            SetSkill(SkillName.Meditation, 109.2, 120.0);
            SetSkill(SkillName.Spellweaving, 120.0);

            Fame = 21000;
            Karma = -21000;

            SetSpecialAbility(SpecialAbility.ManaDrain);
        }

        public Spite(Serial serial)
            : base(serial)
        {
        }
        public override bool CanBeParagon => false;
        public override string DefaultName => "Spite";
        public override int DefaultHue => 0x21;
        public override bool GivesMLMinorArtifact => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
           // AddLoot(LootPack.ArcanistScrolls);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
