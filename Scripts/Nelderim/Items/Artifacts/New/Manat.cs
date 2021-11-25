﻿using System;
using Server;

namespace Server.Items
{
    public class Manat : Maul
    {

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Manat()
        {
            Hue = 2974;
			Name = "Tępy Cień Mgły";
            Attributes.WeaponDamage = 40;
            WeaponAttributes.HitLeechHits = 25;
            WeaponAttributes.HitLeechMana = 25;
            WeaponAttributes.HitLeechStam = 25;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponSpeed = -10;
        }

        public Manat(Serial serial)
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
