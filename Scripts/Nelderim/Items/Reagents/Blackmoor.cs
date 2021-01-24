using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Blackmoor : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
                return new TextDefinition( LabelNumber, String.Format( "{0} Czarny wrzos", Amount ) );
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
        public Blackmoor() : this( 1 )
		{
		}

		[Constructable]
        public Blackmoor( int amount ) : base( 3961, amount )
		{
            Name = "Czarny wrzos";
		}

        public Blackmoor( Serial serial ) : base( serial )
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
