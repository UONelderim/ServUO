using System;
using Server;

namespace Server.Items
{
    public class OrleSkrzydla : BladedStaff
    {
        public override int LabelNumber { get { return 1065751; } } // Orle Skrzydla
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public OrleSkrzydla()
        {
            Hue = 1627;

            Attributes.AttackChance = 5;
            Attributes.DefendChance = 5;
            WeaponAttributes.HitLeechHits = 10;
            WeaponAttributes.HitFireball = 10;
            WeaponAttributes.HitMagicArrow = 10;
            Attributes.WeaponDamage = 35;
        }

        public OrleSkrzydla(Serial serial)
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
