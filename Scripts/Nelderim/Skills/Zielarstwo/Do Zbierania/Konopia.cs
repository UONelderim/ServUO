using System;

namespace Server.Items.Crops
{
	public class ZrodloKonopia : ResourceVein
    {
        public override Type CropType => typeof(SurowiecJedwab);
		protected override int MaturePlantGraphics => 0x0CC3;

		public static SkillName[] cannabisSkills = new SkillName[] { SkillName.Herbalism, SkillName.Fletching };
        public override SkillName[] SkillsRequired { get{ return cannabisSkills; } }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloKonopia() : base( 0x0CC3 )
		{ 
			//Hue = 263;
			Name = "Krzak konopi"; // 1032612
			Stackable = true;
		}

		public ZrodloKonopia( Serial serial ) : base( serial ) 
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
	
	public class SurowiecKonopia : ResourceCrop
    {
        public override int DefaulReagentCount => 12;
        public override Type ReagentType => typeof(CannabisFiber);
		public override SkillName[] SkillsRequired { get{ return ZrodloKonopia.cannabisSkills; } }

		[Constructable]
		public SurowiecKonopia( int amount ) : base( amount, 0x0C5F )
		{
			//Hue = 0;
			Name = "Lodyga konopi"; // 1032615
			Stackable = true;
		}

		[Constructable]
		public SurowiecKonopia() : this( 1 )
		{
		}

		public SurowiecKonopia( Serial serial ) : base( serial )
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
