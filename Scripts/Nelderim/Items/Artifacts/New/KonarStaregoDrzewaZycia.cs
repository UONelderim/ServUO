using System;
using Server;

namespace Server.Items
{
    public class KonarStaregoDrzewaZycia : Bokuto
    {
        public override int LabelNumber { get { return 1065760; } } // Konar Starego Drzewa Zycia
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public KonarStaregoDrzewaZycia()
        {
            Hue = 2200;
            WeaponAttributes.HitLeechHits = 40;
            WeaponAttributes.HitLowerAttack = 10;
            WeaponAttributes.HitLowerDefend = 10;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 5;
            Attributes.WeaponDamage = 35;
            Hue = 1177;
        }
        public KonarStaregoDrzewaZycia(Serial serial)
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

            if (Attributes.WeaponDamage != 35)
                Attributes.WeaponDamage = 35;

            if (Hue != 1177)
                Hue = 1177;
        }
    }
}
