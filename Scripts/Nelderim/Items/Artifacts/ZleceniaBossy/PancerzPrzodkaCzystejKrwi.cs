using System;
using Server.Spells;

namespace Server.Items
{
    public class PancerzPrzodkaCzystejKrwi : DragonChest
    {
        private DateTime m_CureLastUsage;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime CureLastUsage
        {
            get => m_CureLastUsage;
            set { m_CureLastUsage = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CureActive => DateTime.Now - m_CureLastUsage > m_CureCooldown;

        private static TimeSpan m_CureCooldown = TimeSpan.FromMinutes(15); // tutaj jest ustawiany cooldown

        public override int BasePhysicalResistance => 10;
        public override int BaseFireResistance => 15;
        public override int BaseColdResistance => 5;
        public override int BasePoisonResistance => 14;
        public override int BaseEnergyResistance => 8;

        [Constructable]
        public PancerzPrzodkaCzystejKrwi() : base()
        {
            Name = "Pancerz Przodka Czystej Krwi";
            Hue = 0x455;

            Attributes.BonusHits = 10;
            Attributes.BonusStam = 6;
            Attributes.RegenHits = 5;

            m_CureLastUsage = DateTime.MinValue;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (CureActive)
                list.Add("Grawerowana runa emanuje moca oczyszczenia krwii");
            else
                list.Add("Grawerowana runa jest wyblakla");
        }

        //public override void GetProperties(ObjectPropertyList list)
        //{
        //    base.GetProperties(list);
        //    if (CureActive)
        //        list.Add("Grawerowana runa emanuje moc� oczyszczenia krwii");
        //    else
        //        list.Add("Grawerowana runa jest wyblak�a");
        //}

        public override void OnDoubleClick(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.InnerTorso) != this)
            {
                from.SendLocalizedMessage(502641); // You must equip this item to use it.
                return;
            }

            if (!CureActive)
            {
                TimeSpan wait = m_CureCooldown - (DateTime.Now - m_CureLastUsage);
                from.SendMessage("Moc oczyszczenia jest wyczerpana. Sprobuj ponownie za " + Math.Ceiling(wait.TotalMinutes) + " minut.");
                return;
            }

            if (from.Poison == null)
            {
                from.SendMessage("Nie jestes zatruty.");
                return;
            }

            SelfArchCureSpell archCureSpell = new SelfArchCureSpell(from, this);
            archCureSpell.Cast();
        }

        private class SelfArchCureSpell : Spell
        {
            private static SpellInfo m_Info = new SpellInfo(
                    "Self Arch Cure", "Vas An Nox",
                    215,
                    9061
                );

            PancerzPrzodkaCzystejKrwi m_Artefact;

            public SelfArchCureSpell(Mobile caster, PancerzPrzodkaCzystejKrwi artefact) : base(caster, null, m_Info)
            {
                m_Artefact = artefact;
            }

            public override void OnCast()
            {
                Poison poison = Caster.Poison;
                if (poison == null)
                {
                    Caster.SendMessage("Nie jestes zatruty.");
                    return;
                }

                if (CheckSequence())
                {
                    double mageryValue = 120;
                    int chanceToCure = 10000 + (int)(mageryValue * 75) - ((poison.Level + 1) * 1750);
                    chanceToCure /= 100;
                    chanceToCure -= 1;

                    Caster.SendMessage("Uzyles mocy oczyszczenia.");

                    if (chanceToCure > Utility.Random(100) && Caster.CurePoison(Caster))
                        Caster.SendLocalizedMessage(1010058); // You have cured the target of all poisons!

                    if (Caster.Hidden == false)
                    {
                        Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                    }
                    Caster.PlaySound(0x1E0);

                    m_Artefact.m_CureLastUsage = DateTime.Now;
                    m_Artefact.InvalidateProperties();
                }

                FinishSequence();
            }

            public override int GetMana()
            {
                return 0;
            }

            public override TimeSpan CastDelayBase
            {
                get
                {
                    TimeSpan magerySpellsCastDelayBase = TimeSpan.FromSeconds((3 + (int)SpellCircle.Fourth) * CastDelaySecondsPerTick);

                    return magerySpellsCastDelayBase - TimeSpan.FromSeconds(0.25);
                }
            }
        }

        public PancerzPrzodkaCzystejKrwi(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            writer.Write(m_CureLastUsage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CureLastUsage = reader.ReadDateTime();
        }
    }
}
