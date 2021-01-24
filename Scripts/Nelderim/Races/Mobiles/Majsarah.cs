using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Majsarah : BaseCreature
	{


		[Constructable]
		public Majsarah() : base( AIType.AI_Mage, FightMode.None, 12, 5, 0.2, 0.4  )
		{
            Race = Race.NTamael;
            Female = true;
			Name = "Majsarah";
			Title = "- Mag";
			Body = 0x190;
			Hue = 0x906;
			HairItemID = 0x2048;
			HairHue = 0x457;
			FacialHairItemID = 0x203F;
			HairHue = 0x457;
							
			Str = 120;
			Dex = 100;
			Int = 300;
			Hits = 300;

			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Alchemy, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.ItemID, 60.0 );


			
			FancyShirt chest = new FancyShirt ();
			chest.Hue = 456;
			EquipItem ( chest );

			ShortPants legs = new ShortPants ();
			legs.Hue = 455;
			EquipItem ( legs );
			
			WizardsHat band = new WizardsHat ();
			band.Hue = 355;
			AddItem( band ); 
			
			Sandals Boot = new Sandals ();
			Boot.Hue = 641;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 448;
			AddItem ( Cloa );

			BlackStaff staff = new BlackStaff();	
			staff.Attributes.SpellChanneling = 1;
			AddItem(staff);				

		
			
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem( pack );
		}


		public Majsarah( Serial serial )
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
