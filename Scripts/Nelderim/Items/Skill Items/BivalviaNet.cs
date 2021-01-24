using System;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Engines.Harvest;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    public class BivalviaNet : BaseHarvestTool
    {
        public override HarvestSystem HarvestSystem{ get{ return Bivalvia.System; } }

		public override int LabelNumber{ get{ return 1031011; } }	// Siec do polowu malz

        [Constructable]
        public BivalviaNet() : base( 0x0DD2 )
        {
			UsesRemaining = 50;
        }

        public BivalviaNet( int uses ) : base( uses, 0x0DD2 )
        {
            //Layer = Layer.OneHanded;
			Name = "Siec do polowu malz";
            Weight = 1.0;
        }

        public BivalviaNet( Serial serial ) : base( serial )
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