using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{

    public class Orion : MondainQuester
    {
        [Constructable]
        public Orion()
            : base("Orion", "- Herdeista")
        {
        }

        public Orion(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests => new Type[]
                {
                    typeof(ClericQuest)
                };
        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.NTamael;
            
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            SetWearable(new Boots(), 0x901, 1);
            SetWearable(new NorseHelm(), dropChance: 1);
            SetWearable(new PlateArms(), dropChance: 1);
			SetWearable(new PlateChest(), 1150, 1);
			SetWearable(new PlateLegs(), 1150, 1);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
