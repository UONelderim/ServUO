using Server;
using System;

namespace Nelderim
{
    public class LabelsInfo : NExtensionInfo
    {
        private string[] m_Labels;
        private string m_ModifiedBy;
        private DateTime m_ModifiedDate;

        public LabelsInfo()
        {
            m_Labels = new string[5];
        }

        public string[] Labels { get { return m_Labels; } set { m_Labels = value; } }
        public string ModifiedBy { get { return m_ModifiedBy; } set { m_ModifiedBy = value; } }
        public DateTime ModifiedDate { get { return m_ModifiedDate; } set { m_ModifiedDate = value; } }

        public override void Deserialize( GenericReader reader )
        {
            int labels_count = reader.ReadInt();
            m_Labels = new string[labels_count];
            for ( int j = 0; j < labels_count; j++ )
                m_Labels[j] = reader.ReadString();

            m_ModifiedBy = reader.ReadString();
            m_ModifiedDate = reader.ReadDateTime();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( m_Labels.Length );
            foreach ( string label in m_Labels )
                writer.Write( label );

            writer.Write( m_ModifiedBy );
            writer.Write( m_ModifiedDate );
        }
    }
}
