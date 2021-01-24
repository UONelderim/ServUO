using Nelderim.Towns;
using Server.Network;

namespace Server.Gumps
{
    public class TownRelationsTableGump : Gump
    {
        private const int LabelColor = 0x484;
        private const int SelectedColor = 0x7A1;

        public void AddButton(int x, int y, int buttonID, string title)
        {
            AddButton(x, y, 4006, 4007, buttonID, GumpButtonType.Reply, 0);
            AddLabel(x + 40, y, SelectedColor, title);
        }

        private int GetLabelHue(int relations)
        {
            if (relations == 100)
            {
                return 163;
            }
            else if (relations >= 70 && relations < 100)
            {
                return 169;
            }
            else if (relations >= 40 && relations < 70)
            {
                return 188;
            }
            else if (relations >= 10 && relations < 40)
            {
                return 193;
            }
            else if (relations == 0)
            {
                return LabelColor;
            }
            else if (relations <= -10 && relations > -40)
            {
                return 55;
            }
            else if (relations <= -30 && relations > -70)
            {
                return 153;
            }
            else if (relations <= -60 && relations > -100)
            {
                return 148;
            }
            else if (relations == -100)
            {
                return 138;
            }
            return LabelColor;
        }

        public TownRelationsTableGump(Towns town, Mobile from)
            : base(50, 40)
        {
            AddPage(0);

            AddBackground(0, 0, 850, 400, 5054);

            AddHtmlLocalized(10, 10, 350, 250, 1063901, LabelColor, false, false); // Wywiadowcze informacje o miastach

            int yLab = 90;
            int xLab = 150;
            int tmpRel = 0;

            AddLabel(10, 70, SelectedColor, string.Format("Relacje miasta"));
            AddLabel(70, 50, SelectedColor, string.Format("z miastem"));

            foreach (Towns tr in TownDatabase.GetTownsNames())
            {
                if (tr != Towns.None)
                {
                    AddLabel(xLab, 50, SelectedColor, string.Format("{0}", tr.ToString()));
                    xLab += 90;
                }
            }

            foreach (Towns t in TownDatabase.GetTownsNames())
            {
                if (t != Towns.None)
                {
                    AddLabel(10, yLab, SelectedColor, string.Format("{0}", t.ToString()));
                    xLab = 150;
                    foreach (Towns tr in TownDatabase.GetTownsNames())
                    {
                        if (tr != Towns.None)
                        {
                            if (tr != t)
                            {
                                tmpRel = TownDatabase.GetRelationOfATownWithTown(t, tr);
                                AddLabel(xLab + 25, yLab, GetLabelHue(tmpRel), string.Format("{0}", tmpRel.ToString()));
                            }
                            else
                            {
                                AddLabel(xLab + 25, yLab, GetLabelHue(0), string.Format("X"));
                            }
                            xLab += 90;
                        }
                    }
                    yLab += 20;
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
        }
    }
}
