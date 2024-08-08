using System;

namespace Server.Items.Crops
{
	public class ZrodloJedwab : ResourceVein  // DEPRECATED (replaced by: KrzakJedwab), to be removed from spawners
    {
        public override Type CropType => typeof(PlonJedwab);
		protected override int MaturePlantGraphics => Utility.Random(3153, 4);
        public override SkillName[] SkillsRequired { get{ return KrzakJedwab.silkSkills; } }

		public override bool GivesSeed{ get{ return false; } }

		//[Constructable]
		private ZrodloJedwab() : base( Utility.Random(3153, 4) )
		{ 
			Hue = 1946;
			Name = "Roslina z jedwabnikiem"; // 1032611
			Stackable = true;
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
	
	public class SurowiecJedwab : ResourceCrop  // DEPRECATED (see: PlonJedwab), to be removed/used/replaced from player's possesion
	{
		public override int DefaulReagentCount => 12;
		public override Type ReagentType => typeof(SilkFiber);
		public override SkillName[] SkillsRequired { get { return KrzakJedwab.silkSkills; } }

		//[Constructable]
		private SurowiecJedwab( int amount ) : base( amount, 0x0DF9 ) //0x0DEF
		{
			Hue = 2886;
			Name = "Kokon jedwabiu"; // 1032614
			Stackable = true;
		}

		//[Constructable]
		private SurowiecJedwab() : this( 1 )
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
}