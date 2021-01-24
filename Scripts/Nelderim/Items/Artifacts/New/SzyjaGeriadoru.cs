using System;
using Server;

namespace Server.Items
{
    public class SzyjaGeriadoru : PlateGorget
    {
        public override int LabelNumber { get { return 1065801; } } // Szyja Geriadoru
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseFireResistance { get { return 11; } }
        public override int BaseColdResistance { get { return 9; } }
        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseEnergyResistance { get { return 9; } }

        [Constructable]
        public SzyjaGeriadoru()
        {
            Hue = 794;
            Attributes.BonusStr = 5;
            Attributes.RegenHits = 2;
            Attributes.DefendChance = 10;
        }

        public SzyjaGeriadoru(Serial serial)
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
