using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki Silk")]
    public class Silk : GiantBlackWidow
    {
        [Constructable]
        public Silk()
        {
            Name = "Silk";
            Hue = 0x47E;

            SetStr(80, 131);
            SetDex(126, 156);
            SetInt(63, 102);

            SetHits(279, 378);
            SetStam(126, 156);
            SetMana(63, 102);

            SetDamage(15, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 39);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 70, 76);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Wrestling, 114.1, 123.7);
            SetSkill(SkillName.Tactics, 102.6, 118.3);
            SetSkill(SkillName.MagicResist, 78.6, 94.8);
            SetSkill(SkillName.Anatomy, 81.3, 105.7);
            SetSkill(SkillName.Poisoning, 106.0, 119.2);

            Fame = 18900;
            Karma = -18900;

            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
        }

        public Silk(Serial serial)
            : base(serial)
        {
        }
        public override bool CanBeParagon => false;
        public override bool GivesMLMinorArtifact => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            //AddLoot(LootPack.ArcanistScrolls);
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
