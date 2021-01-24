using System;
using Server;

namespace Server.Items
{
    public class Ancile : MetalKiteShield
    {
        public override int LabelNumber { get { return 1065775; } } // Ancile
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BasePhysicalResistance { get { return 10; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 15; } }
        public override int BaseEnergyResistance { get { return 10; } }
        public override int BasePoisonResistance { get { return 5; } }

        [Constructable]
        public Ancile()
        {
            Hue = 2961;
            StrRequirement = 105;
            Attributes.BonusDex = 1;
            Attributes.RegenHits = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.Luck = 175; 
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.SpellChanneling = 1;
        }

        public Ancile(Serial serial)
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
