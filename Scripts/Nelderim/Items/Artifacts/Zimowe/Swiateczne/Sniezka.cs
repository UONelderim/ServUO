using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class Sniezka : MagicalShortbow
    {
        public override int InitMinHits
        {
            get { return 60; }
        }

        public override int InitMaxHits
        {
            get { return 60; }
        }

        [Constructable]
        public Sniezka()
        {
            Hue = 2904;
            Name = "Sniezka";
            Attributes.WeaponDamage = 55;
            WeaponAttributes.HitLeechStam = 40;
            WeaponAttributes.HitLeechMana = 50;
            WeaponAttributes.HitLowerDefend = 20;
            Attributes.AttackChance = 15;
        }

		public Sniezka( Serial serial ) : base( serial )
		{
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