using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki  kamiennej wyverny")]
    public class KamiennaWyverna : BaseCreature // Wyvern
    {
        [Constructable]
        public KamiennaWyverna()
            : base(AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Name = "kamienna wyverna";
            Body = 62;
            Hue = 1246;
            BaseSoundID = 362;

            SetStr(102, 140);
            SetDex(88, 102);
            SetInt(51, 90);

            SetHits(88, 111);

            SetDamage(8, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.Poisoning, 10.1, 30.0);
            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.Wrestling, 45.1, 65.0);

            Fame = 2000;
            Karma = 0;

            VirtualArmor = 40;

            PackItem(new PoisonPotion());

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 110.1;
        }

        public override Poison PoisonImmune { get { return Poison.Lesser; } }
        public override Poison HitPoison { get { return Poison.Lesser; } }

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 4; } }

        public override int GetAttackSound()
        {
            return 713;
        }

        public override int GetAngerSound()
        {
            return 718;
        }

        public override int GetDeathSound()
        {
            return 716;
        }

        public override int GetHurtSound()
        {
            return 721;
        }

        public override int GetIdleSound()
        {
            return 725;
        }

        public KamiennaWyverna(Serial serial)
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