using System;
using Server;

namespace Server.Items
{
    public class CzkawkaPijawki : CrescentBlade
    {

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public CzkawkaPijawki()
        {
            Hue = 0x3B;
			Name = "Zbalansowane Ostrza";

            Attributes.BonusDex = 5;
            Attributes.AttackChance = 5;
            WeaponAttributes.HitLeechMana = 5;
            WeaponAttributes.HitLeechStam = 10; 
            WeaponAttributes.HitLeechHits = 15;
            Attributes.WeaponDamage = 30;
            Attributes.WeaponSpeed = 20;
        }

        public CzkawkaPijawki(Serial serial)
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
