using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("zwloki skalnego jaszczura")]
    [TypeAlias("Server.Mobiles.Skalnyjaszczur")]
    public class SkalnyJaszczur : BaseCreature  // LavaLizard
    {
        [Constructable]
        public SkalnyJaszczur()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Name = "skalny jaszczur";
            Body = 0xCE;
            Hue = 1134;
            BaseSoundID = 0x5A;

            SetStr(120, 150);
            SetDex(56, 75);
            SetInt(11, 20);

            SetHits(76, 90);
            SetMana(0);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 35.1, 50.0);
            SetSkill(SkillName.Tactics, 30.1, 40.0);
            SetSkill(SkillName.Wrestling, 30.1, 40.0);

            Fame = 1000;
            Karma = 0;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 80.1;

            PackItem(new SulfurousAsh(Utility.Random(4, 10)));
        }

        public override int Hides { get { return 3; } }

        public SkalnyJaszczur(Serial serial)
            : base(serial)
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