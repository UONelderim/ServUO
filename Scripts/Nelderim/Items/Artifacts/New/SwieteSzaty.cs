using System;
using Server;

namespace Server.Items
{
    public class SwieteSzaty : JinBaori
    {
        public override int LabelNumber { get { return 1065786; } } // Swiete Szaty
        public override int InitMinHits { get { return 10; } }
        public override int InitMaxHits { get { return 10; } }

        [Constructable]
        public SwieteSzaty()
        {
            Hue = 2125;
            Attributes.Luck = 150;
        }

        public SwieteSzaty(Serial serial)
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
