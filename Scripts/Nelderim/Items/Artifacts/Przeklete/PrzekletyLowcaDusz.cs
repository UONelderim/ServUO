using System;
using Server;

namespace Server.Items
{
	public class PrzekletyLowcaDusz : Scythe
	{
        
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PrzekletyLowcaDusz()
		{
			Hue = 0x497;
            Name = "Przeklęty Łowca Dusz";
           LootType = LootType.Cursed;
            Label2 = "Dar Awatara Dusz";
			Slayer = SlayerName.Exorcism;
			WeaponAttributes.HitLeechHits = 100;
			Attributes.WeaponDamage = 30;
			WeaponAttributes.HitLowerDefend = 30;
		}

		public PrzekletyLowcaDusz( Serial serial ) : base( serial )
		{
		}
		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = pois = nrgy = 30;
			fire = 10;
			cold = chaos = direct = 0;
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

			if ( Slayer == SlayerName.DaemonDismissal )
			{
				Slayer = SlayerName.Exorcism;
			}
		}
	}
}
