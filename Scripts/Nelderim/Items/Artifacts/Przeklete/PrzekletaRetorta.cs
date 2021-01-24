using System;
using Server;

namespace Server.Items
{
    public class PrzekletaRetorta : WarFork
    {
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }


        [Constructable]
        public PrzekletaRetorta()
        {
            Hue = 2700;
			Name = "Przekleta Retorta";
            WeaponAttributes.HitLowerDefend = 45;
            Attributes.BonusDex = 9;
            Attributes.WeaponDamage = 30;
            Attributes.WeaponSpeed = 25;
			LootType = LootType.Cursed;
        }

        public PrzekletaRetorta(Serial serial)
            : base(serial)
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
