using System;
using Server;

namespace Server.Items
{
    public class KlingaCienia : Longsword
    {
        public override int LabelNumber { get { return 1065759; } } // Klinga Cienia
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KlingaCienia()
        {
            Hue = 1899;
            Attributes.AttackChance = 30;
            Attributes.ReflectPhysical = 15;
            Attributes.WeaponSpeed = 10;
            WeaponAttributes.HitLeechMana = 10;
            WeaponAttributes.HitLeechStam = 10;
            Attributes.WeaponDamage = 25;
        }
        public KlingaCienia(Serial serial)
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
