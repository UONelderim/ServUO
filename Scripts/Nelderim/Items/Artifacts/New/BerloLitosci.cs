using System;
using Server;

namespace Server.Items
{
    public class BerloLitosci : Scepter
    {
        public override int LabelNumber { get { return 1065799; } } // Berlo Litosci
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public BerloLitosci()
        {
            Hue = 1170;
            Attributes.WeaponDamage = 25;
            WeaponAttributes.HitLeechHits = 30;
            WeaponAttributes.HitLeechStam = 30;
            Attributes.CastSpeed = 1;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 10;
        }

        public BerloLitosci(Serial serial)
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
