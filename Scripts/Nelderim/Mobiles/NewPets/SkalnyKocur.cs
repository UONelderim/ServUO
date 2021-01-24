using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("zwloki skalnego kocura")]
    [TypeAlias("Server.Mobiles.Skalnykocur")]
    public class SkalnyKocur : BaseCreature // HellCat
    {
        [Constructable]
        public SkalnyKocur()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Name = "skalny kocur";
            Body = 0xC9;
            Hue = 1102;
            BaseSoundID = 0x69;

            SetStr(51, 100);
            SetDex(52, 150);
            SetInt(13, 85);

            SetHits(48, 67);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 40.1, 55.0);
            SetSkill(SkillName.Wrestling, 30.1, 40.0);

            Fame = 800;
            Karma = 0;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 92.1;
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public SkalnyKocur(Serial serial)
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