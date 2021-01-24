using System;
using Server;

namespace Server.Items
{
    public class Nasr : CompositeBow
    {
        public override int LabelNumber { get { return 1065782; } } // Nasr
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Nasr()
        {
            Hue = 2511;
            Attributes.WeaponDamage = 25;
            Attributes.AttackChance = 15;
            WeaponAttributes.HitLightning = 45;
            Attributes.RegenHits = 4;
            Attributes.SpellChanneling = 1;
        }

        public Nasr(Serial serial)
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
