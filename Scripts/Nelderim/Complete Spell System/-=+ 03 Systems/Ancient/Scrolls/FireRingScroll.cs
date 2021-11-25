using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientFireRingScroll : CSpellScroll
    {
        [Constructable]
        public AncientFireRingScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientFireRingScroll(int amount)
            : base(typeof(AncientFireRingSpell), 0x1F56, amount)
        {
            Name = "Zwój Pierścienia Ognia";
            Hue = 1355;
        }

        public AncientFireRingScroll(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
