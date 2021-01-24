using System;
using Server;

namespace Server.Items
{
    public class OstrzeCienia : Wakizashi
    {
        public override int LabelNumber { get { return 1065757; } } // Ostrze Cienia
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public OstrzeCienia()
        {
            Hue = 2111;
            WeaponAttributes.HitLowerAttack = 15;
            WeaponAttributes.HitLowerDefend = 15;
            Attributes.BonusDex = 3;
            Attributes.BonusStam = 15;
            Attributes.RegenStam = 3;
            Attributes.WeaponSpeed = 15;
            Attributes.WeaponDamage = 35;
        }

        public OstrzeCienia(Serial serial)
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
