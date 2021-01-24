using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nelderim;

namespace Nelderim
{
    class LanguagesInfo : NExtensionInfo
    {
        private KnownLanguages m_LanguagesKnown;
        private Language m_LanguageSpeaking;

        public LanguagesInfo()
        {
            m_LanguagesKnown = new KnownLanguages();
        }

        public KnownLanguages LanguagesKnown { get { return m_LanguagesKnown; } set { m_LanguagesKnown = value; } }
        public Language LanguageSpeaking { get { return m_LanguageSpeaking; } set { m_LanguageSpeaking = value; } }

        public override void Deserialize( GenericReader reader )
        {
            m_LanguagesKnown = new KnownLanguages((Language)reader.ReadInt());
            m_LanguageSpeaking = (Language)reader.ReadInt();

            if ( m_LanguagesKnown == null ) m_LanguagesKnown = new KnownLanguages();
        }

        public override void Serialize( GenericWriter writer )
        {
            writer.Write( (int)m_LanguagesKnown.Value );
            writer.Write( (int)m_LanguageSpeaking );
        }
    }
}
