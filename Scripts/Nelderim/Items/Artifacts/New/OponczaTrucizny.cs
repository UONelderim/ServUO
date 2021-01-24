using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class OponczaTrucizny : Surcoat
    {
        public override int LabelNumber { get { return 1065744; } } // Oponcza Trucizny
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseEnergyResistance { get { return -12; } }
        public override int BasePoisonResistance { get { return 12; } }

        [Constructable]
        public OponczaTrucizny()
        {
            Hue = 0x505;
        }

        public OponczaTrucizny(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}