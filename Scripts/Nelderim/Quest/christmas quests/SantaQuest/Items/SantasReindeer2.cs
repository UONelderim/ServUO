//Created By Milva

using System;
using Server;

namespace Server.Items
{	


    public class  SantasReindeer2 : Item
                               
	             {
		[Constructable]
		public SantasReindeer2  () : base( 0x3A5F)
		{                
			
                              Weight = 3;
                             Name = "Renifer Pana";
                             ItemID = 14943;   
                                                
		}

        public SantasReindeer2(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}