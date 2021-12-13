//Created By Milva

using System;
using Server;

namespace Server.Items
{	


    public class  SantasReindeer1 : Item
                               
	             {
		[Constructable]
		public SantasReindeer1  () : base( 0x3A67)
		{                
			
                              Weight = 3;
                             Name = "Renifer Pana";
                             ItemID = 14951;   
                                                
		}

        public SantasReindeer1(Serial serial)
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