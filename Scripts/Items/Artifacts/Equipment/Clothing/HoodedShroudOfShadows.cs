namespace Server.Items
{
    [Flipable(0x2684, 0x2683)]
    public class HoodedShroudOfShadows : BaseOuterTorso
    {
        [Constructable]
        public HoodedShroudOfShadows()
            : this(0x455)
        {
        }

        [Constructable]
        public HoodedShroudOfShadows(int hue)
            : base(0x2684, hue)
        {
            LootType = LootType.Blessed;
            Weight = 3.0;
        }
        public override void OnDoubleClick( Mobile from )
        {
	        if (ItemID == 0x2684)
	        {
		        ItemID = 7939;
		        //OnRemoved(from);
	        }	
	        else
	        {
                    
		        ItemID = 0x2684;
	        }
	        base.OnDoubleClick(from);
            
        }

        public HoodedShroudOfShadows(Serial serial)
            : base(serial)
        {
        }

        public override bool Dye(Mobile from, DyeTub sender)
        {
            from.SendLocalizedMessage(sender.FailMessage);
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
