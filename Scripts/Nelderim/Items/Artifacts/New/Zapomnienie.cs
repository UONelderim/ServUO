﻿using System;
using Server;

namespace Server.Items
{
    public class Zapomnienie : SilverRing
    {
        public override int LabelNumber { get { return 1065748; } } // Zapomnienie
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 45; } }

        [Constructable]
        public Zapomnienie()
        {
            Hue = 0x498;

            Attributes.BonusHits = 10;
            Attributes.BonusMana = 10;
            Attributes.BonusStam = 10;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 8;
        }

        public Zapomnienie(Serial serial)
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
