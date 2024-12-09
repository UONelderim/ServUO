namespace Server.Items
{
    public class Annihilation : Bardiche
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;
		
		[Constructable]
        public Annihilation()
        {
            Name = "Annihilacja";
			Hue = 1154;
            Attributes.WeaponDamage = 30;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 5;
            WeaponAttributes.HitLeechHits = 35;
            WeaponAttributes.HitLightning = 20;
            WeaponAttributes.SelfRepair = 3;
            Attributes.Luck = 50;
            Attributes.WeaponSpeed = 25;
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
        public Annihilation( Serial serial )
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
