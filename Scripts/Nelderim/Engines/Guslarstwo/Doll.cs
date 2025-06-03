using System;
using System.Linq;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    [Flipable(0x13CA, 0x13D1)]
    public class Doll : Item
    {
        private Mobile m_CursedPerson;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile CursedPerson
        {
            get => m_CursedPerson;
            set { m_CursedPerson = value; InvalidateProperties(); }
        }

        [Constructable]
        public Doll() : base(0x13CA)
        {
            Name      = "laleczka";
            Hue       = 1096;
            Weight    = 3.0;
            Stackable = false;
        }

        public Doll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // wersja
            writer.Write(m_CursedPerson);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CursedPerson = reader.ReadMobile();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (m_CursedPerson != null)
                list.Add($"Ofiara: {m_CursedPerson.Name}");
        }

        /// <summary>
        /// Przy podwójnym kliknięciu informuje, że laleczka służy jako baza do voodoo.
        /// Obsługa bindowania odbywa się w VoodooPin.BindTarget.
        /// </summary>
        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable) return;
            from.SendMessage("Użyj szpilki lub mikstury, aby powiązać tę laleczkę.");
        }
    }
}
