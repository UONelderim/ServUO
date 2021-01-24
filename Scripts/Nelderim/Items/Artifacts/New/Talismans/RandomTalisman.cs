using System;
using Server;

namespace Server.Items
{
	public class TalismanLevel2 : NBaseTalisman
	{
        public override int LabelNumber { get { return 1024246; } } // Talizman

		[Constructable]
		public TalismanLevel2() : base( 0x2F5A, 2 )
		{
			ItemID = Utility.RandomList(12120,12121);
			Weight = 1.0;
		}

        public TalismanLevel2(Serial serial)
            : base(serial)
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

    public class TalismanLevel3 : NBaseTalisman
    {
        public override int LabelNumber { get { return 1024246; } } // Talizman

        [Constructable]
        public TalismanLevel3()
            : base(0x2F5A, 3)
        {
            ItemID = Utility.RandomList(12122, 12123);
            Weight = 1.0;
        }

        public TalismanLevel3(Serial serial)
            : base(serial)
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
}
