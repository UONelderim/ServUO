using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FullSpellbook : Spellbook
    {
        [Constructable]
        public FullSpellbook() : base( UInt64.MaxValue )
        {
        }
        
        public FullSpellbook( Serial serial ) : base( serial )
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            
            writer.Write( (int) 0 );
        }
        
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            
            int version = reader.ReadInt();
        }
    }

    public class FullBushidoBook : BookOfBushido
    {
        [Constructable]
        public FullBushidoBook()
            : base((ulong)0x3F)
        {
        }

        public FullBushidoBook(Serial serial)
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

    public class FullChivalryBook : BookOfChivalry
    {
        [Constructable]
        public FullChivalryBook()
            : base((ulong)0x3FF)
        {
        }

        public FullChivalryBook(Serial serial)
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

    public class FullNinjitsuBook : BookOfNinjitsu
    {
        [Constructable]
        public FullNinjitsuBook()
            : base((ulong)0xFF)
        {
        }

        public FullNinjitsuBook(Serial serial)
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