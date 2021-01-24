using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
    public class UndeadCauseFearScroll : CSpellScroll
    {
        [Constructable]
        public UndeadCauseFearScroll()
            : this(1)
        {
        }

        [Constructable]
        public UndeadCauseFearScroll(int amount)
            : base(typeof(UndeadCauseFearSpell), 0xE39, amount)
        {
            Name = "Zwój Strachu";
            Hue = 38;
        }

        public UndeadCauseFearScroll(Serial serial)
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
