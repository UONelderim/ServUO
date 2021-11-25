namespace Server.Mobiles
{
    [CorpseName("zwloki wampierzego nietoperza")]
    public class VampireBatFamiliar : BaseFamiliar
    {
        public VampireBatFamiliar()
        {
            Name = "wampirzy nietoperz";
            Body = 317;
            BaseSoundID = 0x270;

            SetStr(120);
            SetDex(120);
            SetInt(100);

            SetHits(90);
            SetStam(120);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 95.1, 100.0);
            SetSkill(SkillName.Tactics, 50.0);

            ControlSlots = 1;
        }

        public VampireBatFamiliar(Serial serial)
            : base(serial)
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
            reader.ReadInt();
        }
    }
}
