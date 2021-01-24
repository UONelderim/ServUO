using System;
using Server;

namespace Server.Items
{
    public class PrzekleteOrleSkrzydla : BladedStaff
    {

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public PrzekleteOrleSkrzydla()
        {
            Hue = 2700;
			Name = "Przeklęte Orle Skrzydla";
            Attributes.AttackChance = 5;
            Attributes.DefendChance = 5;
            WeaponAttributes.HitLeechHits = 10;
            WeaponAttributes.HitFireball = 100;
            WeaponAttributes.HitMagicArrow = 10;
			LootType = LootType.Cursed;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
        }

        public PrzekleteOrleSkrzydla(Serial serial)
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
