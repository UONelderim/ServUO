using System;
using Server.Mail;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class Parcel : MailItem
    {
        public int DefaultGumpID => 0x102;
        public int DefaultDropSound => 0x48;

        [Constructable]
        public Parcel() : base(0x232A)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public Parcel(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == Recipient)
                base.OnDoubleClick(from);
            else
                from.SendMessage("Ta paczka nie jest dla Ciebie. Precz te lapki!");
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060658, "{0}	{1}", "Od", Sender.Name);
            list.Add(1060659, "{0}	{1}", "Do", Recipient.Name);
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
