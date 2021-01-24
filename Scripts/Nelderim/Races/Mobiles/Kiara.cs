using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Kiara : BaseCreature
	{


		[Constructable]
		public Kiara() : base( AIType.AI_Archer, FightMode.None, 12, 5, 0.2, 0.4  )
		{
            Race = Race.NTamael;
            Female = true;
			Name = "Kiara";
			Title = "- Lowczyni";
			Body = 0x191;
			Hue = 0x903;
			HairItemID = 0x203D;
			HairHue = 0x457;
							
			Str = 120;
			Dex = 160;
			Int = 60;
			Hits = 300;

			SetSkill( SkillName.Archery, 120.0 );
			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.Healing, 100.0 );
			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Swords, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Tracking, 100.0 );


			
			FemaleLeatherChest chest = new FemaleLeatherChest ();
			chest.Hue = 926;
			EquipItem ( chest );

			LeatherShorts legs = new LeatherShorts ();
			legs.Hue = 926;
			EquipItem ( legs );

			Bandana band = new Bandana ();
			band.Hue = 570;
			AddItem( band ); 
			
			Sandals Boot = new Sandals ();
			Boot.Hue = 926;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 563;
			AddItem ( Cloa );
			
			CompositeBow bow = new CompositeBow();
			bow.Hue = 570;

			AddItem( bow );			
			
			Container pack = new Backpack();

			pack.DropItem( new Arrow( 100 ) );

			pack.Movable = false;

			AddItem( pack );
		}


		public Kiara( Serial serial )
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
