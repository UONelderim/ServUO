using System;

namespace Server.Items.Crops
{
	public class SzczepkaJedwab : BaseSeedling
	{
		public override Type PlantType => typeof(KrzakJedwab);
		public override SkillName[] SkillsRequired { get { return KrzakJedwab.silkSkills; } }

		[Constructable]
		public SzczepkaJedwab(int amount) : base(amount, 0xF27)
		{
			Hue = 0x5E2;
			Name = "Nasiona rosliny dla jedwabnikow";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaJedwab() : this(1)
		{
		}

		public SzczepkaJedwab(Serial serial) : base(serial)
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

	public class KrzakJedwab : Plant
	{
		public override Type SeedType => typeof(SzczepkaJedwab);
		public override Type CropType => typeof(PlonJedwab);

		protected override int YoungPlantGraphics => Utility.Random(0xC51, 2);
		protected override int MaturePlantGraphics => Utility.Random(0xC53, 2);

		public static SkillName[] silkSkills = new SkillName[] { SkillName.Herbalism, SkillName.Fletching };
        public override SkillName[] SkillsRequired { get{ return silkSkills; } }

		[Constructable] 
		public KrzakJedwab() : base(Utility.Random(0xC51, 2))
		{ 
			Hue = 2082;
			Name = "Roslina z jedwabnikiem"; // 1032611
			Stackable = true;
		}

		public KrzakJedwab( Serial serial ) : base( serial ) 
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
	
	public class PlonJedwab : Crop
	{
		public static int ReagentCount = 12;
		public override int DefaulReagentCount => ReagentCount; // Tyle samo co dawniej z dzikich krzaczkow. Wiecej niz z ziol typu mandragora, gdyz glownym harvest-skillem dla lukmistrza jest drwal.
		public override Type ReagentType => typeof(SilkFiber);
		public override SkillName[] SkillsRequired { get { return KrzakJedwab.silkSkills; } }

		[Constructable]
		public PlonJedwab( int amount ) : base( amount, 0x0DF9 ) //0x0DEF
		{
			Hue = 2886;
			Name = "Kokon jedwabiu"; // 1032614
			Stackable = true;
		}

		[Constructable]
		public PlonJedwab() : this( 1 )
		{
		}

		public PlonJedwab( Serial serial ) : base( serial )
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
		public SilkFiber( int amount ) : base(0xC5E)
		{
            Stackable = true;
            Amount = amount;
			Hue = 0x47E; // 2796
			Name = "Jedwabne wlokno";
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
