using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class Rangwak : BaseCreature
	{


		[Constructable]
		public Rangwak() : base( AIType.AI_Mage, FightMode.None, 12, 5, 0.2, 0.4  )
		{
            Race = Race.NTamael;
            Female = true;
			Name = "Rangwak";
			//Title = "- Wojownik";
			Body = 0x190;
			Hue = 0x389;
			HairItemID = 0x203C;
			HairHue = 916;
			FacialHairItemID = 0x204C;
			FacialHairHue = 916;
							
			Str = 100;
			Dex = 100;
			Int = 200;
			Hits = 300;

			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.Necromancy, 100.0 );
			SetSkill( SkillName.SpiritSpeak, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			
			Shirt chest = new Shirt ();
			chest.Hue = 216;
			EquipItem ( chest );

			LongPants legs = new LongPants ();
			legs.Hue = 216;
			EquipItem ( legs );
			
			WizardsHat band = new WizardsHat ();
			band.Hue = 500;
			AddItem( band ); 
			
			Sandals Boot = new Sandals ();
			Boot.Hue = 1337;
			AddItem ( Boot );
			
			Cloak Cloa = new Cloak();
			Cloa.Hue = 842;
			AddItem ( Cloa );

			QuarterStaff staff = new QuarterStaff();
			staff.Attributes.SpellChanneling = 1;
			AddItem(staff);			
			
			Container pack = new Backpack();

			pack.Movable = false;

			AddItem( pack );
		}


		public Rangwak( Serial serial )
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
