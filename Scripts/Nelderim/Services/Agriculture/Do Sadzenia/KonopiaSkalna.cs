using System;

namespace Server.Items.Crops
{
	public class SzczepkaKonopiaSkalna : BaseSeedling
	{
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakKonopiaSkalna);
		public override SkillName[] SkillsRequired { get { return KrzakKonopia.cannabisSkills; } }

		[Constructable]
		public SzczepkaKonopiaSkalna(int amount) : base(amount, 0xF27)
		{
			Hue = 0x5E2;
			Name = "Nasiona konopii skalnej";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaKonopiaSkalna() : this(1)
		{
		}

		public SzczepkaKonopiaSkalna(Serial serial) : base(serial)
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

	public class KrzakKonopiaSkalna : Plant
	{
		public override Type SeedType => typeof(SzczepkaKonopiaSkalna);
		public override Type CropType => typeof(PlonKonopiaSkalna);

		protected override int YoungPlantGraphics => 0xD31;
		protected override int MaturePlantGraphics => 0xD31;

		public override SkillName[] SkillsRequired { get { return KrzakKonopia.cannabisSkills; } }

		[Constructable] 
		public KrzakKonopiaSkalna() : base(0xD31)
		{
			Hue = 0xB42;
			Name = "Konopia skalna";
			Stackable = true;
		}

		public KrzakKonopiaSkalna( Serial serial ) : base( serial ) 
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
	
	public class PlonKonopiaSkalna : Crop
	{
        public override int DefaulReagentCount => PlonKonopia.ReagentCount;
        public override Type ReagentType => typeof(CannabisFiber);
		public override SkillName[] SkillsRequired { get{ return KrzakKonopia.cannabisSkills; } }

		public override void MutateReagent(Item reagent)
		{
			reagent.Hue = 0x949;
		}

		[Constructable]
		public PlonKonopiaSkalna( int amount ) : base( amount, 0x0C5F )
		{
			Hue = 0x949;
			Name = "Lodyga konopi"; // 1032615
			Stackable = true;
		}

		[Constructable]
		public PlonKonopiaSkalna() : this( 1 )
		{
		}

		public PlonKonopiaSkalna( Serial serial ) : base( serial )
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