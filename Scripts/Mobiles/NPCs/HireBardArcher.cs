using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HireBardArcher : BaseAdvancedHire
    {
        [Constructable]
        public HireBardArcher() : base(AIType.AI_Archer)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Race.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");

                switch (Utility.Random(2))
                {
                    case 0:
                        SetWearable(new Skirt(), Utility.RandomDyedHue(), 1);
                        break;
                    case 1:
                        SetWearable(new Kilt(), Utility.RandomNeutralHue(), 1);
                        break;
                }
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                SetWearable(new ShortPants(), Utility.RandomNeutralHue(), 1);
            }
            Title = "- bard łucznik";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(16, 16);
            SetDex(26, 26);
            SetInt(26, 26);

            SetDamage(5, 10);

            SetSkill(SkillName.Tactics, 35, 57);
            SetSkill(SkillName.Magery, 22, 22);
            SetSkill(SkillName.Swords, 45, 67);
            SetSkill(SkillName.Archery, 36, 67);
            SetSkill(SkillName.Parry, 45, 60);
            SetSkill(SkillName.Wrestling, 30, 50);
            SetSkill(SkillName.Musicianship, 66.0, 97.5);
            SetSkill(SkillName.Peacemaking, 65.0, 87.5);

            Fame = 100;
            Karma = 100;

            SetWearable(new Shoes(), Utility.RandomNeutralHue(), 1);

            switch (Utility.Random(2))
            {
                case 0:
                    SetWearable(new Doublet(), Utility.RandomDyedHue(), 1);
                    break;
                case 1:
                    SetWearable(new Shirt(), Utility.RandomDyedHue(), 1);
                    break;
            }
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Bard Archer gets extra Dexterity and archery skills
            RawDex += 1;
            
            // Improve archery and musical skills
            SetSkill(SkillName.Archery, Skills[SkillName.Archery].Base + 1);
            
            if (Utility.RandomBool())
                SetSkill(SkillName.Musicianship, Skills[SkillName.Musicianship].Base + 1);
            else
                SetSkill(SkillName.Peacemaking, Skills[SkillName.Peacemaking].Base + 1);

            // Random chance to improve combat skills
            if (Utility.RandomDouble() < 0.3)
                SetSkill(SkillName.Tactics, Skills[SkillName.Tactics].Base + 1);
        }

        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Bard Archers with high archery and musicianship are more valuable
            double archeryBonus = Skills[SkillName.Archery].Base / 100.0;
            double musicBonus = (Skills[SkillName.Musicianship].Base + Skills[SkillName.Peacemaking].Base) / 200.0;
            
            return (int)(baseFee * (1 + archeryBonus + musicBonus));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Occasionally play music or practice archery when idle
            if (Utility.RandomDouble() < 0.1 && CheckCooldown("Performance"))
            {
                if (Utility.RandomBool())
                    PlayRandomMusic();
                else
                    PracticeArchery();
                    
                SetCooldown("Performance", TimeSpan.FromMinutes(5));
            }
        }

        private void PlayRandomMusic()
        {
            if (Happiness < 50)
            {
                Say("Jestem zbyt przygnębiony, by grać muzykę...");
                return;
            }

            string[] songs = new string[]
            {
                "♪ Strzała świszczy w powietrzu... ♫",
                "♫ Opowieść o łuczniku... ♪",
                "♪ Pieśń o celnym strzale... ♫",
                "♫ Ballada o łowach... ♪"
            };

            Say(songs[Utility.Random(songs.Length)]);
            
            if (Utility.RandomDouble() < 0.2)
                Experience += 5;
        }

        private void PracticeArchery()
        {
            if (Happiness < 50)
            {
                Say("Nie mam nastroju na ćwiczenia...");
                return;
            }

            string[] practice = new string[]
            {
                "* napina cięciwę *",
                "* ćwiczy celowanie *",
                "* sprawdza stan łuku *",
                "* przygotowuje strzały *"
            };

            Say(practice[Utility.Random(practice.Length)]);
            
            if (Utility.RandomDouble() < 0.2)
                Experience += 5;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.RandomLootItem(new System.Type[] { typeof(Harp), typeof(Lute), typeof(Drums), typeof(Tambourine) }));
            AddLoot(LootPack.LootItem<Longsword>(true));
            AddLoot(LootPack.LootItem<Bow>(true));
            AddLoot(LootPack.LootItem<Arrow>(100, true));
            AddLoot(LootPack.LootGold(10, 50));
        }

        public HireBardArcher(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);// version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
