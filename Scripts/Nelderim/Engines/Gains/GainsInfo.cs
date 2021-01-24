using Server;
using System;
using Nelderim;

namespace Nelderim.Gains
{
    class GainsInfo : NExtensionInfo
    {
        private DateTime m_LastPowerHour;
        private double m_StrGain;
        private double m_DexGain;
        private double m_IntGain;

        public DateTime LastPowerHour { get { return m_LastPowerHour; } set { m_LastPowerHour = value; } }
        public double StrGain { get { return m_StrGain; } set { m_StrGain = value; } }
        public double DexGain { get { return m_DexGain; } set { m_DexGain = value; } }
        public double IntGain { get { return m_IntGain; } set { m_IntGain = value; } }

        public override void Deserialize( GenericReader reader )
        {
            m_LastPowerHour = reader.ReadDateTime();
            m_StrGain = reader.ReadDouble();
            m_DexGain = reader.ReadDouble();
            m_IntGain = reader.ReadDouble();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( m_LastPowerHour );
            writer.Write( m_StrGain );
            writer.Write( m_DexGain );
            writer.Write( m_IntGain );
        }
    }
}
