
using System;
using Server;

namespace Server.Items
{
    public class BowOfHarps : MagicalShortbow
    {
        public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.MovingShot; } }
        public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }
        
        public override int LabelNumber => 3070058;//Luk Barda
        public override int InitMinHits{ get{ return 255; } }
        public override int InitMaxHits{ get{ return 255; } }

        [Constructable]
        public BowOfHarps()
        {
            Name = "Luk Barda";
            Hue = 403;
            Attributes.BonusDex = 5;
            WeaponAttributes.HitLeechMana = 35;
            Attributes.AttackChance = 15;
            Attributes.WeaponDamage = 45;
            Attributes.Luck = 120;
            WeaponAttributes.HitPhysicalArea = 100;
            WeaponAttributes.HitDispel = 30;
            SkillBonuses.SetValues(0, SkillName.Discordance, 5.0);
        }

        public BowOfHarps(Serial serial) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    } // End Class
} // End Namespace
