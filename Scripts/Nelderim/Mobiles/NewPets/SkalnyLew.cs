using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("zwloki skalnego lwa")]
    [TypeAlias("Server.Mobiles.Skalnylew")]
    public class SkalnyLew : BaseCreature   // PredatorHellCat
    {
        [Constructable]
        public SkalnyLew()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Name = "skalny lew";
            Body = 127;
            Hue = 1102;
            BaseSoundID = 0xBA;

            SetStr(111, 145);
            SetDex(96, 115);
            SetInt(76, 100);

            SetHits(77, 91);

            SetDamage(5, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 75.1, 90.0);
            SetSkill(SkillName.Tactics, 50.1, 65.0);
            SetSkill(SkillName.Wrestling, 40.1, 45.0);

            Fame = 1000;
            Karma = 0;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 89.1;
        }

        public override int Hides { get { return 4; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public SkalnyLew(Serial serial)
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