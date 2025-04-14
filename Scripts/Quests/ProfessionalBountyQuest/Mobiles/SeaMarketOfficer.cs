using Server.Engines.Quests;
using Server.Items;
using Server.Multis;
using System;

namespace Server.Mobiles
{
    public class SeaMarketOfficer : MondainQuester
    {
        public override Type[] Quests => new Type[] { typeof(ProfessionalBountyQuest) };
	    
        [Constructable]
        public SeaMarketOfficer()
        {
            Title = "- oficer marynarki pod bandera Kompanii Handlowej";
            Name = NameList.RandomName("male");

            CantWalk = true;
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);
            Female = false;
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SetWearable(new ChainChest(), 1346);
            SetWearable(new LeatherGloves(), 1346);
            SetWearable(new Necklace(), 1153);
            SetWearable(new Bandana());
            SetWearable(new Scimitar());
            SetWearable(new ThighBoots());
        }

        public override void OnTalk(PlayerMobile pm)
        {
            if (!HasQuest(pm))
            {
                BaseBoat boat = FishQuestHelper.GetBoat(pm);

                if (boat != null && boat is BaseGalleon)
                {
                    if (((BaseGalleon)boat).Scuttled)
                    {
                        pm.SendLocalizedMessage(1116752); //Your ship is a mess!  Fix it first and then we can talk about catching pirates.
                    }
                    else
                    {
                        ProfessionalBountyQuest q = new ProfessionalBountyQuest((BaseGalleon)boat)
                        {
                            Owner = pm,
                            Quester = this
                        };

                        pm.CloseGump(typeof(MondainQuestGump));
                        pm.SendGump(new MondainQuestGump(q));
                    }
                }
                else
                {
                    SayTo(pm, 1116751); //The ship you are captaining could not take on a pirate ship.  Bring a warship if you want this quest.
                    OnOfferFailed();
                }
            }
        }

        public override void Advertise()
        {
        }

        public bool HasQuest(PlayerMobile pm)
        {
            if (pm.Quests == null)
                return false;

            for (int i = 0; i < pm.Quests.Count; i++)
            {
                BaseQuest quest = pm.Quests[i];

                if (quest is ProfessionalBountyQuest)
                {
                    if (this == quest.Quester)
                    {
                        for (int j = 0; j < quest.Objectives.Count; j++)
                        {
                            if (quest.Objectives[j].Update(pm))
                                quest.Objectives[j].Complete();
                        }
                    }

                    if (quest.Completed)
                    {
                        quest.OnCompleted();
                        pm.SendGump(new MondainQuestGump(quest, MondainQuestGump.Section.Complete, false, true));
                    }
                    else
                    {
                        pm.SendGump(new MondainQuestGump(quest, MondainQuestGump.Section.InProgress, false));
                        quest.InProgress();
                    }

                    return true;
                }
            }
            return false;
        }

        public SeaMarketOfficer(Serial serial) : base(serial) { }

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
