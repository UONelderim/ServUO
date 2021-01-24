using System;

namespace Server.ACC.CSS.Systems.Undead
{
    public class UndeadSeanceScroll : CSpellScroll
    {
        [Constructable]
        public UndeadSeanceScroll()
            : this(1)
        {
        }

        [Constructable]
        public UndeadSeanceScroll(int amount)
            : base(typeof(UndeadSeanceSpell), 0xE39, amount)
        {
            Name = "Seans";
            Hue = 38;
        }

        public UndeadSeanceScroll(Serial serial)
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
