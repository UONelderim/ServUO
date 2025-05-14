namespace Server.Items
{
    public class CaptainsHook : SoulGlaive
    {

		public override int LabelNumber => 3070071;//Kapitanski Hak

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
		

        [Constructable]
        public CaptainsHook()
        {
            Hue = 1151;
            Name = "Kapitanski Hak";
           
            Attributes.WeaponSpeed = 70;
            Attributes.WeaponDamage = 55;
            Attributes.BalancedWeapon = 1;
            Attributes.CastSpeed = -1;
            WeaponAttributes.HitColdArea = 100;
            WeaponAttributes.HitFatigue = 50;
        }

        public CaptainsHook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
