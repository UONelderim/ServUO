using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki pisklecia gorskiego smoka")]
    public class MlodyGorskiSmok : BaseCreature // Drake
    {
        [Constructable]
        public MlodyGorskiSmok()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Name = "piskle gorskiego smoka";
            Body = Utility.RandomList(60, 61);
            Hue = 1247;
            BaseSoundID = 362;

            SetStr(140, 170);
            SetDex(90, 110);
            SetInt(101, 140);

            SetHits(86, 104);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 55.1, 60.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 150;
            Karma = 0;

            VirtualArmor = 36;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 106.1;
        }

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 4; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public MlodyGorskiSmok(Serial serial)
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