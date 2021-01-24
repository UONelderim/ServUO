using Server;
using System;
using Nelderim;
using Server.Items;

namespace Nelderim.ExtraCraftResource
{
    class ExtraCraftResourceInfo : NExtensionInfo
    {
        private CraftResource m_Resource2;

        public CraftResource Resource2 { get { return m_Resource2; } set { m_Resource2 = value; } }

        public override void Deserialize( GenericReader reader )
        {
            m_Resource2 = (CraftResource)reader.ReadInt();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( (int)m_Resource2 );
        }
    }
}
