using Server.Items;
using Server.Spells;
using System;

namespace Server.Mobiles
{
    public class HireMage : BaseAdvancedHire
    {
        private int m_SpellPower;
        private int m_SpellsCast;
        private DateTime m_LastSpellTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpellPower 
        { 
            get => m_SpellPower; 
            set => m_SpellPower = Math.Max(0, Math.Min(100, value)); 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpellsCast 
        { 
            get => m_SpellsCast; 
            set => m_SpellsCast = value; 
        }

        [Constructable]
        public HireMage() : base(AIType.AI_Mage)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Race.RandomSkinHue();
            Title = "- mag";
            
            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                SetWearable(new ShortPants(), Utility.RandomNeutralHue(), 1);
            }

            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(61, 75);
            SetDex(81, 95);
            SetInt(86, 100);

            SetDamage(10, 23);

            SetSkill(SkillName.EvalInt, 100.0, 125);
            SetSkill(SkillName.Magery, 100, 125);
            SetSkill(SkillName.Meditation, 100, 125);
            SetSkill(SkillName.MagicResist, 100, 125);
            SetSkill(SkillName.Tactics, 100, 125);
            SetSkill(SkillName.Macing, 100, 125);

            Fame = 100;
            Karma = 100;

            SetWearable(new Shirt(), dropChance: 1);
            SetWearable(new Robe(), Utility.RandomNeutralHue(), 1);

            if (Utility.RandomBool())
                SetWearable(new Shoes(), Utility.RandomNeutralHue(), 1);
            else
                SetWearable(new ThighBoots(), dropChance: 1);

            PackGold(20, 100);

            m_SpellPower = 0;
            m_SpellsCast = 0;
            m_LastSpellTime = DateTime.MinValue;
        }

        public override void OnThink()
        {
            base.OnThink();

            // Magical study when idle
            if (Combatant == null && Utility.RandomDouble() < 0.1 && CheckCooldown("Study"))
            {
                StudyMagic();
                SetCooldown("Study", TimeSpan.FromMinutes(5));
            }

            // Mana regeneration based on meditation
            if (Mana < ManaMax && CheckCooldown("ManaRegen"))
            {
                int regenAmount = (int)(Skills[SkillName.Meditation].Base / 10);
                Mana = Math.Min(ManaMax, Mana + regenAmount);
                SetCooldown("ManaRegen", TimeSpan.FromSeconds(5));
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            
            // Chance to cast a spell after melee attack
            if (Mana >= 20 && CheckCooldown("CombatSpell"))
            {
                CastCombatSpell(defender);
                SetCooldown("CombatSpell", TimeSpan.FromSeconds(10));
            }
        }

        private void StudyMagic()
        {
            if (Happiness < 50)
            {
                Say("Nie mogę się skupić na nauce magii...");
                return;
            }

            string[] studies = new string[]
            {
                "* studiuje starożytne zwoje *",
                "* praktykuje gesty magiczne *",
                "* recytuje zaklęcia *",
                "* medytuje nad naturą magii *",
                "* zapisuje nowe formuły magiczne *"
            };

            Say(studies[Utility.Random(studies.Length)]);
            
            if (Utility.RandomDouble() < 0.3)
            {
                SpellPower += 5;
                Experience += 10;
                Say("Odkryłem nowy aspekt magii!");
            }
        }

        private void CastCombatSpell(Mobile defender)
        {
            if (defender == null || !defender.Alive)
                return;

            m_LastSpellTime = DateTime.UtcNow;
            m_SpellsCast++;

            int spellEffect = Utility.Random(100) + SpellPower;
            
            if (spellEffect > 150)
            {
                // Master level spell
                Say("* rzuca potężne zaklęcie *");
                defender.Damage(30 + Utility.Random(20), this);
                Effects.SendLocationEffect(defender.Location, defender.Map, 0x3709, 15);
            }
            else if (spellEffect > 100)
            {
                // Advanced spell
                Say("* przywołuje magiczną burzę *");
                defender.Damage(20 + Utility.Random(15), this);
                Effects.SendLocationEffect(defender.Location, defender.Map, 0x37CC, 10);
            }
            else
            {
                // Basic spell
                Say("* rzuca magiczny pocisk *");
                defender.Damage(10 + Utility.Random(10), this);
                Effects.SendLocationEffect(defender.Location, defender.Map, 0x36B0, 5);
            }

            Mana -= 20;
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Mage-specific level up bonuses
            RawInt += 2;
            
            // Improve magical abilities
            SetSkill(SkillName.Magery, Skills[SkillName.Magery].Base + 1);
            SetSkill(SkillName.EvalInt, Skills[SkillName.EvalInt].Base + 1);
            
            if (Utility.RandomBool())
                SetSkill(SkillName.Meditation, Skills[SkillName.Meditation].Base + 1);
            else
                SetSkill(SkillName.MagicResist, Skills[SkillName.MagicResist].Base + 1);

            Say("Czuję, jak moja magiczna moc rośnie!");
        }

        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Mages with more spell power and experience are more valuable
            double magicBonus = (SpellPower / 100.0) + (m_SpellsCast / 1000.0);
            return (int)(baseFee * (1 + magicBonus));
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            SpellPower = Math.Max(0, SpellPower - 10);
            Say("Moja moc słabnie...");
        }

        public HireMage(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_SpellPower);
            writer.Write(m_SpellsCast);
            writer.Write(m_LastSpellTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_SpellPower = reader.ReadInt();
            m_SpellsCast = reader.ReadInt();
            m_LastSpellTime = reader.ReadDateTime();
        }
    }
}
