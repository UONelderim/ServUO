using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Suriya : BaseCreature
	{


		[Constructable]
		public Suriya() : base( AIType.AI_Archer, FightMode.None, 12, 5, 0.2, 0.4  )
		{
			Race = Race.NTamael;
			Female = true;
			Name = "Suriya";
			Title = "- Zwiadowca";
			Body = 0x190;
			Hue = 0x361;
			HairItemID = 0x203B;
			HairHue = 0x2B3;
			FacialHairItemID = 0x204B;
			FacialHairHue = 0x2B3;
							
			Str = 140;
			Dex = 140;
			Int = 60;
			Hits = 300;

			SetSkill( SkillName.Archery, 110.0 );
			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Healing, 100.0 );
			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Swords, 80.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Tracking, 60.0 );


			
			LeatherChest chest = new LeatherChest ();
			chest.Hue = 1053;
			EquipItem ( chest );

			LeatherLegs legs = new LeatherLegs ();
			legs.Hue = 1053;
			EquipItem ( legs );
			
			LeatherArms arms = new LeatherArms ();
			arms.Hue = 1053;
			EquipItem ( arms );
			
			LeatherGloves gloves = new LeatherGloves ();
			gloves.Hue = 1053;
			EquipItem ( gloves );

			LeatherCap band = new LeatherCap ();
			band.Hue = 1053;
			AddItem( band ); 
			
			FurBoots Boot = new FurBoots ();
			Boot.Hue = 1044;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 1044;
			AddItem ( Cloa );
			
			Bow bow = new Bow();
			bow.Hue = 595;

			AddItem( bow );			
			
			Container pack = new Backpack();

			pack.DropItem( new Arrow( 100 ) );

			pack.Movable = false;

			AddItem( pack );
		}


		public Suriya( Serial serial )
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
