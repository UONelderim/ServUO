/* 
	Mail System - Version 1.0
	
	Newly Modified On 15/11/2016 
	
	By Veldian 
	Dragon's Legacy Uo Shard 
*/

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{
    public class PlayerLetter : Item
    {
        public string BodyText;
        public Mobile m_From;
        public Mobile m_To;
        public DateTime m_Time;
        public bool m_Read;
        public bool m_Replied;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Message
        {
            get { return BodyText; }
        }

        public PlayerLetter()
            : base(0xE35)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
            Movable = true;
            Hue = 1150;
            Name = "zalakowana wiadomosc";
        }

        public PlayerLetter(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == m_To || from == m_From)
            {
                from.SendGump(new LetterGump(from, BodyText, m_From, this));
                m_Read = true;
                this.Hue = 1102;
                this.Name = this.Name;
            }
            else
                from.SendMessage("Ten list jest zalakowany. Nie mozesz go otwoerzyc!");
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            string time = m_Time.ToString("hh:mm tt, dd/MM/yyyy"); ;
            list.Add(1060658, "{0}\t{1}", "Od", m_From.Name); // Display from
            list.Add(1060659, "{0}\t{1}", "Do", m_To.Name); // Display t
            list.Add(1060660, "{0}\t{1}", "Wyslany", time); // Display to
            list.Add(1060661, "{0}\t{1}", "Przeczytany", m_Read); // Display to
            list.Add(1060662, "{0}\t{1}", "Odpowiedziano", m_Replied); // Display to
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
            writer.Write(m_Read);
            writer.Write(m_Replied);
            writer.Write(BodyText);
            writer.Write(m_From);
            writer.Write(m_To);
            writer.Write(m_Time);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    {
                        m_Read = reader.ReadBool();
                        m_Replied = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        BodyText = reader.ReadString();
                        m_From = reader.ReadMobile();
                        m_To = reader.ReadMobile();
                        m_Time = reader.ReadDateTime();
                        break;
                    }
            }
        }
    }
}


