using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Ancient;

namespace Server.Engines.Quests
{

    public class Anche : MondainQuester
    {
        [Constructable]
        public Anche()
            : base("Anche", "- Starozytny Mag")
        {
        }

        public Anche(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests => new Type[]
                {
                    typeof(AncientQuest)
                };
		
        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.NJarling;
            
        }

        public override void InitOutfit()
        {
			SetWearable(new Boots(), 0, 1);
			SetWearable(new CommemorativeRobe(), 0, dropChance: 1);
			SetWearable(new AncientSpellbook(), 1355, dropChance: 1);
			SetWearable(new Obi(), 1355, dropChance: 1);
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
