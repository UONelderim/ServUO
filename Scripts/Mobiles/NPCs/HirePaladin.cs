using Server.Items;
using System;

namespace Server.Mobiles
{
    public class HirePaladin : BaseAdvancedHire
    {
        private int m_HolyPower;
        private int m_EvilsSlain;
        private DateTime m_LastPrayerTime;
        private bool m_IsConsecrated;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HolyPower 
        { 
            get => m_HolyPower; 
            set => m_HolyPower = Math.Max(0, Math.Min(100, value)); 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EvilsSlain 
        { 
            get => m_EvilsSlain; 
            set => m_EvilsSlain = value; 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsConsecrated 
        { 
            get => m_IsConsecrated; 
            set => m_IsConsecrated = value; 
        }

        [Constructable]
        public HirePaladin() : base(AIType.AI_Paladin)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Race.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            Title = "- paladyn";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            InitializeEquipment();

            SetStr(86, 100);
            SetDex(81, 95);
            SetInt(61, 75);

            SetDamage(10, 23);

            SetSkill(SkillName.Swords, 66.0, 97.5);
            SetSkill(SkillName.Anatomy, 65.0, 87.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Healing, 65.0, 87.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);
            SetSkill(SkillName.Parry, 45.0, 60.5);
            SetSkill(SkillName.Chivalry, 85, 100);

            Fame = 100;
            Karma = 250;

            m_HolyPower = 0;
            m_EvilsSlain = 0;
            m_LastPrayerTime = DateTime.MinValue;
            m_IsConsecrated = false;
        }

        private void InitializeEquipment()
        {
	        switch (Utility.Random(5))
	        {
		        case 0:
			        break;
		        case 1:
			        SetWearable(new Bascinet(), dropChance: 1);
			        break;
		        case 2:
			        SetWearable(new CloseHelm(), dropChance: 1);
			        break;
		        case 3:
			        SetWearable(new NorseHelm(), dropChance: 1);
			        break;
		        case 4:
			        SetWearable(new Helmet(), dropChance: 1);
			        break;
	        }
	        SetWearable(new Shoes(), Utility.RandomNeutralHue(), 1);
	        SetWearable(new Shirt(), dropChance: 1);
	        SetWearable(new VikingSword(), dropChance: 1);
	        SetWearable(new MetalKiteShield(), dropChance: 1);

	        SetWearable(new PlateChest(), dropChance: 1);
	        SetWearable(new PlateLegs(), dropChance: 1);
	        SetWearable(new PlateArms(), dropChance: 1);
	        SetWearable(new LeatherGorget(), dropChance: 1);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Prayer and meditation when idle
            if (Combatant == null && Utility.RandomDouble() < 0.1 && CheckCooldown("Prayer"))
            {
                PerformPrayer();
                SetCooldown("Prayer", TimeSpan.FromMinutes(5));
            }

            // Healing aura when consecrated
            if (IsConsecrated && CheckCooldown("HealingAura"))
            {
                PerformHealingAura();
                SetCooldown("HealingAura", TimeSpan.FromSeconds(30));
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Holy strike chance
            if (HolyPower >= 50 && CheckCooldown("HolyStrike"))
            {
                PerformHolyStrike(defender);
                SetCooldown("HolyStrike", TimeSpan.FromSeconds(30));
            }

            // Check if target is evil
            if (defender != null && !defender.Alive && defender.Karma < 0)
            {
                EvilsSlain++;
                HolyPower += 5;
                Experience += 50;
                Say($"Zło zostało pokonane! ({EvilsSlain} pokonanych)");
            }
        }

        private void PerformPrayer()
        {
            if (Happiness < 50)
            {
                Say("Moja wiara jest zbyt słaba na modlitwę...");
                return;
            }

            string[] prayers = new string[]
            {
                "* klęka do modlitwy *",
                "* intonuje święte słowa *",
                "* medytuje nad świętymi tekstami *",
                "* prosi o błogosławieństwo *",
                "* odprawia rytuał konsekracji *"
            };

            Say(prayers[Utility.Random(prayers.Length)]);
            
            if (Utility.RandomDouble() < 0.3)
            {
                HolyPower += 10;
                Experience += 10;
                
                if (!IsConsecrated && HolyPower >= 100)
                {
                    IsConsecrated = true;
                    Say("Zostałem konsekrowany! Święta moc przepełnia mnie!");
                    Effects.SendLocationEffect(Location, Map, 0x373A, 16, 10);
                }
            }
        }

        private void PerformHealingAura()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != null && m.Alive && m != this && m.Karma >= 0)
                {
                    int healAmount = 5 + (HolyPower / 10);
                    m.Heal(healAmount);
                    Effects.SendLocationEffect(m.Location, m.Map, 0x373A, 16, 10);
                }
            }
        }

        private void PerformHolyStrike(Mobile defender)
        {
            if (defender == null || !defender.Alive)
                return;

            Say("* przywołuje świętą moc *");
            
            int damage = 15 + (HolyPower / 5);
            if (defender.Karma < 0)
                damage *= 2;

            defender.Damage(damage, this);
            Effects.SendLocationEffect(defender.Location, defender.Map, 0x37C4, 16, 10);
            HolyPower -= 50;
        }

        public override bool OnBeforeDeath()
        {
            // Chance for divine intervention
            if (IsConsecrated && Utility.RandomDouble() < 0.5)
            {
                Hits = HitsMax;
                Mana = ManaMax;
                Stam = StamMax;
                IsConsecrated = false;
                HolyPower = 0;
                Say("Święta moc ochrania mnie przed śmiercią!");
                Effects.SendLocationEffect(Location, Map, 0x373A, 16, 10);
                return false;
            }

            return base.OnBeforeDeath();
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Paladin-specific level up bonuses
            RawStr += 1;
            RawDex += 1;
            RawInt += 1;
            
            // Improve paladin abilities
            SetSkill(SkillName.Chivalry, Skills[SkillName.Chivalry].Base + 1);
            SetSkill(SkillName.Healing, Skills[SkillName.Healing].Base + 1);
            
            if (Utility.RandomBool())
                SetSkill(SkillName.Swords, Skills[SkillName.Swords].Base + 1);
            else
                SetSkill(SkillName.MagicResist, Skills[SkillName.MagicResist].Base + 1);

            Say("Święta moc wzmacnia moje ciało i ducha!");
        }

        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Paladins with more holy power and evil slain are more valuable
            double holyBonus = (HolyPower / 100.0) + (EvilsSlain * 0.05);
            return (int)(baseFee * (1 + holyBonus));
        }

        public HirePaladin(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_HolyPower);
            writer.Write(m_EvilsSlain);
            writer.Write(m_LastPrayerTime);
            writer.Write(m_IsConsecrated);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_HolyPower = reader.ReadInt();
            m_EvilsSlain = reader.ReadInt();
            m_LastPrayerTime = reader.ReadDateTime();
            m_IsConsecrated = reader.ReadBool();
        }
    }
}
