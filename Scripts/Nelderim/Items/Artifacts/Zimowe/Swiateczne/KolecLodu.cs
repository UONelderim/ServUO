using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class KolecLodu : RepeatingCrossbow
    {
        private Mobile m_Owner;
        private Timer m_ProximityTimer;

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public KolecLodu()
        {
            ItemID = 0x26CD;
            Hue = 1152;
            Name = "Kolec Lodu";
            Slayer = SlayerName.WaterDissipation;
            AosElementDamages[AosElementAttribute.Cold] = 100;
            AosElementDamages[AosElementAttribute.Physical] = 0;
            Attributes.WeaponSpeed = 25;
            Attributes.WeaponDamage = 50;
            Attributes.EnhancePotions = 15;
            WeaponAttributes.HitLeechMana = 23;
            Label1 = "[Topi sie i rani Cie, gdy jest w poblizu ognistych stworzen]";
        }

        public KolecLodu(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(IEntity parent)
        {
            base.OnAdded(parent);
            
            if (parent is Mobile mobile && mobile.FindItemOnLayer(Layer.TwoHanded) == this)
            {
                m_Owner = mobile;
                StartProximityCheck();
            }
        }

        public override void OnRemoved(IEntity parent)
        {
            base.OnRemoved(parent);
            
            StopProximityCheck();
            m_Owner = null;
        }

        private void StartProximityCheck()
        {
            StopProximityCheck();
            
            m_ProximityTimer = new ProximityCheckTimer(this);
            m_ProximityTimer.Start();
        }

        private void StopProximityCheck()
        {
            m_ProximityTimer?.Stop();
            m_ProximityTimer = null;
        }

        private bool IsFireCreature(Mobile mob)
        {
            return mob is FireElemental || 
                   mob is FireGargoyle || 
                   mob is FireSteed || 
                   mob is FireBeetle;
        }

        private void CheckProximityAndDamage()
        {
            if (m_Owner == null)
                return;
            
            bool fireCreatuteNearby = false;
            foreach (Mobile mob in m_Owner.GetMobilesInRange(5))
            {
                if (mob != m_Owner && IsFireCreature(mob))
                {
                    fireCreatuteNearby = true;
                    break;
                }
            }
            
            if (fireCreatuteNearby)
            {
                m_Owner.Hits -= 5;
                m_Owner.SendMessage("*bron zaczyna sie topic, a magiczny lod zaczyna Cie ranic*!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // Increased version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
        }
        
        private class ProximityCheckTimer : Timer
        {
            private KolecLodu m_Crossbow;

            public ProximityCheckTimer(KolecLodu crossbow) : base(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2))
            {
                m_Crossbow = crossbow;
            }

            protected override void OnTick()
            {
                m_Crossbow.CheckProximityAndDamage();
            }
        }
    }
}
