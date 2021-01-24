using System;
using Server;

namespace Server.Items
{
    public class KasraShamshir : RadiantScimitar
    {
        public override int LabelNumber { get { return 1065787; } } // Kasra Shamshir
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KasraShamshir()
        {
            Hue = 2964;

            WeaponAttributes.HitColdArea = 10;
            WeaponAttributes.HitEnergyArea = 10;
            WeaponAttributes.HitFireArea = 10;
            WeaponAttributes.HitPhysicalArea = 10;
            WeaponAttributes.HitPoisonArea = 10;
            Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 35;
        }

        public KasraShamshir(Serial serial)
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
