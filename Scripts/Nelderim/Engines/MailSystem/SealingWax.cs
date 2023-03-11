/* 
	Mail System - Version 1.0
	
	Newly Modified On 15/11/2016 
	
	By Veldian 
	Dragon's Legacy Uo Shard 
*/


using System;

namespace Server.Items
{
    public class SealingWax : Item
    {
        [Constructable]
        public SealingWax()
            : this(1)
        {
        }

        [Constructable]
        public SealingWax(int amount)
            : base(0x1422)
        {
            Name = "wosk do lakowania";
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public SealingWax(Serial serial)
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
