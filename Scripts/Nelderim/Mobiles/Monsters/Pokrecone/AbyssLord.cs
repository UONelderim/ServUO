namespace Server.Mobiles
{
    [CorpseName("resztki wladcy otchlani")]
    public class AbyssLord : BaseCreature
    {
        [Constructable]
        public AbyssLord()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Wladca Otchlani";

            Body = 174;
            BaseSoundID = 0x4B0;
            Kills = 10;

            SetStr(502, 600);
            SetDex(102, 200);
            SetInt(601, 750);

            SetHits(10000);
            SetStam(103, 250);

            SetDamage(29, 35);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 65, 75);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 65, 75);
            SetResistance(ResistanceType.Energy, 65, 75);

            SetSkill(SkillName.EvalInt, 95.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 105.0);
            SetSkill(SkillName.Meditation, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 120.2, 140.0);
            SetSkill(SkillName.Tactics, 90.1, 105.0);
            SetSkill(SkillName.Wrestling, 90.1, 105.0);
            SetSkill(SkillName.Hiding, 95.1, 100.0);
            SetSkill(SkillName.Necromancy, 95.1, 100.0);
            SetSkill(SkillName.DetectHidden, 95.1, 100.0);
        }

        public AbyssLord(Serial serial) : base(serial)
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
