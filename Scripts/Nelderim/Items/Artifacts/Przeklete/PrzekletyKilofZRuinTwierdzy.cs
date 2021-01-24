using System;
using Server;

namespace Server.Items
{
    public class PrzekletyKilofZRuinTwierdzy : Pickaxe
    {
        
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public PrzekletyKilofZRuinTwierdzy()
        {
            Weight = 10.0;
            Hue = 2700;
			Name = "Przeklęty Kilof Z Ruin Twierdzy";
            Attributes.WeaponDamage = 40;
            Attributes.AttackChance = 25;
            Attributes.DefendChance = 25;
            WeaponAttributes.HitLowerAttack = 35;
            Attributes.Luck = 100;
            Attributes.ReflectPhysical = 15;
            Attributes.WeaponSpeed = 20;
			LootType = LootType.Cursed;
			//Server.Engines.XmlSpawner2.XmlAttach.AttachTo(this, new Server.Engines.XmlSpawner2.TemporaryQuestObject("CursedArtifact", 20160));
        }

        public PrzekletyKilofZRuinTwierdzy(Serial serial)
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

            if ( Weight != 10.0 ) Weight = 10.0;
        }
    }
}
