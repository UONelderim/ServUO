using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Abnor : BaseCreature
	{


		[Constructable]
		public Abnor() : base( AIType.AI_Melee, FightMode.None, 12, 5, 0.2, 0.4  )
		{
            Race = Race.NTamael;
            Female = true;
			Name = "Abnor";
			Title = "- Zwiadowca";
			Body = 0x190;
			Hue = 0x361;
			HairItemID = 0x203C;
			HairHue = 0x2B2;
			FacialHairItemID = 0x204C;
			FacialHairHue = 0x2B2;
							
			Str = 180;
			Dex = 100;
			Int = 60;
			Hits = 400;

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Healing, 100.0 );
			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Swords, 120.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Parry, 120.0 );


			
			RingmailChest chest = new RingmailChest ();
			chest.Hue = 761;
			EquipItem ( chest );

			RingmailLegs legs = new RingmailLegs ();
			legs.Hue = 761;
			EquipItem ( legs );
			
			RingmailArms arms = new RingmailArms ();
			arms.Hue = 761;
			EquipItem ( arms );
			
			RingmailGloves gloves = new RingmailGloves ();
			gloves.Hue = 761;
			EquipItem ( gloves );

			NorseHelm band = new NorseHelm ();
			band.Hue = 761;
			AddItem( band ); 
			
			FurBoots Boot = new FurBoots ();
			Boot.Hue = 1044;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 1044;
			AddItem ( Cloa );
			
			VikingSword sword = new VikingSword();
			sword.Hue = 711;
			EquipItem ( sword );
			
			BronzeShield shield = new BronzeShield();
			shield.Hue = 711;
			EquipItem ( shield );
		
			
			Container pack = new Backpack();

			pack.DropItem( new Arrow( 100 ) );

			pack.Movable = false;

			AddItem( pack );
		}


		public Abnor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
