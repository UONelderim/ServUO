using Server;

namespace Nelderim.Towns
{
    class TownsVendorInfo : NExtensionInfo
    {
        private Towns m_TownAssigned;
        private TownBuildingName m_TownBuildingAssigned;
        private bool m_TradesWithCriminals;

        public Towns TownAssigned { get { return m_TownAssigned; } set { m_TownAssigned = value; } }
        public TownBuildingName TownBuildingAssigned { get { return m_TownBuildingAssigned; } set { m_TownBuildingAssigned = value; } }
        public bool TradesWithCriminals { get { return m_TradesWithCriminals; } set { m_TradesWithCriminals = value; } }

        public override void Deserialize( GenericReader reader )
        {
            m_TownAssigned = (Towns)reader.ReadInt();
            m_TownBuildingAssigned = (TownBuildingName)reader.ReadInt();
            m_TradesWithCriminals = reader.ReadBool();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( (int)m_TownAssigned );
            writer.Write( (int)m_TownBuildingAssigned );
            writer.Write( m_TradesWithCriminals );
        }
    }
}
