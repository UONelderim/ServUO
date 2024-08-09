using System;

namespace Server.Items.Crops
{
	public class SzczepkaJedwabSkalny : BaseSeedling
	{
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakJedwabSkalny);
		public override SkillName[] SkillsRequired { get { return KrzakJedwab.silkSkills; } }

		[Constructable]
		public SzczepkaJedwabSkalny(int amount) : base(amount, 0xF27)
		{
			Hue = 0x5E2;
			Name = "Nasiona skalnej rosliny dla jedwabnikow";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaJedwabSkalny() : this(1)
		{
		}

		public SzczepkaJedwabSkalny(Serial serial) : base(serial)
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

	public class KrzakJedwabSkalny : Plant
	{
		public override Type CropType => typeof(PlonJedwabSkalny);

		protected override int YoungPlantGraphics => Utility.Random(0xC51, 2);
		protected override int MaturePlantGraphics => Utility.Random(0xC53, 2);

        public override SkillName[] SkillsRequired { get{ return KrzakJedwab.silkSkills; } }

		[Constructable] 
		public KrzakJedwabSkalny() : base(Utility.Random(0xC51, 2))
		{
			Hue = 0xB42;
			Name = "Galezie z jedwabnikiem"; // 1032611
			Stackable = true;
		}

		public KrzakJedwabSkalny( Serial serial ) : base( serial ) 
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
	
	public class PlonJedwabSkalny : Crop
	{
		public override int DefaulReagentCount => PlonJedwab.ReagentCount;
		public override Type ReagentType => typeof(SilkFiber);
		public override SkillName[] SkillsRequired { get { return KrzakJedwab.silkSkills; } }

		[Constructable]
		public PlonJedwabSkalny( int amount ) : base( amount, 0x0DF9 ) //0x0DEF
		{
			Hue = 0xBAB;
			Name = "Kokon jedwabiu";
			Stackable = true;
		}
				public override void MutateReagent(Item reagent)
		{
			reagent.Hue = 0xAF5;
		}

		[Constructable]
		public PlonJedwabSkalny() : this( 1 )
		{
		}

		public PlonJedwabSkalny( Serial serial ) : base( serial )
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