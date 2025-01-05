using System;
using Server.Targeting;
using Server.Mobiles;


namespace Server.Items
{
    public class FeedTarget : Target
    {
        private GluttonousBlade m_Item;

        public FeedTarget(GluttonousBlade item)
            : base(1, false, TargetFlags.None)
        {
            m_Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is Gold gold)
            {
                m_Item.TimeLeft = TimeSpan.FromMinutes(gold.Amount / 100.0);
                from.SendMessage($"Bron konsumuje {gold.Amount} centarow");
                from.PlaySound(1073);
                m_Item.StartTimer();
                gold.Delete();
            }
            else
            {
                from.PlaySound(1094);
                from.Say("Obrzydlistwo... To nie jest zloto!");
            }
        }
    }

    public class GluttonousBlade : Kryss
    {
        private Timer m_Timer;
        private TimeSpan m_TimeLeft;

        public override int InitMinHits => 100;
        public override int InitMaxHits => 100;

        public int AosMinDamage => 15;
        public int AosMaxDamage => 18;
        public int AosSpeed => 60;

        [Constructable]
        public GluttonousBlade()
            : base()
        {
            Name = "Krys Przekletego Glodu";
            Hue = 1287;
            LootType = LootType.Cursed;

            m_TimeLeft = TimeSpan.FromMinutes(10);
            StartTimer();
        }

        public GluttonousBlade(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan TimeLeft
        {
            get => m_TimeLeft;
            set
            {
                m_TimeLeft = value;
                InvalidateProperties();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Timer == null)
            {
                list.Add(1114057, "[Jestem głodny!]");
                return;
            }

            int hours = (int)m_TimeLeft.TotalHours;
            int minutes = m_TimeLeft.Minutes;

            list.Add($"[Bede jeszcze najedzony: {hours} i {minutes} godzin ziemi Nelderim]");
        }

        public void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            if (defender is BaseCreature && 0.03 > Utility.RandomDouble())
            {
                defender.Hits -= 50;
                defender.PlaySound(1067);
                attacker.SendMessage("Klątwa ostrza zaczyna się uaktywniać.");
                attacker.Hits -= 15;
                attacker.Mana -= 15;
                attacker.Stam -= 15;
                attacker.Say("DAJ MI ZŁOTA!!!");
            }
            base.OnHit(attacker, defender, damageBonus);
        }

        public virtual void StartTimer()
        {
            StopTimer();

            Name = "Krys Przekletego Glodu";
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 45;
            Attributes.DefendChance = -10;
            Attributes.SpellDamage = 3;
            WeaponAttributes.HitFireball = 30;
            WeaponAttributes.HitLeechMana = 35;
            WeaponAttributes.HitLightning = 10;
            Attributes.BonusStr = 3;
            Attributes.BonusDex = 3;
            Attributes.BonusInt = 3;
            Hue = 2675;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), new TimerCallback(Slice));
        }

        public virtual void Slice()
        {
            if (m_TimeLeft <= TimeSpan.Zero)
            {
                Delete();
                return;
            }
            m_TimeLeft -= TimeSpan.FromSeconds(1);
            InvalidateProperties();
        }

        public virtual void Delete()
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            Effects.PlaySound(Location, Map, 0x201);

            StopTimer();
            m_TimeLeft = TimeSpan.Zero;

            Name = "Nienazarty kryss";
            Attributes.AttackChance = 0;
            Attributes.WeaponDamage = 0;
            Attributes.DefendChance = 0;
            WeaponAttributes.HitLeechMana = 0;
            Attributes.SpellDamage = 0;
            WeaponAttributes.HitFireball = 0;
            WeaponAttributes.HitLightning = 0;
            Attributes.BonusStr = 0;
            Attributes.BonusDex = 0;
            Attributes.BonusInt = 0;
            Hue = 1287;
        }

        public virtual void StopTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                from.SendMessage("Nakarm mnie!!!");
                from.Target = new FeedTarget(this);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);
            writer.Write(TimeLeft);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            TimeLeft = reader.ReadTimeSpan();
            StartTimer();
        }
    }
}
