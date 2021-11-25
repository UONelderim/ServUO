﻿using System;
using Server;

namespace Server.Items
{
    public class KilofZRuinTwierdzy : Pickaxe
    {
        public override int LabelNumber { get { return 1065771; } } // Kilof Z Ruin Twierdzy
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KilofZRuinTwierdzy()
        {
            Hue = 974;
            Attributes.WeaponDamage = 25;
            Attributes.AttackChance = 25;
            Attributes.DefendChance = 25;
            WeaponAttributes.HitLowerAttack = 35;
            Attributes.Luck = 100;
            Attributes.ReflectPhysical = 15;
            Attributes.WeaponSpeed = 20;
        }

        public KilofZRuinTwierdzy(Serial serial)
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
