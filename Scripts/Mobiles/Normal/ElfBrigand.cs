using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an elf corpse")]
    public class ElfBrigand : BaseCreature
    {
        [Constructable]
        public ElfBrigand()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Race = Race.Elf;

            if (Female = Utility.RandomBool())
            {
                Body = 606;
                Name = NameList.RandomName("Elf female");
            }
            else
            {
                Body = 605;
                Name = NameList.RandomName("Elf male");
            }

            Title = "- banita";
            Hue = Race.RandomSkinHue();

            SetStr(86, 100);
            SetDex(81, 95);
            SetInt(61, 75);

            SetDamage(10, 23);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Spellweaving, 50.0, 75.0);
            SetSkill(SkillName.Focus, 50.0, 75.0);

            Fame = 1000;
            Karma = -1000;

            // outfit
            SetWearable(new Shirt(), Utility.RandomNeutralHue(), 1);

            switch (Utility.Random(4))
            {
                case 0:
					SetWearable(new Sandals(), dropChance: 1);
                    break;
                case 1:
					SetWearable(new Shoes(), dropChance: 1);
                    break;
                case 2:
					SetWearable(new Boots(), dropChance: 1);
                    break;
                case 3:
					SetWearable(new ThighBoots(), dropChance: 1);
                    break;
            }

            if (Female)
            {
                if (Utility.RandomBool())
					SetWearable(new Skirt(), Utility.RandomNeutralHue(), 1);
                else
					SetWearable(new Kilt(), Utility.RandomNeutralHue(), 1);
            }
            else
				SetWearable(new ShortPants(), Utility.RandomNeutralHue(), 1);

            // hair, facial hair			
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // weapon, shield
            BaseWeapon weapon = Loot.RandomWeapon();

			SetWearable(weapon, dropChance: 1);
			SetSkill(weapon.Skill, 15.0, 37.5);

            if (weapon.Layer == Layer.OneHanded && Utility.RandomBool())
                SetWearable(Loot.RandomShield(), dropChance: 1);
        }

        public ElfBrigand(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LootGold(50, 150));
            AddLoot(LootPack.LootItem<SeveredElfEars>(75.0, 1));
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
