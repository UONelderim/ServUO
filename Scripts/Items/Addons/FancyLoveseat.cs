using System.Collections.Generic;

namespace Server.Items
{
	public class FancyLoveseatDeed : BaseFlippableAddonDeed
	{
		private List<FlippableAddonEntry> _Entries = [
			new(1154137, () => new FancyLoveseatSouthAddon()),
			new(1154138, () => new FancyLoveseatEastAddon()),
			new(1156560, () => new FancyLoveseatNorthAddon()),
			new(1156561, () => new FancyLoveseatWestAddon())
		];

		public override List<FlippableAddonEntry> Entries => _Entries;
		public override int LabelNumber => 1098462;
		
		[Constructable]
		public FancyLoveseatDeed()
		{
		}
		
		public FancyLoveseatDeed(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
    [Furniture]
    public class FancyLoveseatEastAddon : BaseAddon
    {
        public override BaseAddonDeed Deed => new FancyLoveseatDeed();
        public override bool RetainDeedHue => true;

        [Constructable]
        public FancyLoveseatEastAddon()
        {
            AddComponent(new AddonComponent(0x4C88), 0, 0, 0);
            AddComponent(new AddonComponent(0x4C89), 0, 1, 0);
        }

        public FancyLoveseatEastAddon(Serial serial)
            : base(serial)
        {
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
    [Furniture]
    public class FancyLoveseatNorthAddon : BaseAddon
    {
	    public override BaseAddonDeed Deed => new FancyLoveseatDeed();
	    public override bool RetainDeedHue => true;

	    [Constructable]
	    public FancyLoveseatNorthAddon()
	    {
		    AddComponent(new AddonComponent(0x9C5A), 0, 0, 0);
		    AddComponent(new AddonComponent(0x9C59), 1, 0, 0);
	    }

	    public FancyLoveseatNorthAddon(Serial serial)
		    : base(serial)
	    {
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
    
    [Furniture]
    public class FancyLoveseatSouthAddon : BaseAddon
    {
	    public override BaseAddonDeed Deed => new FancyLoveseatDeed();
	    public override bool RetainDeedHue => true;

	    [Constructable]
	    public FancyLoveseatSouthAddon()
	    {
		    AddComponent(new AddonComponent(0x4C87), 0, 0, 0);
		    AddComponent(new AddonComponent(0x4C86), 1, 0, 0);
	    }

	    public FancyLoveseatSouthAddon(Serial serial)
		    : base(serial)
	    {
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
    
    [Furniture]
    public class FancyLoveseatWestAddon : BaseAddon
    {
	    public override BaseAddonDeed Deed => new FancyLoveseatDeed();
	    public override bool RetainDeedHue => true;

	    [Constructable]
	    public FancyLoveseatWestAddon()
	    {
		    AddComponent(new AddonComponent(0x9C58), 0, 0, 0);
		    AddComponent(new AddonComponent(0x9C57), 0, 1, 0);
	    }

	    public FancyLoveseatWestAddon(Serial serial)
		    : base(serial)
	    {
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
