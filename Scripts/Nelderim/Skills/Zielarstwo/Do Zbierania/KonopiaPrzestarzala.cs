using System;

namespace Server.Items.Crops
{
	public class ZrodloKonopia : ResourceVein  // DEPRECATED (see: KrzakKonopia), to be removed from spawners
	{
        public override Type CropType => typeof(PlonKonopia);
		protected override int MaturePlantGraphics => 0x0CC3;

        public override SkillName[] SkillsRequired { get{ return KrzakKonopia.cannabisSkills; } }

		public override bool GivesSeed{ get{ return false; } }

		//[Constructable] 
		private ZrodloKonopia() : base( 0x0CC3 )
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
	
	public class SurowiecKonopia : ResourceCrop // DEPRECATED (see: PlonKonopia), to be removed/used/replaced from player's possesion
	{
        public override int DefaulReagentCount => 12;
        public override Type ReagentType => typeof(CannabisFiber);
		public override SkillName[] SkillsRequired { get{ return KrzakKonopia.cannabisSkills; } }

		//[Constructable]
		private SurowiecKonopia( int amount ) : base( amount, 0x0C5F )
		{
			//Hue = 0;
			Name = "Lodyga konopi"; // 1032615
			Stackable = true;
		}

		//[Constructable]
		private SurowiecKonopia() : this( 1 )
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

}