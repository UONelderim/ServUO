using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Riddock : BaseCreature
	{


		[Constructable]
		public Riddock() : base( AIType.AI_Mage, FightMode.None, 12, 5, 0.2, 0.4  )
		{
            Race = Race.NTamael;
            Female = true;
			Name = "Riddock";
			Title = "- Mag";
			Body = 0x190;
			Hue = 0x906;
			HairItemID = 0x2046;
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
			SetSkill( SkillName.ItemID, 80.0 );


			
			FancyShirt chest = new FancyShirt ();
			chest.Hue = 51;
			EquipItem ( chest );

			LongPants legs = new LongPants ();
			legs.Hue = 800;
			EquipItem ( legs );
			
			WizardsHat band = new WizardsHat ();
			band.Hue = 111;
			AddItem( band ); 
			
			Boots Boot = new Boots ();
			Boot.Hue = 2108;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 560;
			AddItem ( Cloa );

			GnarledStaff staff = new GnarledStaff();	
			staff.Attributes.SpellChanneling = 1;
			AddItem(staff);				

			//EquipItem ( sword );
					
			
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem( pack );
		}


		public Riddock( Serial serial )
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
