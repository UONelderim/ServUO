﻿namespace Server.Items
{
    public class EnchantedBladeAddon : BaseAddon
    {

        public override BaseAddonDeed Deed => new EnchantedBladeDeed();

        [Constructable]
        public EnchantedBladeAddon()
        {
            AddComponent(new LocalizedAddonComponent(14240, 1034240), 0, 0, 0);
        }

        public EnchantedBladeAddon(Serial serial)
            : base(serial)
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

    public class EnchantedBladeDeed : BaseAddonDeed
    {
        public override BaseAddon Addon => new EnchantedBladeAddon();

        [Constructable]
        public EnchantedBladeDeed()
        {
            Name = "zwoj na zaklete ostrze";
        }

        public EnchantedBladeDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }

    }
}
