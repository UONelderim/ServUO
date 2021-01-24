using Server;

namespace Nelderim
{
    class ItemHitPointsInfo : NExtensionInfo
    {

        private int m_MaxHitPoints;
        private int m_HitPoints;

        public int MaxHitPoints { get { return m_MaxHitPoints; } set { m_MaxHitPoints = value; } }
        public int HitPoints { get { return m_HitPoints; } set { m_HitPoints = value; } }

        public override void Deserialize( GenericReader reader )
        {
            m_MaxHitPoints = reader.ReadInt();
            m_HitPoints = reader.ReadInt();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( m_MaxHitPoints );
            writer.Write( m_HitPoints );
        }
    }
}
