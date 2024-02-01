namespace Server.Items
{
    public class HessianSword : Longsword
    {
        public override int InitMinHits => 25;

        public override int InitMaxHits => 25;

        [Constructable]
        public HessianSword()
        {
            Weight = 8;
            Name = "Miecz Hessiana";
            Hue = 0x497;

            WeaponAttributes.HitFireball = 50;
            WeaponAttributes.HitFireArea = 40;
            WeaponAttributes.ResistFireBonus = 15;
            WeaponAttributes.ResistPhysicalBonus = 10;


            Attributes.Luck = 100;
            Attributes.ReflectPhysical = 10;
            Attributes.WeaponDamage = 5;
        }

        public HessianSword(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
