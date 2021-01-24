using System;
using Server;

namespace Server.Items
{
    public class PasMurdulfaZlotobrodego : Obi
    {
        public override int LabelNumber { get { return 1065792; } } // Pas Murdulfa Zlotobrodego
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public PasMurdulfaZlotobrodego()
        {
            Hue = 2111;
            Attributes.BonusInt = 1;
            Attributes.DefendChance = 3;
            Attributes.ReflectPhysical = 5;
            Attributes.RegenHits = 3;
        }

        public PasMurdulfaZlotobrodego(Serial serial)
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
