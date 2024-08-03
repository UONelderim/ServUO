using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAttachmentScroll : Item
    {
        private string m_AttachType;
        private string m_Args;
        private bool m_Reusable;
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string AttachType
        {
            get { return m_AttachType; }
            set
            {
                m_AttachType = value;
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string Args
        {
            get { return m_Args; }
            set
            {
                m_Args = value;
            }
        }
        
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool Reusable
        {
            get { return m_Reusable; }
            set
            {
                m_Reusable = value;
            }
        }

        [Constructable]
        public XmlAttachmentScroll() : this("", "" )
        {
        }

        [Constructable]
        public XmlAttachmentScroll( string attachType, string args ) : base(0x14F0)
        {
            m_AttachType = attachType;
            m_Args = args;
            m_Reusable = false;
        }
        
        public XmlAttachmentScroll( Serial serial ) : base( serial )
        {
        }
        
        public override void OnDoubleClick( Mobile from )
        {
            if ( !this.IsChildOf( from.Backpack ) )
            {
                from.SendLocalizedMessage(1042001); // Musisz miec przedmiot w plecaku, zeby go uzyc.
                return;
            }
            
            Target t = new InternalTarget( this );

            from.SendMessage( "Wybierz cel zalacznika." ); 
            from.Target = t;
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 ); // version
            writer.Write( m_AttachType );
            writer.Write( m_Args );
            writer.Write( m_Reusable );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            m_AttachType = reader.ReadString();
            m_Args = reader.ReadString();
            m_Reusable = reader.ReadBool();
        }
        
        private class InternalTarget : Target
        {
            private XmlAttachmentScroll m_Scroll;

            public InternalTarget( XmlAttachmentScroll scroll) : base( 3, false, TargetFlags.None )
            {
                m_Scroll = scroll;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                
                Type attachtype = SpawnerType.GetType(m_Scroll.AttachType);
                string[] args = m_Scroll.Args.Split(' ');

                if (attachtype == null || !attachtype.IsSubclassOf(typeof(XmlAttachment)))
                {
                    from.SendMessage( "Invalid attachment type " + m_Scroll.AttachType );
                    return;
                }
                
                XmlAttachment o = (XmlAttachment)XmlSpawner.CreateObject(attachtype, args, false, true);

                if (o == null)
                {
                    from.SendMessage( String.Format("Unable to construct {0} with specified args", attachtype.Name ));
                    return;
                }

                if (XmlAttach.AttachTo(targeted, o))
                {
                    from.SendMessage("Sukces");
                    if (!m_Scroll.Reusable)
                        m_Scroll.Delete();
                }
                else
                    from.SendMessage("Niepowodzenie");
            }
        }
    }
}