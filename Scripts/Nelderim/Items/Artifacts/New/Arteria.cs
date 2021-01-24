using System;
using Server;

namespace Server.Items
{
    public class Arteria : Crossbow
    {
        public override int LabelNumber { get { return 1065750; } } // Arteria
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Arteria()
        {
            Hue = 2412;

            Attributes.WeaponDamage = 30;
            WeaponAttributes.HitLeechHits = 25;
			WeaponAttributes.HitLightning = 45;
            Attributes.WeaponSpeed = 20;
        }

        public Arteria(Serial serial)
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
            
            if (Attributes.WeaponDamage != 30)
                Attributes.WeaponDamage = 30;

        }
    }
}
