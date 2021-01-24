using System;
using System.Xml;
using Server;

namespace Server.Regions
{
    public class DungeonsRegion : BaseRegion
    {
        public DungeonsRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = 28;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        /* // For debuging purposes:
        public override void OnEnter(Mobile m)
        {
            if (this.Name != String.Empty)
                m.SendMessage("Wkraczasz do dungeonu.");

            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            if (this.Name != String.Empty)
                m.SendMessage("Wychodzisz z dungeonu.");

            base.OnExit(m);
        }
        */
    }
}