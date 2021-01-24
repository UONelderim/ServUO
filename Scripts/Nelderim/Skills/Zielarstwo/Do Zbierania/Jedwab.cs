using System;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targets;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items.Crops
{
	public class ZrodloJedwab : WeedPlantZbieractwo
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecJedwab(count) ); }
        public override SkillName SkillRequired { get{ return SkillName.Herbalism; } }
        //public override int CropAmount { get{ return 5; } }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloJedwab() : base( Utility.Random(3153, 4) )
		{ 
			Hue = 1946;
			Name = "Roslina z jedwabnikiem"; // 1032611
		}

		public ZrodloJedwab( Serial serial ) : base( serial ) 
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
	
	public class SurowiecJedwab : WeedCropZbieractwo
	{
        public override int AmountOfReagent(double skill) { return 12; }
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new SilkFiber(count) ); }
		public override SkillName SkillRequired { get{ return SkillName.Fletching; } }

		[Constructable]
		public SurowiecJedwab( int amount ) : base( amount, 0x0DF9 ) //0x0DEF
		{
			Hue = 2886;
			Name = "Kokon jedwabiu"; // 1032614
		}

		[Constructable]
		public SurowiecJedwab() : this( 1 )
		{
		}

		public SurowiecJedwab( Serial serial ) : base( serial )
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
    
    public class SilkFiber : Item
    {
        [Constructable]
		public SilkFiber( int amount ) : base( 3166 )
		{
            Stackable = true;
            Amount = amount;
			Hue = 1150; // 2796
			Name = "Jedwabne wlokno"; // 1032617
            Weight = 0.15;
		}

        [Constructable]
		public SilkFiber() : this(1)
		{
        }

		public SilkFiber( Serial serial ) : base( serial )
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