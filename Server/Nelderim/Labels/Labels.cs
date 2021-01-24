using Server;
using System.Collections.Generic;

namespace Nelderim
{
    public class Labels : NExtension<LabelsInfo>
    {
        public static void Cleanup()
        {
            List<Serial> toRemove = new List<Serial>();
            foreach ( Serial serial in m_ExtensionInfo.Keys ) {	
                if ( World.FindEntity( serial ) == null )
                    toRemove.Add( serial );
            }
            foreach(Serial serial in toRemove )
            {
                m_ExtensionInfo.Remove( serial );
            }
        }
    }
}
