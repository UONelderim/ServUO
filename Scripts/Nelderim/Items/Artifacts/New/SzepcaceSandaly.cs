using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class SzepcaceSandaly : Sandals
    {
        public override int LabelNumber { get { return 1065758; } } // Szepcace Sandaly
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 50; } }

        [Constructable]
        public SzepcaceSandaly()
        {
            Hue = 1372;
            Attributes.BonusStam = 3;
            Attributes.RegenStam = 3;
        }

        public SzepcaceSandaly(Serial serial)
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