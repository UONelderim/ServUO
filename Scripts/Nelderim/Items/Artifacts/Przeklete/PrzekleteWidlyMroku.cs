using System;
using Server;

namespace Server.Items
{
    public class PrzekleteWidlyMroku : Pitchfork
    {

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public PrzekleteWidlyMroku()
        {
            Hue = 2700;
			Name = "Przeklęte Widly Mroku";
            Attributes.WeaponDamage = 50;
            WeaponAttributes.HitFireArea = 65;
            WeaponAttributes.HitFireball = 25;
            WeaponAttributes.ResistFireBonus = 5;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponSpeed = -25;
			LootType = LootType.Cursed;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
        }

        public PrzekleteWidlyMroku(Serial serial)
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
