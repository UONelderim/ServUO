using System;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLifeDrain : XmlAttachment
    {
        private int m_Drain = 0;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);// 5 seconds default time between activations
        private DateTime m_EndTime;
        private int proximityrange = 5;// default movement activation from 5 tiles away

        // a serial constructor is REQUIRED
        public XmlLifeDrain(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlLifeDrain(int drain)
        {
            this.m_Drain = drain;
        }

        [Attachable]
        public XmlLifeDrain(int drain, double refractory)
        {
            this.m_Drain = drain;
            this.Refractory = TimeSpan.FromSeconds(refractory);
        }

        [Attachable]
        public XmlLifeDrain(int drain, double refractory, double expiresin)
        {
            this.m_Drain = drain;
            this.Expiration = TimeSpan.FromMinutes(expiresin);
            this.Refractory = TimeSpan.FromSeconds(refractory);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Drain
        {
            get
            {
                return this.m_Drain;
            }
            set
            {
                this.m_Drain = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Range
        {
            get
            {
                return this.proximityrange;
            }
            set
            {
                this.proximityrange = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory
        {
            get
            {
                return this.m_Refractory;
            }
            set
            {
                this.m_Refractory = value;
            }
        }
        // These are the various ways in which the message attachment can be constructed.
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments
        public override bool HandlesOnMovement
        {
            get
            {
                return true;
            }
        }
        // note that this method will be called when attached to either a mobile or a weapon
        // when attached to a weapon, only that weapon will do additional damage
        // when attached to a mobile, any weapon the mobile wields will do additional damage
        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            // if it is still refractory then return
            if (DateTime.UtcNow < this.m_EndTime)
                return;

            int drain = 0;

            if (this.m_Drain > 0)
                drain = Utility.Random(this.m_Drain);

            if (defender != null && attacker != null && drain > 0)
            {
                defender.Hits -= drain;
                if (defender.Hits < 0)
                    defender.Hits = 0;
                attacker.Hits += drain;
                if (attacker.Hits < 0)
                    attacker.Hits = 0;

                this.DrainEffect(defender);

                this.m_EndTime = DateTime.UtcNow + this.Refractory;
            }
        }

        public void DrainEffect(Mobile m)
        {
            if (m == null)
                return;

            m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
            m.PlaySound(0x231);

            m.SendMessage("You feel the life drain out of you!");
        }

        public override void OnMovement(MovementEventArgs e)
        {
            base.OnMovement(e);
		    
            if (e.Mobile == null || e.Mobile.AccessLevel > AccessLevel.Player)
                return;

            if (this.AttachedTo is Item && (((Item)this.AttachedTo).Parent == null) && Utility.InRange(e.Mobile.Location, ((Item)this.AttachedTo).Location, this.proximityrange))
            {
                this.OnTrigger(null, e.Mobile);
            }
            else
                return;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
            // version 1
            writer.Write(this.proximityrange);
            // version 0
            writer.Write(this.m_Drain);
            writer.Write(this.m_Refractory);
            writer.Write(this.m_EndTime - DateTime.UtcNow);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch(version)
            {
                case 1:
                    // version 1
                    this.Range = reader.ReadInt();
                    goto case 0;
                case 0:
                    // version 0
                    this.m_Drain = reader.ReadInt();
                    this.Refractory = reader.ReadTimeSpan();
                    TimeSpan remaining = reader.ReadTimeSpan();
                    this.m_EndTime = DateTime.UtcNow + remaining;
                    break;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            string msg = null;

            if (this.Expiration > TimeSpan.Zero)
            {
                msg = String.Format("Life drain {0} expires in {1} mins", this.m_Drain, this.Expiration.TotalMinutes);
            }
            else
            {
                msg = String.Format("Life drain {0}", this.m_Drain);
            }
            
            if (this.Refractory > TimeSpan.Zero)
            {
                return String.Format("{0} : {1} secs between uses", msg, this.Refractory.TotalSeconds);
            }
            else
                return msg;
        }

        public override void OnAttach()
        {
            base.OnAttach();

            // announce it to the mob
            if (this.AttachedTo is Mobile)
            {
                if (this.m_Drain > 0)
                    ((Mobile)this.AttachedTo).SendMessage("You have been granted the power of Life Drain!");
                else
                    ((Mobile)this.AttachedTo).SendMessage("You have been cursed with Life Drain!");
            }
        }

        public override void OnTrigger(object activator, Mobile m)
        {
            if (m == null)
                return;

            // if it is still refractory then return
            if (DateTime.UtcNow < this.m_EndTime)
                return;

            int drain = 0;

            if (this.m_Drain > 0)
                drain = Utility.Random(this.m_Drain);

            if (drain > 0)
            {
                m.Hits -= drain;
                if (m.Hits < 0)
                    m.Hits = 0;

                this.DrainEffect(m);
            }

            this.m_EndTime = DateTime.UtcNow + this.Refractory;
        }
    }
}