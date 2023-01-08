using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("zwloki pirata")]
    public class NPirateCrew : BaseCreature
    {
        [Constructable]
        public NPirateCrew() : base(AIType.AI_Archer, FightMode.Closest, 15, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            Hue = Race.Human.RandomSkinHue();

            if (Female == Utility.RandomBool())
            {
                Body = 0x191;
                //Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                //Name = NameList.RandomName("male");           
            }

            Title = "- pirat";

            AddItem(new ThighBoots());

            SetStr(145, 155);
            SetDex(181, 195);
            SetInt(61, 75);
            SetHits(288, 308);

            SetDamage(10, 15);

            SetSkill(SkillName.Fencing, 86.0, 97.5);
            SetSkill(SkillName.Macing, 85.0, 87.5);
            SetSkill(SkillName.MagicResist, 55.0, 67.5);
            SetSkill(SkillName.Swords, 85.0, 87.5);
            SetSkill(SkillName.Tactics, 85.0, 87.5);
            SetSkill(SkillName.Wrestling, 35.0, 37.5);
            SetSkill(SkillName.Archery, 85.0, 87.5);

            CantWalk = false;

            Fame = 2000;
            Karma = -2000;
            VirtualArmor = 66;

            switch (Utility.Random(1))
            {
                case 0:
                    AddItem(new LongPants(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new ShortPants(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new FancyShirt(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new Shirt(Utility.RandomRedHue()));
                    break;
                case 2:
                    AddItem(new Doublet(Utility.RandomRedHue()));
                    break;
            }


            switch (Utility.Random(3))
            {
                case 0:
                    AddItem(new Bandana(Utility.RandomRedHue()));
                    break;
                case 1:
                    AddItem(new SkullCap(Utility.RandomRedHue()));
                    break;
            }

            switch (Utility.Random(5))
            {
                case 0:
                    AddItem(new Bow());
                    break;
                case 1:
                    AddItem(new CompositeBow());
                    break;
                case 2:
                    AddItem(new Crossbow());
                    break;
                case 3:
                    AddItem(new RepeatingCrossbow());
                    break;
                case 4:
                    AddItem(new HeavyCrossbow());
                    break;
            }
        }

        protected override void OnCreate()
        {
	        base.OnCreate();
	        
	        RaceGenerator.Init(this);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override bool IsScaredOfScaryThings => false;

        public override bool AlwaysMurderer => true;

        public override bool CanRummageCorpses => true;

        public override bool PlayerRangeSensitive => false;

        public NPirateCrew(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
