using Server.Items;
using Server.Spells;
using System;

namespace Server.Mobiles
{
    public class HireBeggar : BaseAdvancedHire
    {
        private int m_MeditationPoints;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MeditationPoints 
        { 
            get => m_MeditationPoints; 
            set => m_MeditationPoints = Math.Max(0, Math.Min(100, value)); 
        }

        [Constructable]
        public HireBeggar() : base(AIType.AI_Mystic)
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
                        SetWearable(new Skirt(), Utility.RandomNeutralHue(), 1);
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
            
            Title = "- mnich";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(40, 46);
            SetDex(21, 21);
            SetInt(80, 80);

            SetDamage(1, 1);

            SetSkill(SkillName.Begging, 66, 97);
            SetSkill(SkillName.Tactics, 5, 27);
            SetSkill(SkillName.Wrestling, 5, 27);
            SetSkill(SkillName.Magery, 50, 80);
            SetSkill(SkillName.Meditation, 50, 80);
            SetSkill(SkillName.Mysticism, 50, 80);
            SetSkill(SkillName.Focus, 50, 80);

            Fame = 0;
            Karma = 0;

            SetWearable(new Sandals(), Utility.RandomNeutralHue(), 1);

            switch (Utility.Random(2))
            {
                case 0:
                    SetWearable(new Doublet(), Utility.RandomNeutralHue(), 1);
                    break;
                case 1:
                    SetWearable(new Shirt(), Utility.RandomNeutralHue(), 1);
                    break;
            }

            PackGold(0, 25);
            m_MeditationPoints = 0;
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Monks get extra Intelligence and mystical skills
            RawInt += 2;
            
            // Improve mystical abilities
            SetSkill(SkillName.Meditation, Skills[SkillName.Meditation].Base + 1);
            SetSkill(SkillName.Mysticism, Skills[SkillName.Mysticism].Base + 1);
            
            if (Utility.RandomBool())
                SetSkill(SkillName.Focus, Skills[SkillName.Focus].Base + 1);
            else
                SetSkill(SkillName.Magery, Skills[SkillName.Magery].Base + 1);

            Say("Moja duchowa moc wzrasta!");
        }

        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Monks with high mysticism and meditation are more valuable
            double mysticBonus = (Skills[SkillName.Mysticism].Base + Skills[SkillName.Meditation].Base) / 200.0;
            return (int)(baseFee * (1 + mysticBonus));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Meditation and mystical activities
            if (Utility.RandomDouble() < 0.1 && CheckCooldown("Meditation"))
            {
                PerformMeditation();
                SetCooldown("Meditation", TimeSpan.FromMinutes(5));
            }

            // Healing through meditation
            if (Hits < HitsMax && MeditationPoints >= 20)
            {
                Heal(10);
                MeditationPoints -= 20;
                Say("*używa duchowej energii do leczenia*");
            }
        }

        private void PerformMeditation()
        {
            if (Happiness < 50)
            {
                Say("Mój umysł jest zbyt niespokojny na medytację...");
                return;
            }

            string[] meditations = new string[]
            {
                "* medytuje w ciszy *",
                "* intonuje starożytne mantry *",
                "* koncentruje duchową energię *",
                "* poszukuje wewnętrznego spokoju *",
                "* studiuje mistyczne wzory *"
            };

            Say(meditations[Utility.Random(meditations.Length)]);
            
            if (Utility.RandomDouble() < 0.3)
            {
                MeditationPoints += 10;
                Experience += 5;
                
                if (MeditationPoints >= 100)
                {
                    Say("Osiągnąłem stan duchowej harmonii!");
                    Effects.SendLocationEffect(Location, Map, 0x373A, 16, 10);
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            // Monks have a chance to avoid death through spiritual power
            if (MeditationPoints >= 50 && Utility.RandomDouble() < 0.5)
            {
                Hits = HitsMax;
                Mana = ManaMax;
                MeditationPoints = 0;
                Say("*duchowa energia chroni przed śmiercią*");
                Effects.SendLocationEffect(Location, Map, 0x373A, 16, 10);
                return false;
            }

            return base.OnBeforeDeath();
        }

        public HireBeggar(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);// version
            writer.Write(m_MeditationPoints);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_MeditationPoints = reader.ReadInt();
        }
    }
}
