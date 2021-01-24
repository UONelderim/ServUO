using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class VolcanicAsh : BaseReagent, ICommodity
	{
        TextDefinition ICommodity.Description
        {
            get
            {
                return new TextDefinition( LabelNumber, String.Format( "{0} pyl wulkaniczny", Amount ) );
            }
        }

        bool ICommodity.IsDeedable { get { return false; } }

        [Constructable]
        public VolcanicAsh() : this( 1 )
		{
		}

		[Constructable]
        public VolcanicAsh( int amount ): base( 3983, amount )
		{
            Name = "Pyl wulkaniczny";
            Hue = 2105;
		}

		public VolcanicAsh( Serial serial ) : base( serial )
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
