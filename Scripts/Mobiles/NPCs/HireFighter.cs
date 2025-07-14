using Server.Items;
using System;

namespace Server.Mobiles
{
    public class HireFighter : BaseAdvancedHire
    {
        private int m_ComboPoints;
        private DateTime m_LastCombatAction;
        private int m_BattlesWon;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ComboPoints 
        { 
            get => m_ComboPoints; 
            set => m_ComboPoints = Math.Max(0, Math.Min(100, value)); 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BattlesWon 
        { 
            get => m_BattlesWon; 
            set => m_BattlesWon = value; 
        }

        [Constructable]
        public HireFighter() : base(AIType.AI_Melee)
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

            Title = "- wojownik";
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            SetStr(91, 91);
            SetDex(91, 91);
            SetInt(50, 50);

            SetDamage(7, 14);

            SetSkill(SkillName.Tactics, 36, 67);
            SetSkill(SkillName.Magery, 22, 22);
            SetSkill(SkillName.Swords, 64, 100);
            SetSkill(SkillName.Parry, 60, 82);
            SetSkill(SkillName.Macing, 36, 67);
            SetSkill(SkillName.Focus, 36, 67);
            SetSkill(SkillName.Wrestling, 25, 47);

            Fame = 100;
            Karma = 100;

            // Equipment setup (existing code)...
            InitializeEquipment();

            m_ComboPoints = 0;
            m_LastCombatAction = DateTime.MinValue;
            m_BattlesWon = 0;
        }

        private void InitializeEquipment()
        {
	        switch (Utility.Random(2))
	        {
		        case 0:
			        SetWearable(new Shoes(), Utility.RandomNeutralHue(), 1);
			        break;
		        case 1:
			        SetWearable(new Boots(), Utility.RandomNeutralHue(), 1);
			        break;
	        }

	        SetWearable(new Shirt());

	        // Pick a random sword
	        switch (Utility.Random(5))
	        {
		        case 0:
			        SetWearable(new Longsword(), dropChance: 1);
			        break;
		        case 1:
			        SetWearable(new Broadsword(), dropChance: 1);
			        break;
		        case 2:
			        SetWearable(new VikingSword(), dropChance: 1);
			        break;
		        case 3:
			        SetWearable(new BattleAxe(), dropChance: 1);
			        break;
		        case 4:
			        SetWearable(new TwoHandedAxe(), dropChance: 1);
			        break;
	        }

	        // Pick a random shield
	        if (FindItemOnLayer(Layer.TwoHanded) == null)
	        {
		        switch (Utility.Random(8))
		        {
			        case 0:
				        SetWearable(new BronzeShield(), dropChance: 1);
				        break;
			        case 1:
				        SetWearable(new HeaterShield(), dropChance: 1);
				        break;
			        case 2:
				        SetWearable(new MetalKiteShield(), dropChance: 1);
				        break;
			        case 3:
				        SetWearable(new MetalShield(), dropChance: 1);
				        break;
			        case 4:
				        SetWearable(new WoodenKiteShield(), dropChance: 1);
				        break;
			        case 5:
				        SetWearable(new WoodenShield(), dropChance: 1);
				        break;
			        case 6:
				        SetWearable(new OrderShield(), dropChance: 1);
				        break;
			        case 7:
				        SetWearable(new ChaosShield(), dropChance: 1);
				        break;
		        }
	        }

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

	        // Pick some armour
	        switch (Utility.Random(4))
	        {
		        case 0: // Leather
			        SetWearable(new LeatherChest(), dropChance: 1);
			        SetWearable(new LeatherArms(), dropChance: 1);
			        SetWearable(new LeatherGloves(), dropChance: 1);
			        SetWearable(new LeatherGorget(), dropChance: 1);
			        SetWearable(new LeatherLegs(), dropChance: 1);
			        break;
		        case 1: // Studded Leather
			        SetWearable(new StuddedChest(), dropChance: 1);
			        SetWearable(new StuddedArms(), dropChance: 1);
			        SetWearable(new StuddedGloves(), dropChance: 1);
			        SetWearable(new StuddedGorget(), dropChance: 1);
			        SetWearable(new StuddedLegs(), dropChance: 1);
			        break;
		        case 2: // Ringmail
			        SetWearable(new RingmailChest(), dropChance: 1);
			        SetWearable(new RingmailArms(), dropChance: 1);
			        SetWearable(new RingmailGloves(), dropChance: 1);
			        SetWearable(new RingmailLegs(), dropChance: 1);
			        break;
		        case 3: // Chain
			        SetWearable(new ChainChest(), dropChance: 1);
			        //SetWearable(new ChainCoif(), dropChance: 1);
			        SetWearable(new ChainLegs(), dropChance: 1);
			        break;
	        }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Combat training when idle
            if (Combatant == null && Utility.RandomDouble() < 0.1 && CheckCooldown("Training"))
            {
                PerformTraining();
                SetCooldown("Training", TimeSpan.FromMinutes(5));
            }

            // Reset combo points if out of combat too long
            if (DateTime.UtcNow - m_LastCombatAction > TimeSpan.FromSeconds(10))
                ComboPoints = 0;
        }

        public override void OnKilledBy(Mobile killer)
        {
            base.OnKilledBy(killer);
            ComboPoints = 0;
            Say("Zostałem pokonany... Muszę więcej trenować!");
        }

// Replace OnKilled with OnDeath
        public override void OnDeath(Container c)
        {
	        base.OnDeath(c);
	        ComboPoints = 0;
	        Say("Zostałem pokonany... Muszę więcej trenować!");
        }

// Remove OnAfterKill and modify OnGaveMeleeAttack to handle victories
        public override void OnGaveMeleeAttack(Mobile defender)
        {
	        base.OnGaveMeleeAttack(defender);
    
	        m_LastCombatAction = DateTime.UtcNow;
	        ComboPoints = Math.Min(100, ComboPoints + 10);

	        // Check if the hit killed the defender
	        if (defender != null && !defender.Alive)
	        {
		        BattlesWon++;
		        Experience += 50; // Bonus XP for victory
        
		        if (BattlesWon % 10 == 0)
			        Say($"To moje {BattlesWon} zwycięstwo w walce!");
	        }

	        // Special attacks based on combo points
	        if (ComboPoints >= 50 && CheckCooldown("SpecialAttack"))
	        {
		        PerformSpecialAttack(defender);
		        SetCooldown("SpecialAttack", TimeSpan.FromSeconds(30));
	        }
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Fighter-specific level up bonuses
            RawStr += 2;
            RawDex += 1;
            
            // Combat skill improvements
            SetSkill(SkillName.Tactics, Skills[SkillName.Tactics].Base + 1);
            SetSkill(SkillName.Swords, Skills[SkillName.Swords].Base + 1);
            
            if (Utility.RandomBool())
                SetSkill(SkillName.Parry, Skills[SkillName.Parry].Base + 1);
            else
                SetSkill(SkillName.Focus, Skills[SkillName.Focus].Base + 1);

            Say("Czuję się silniejszy! Moje umiejętności bojowe rosną!");
        }

        private void PerformTraining()
        {
            if (Happiness < 50)
            {
                Say("Jestem zbyt zmęczony na trening...");
                return;
            }

            string[] training = new string[]
            {
                "* ćwiczy ciosy mieczem *",
                "* trenuje bloki tarczą *",
                "* wykonuje sekwencje bojowe *",
                "* doskonali technikę walki *",
                "* hartuje ciało i ducha *"
            };

            Say(training[Utility.Random(training.Length)]);
            
            if (Utility.RandomDouble() < 0.3)
            {
                Experience += 10;
                Say("Ten trening był owocny!");
            }
        }

// For DefenseChance, we'll use a different approach with temporary skill bonus
        private void PerformSpecialAttack(Mobile defender)
        {
	        if (defender == null || !defender.Alive)
		        return;

	        switch (Utility.Random(3))
	        {
		        case 0: // Power Attack
			        Say("* wykonuje potężne uderzenie *");
			        defender.Damage(Utility.Random(10, 20), this);
			        break;
            
		        case 1: // Stunning Strike
			        Say("* próbuje ogłuszyć przeciwnika *");
			        defender.Freeze(TimeSpan.FromSeconds(2));
			        break;
            
		        case 2: // Defensive Stance
			        Say("* przyjmuje pozycję obronną *");
			        // Instead of DefenseChance, temporarily boost Parry skill
			        Skills[SkillName.Parry].Base += 20;
			        Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
			        {
				        Skills[SkillName.Parry].Base -= 20;
			        });
			        break;
	        }

	        ComboPoints -= 50;
        }

        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Warriors with more victories are more expensive
            double victoryBonus = BattlesWon * 0.05;
            return (int)(baseFee * (1 + victoryBonus));
        }

        public HireFighter(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle => false;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_ComboPoints);
            writer.Write(m_BattlesWon);
            writer.Write(m_LastCombatAction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ComboPoints = reader.ReadInt();
            m_BattlesWon = reader.ReadInt();
            m_LastCombatAction = reader.ReadDateTime();
        }
    }
}
