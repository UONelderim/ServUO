using System;
using Server;

namespace Server.Items
{
    public class ZlamanyGungnir : BlackStaff
    {

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public ZlamanyGungnir()
        {
            Hue = 1161;
			Name = "Zlamany Gungnir";
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 5;
            Attributes.WeaponDamage = 20;
            Attributes.BonusStr = 5;
        }

        public ZlamanyGungnir(Serial serial)
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
