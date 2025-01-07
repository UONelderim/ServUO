namespace Server.Mobiles
{
    [CorpseName("zainfekowany trup")]
    public class WidowSpawn : GiantBlackWidow
    {
        public override bool CanRegenHits => true;
        public override bool IsScaredOfScaryThings => false;
        public override bool IsScaryToPets => true;

        [Constructable]
        public WidowSpawn()
        {
            Name = "pomiot wodwy";
            Body = 728;
            BaseSoundID = 471;
            Hue = 1931;

            SetStr(200);
            SetDex(25);
            SetInt(10);

            SetHits(300);
            SetStam(150);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 80);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 99, 100);
            SetResistance(ResistanceType.Poison, 99, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
        }

        public WidowSpawn(Serial serial) : base(serial)
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
