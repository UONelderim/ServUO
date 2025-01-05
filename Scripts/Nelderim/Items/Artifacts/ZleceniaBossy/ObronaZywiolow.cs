using System;
using Server.ACC.CSS.Systems;
using Server.ACC.CSS.Systems.Cleric;


namespace Server.Items
{
    public class ObronaZywiolow : ChaosShield
    {
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        private DateTime m_ShieldEquipTime; // Track the time when the shield is equipped
        private Timer m_RemoveEffectTimer; // Timer to remove the effect
        private Timer m_DamageCooldownTimer; // Timer to prevent multiple damage applications

        public static void Initialize()
        {
            PlayerEvent.HitByWeapon += new PlayerEvent.OnWeaponHit(InternalCallback);
        }

        [Constructable]
        public ObronaZywiolow()
        {
            Hue = 2613;
            Name = "Obrona Zywiolow - Zimno";
            Attributes.DefendChance = 25;
            Attributes.EnhancePotions = 15;
            // Fixed: Changed Label1 to Name
            Name = "okruchy magicznego lodu pokrywaja tarcze";
        }

        public ObronaZywiolow(Serial serial) : base(serial)
        {
        }

        public override bool OnEquip(Mobile from)
        {
            bool baseResult = base.OnEquip(from);

            m_ShieldEquipTime = DateTime.UtcNow;
            StartRemoveEffectTimer();

            return baseResult;
        }

        public void OnRemoved(IEntity parent)
        {
            base.OnRemoved(parent);

            StopRemoveEffectTimer();
            StopDamageCooldownTimer();

            Mobile mobile = parent as Mobile;
            if (mobile != null)
            {
                RemoveEffectCallback(); // Call directly on unequip
            }
        }

        private void StartRemoveEffectTimer()
        {
            if (m_RemoveEffectTimer != null)
                StopRemoveEffectTimer();

            TimeSpan duration = TimeSpan.FromSeconds(10); // Adjust the duration as needed

            m_RemoveEffectTimer = Timer.DelayCall(duration, RemoveEffectCallback);
        }

        private void StopRemoveEffectTimer()
        {
            if (m_RemoveEffectTimer != null)
            {
                m_RemoveEffectTimer.Stop();
                m_RemoveEffectTimer = null;
            }
        }

        private void RemoveEffectCallback()
        {
            Mobile mobile = RootParent as Mobile;
            if (mobile != null)
            {
                RemoveDamagingEffect(mobile);
            }
        }

        private void RemoveDamagingEffect(Mobile mobile)
        {
            int coldDamage = 10;
            mobile.Damage(coldDamage);
            mobile.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
            mobile.SendMessage("The cold shield effect wears off.");
        }

        // Added these functions
        public void StartDamageCooldownTimer()
        {
            if (m_DamageCooldownTimer != null)
                StopDamageCooldownTimer();

            TimeSpan duration = TimeSpan.FromSeconds(5); // Adjust the cooldown as needed

            m_DamageCooldownTimer = Timer.DelayCall(duration, StopDamageCooldownTimer);
        }

        public void StopDamageCooldownTimer()
        {
            if (m_DamageCooldownTimer != null)
            {
                m_DamageCooldownTimer.Stop();
                m_DamageCooldownTimer = null;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_ShieldEquipTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ShieldEquipTime = reader.ReadDateTime();
            StartRemoveEffectTimer();
        }

        public static void InternalCallback(Mobile attacker, Mobile defender, int damage, WeaponAbility a)
        {
            // Cast attacker to ObronaZywiolow type to access custom methods/timers
            if (damage > 20 && attacker != null && defender != null && defender.Weapon is BaseMeleeWeapon)
            {
                ObronaZywiolow obrona = defender.FindItemOnLayer(Layer.TwoHanded) as ObronaZywiolow;
                
                if (obrona != null && obrona.m_DamageCooldownTimer == null)
                {
                    int coldDamage = 10;
                    attacker.Damage(coldDamage);
                    defender.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
                    attacker.SendMessage("Lodowa tarcza zmraza krew w Twych zylach");
                    obrona.StartDamageCooldownTimer();
                }
            }
        }
    }
}
