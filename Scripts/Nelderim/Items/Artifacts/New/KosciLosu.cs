using System;
using Server;

namespace Server.Items
{
    public class KosciLosu : BoneArms
    {
        public override int LabelNumber { get { return 1065796; } } // Kosci Losu
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KosciLosu()
        {
            Hue = 1165;
            Attributes.AttackChance = 5;
            Attributes.BonusDex = 5;
            Attributes.DefendChance = 5;
            Attributes.EnhancePotions = 10;
            Attributes.NightSight = 1;
        }

        public KosciLosu(Serial serial)
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
