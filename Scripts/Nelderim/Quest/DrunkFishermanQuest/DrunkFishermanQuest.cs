using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{
    public class DrunkMinersQuest : BaseQuest
    {
        public DrunkMinersQuest()
        {
            AddObjective(new ObtainObjective(typeof(DrunkMinersPickaxe), "Kilof Pijanego Gornika", 1, 0x0E85));
            AddReward(new BaseReward(typeof(EarringsOfTheMinersFormerWife), 3050029));//Kolczyki Bylej Zony Gornika

        }
        
        public override bool DoneOnce => true;

        /*Zagubiony Kilof Gornika*/
        public override object Title => 3050029;
        /*Jako zem zakonczyl moja dzialalnosc wydobywcza *czka* takze, wyrzucilem swoj kilof prosto do morza! Lezy na dnie juz drugi dzien, a w sumie to zatesknilem za nim. Wyciagnij go, a odplace Ci sie z nawiazka! Zostawilem go gdzies w okolicy portu, o tu w Celendri, ale cholera, osleplem od tej przepalanki haha*/
        public override object Description => 3050030;
        //Ej kmiocie, bo zaraz mnie wkurzysz *wywija nad glowa niezrecznie pusta butelka, lepiej sie odsunac!*
        public override object Refuse => 3050031;
        //No chyba sobie zartujesz, ze dam Ci moje ciezko zarobione centary za nic. A kysz! Nie wracaj bez kilfoaaaa *czka*
        public override object Uncomplete => 3050032;
        //Potworne dzieki za pomoc *czka*. Dzieki Tobie moge wrocic do pracy *czka* ...no, ale moze jutro. Jeno buteleczke dokocze!
		public override object Complete => 3050033;

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

    public class DrunkMiner : MondainQuester
    {

        public override void InitSBInfo()
        {
            SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public DrunkMiner()
            : base("Arbour", "- pijany rybak")
        {
            SetSkill(SkillName.Mining, 60.0, 83.0);

        }

        public DrunkMiner(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests => new Type[]
                {
                    typeof(DrunkMinersQuest),
                };
        public override void InitBody()
        {
            Female = false;
            BodyValue = 400;
            Race = Race.Human;

            base.InitBody();
        }
        public override void InitOutfit()
        {
            SetWearable(new Backpack());

            SetWearable(new Kilt(), Utility.RandomNeutralHue(), 1);
            SetWearable(new Shirt(), Utility.RandomNeutralHue(), 1);
            SetWearable(new Sandals(), dropChance: 1);
        }
        
        public override void Advertise()
        {
	        Say(3050034); // Cholera, gdzie moj kilof... HEJ! TY! *czka* POOOOMOZ MI!!
        }

        public override void OnOfferFailed()
        {
	        Say(3050035); // I cannot teach you, for you know all I can teach!
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
