
//////////////////////////////////////////////////////////////////////
// Custom DetectHidden by ViWinfii 
// Version 3.1415
// Date:  3-14-15
//////////////////////////////////////////////////////////////////////

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.SkillHandlers;

namespace Server.Gumps
{
    public class DetectHiddenMenu : Gump
    {

        public DetectHiddenMenu()
            : base(75, 75)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 2, 203, 100, 9200);
            AddLabel(25, 7, 0, @"Wykrywanie niewidzialnych");
            AddButton(8, 79, 2117, 2118, (int)Buttons.Detect, GumpButtonType.Reply, 0);
            AddButton(115, 79, 2117, 2118, (int)Buttons.Reveal, GumpButtonType.Reply, 0);
            AddButton(172, 7, 5052, 5053, (int)Buttons.Close, GumpButtonType.Reply, 0);
            AddLabel(30, 76, 0, @"Wyczucie");
            AddLabel(138, 76, 0, @"Wykrywanie");
            AddButton(35, 42, 9762, 9763, (int)Buttons.Description, GumpButtonType.Reply, 0);
            AddLabel(8, 40, 0, @"Opis");

        }

        public enum Buttons
        {
            Detect,
            Reveal,
            Close,
            Description,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Detect:
                    {
                        Server.SkillHandlers.NDetectHidden.Detection(from);
                        from.SendGump(new DetectHiddenMenu());
                        break;
                    }
                case (int)Buttons.Reveal:
                    {
                        Server.SkillHandlers.NDetectHidden.OnUseOriginal(from);
                        from.SendGump(new DetectHiddenMenu());
                        break;
                    }
                case (int)Buttons.Close:
                    {
                        from.CloseGump(typeof(DetectHiddenMenu));
                        break;
                    }
                case (int)Buttons.Description:
                    {
                        from.SendGump(new DescDetectHidden());
                        from.SendGump(new DetectHiddenMenu());
                        break;
                    }
                default:
                    break;

            }
        }
    }
}
