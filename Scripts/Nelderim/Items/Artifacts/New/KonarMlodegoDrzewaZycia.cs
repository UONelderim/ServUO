﻿using System;
using Server;

namespace Server.Items
{
    public class KonarMlodegoDrzewaZycia : Bokuto
    {
        public override int LabelNumber { get { return 1065761; } } // Konar Mlodego Drzewa Zycia
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KonarMlodegoDrzewaZycia()
        {
            Hue = 2408;
            WeaponAttributes.HitLowerAttack = 30;
            WeaponAttributes.HitLowerDefend = 30;
            Attributes.RegenHits = 5;
            Attributes.WeaponDamage = 35;
        }
        public KonarMlodegoDrzewaZycia(Serial serial)
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

            if (Attributes.WeaponDamage != 35)
                Attributes.WeaponDamage = 35;
        }
    }
}
