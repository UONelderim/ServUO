﻿using System;
using Server;

namespace Server.Items
{
    public class RekawiceBulpa : LeatherGloves
    {
        public override int LabelNumber { get { return 1065791; } } // Rekawice Bulpa
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        public override int BaseColdResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }
        public override int BasePhysicalResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseFireResistance { get { return 8; } } 

        [Constructable]
        public RekawiceBulpa()
        {
            Hue = 2070;
            Attributes.DefendChance = 5;
            Attributes.SpellDamage = 5;
            Attributes.WeaponDamage = 5;
			SkillBonuses.SetValues(0, SkillName.EvalInt, 10.0);
        }

        public RekawiceBulpa(Serial serial)
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
