using Server.Items;
using System;

namespace Server.Custom.Misc
{
    public class BlabberBlade : Broadsword
    {
        private static readonly string[] _blabberPhrases = new string[]
        {
            "Ajj, jestem glodny krwii!",
            "A coz to za pyszne miesko?! Hahahahah",
            "To nie ja zabijam... To TWOJA BRUDNA LAPA!!!!",
            "OOO.... byles niegrzeczny i teraz musze Cie zabic",
            "Ale pyszna skora...",
            "*wydaje dzwieki ssania krwii*",
            "Na Smierci! To jest najgorsza rana, ktora kiedykolwiek zadalem hahahahah"
        };
        
        public override int LabelNumber => 3070057; // Gadajace ostrze
        
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
                if (Utility.RandomDouble() < 0.10)
                {
                    string randomPhrase = GetRandomBlabber();
                    
                    attacker.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x34, false, randomPhrase);
                }
            }

            base.OnHit(attacker, damageable, damageBonus);
        }
        
        private string GetRandomBlabber()
        {
            return _blabberPhrases[Utility.Random(_blabberPhrases.Length)];
        }

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

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
