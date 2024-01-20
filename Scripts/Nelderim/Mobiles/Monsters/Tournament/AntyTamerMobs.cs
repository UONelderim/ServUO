using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

namespace Server.Mobiles.Swiateczne
{
    public class AntyTamerMob : BaseCreature
    {

            [Constructable]
        public AntyTamerMob() : base(AIType.AI_Melee, FightMode.Closest, 10, 7, 0.2, 0.4)
        {
            Name = "Kanibal z Tasandory";

            Body = 400;
            Hue = Tamael.Instance.RandomSkinHue();

            SetStr(800, 900);
            SetDex(200, 220);
            SetInt(600, 650);
            SetHits(1500);
            SetStam(205, 300);

            SetDamage(19, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 55);
            SetResistance(ResistanceType.Fire, 55);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 75);

            SetSkill(SkillName.MagicResist, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.Archery, 120.0, 120.0);
            SetSkill(SkillName.Anatomy, 120.0, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 80;
            AddItem(new Shirt(38));
            AddItem(new Surcoat());
            AddItem(new LongPants());
            AddItem(new FurCape(0x497));
            AddItem(new ElvenBoots());
            
            AddItem(new Katana());
      

            HairItemID = 0x203C;
            HairHue = 0x47F;

            FacialHairItemID = 0x204B;
            FacialHairHue = 0x47F;
            

        }
		
				public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );
			
			// eats pet or summons
			if ( from is BaseCreature )
			{
				BaseCreature creature = (BaseCreature) from;
				
				if ( creature.Controlled || creature.Summoned )
				{
					Heal( creature.Hits );					
					creature.Kill();				
					
					Effects.PlaySound( Location, Map, 0x574 );
				}
			}
		}
		
        public override void OnDeath( Container c )
		{
			base.OnDeath( c );

		}

        public AntyTamerMob(Serial serial) : base(serial)
        {
        }

        public override bool BardImmune
        {
            get { return true; }
        }
        
        public override double DispelDifficulty
        {
            get { return 135.0; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Lethal; }
        }



        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}