using System;
using Server;

namespace Server.Items
{
    public class RekawiceFredericka : LeatherGloves
    {
        public override int LabelNumber { get { return 1065790; } } // Rekawice Fredericka
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseColdResistance { get { return 5; } }
        public override int BaseEnergyResistance { get { return 10; } }
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 10; } }
        public override int BaseFireResistance { get { return 10; } }

        [Constructable]
        public RekawiceFredericka()
        {
            Hue = 1284;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.RegenStam = 5;
            SkillBonuses.SetValues(0, SkillName.Stealing, 10.0);
			SkillBonuses.SetValues(0, SkillName.Snooping, 10.0);
        }

        public RekawiceFredericka(Serial serial)
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
