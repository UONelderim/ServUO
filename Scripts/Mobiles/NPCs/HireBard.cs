using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HireBard : BaseAdvancedHire
    {
        [Constructable]
        public HireBard() : base(AIType.AI_Archer)
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
	        Title = "- bard";
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

        // Add bard-specific level up bonuses
        protected override void OnLevelUp()
        {
            base.OnLevelUp();

            // Bards get extra Dexterity and Intelligence
            if (Utility.RandomBool())
                RawDex += 1;
            else
                RawInt += 1;

            // Improve musical skills
            SetSkill(SkillName.Musicianship, Skills[SkillName.Musicianship].Base + 1);
            SetSkill(SkillName.Peacemaking, Skills[SkillName.Peacemaking].Base + 1);

            // Random chance to improve combat skills
            if (Utility.RandomDouble() < 0.3)
                SetSkill(SkillName.Archery, Skills[SkillName.Archery].Base + 1);
            if (Utility.RandomDouble() < 0.3)
                SetSkill(SkillName.Swords, Skills[SkillName.Swords].Base + 1);
        }

        // Override the hourly fee calculation for bards
        protected override int CalculateHourlyFee()
        {
            int baseFee = base.CalculateHourlyFee();
            
            // Bards with high musicianship and peacemaking are more valuable
            double musicBonus = (Skills[SkillName.Musicianship].Base + Skills[SkillName.Peacemaking].Base) / 200.0;
            return (int)(baseFee * (1 + musicBonus));
        }

        // Add bard-specific behavior
        public override void OnThink()
        {
	        base.OnThink();

	        // Occasionally play music when idle
	        if (Utility.RandomDouble() < 0.1 && CheckCooldown("PlayMusic"))
	        {
		        PlayRandomMusic();
		        SetCooldown("PlayMusic", TimeSpan.FromMinutes(5));
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
                "♪ La la la... ♫",
                "♫ W krainie dalekiej... ♪",
                "♪ Opowiem ci historię... ♫",
                "♫ Posłuchaj pieśni starej... ♪"
            };

            Say(songs[Utility.Random(songs.Length)]);
            
            // Small chance to gain experience from performing
            if (Utility.RandomDouble() < 0.2)
                Experience += 5;
        }

        public override void GenerateLoot()
        {
        }

        public HireBard(Serial serial) : base(serial)
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
