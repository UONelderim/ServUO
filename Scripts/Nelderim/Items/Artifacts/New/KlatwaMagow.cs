﻿using System;
using Server;

namespace Server.Items
{
    public class KlatwaMagow : Bow
    {
        public override int LabelNumber { get { return 1065764; } } // Klatwa Magow
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KlatwaMagow()
        {
            Hue = 2101;
            WeaponAttributes.HitDispel = 25;
            WeaponAttributes.HitLightning = 10;
            WeaponAttributes.HitLowerAttack = 10;
            Attributes.BonusHits = 15;
            Attributes.WeaponDamage = 25;
        }

        public KlatwaMagow(Serial serial)
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

            if (Attributes.WeaponDamage != 25)
                Attributes.WeaponDamage = 25;

        }
    }
}
