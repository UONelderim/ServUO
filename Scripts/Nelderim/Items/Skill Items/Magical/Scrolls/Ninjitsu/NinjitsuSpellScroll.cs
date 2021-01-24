using System;

namespace Server.Items
{
    public abstract class NinjitsuSpellScroll : SpellScroll
	{
        public NinjitsuSpellScroll(int spellID, int itemID, int amount) : base(spellID, itemID, amount)
		{
            Hue = 1000;
		}

        public NinjitsuSpellScroll(Serial serial) : base(serial)
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