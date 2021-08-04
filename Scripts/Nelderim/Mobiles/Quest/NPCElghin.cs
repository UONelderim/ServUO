using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class NPCElghin : BaseCreature
    {
        [Constructable]
        public NPCElghin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            InitStats(100, 125, 25);

            Body = 400;
            CantWalk = true;
            Hue = 1641;
            Blessed = true;
            Name = "NPCElghin";

            Skills[SkillName.Anatomy].Base = 120.0;
            Skills[SkillName.Tactics].Base = 120.0;
            Skills[SkillName.Magery].Base = 120.0;
            Skills[SkillName.MagicResist].Base = 120.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

        }

        public NPCElghin(Serial serial)
            : base(serial)
        {
        }


        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            Mobile from = e.Mobile;

            if (from.InRange(this, 2))
            {
                if (e.Speech.ToLower().IndexOf("zadan") >= 0 || e.Speech.ToLower().IndexOf("witaj") >= 0)
                {
                    string message1 = "ELO KURWINOXY! ZAJEBIECIE W NOSA!";
                    this.Say(message1);
                }
            }
        }


        public int sercesmoka = 0;
        public int serceczerwonegosmoka = 1;
        public int sercewyrma = 2;

        public override bool OnDragDrop(Mobile from, Item dropped)
        {

            if (dropped is DragonsHeart)
            {
                dropped.Delete();
                sercesmoka++;

                if (sercesmoka > 0)
                {
                    Point3D loc = new Point3D(5914, 3227, 0);
                    Map map = Map.Felucca;
                    WrotaElghin portal = new WrotaElghin();
                    portal.MoveToWorld(loc, map);

                    Say(true, " WBIJAC MORDY! ");

                    sercesmoka = 0;
                }
            }

            else if (dropped is RedDragonsHeart)
            {
                dropped.Delete();
                sercewyrma++;

                if (sercewyrma > 1)
                {
                    Point3D loc = new Point3D(5914, 3227, 0);
                    Map map = Map.Felucca;
                    WrotaElghin portal = new WrotaElghin();
                    portal.MoveToWorld(loc, map);

                    Say(true, " WBIJAC MORDY! ");

                    sercewyrma = 1;
                }
            }

            else if (dropped is WyrmsHeart)
            {
                dropped.Delete();
                serceczerwonegosmoka++;

                if (serceczerwonegosmoka > 2)
                {
                    Point3D loc = new Point3D(5914, 3227, 0);
                    Map map = Map.Felucca;
                    WrotaElghin portal = new WrotaElghin();
                    portal.MoveToWorld(loc, map);

                    Say(true, " WBIJAC MORDY! ");

                    serceczerwonegosmoka = 2;
                }
            }

            return base.OnDragDrop(from, dropped);
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