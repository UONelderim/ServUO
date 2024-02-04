using System;

namespace Server.Items.Crops
{
	// TODO: ustawic dodatkowy skill krawiectwo i zwiekszyc progi umozliwiajace zbieranie
	
	public class SzczepkaBawelna : VegetableSeedling
	{
		public override Type PlantType => typeof(KrzakBawelna);

        [Constructable]
		public SzczepkaBawelna( int amount ) : base( amount, 6946 ) 
		{
			Hue = 661;
			Name = "Ziarno bawelny";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaBawelna() : this( 1 )
		{
		}

		public SzczepkaBawelna( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class KrzakBawelna : VegetablePlant
    {
        public override Type SeedType => typeof(SzczepkaBawelna);
        public override Type CropType => typeof(Cotton);
		protected override int YoungPlantGraphics => Utility.RandomList(0x0C53, 0x0C54);
		protected override int MaturePlantGraphics => Utility.RandomList(0x0C4F, 0x0C50);

		[Constructable] 
		public KrzakBawelna() : base(0x0C53)
		{
			Hue = 0;
			Name = "Krzak bawelny";
			Stackable = true;
        }

		public KrzakBawelna( Serial serial ) : base( serial ) 
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

	public class PlonBawelna : Crop
	{
		public override Type ReagentType => typeof(Cotton);

		[Constructable]
		public PlonBawelna(int amount) : base(amount, 3577)
		{
			Hue = 661;
			Name = "Klebek bawelny";
			Stackable = true;
		}

		[Constructable]
		public PlonBawelna() : this(1)
		{
		}

		public PlonBawelna(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}


}