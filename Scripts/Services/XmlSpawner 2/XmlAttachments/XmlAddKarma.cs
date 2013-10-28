using System;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAddKarma : XmlAttachment
    {
        private int m_DataValue;// default data

        // a serial constructor is REQUIRED
        public XmlAddKarma(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlAddKarma(int value)
        {
            this.Value = value;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Value
        {
            get
            {
                return this.m_DataValue;
            }
            set
            {
                this.m_DataValue = value;
            }
        }
        // These are the various ways in which the message attachment can be constructed.  
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments
        public override bool HandlesOnKilled
        {
            get
            {
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0
            writer.Write(this.m_DataValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
            this.m_DataValue = reader.ReadInt();
        }

        public override void OnAttach()
        {
            base.OnAttach();
		    
            // apply the mod
            if (this.AttachedTo is PlayerMobile)
            {
                // for players just add it immediately
                ((Mobile)this.AttachedTo).Karma += this.Value;

                ((Mobile)this.AttachedTo).SendMessage("Receive {0}", this.OnIdentify((Mobile)this.AttachedTo));

                // and then remove the attachment
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Delete));
                //Delete();
            }
            else if (this.AttachedTo is Item)
            {
                // dont allow item attachments
                this.Delete();
            }
        }

        public override void OnKilled(Mobile killed, Mobile killer)
        {
            base.OnKilled(killed, killer);

            if (killer == null)
                return;
		    
            killer.Karma += this.Value;

            killer.SendMessage("Receive {0}", this.OnIdentify(killer));
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("{0} Karma", this.Value);
        }
    }
}