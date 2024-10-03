using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{

    public class NPCRecieverQuestAvatar : BaseCreature
    {
        [Constructable]
        public NPCRecieverQuestAvatar()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
	        Blessed = true;
	        Name = "Hemmington";
	        Race = Race.NTamael;
	        Female = false;
	        

        }

        public NPCRecieverQuestAvatar(Serial serial)
            : base(serial)
        {
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
