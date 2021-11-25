﻿using System;
using Server;

namespace Server.Items
{
    public class Hrunting : VikingSword
    {
        public override int LabelNumber { get { return 1065770; } } // Hrunting
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Hrunting()
        {
            Hue = 1572;
            Attributes.WeaponDamage = 35;
            Attributes.WeaponSpeed = 30;
            WeaponAttributes.HitLightning = 25;
            Attributes.BonusHits = 10;
        }

        public Hrunting(Serial serial)
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
