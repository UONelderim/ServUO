using System;
using Server;

namespace Server.Items
{
    public class SkalpelDoktoraBrandona : ButcherKnife
    {

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public SkalpelDoktoraBrandona()
        {
			Name = "mas ten heriant";
            Hue = 1372;
            WeaponAttributes.HitLeechHits = 50;
            Attributes.WeaponSpeed = 20;
            Slayer = SlayerName.Repond;
            Attributes.WeaponDamage = 45;
        }

        public SkalpelDoktoraBrandona(Serial serial)
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

            if (Attributes.WeaponDamage != 45)
                Attributes.WeaponDamage = 45;

        }
    }
}
