using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class SmoczyNos : LeatherLegs
    {

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseColdResistance { get { return 14; } }
        public override int BaseEnergyResistance { get { return 8; } }
        public override int BasePhysicalResistance { get { return 6; } }
        public override int BasePoisonResistance { get { return 6; } }
        public override int BaseFireResistance { get { return 7; } }

        [Constructable]
        public SmoczyNos()
        {
            Hue = 1265;
			Name = "Nogawice Ozdobione Łuskami Smoków";
            Attributes.LowerManaCost = 5;
            Attributes.Luck = 150;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 10;
        }

        public SmoczyNos(Serial serial)
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