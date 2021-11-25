using System;
using Server;

namespace Server.Items
{
	public class BlogoslawienstwoBogow : GoldBracelet
	{
        public override int LabelNumber { get { return 1065806; } } // Blogoslawienstwo Bogow
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public BlogoslawienstwoBogow()
		{
			Hue = 0x4F2;
			Attributes.BonusStr = 8;
			Attributes.BonusDex = 8;
			Attributes.BonusInt = 8;
			Resistances.Fire = 10;
			Resistances.Energy = 5;
		}

		public BlogoslawienstwoBogow( Serial serial ) : base( serial )
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