using System;
using Server;
namespace Server.Items
{
    public class MinersPickaxe : Pickaxe
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		
		public override int LabelNumber => 3070054;//Kilof Gornika z Orod


        [Constructable]
        public MinersPickaxe()
        {
            Name = "Kilof Gornika z Orod";
			Hue = 974;
            Attributes.WeaponDamage = 35;
            Attributes.DefendChance = 25;
            WeaponAttributes.HitLowerDefend = 35;
            Attributes.CastRecovery = 1;
            Attributes.WeaponSpeed = 20;
            WeaponAttributes.HitFatigue = 20;
        }
        


        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 0;
            cold = 0;
            fire = 100;
            nrgy = 0;
            pois = 0;
            chaos = 0;
            direct = 0;
        }
        public MinersPickaxe( Serial serial )
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
