using System;
using Server;

namespace Server.Items
{
	public class PromienSlonca : DoubleBladedStaff
	{
        public override int LabelNumber { get { return 1065815; } } // Promien Slonca
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public PromienSlonca()
        {
            Hue = 0x489;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
        }
                        
        public PromienSlonca( Serial serial ) : base( serial )
		{
		}

        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
		    phys = 20;
            fire = 40;
            nrgy = 40;
            cold = pois = chaos = direct = 0;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
