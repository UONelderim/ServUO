using System;
using Server;
namespace Server.Items
{
    public class MadmansHatchet : ExecutionersAxe
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		public override int LabelNumber => 3070051;//Siekiera Szalenca
 
		[Constructable]
        public MadmansHatchet()
        {
            Name = "Siekiera Szalenca";
            Hue = 38;
            Attributes.WeaponDamage = 50;
            WeaponAttributes.HitLeechHits = 35;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.BloodDrinker = 1;
            WeaponAttributes.HitFireball = 20;
		}


        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 0;
            cold = 0;
            fire = 0;
            nrgy = 0;
            pois = 0;
            chaos = 100;
            direct = 0;
        }
        public MadmansHatchet( Serial serial )
            : base( serial )
        {
        }
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 );
        }
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}
