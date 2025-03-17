using Server.Items;

namespace Server.Custom.Misc
{
    public class BlabberBlade : Broadsword
    {

        [Constructable]
        public BlabberBlade()
        {
            Name = "Gadajace ostrze";

            Hue = 2753;

            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 25;

            WeaponAttributes.HitLeechHits = 50;
            WeaponAttributes.BloodDrinker = 1;
            WeaponAttributes.HitLeechStam = 25;

            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.HitLowerDefend = 25;

            Label1 = "*gdy twa zywotnosc jest bliska polowy, ostrze przemowi*";
        }

        public override void OnHit(Mobile attacker, IDamageable damageable, double damageBonus)
        {
            if (damageable is Mobile m && m.Hits < (m.HitsMax / 2))
            {
                if (Utility.RandomDouble() < 0.3)
                {
                    int blabber = GetRandomBlabber();

                    Effects.PlaySound(attacker.Location, attacker.Map, blabber);
                }
            }

            base.OnHit(attacker, damageable, damageBonus);
        }

        private int GetRandomBlabber()
        {
            return 0x53B + Utility.RandomMinMax(0, 46);
        }

        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }

        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }

        public BlabberBlade(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
