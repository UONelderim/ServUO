using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{
    public class TroubleOnTheWingQuest : BaseQuest
    {
        public TroubleOnTheWingQuest()
            : base()
        {
            AddObjective(new SlayObjective(typeof(Gargoyle), "gargulce", 12));

            AddReward(new BaseReward(typeof(TrinketBag), 1072341));
        }

        /* Skrzydlaty problem  */
        public override object Title => 1072371;
        /* Te przeklete gargulce lataja po ziemi Naneth... Ehh.. zajmiesz sie nimi? Czesto tu lataja i nekaja adeptow. Zgladz ich z tuzin. To powinno je skutecznie odstraszyc */
        public override object Description => 1072593;
        /* No nie mow, ze jestes sympatykiem gargulcow *spluwa* */
        public override object Refuse => 1072594;
        /* Cholerne gargulce. Trzeba je zniszczyc */
        public override object Uncomplete => 1072595;
        public override bool CanOffer()
        {
            return MondainsLegacy.Sanctuary;
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

    public class Koole : MondainQuester
    {
        [Constructable]
        public Koole()
            : base("Koole", "- druid")
        {
        }

        public Koole(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests => new Type[]
                {
                    typeof(TroubleOnTheWingQuest),
                  //  typeof(MaraudersQuest),
                    typeof(DisciplineQuest)
                };
        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Elf;

            Hue = 0x83E5;
            HairItemID = 0x2FBF;
            HairHue = 0x386;
        }

        public override void InitOutfit()
        {
            SetWearable(new Boots(), 0x901, 1);
            SetWearable(new RoyalCirclet(), dropChance: 1);
            SetWearable(new LeafTonlet(), dropChance: 1);
			SetWearable(new LeafChest(), 0x1BB, 1);
			SetWearable(new LeafArms(), 0x1BB, 1);
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
