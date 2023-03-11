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
	//[FlipableAttribute(0x4142, 0x4143)] // 0x4141 & 0x4144 have the "you got mail" flag up.
    public class PostBox : Item
    {
        [Constructable]
        public PostBox()
            : base(0x4142)  //na ServUO zmienić na 0x4142
        {
            Weight = 1.0;
            Movable = false;
            Name = "Skrzynka pocztowa";
            Hue = 0;
        }

        public PostBox(Serial serial)
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

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            PlayerLetter mail = dropped as PlayerLetter;
            Parcel box = dropped as Parcel;
            if (mail != null)
            {
                if (mail.m_From != null && mail.m_To != null)
                {
                    from.SendMessage("Wyslales list!");
                    if (mail.m_From != from)
                        mail.m_From.SendMessage("Twoj list zostal wyslany przez " + from.Name + ".");
                    mail.m_To.AddToBackpack(dropped);
                    mail.m_To.SendMessage("Otrzymales list od " + mail.m_From.Name + "!");
                    
                    return true;
                }
                from.SendMessage("Ten list zostal zaadresowany!");

                return false;
            }
            else if (box != null)
            {
                if (box.From != null && box.To != null)
                {
                    from.SendMessage("Wyslales paczke!");
                    if (box.From != from)
                        box.From.SendMessage("Twoja paczka zostala wyslana przez " + from.Name + ".");
                    box.To.AddToBackpack(dropped);
                    box.To.SendMessage("Otrzymales paczke od " + box.From.Name + "!");

                    return true;
                }
                from.SendMessage("Ta paczka nie zostala zaadresowana!");
                return false;
            }
            else
            {
                from.SendMessage("To nie jest smietnik!");
                return false;
            }
        }
    }
}
