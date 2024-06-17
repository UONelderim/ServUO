using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class QuestCorpse : Item
	{

		[Constructable]
        public QuestCorpse() : base( 0x3D68 )
		{
            Name = "Zwloki";
			Movable = false;
		}

        public QuestCorpse( Serial serial ) : base( serial )
		{
		}

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}