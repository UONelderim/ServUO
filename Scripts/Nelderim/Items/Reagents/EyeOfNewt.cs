using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EyeOfNewt : BaseReagent, ICommodity
	{
        TextDefinition ICommodity.Description
        {
            get
            {
                return new TextDefinition( LabelNumber, String.Format( "{0} oko gazera", Amount ) );
            }
        }

        bool ICommodity.IsDeedable { get { return false; } }

        [Constructable]
        public EyeOfNewt() : this( 1 )
		{
			Name = "oko gazera";
		}

		[Constructable]
        public EyeOfNewt( int amount ) : base( 3975, amount )
		{
		}

        public EyeOfNewt( Serial serial ) : base( serial )
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
