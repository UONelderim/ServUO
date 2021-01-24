using System;

namespace Server.Items
{
	public class RawBeeswax : Item
	{
		[Constructable]
		public RawBeeswax() : this( 1 )
		{
		}

		[Constructable]
		public RawBeeswax( int amount ) : base( 0x1422 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
            Hue = 1214;
			Name = "Nieprzetworzony wosk pszczeli";
		}

		public RawBeeswax( Serial serial ) : base( serial )
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

    public class PureRawBeeswax : Item
    {
        [Constructable]
        public PureRawBeeswax()
            : this(1)
        {
        }

        [Constructable]
        public PureRawBeeswax(int amount)
            : base(0x1422)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
            Hue = 1126;
            Name = "Czysty wosk pszczeli";
        }

        public PureRawBeeswax(Serial serial)
            : base(serial)
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