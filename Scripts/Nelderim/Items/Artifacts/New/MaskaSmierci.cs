using System;
using Server;

namespace Server.Items
{
    public class MaskaSmierci : BoneHelm
    {
        public override int LabelNumber { get { return 1065802; } } // Maska Smierci
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseColdResistance { get { return 15; } }
        public override int BaseEnergyResistance { get { return 12; } }
        public override int BasePhysicalResistance { get { return 11; } }
        public override int BasePoisonResistance { get { return 12; } }
        public override int BaseFireResistance { get { return 3; } } 

        [Constructable]
        public MaskaSmierci()
        {
            Hue = 2518;
            ArmorAttributes.MageArmor = 1;
            Attributes.DefendChance = 5;
            Attributes.LowerManaCost = 8;
            Attributes.NightSight = 1;
            Attributes.SpellDamage = 10;
        }

        public MaskaSmierci(Serial serial)
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
