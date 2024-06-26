namespace Server.Items
{
    public class MysticBook : Spellbook
    {
        [Constructable]
        public MysticBook()
            : this((ulong)0)
        {
        }

        [Constructable]
        public MysticBook(ulong content)
            : base(content, 0x2D9D)
        {
            Name = "Ksiega Mistycyzmu";
        }

        public MysticBook(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType => SpellbookType.Mystic;
        public override int BookOffset => 677;
        public override int BookCount => 16;

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