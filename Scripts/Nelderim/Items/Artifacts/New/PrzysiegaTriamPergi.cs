using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class PrzysiegaTriamPergi : WoodlandBelt
    {
        public override int LabelNumber { get { return 1065741; } } // Przysiega Triam Pergi
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 50; } }

        [Constructable]
        public PrzysiegaTriamPergi()
        {
            Hue = 0x87;
            Attributes.Luck = 50;
            Attributes.LowerManaCost = 3;
        }

        public PrzysiegaTriamPergi(Serial serial)
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