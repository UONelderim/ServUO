using Server;
using System;

namespace Nelderim
{
    public abstract class NExtensionInfo
    {
        private Serial m_Serial;

        public Serial Serial { get { return m_Serial; } set { m_Serial = value; } }

        public abstract void Serialize( GenericWriter writer );

        public abstract void Deserialize( GenericReader reader );
    }
}
