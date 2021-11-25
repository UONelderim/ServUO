using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientGreatLightScroll : CSpellScroll
    {
        [Constructable]
        public AncientGreatLightScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientGreatLightScroll(int amount)
            : base(typeof(AncientGreatLightSpell), 0x1F35, amount)
        {
            Name = "Great Light Scroll";
            Hue = 1355;
        }

        public AncientGreatLightScroll(Serial serial)
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
