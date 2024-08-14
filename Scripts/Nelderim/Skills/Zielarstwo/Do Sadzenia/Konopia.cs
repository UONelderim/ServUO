using System;

namespace Server.Items.Crops
{
	public class SzczepkaKonopia : BaseSeedling
	{
		public override Type PlantType => typeof(KrzakKonopia);
		public override SkillName[] SkillsRequired { get { return KrzakKonopia.cannabisSkills; } }

		[Constructable]
		public SzczepkaKonopia(int amount) : base(amount, 0xF27)
		{
			Hue = 0x5E2;
			Name = "Nasiona konopii";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaKonopia() : this(1)
		{
		}

		public SzczepkaKonopia(Serial serial) : base(serial)
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

	public class KrzakKonopia : Plant
	{
		public override Type SeedType => typeof(SzczepkaKonopia);
		public override Type CropType => typeof(PlonKonopia);

		protected override int YoungPlantGraphics => 0xCC7;
		protected override int MaturePlantGraphics => 0xCC7;

		public static SkillName[] cannabisSkills = new SkillName[] { SkillName.Herbalism, SkillName.Fletching };
		public override SkillName[] SkillsRequired { get { return cannabisSkills; } }

		[Constructable] 
		public KrzakKonopia() : base(0xCC7)
		{
			Hue = 0x58B;
			Name = "Krzak konopi"; // 1032612
			Stackable = true;
		}

		public KrzakKonopia( Serial serial ) : base( serial ) 
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
	
	public class PlonKonopia : Crop
	{
		public static int ReagentCount = 12;
		public override int DefaulReagentCount => ReagentCount; // Tyle samo co dawniej z dzikich krzaczkow. Wiecej niz z ziol typu mandragora, gdyz glownym harvest-skillem dla lukmistrza jest drwal.
        public override Type ReagentType => typeof(CannabisFiber);
		public override SkillName[] SkillsRequired { get{ return KrzakKonopia.cannabisSkills; } }

		[Constructable]
		public PlonKonopia( int amount ) : base( amount, 0x0C5F )
		{
			Name = "Lodyga konopi"; // 1032615
			Stackable = true;
		}

		[Constructable]
		public PlonKonopia() : this( 1 )
		{
		}

		public PlonKonopia( Serial serial ) : base( serial )
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

    public class CannabisFiber : Item
    {
        [Constructable]
		public CannabisFiber( int amount ) : base( 3166 )
		{
            Stackable = true;
            Amount = amount;
			//Hue = 0;
			Name = "Konopne wlokno"; // 1032616
            Weight = 0.15;
		}

        [Constructable]
		public CannabisFiber() : this(1)
		{
        }

		public CannabisFiber( Serial serial ) : base( serial )
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
