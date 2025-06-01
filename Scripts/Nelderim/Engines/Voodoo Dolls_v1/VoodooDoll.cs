using System;
using System.Linq;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Gumps;

namespace Server.Items
{
    /// <summary>
    /// Laleczka voodoo: pozwala wiązać postać jako ofiarę, ożywiać miksturą,
    /// a następnie rzucać zaklęcia (ukłucie, klątwa, przeniesienie choroby) na powiązany cel.
    /// </summary>
    public class VoodooDoll : BaseImprisonedMobile
    {
        // --- Pola ---
        private int _uses = 1;
        private Mobile m_CursedPerson;
        private Mobile _linkedTarget;
        private Mobile _binder;
        private DateTime _linkExpire;
        private DateTime _lastCastTime = DateTime.MinValue;
        private int m_Animated;
        private int m_TimesPoked;

        // --- Właściwości ---
        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get => _uses;
            set { _uses = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile CursedPerson
        {
            get => m_CursedPerson;
            set { m_CursedPerson = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Animated
        {
            get => m_Animated;
            set { m_Animated = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimesPoked
        {
            get => m_TimesPoked;
            set { m_TimesPoked = value; InvalidateProperties(); }
        }

        /// <summary>
        /// Tworzy instancję ożywionej laleczki jako stworzenie.
        /// </summary>
        public override BaseCreature Summon => new AnimatedVoodooDoll(m_CursedPerson, true);
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasActiveLink => _linkedTarget != null && DateTime.UtcNow <= _linkExpire;
        public Mobile LinkedTarget => _linkedTarget;


        // --- Konstruktory ---
        [Constructable]
        public VoodooDoll(Mobile m) : base(8457)
        {
            Weight         = 3.0;
            Hue            = 2983;
            Stackable      = false;
            Name = "Lalka Guslarza";
            m_CursedPerson = m;
            // m_Animated i m_TimesPoked inicjalizowane są domyślnie na 0
            InvalidateProperties();
        }

        public VoodooDoll(Serial serial) : base(serial)
        {
        }

        // --- Serializacja ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // wersja
            writer.Write(m_Animated);
            writer.Write(m_TimesPoked);
            writer.Write(m_CursedPerson);
            writer.Write(_uses);
            writer.Write(_linkExpire.ToBinary());
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version     = reader.ReadInt();
            m_Animated      = reader.ReadInt();
            m_TimesPoked    = reader.ReadInt();
            m_CursedPerson  = reader.ReadMobile();
            _uses           = reader.ReadInt();
            _linkExpire     = DateTime.FromBinary(reader.ReadLong());
        }

        // --- Tooltipy ---
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            var targetName = m_CursedPerson?.Name ?? "(nieznana)";
            if (m_Animated >= 1)
                list.Add($"Lalka guślarza - {targetName} [aby animować, potrzebujesz mikstury]");
            else
                list.Add($"Lalka guślarza - {targetName}");
            
            if (m_CursedPerson != null)
	            list.Add($"Ofiara: {m_CursedPerson.Name}");
            if (HasActiveLink)
	            list.Add($"Aktywne połączenie z: {_linkedTarget.Name}");

            if (m_TimesPoked >= 0 && m_TimesPoked <= 9)
                list.Add($"<BASEFONT COLOR=#cc33ff>Uklucia: <BASEFONT COLOR=#ff0000>{m_TimesPoked}");
            else if (m_TimesPoked >= 10)
                list.Add("<BASEFONT COLOR=#cc33ff>Nie da się ukłuć");
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            InvalidateProperties();
        }

        /// <summary>
        /// Obsługa podwójnego kliknięcia:
        /// - jeśli lalka nie w plecaku → komunikat,
        /// - jeśli brak ofiary → wybór ofiary,
        /// - jeśli nie ożywiona → komunikat,
        /// - jeśli skill <100 → komunikat,
        /// - konsumpcja mikstury animacji → ożywienie + animacja,
        /// - następnie powiązanie lub otwarcie Gumpa voodoo.
        /// </summary>
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // "Musisz mieć przedmiot w plecaku"
                return;
            }

            if (m_CursedPerson == null)
            {
                from.SendMessage("Wskaż postać lub potwora jako ofiarę laleczki.");
                from.Target = new CursedPersonTarget(this);
                return;
            }

            if (m_Animated < 1)
            {
                from.SendMessage("Tego nie da się ożywić.");
                return;
            }

            if (from.Skills[SkillName.TasteID].Value < 100.0)
            {
                from.SendMessage("Potrzeba 100.0 w Guślarstwie, by to zrobić.");
                return;
            }

            var pack = from.Backpack;
            // Poprawione sprawdzenie konsumpcji mikstury: ConsumeTotal zwraca bool
            if (pack == null || !pack.ConsumeTotal(typeof(AnimatedVDoll), 1))
            {
                from.SendMessage("Potrzebujesz mikstury animacji.");
                return;
            }

            // Ożywienie laleczki
            TimeSpan duration = TimeSpan.FromMinutes(60);
            SpellHelper.Summon(new AnimatedVoodooDoll(m_CursedPerson, true), from, 0x217, duration, false, false);
            from.SendMessage("Laleczka ożywa!");
            m_Animated++;

            // Następnie powiązanie lub otwarcie gumpa
            if (_linkedTarget == null || DateTime.UtcNow > _linkExpire)
                StartLinking(from);
            else
                from.SendGump(new VoodooSpellGump(this, from));
        }

        /// <summary>
        /// Próba nawiązania połączenia niezależnie od Gump.
        /// </summary>
        public void TryLink(Mobile from)
        {
            if (_linkedTarget != null && !_linkedTarget.Deleted && _linkedTarget.Alive)
            {
                from.SendMessage("Nie możesz zmienić połączenia, dopóki obecny cel jest nadal żywy.");
                return;
            }

            _binder     = from;
            _linkExpire = DateTime.UtcNow.AddMinutes(1);

            double chance = from.Skills[SkillName.TasteID].Value / 100.0;
            if (Utility.RandomDouble() > chance)
            {
                from.SendMessage("Próba nawiązania połączenia nie powiodła się.");
                return;
            }

            from.SendMessage("Udana inkantacja! Wskaż postać lub stworzenie, do którego chcesz przywiązać swego ducha.");
            from.Target = new LinkTarget(this);
        }

        /// <summary>
        /// Rozpoczyna animowaną inkantację łączenia.
        /// </summary>
        private void StartLinking(Mobile from)
        {
            from.SendMessage("Inkantuję połączenie z celem... Nie ruszaj się!");
            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                if (from.Deleted || this.Deleted) return;
                if (!from.InRange(GetWorldLocation(), 5))
                {
                    from.SendMessage("Zbyt daleko – inkantacja przerwana.");
                    return;
                }
                from.SendMessage("Wskaż cel, na którym chcesz nawiązać połączenie.");
                from.Target = new LinkTarget(this);
            });
        }

        /// <summary>
        /// Rzuca wybrane zaklęcie voodoo (ukłucie, klątwa, choroba).
        /// </summary>
        public void CastVoodooSpell(Mobile from, VoodooSpellType type)
        {
            DateTime now = DateTime.UtcNow;
            if (now < _lastCastTime + TimeSpan.FromSeconds(3.0))
            {
                from.SendMessage("Odczekaj chwilę, zanim użyjesz kolejnego zaklęcia guślarstwa.");
                return;
            }
            _lastCastTime = now;

            if (_linkedTarget == null || now > _linkExpire)
            {
                from.SendMessage("Brak aktywnego połączenia – nawiąż je ponownie.");
                return;
            }

            var target = _linkedTarget;
            var pin    = from.Backpack.FindItemByType<VoodooPin>();
            if (pin == null)
            {
                from.SendMessage("Brakuje szpilek!");
                return;
            }
            pin.Consume();

            switch (type)
            {
                case VoodooSpellType.Stab:
                    {
                        double parryChance = target.Skills[SkillName.Parry].Value / 100.0;
                        if (Utility.RandomDouble() < parryChance)
                        {
                            from.SendMessage("Ukłucie zostało odparte przez parowanie!");
                            target.SendMessage("Parowanie ochroniło cię przed ukłuciem.");
                            return;
                        }
                        ApplyStab(from);
                        break;
                    }
                case VoodooSpellType.Curse:
                    {
                        double resist = target.Skills[SkillName.MagicResist].Value / 100.0;
                        if (Utility.RandomDouble() < resist)
                        {
                            from.SendMessage("Klątwa została odparta dzięki odporności na magię!");
                            target.SendMessage("Odporność na magię ochroniła cię przed klątwą.");
                            return;
                        }
                        ApplyCurse(from);
                        break;
                    }
                case VoodooSpellType.Disease:
                    {
                        double resist2 = target.Skills[SkillName.MagicResist].Value / 100.0;
                        if (Utility.RandomDouble() < resist2)
                        {
                            from.SendMessage("Przeniesienie choroby zostało odparte dzięki odporności na magię!");
                            target.SendMessage("Odporność na magię ochroniła cię przed przeniesieniem choroby.");
                            return;
                        }
                        ApplyDisease(from);
                        break;
                    }
                default:
                    from.SendMessage("Nieznany efekt guślarstwa.");
                    break;
            }
        }

        /// <summary>
        /// Konsumuje użycie laleczki i usuwa, gdy zabraknie.
        /// </summary>
        private void Consume()
        {
            if (--_uses <= 0)
                Delete();
        }

        /// <summary>
        /// Zadaje efekt DoT ukłucia.
        /// </summary>
        public void ApplyStab(Mobile from)
        {
            int maxDuration = (_linkedTarget is PlayerMobile) ? 10 : 30;
            int steps       = (int)(from.Skills[SkillName.TasteID].Value / 10);
            int ticks       = steps * (maxDuration / 10);
            if (ticks <= 0)
            {
                from.SendMessage("Brak wystarczającej mocy umiejętności, aby odnieść efekt ukłucia.");
                return;
            }
            new StabDOTTimer(_linkedTarget, ticks, 2, from).Start();
            from.SendMessage($"Ukłucie – cel otrzymuje 2 obrażeń fizycznych co sekundę przez {ticks}s.");
            Consume();
        }

        /// <summary>
        /// Rzuca efekt klątwy.
        /// </summary>
        public void ApplyCurse(Mobile from)
        {
            int maxDuration = (_linkedTarget is PlayerMobile) ? 10 : 30;
            int steps       = (int)(from.Skills[SkillName.TasteID].Value / 10);
            int durationSec = steps * (maxDuration / 10);
            if (durationSec <= 0)
            {
                from.SendMessage("Brak wystarczającej mocy umiejętności, aby rzucić klątwę.");
                return;
            }
            _linkedTarget.FixedParticles(0x375A, 9, 20, 5034, EffectLayer.Head);
            _linkedTarget.PlaySound(0x204);
            _linkedTarget.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, rzucają klątwę na ciebie.");
            from.SendMessage($"Klątwa – statystyki obniżone na {durationSec}s.");
            Consume();
        }

        /// <summary>
        /// Inicjuje przeniesienie choroby.
        /// </summary>
        public void ApplyDisease(Mobile from)
        {
            if (_linkedTarget == null || _linkedTarget.Deleted)
            {
                from.SendMessage("Brak powiązanego celu.");
                return;
            }
            from.SendMessage("Wskaż postać, z której chcesz pobrać truciznę.");
            from.Target = new DiseaseSourceTarget(this);
        }

        // --- Klasy wewnętrzne do targeting i timingów ---

        private class CursedPersonTarget : Target
        {
            private readonly VoodooDoll _doll;
            public CursedPersonTarget(VoodooDoll doll) : base(12, false, TargetFlags.None) { _doll = doll; }
            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_doll.Deleted || !(targeted is Mobile m))
                {
                    from.SendMessage("To nie jest prawidłowy cel.");
                    return;
                }
                _doll.m_CursedPerson = m;
                _doll.InvalidateProperties();
                from.SendMessage($"Laleczka połączona z ofiarą: {m.Name}.");
            }
        }

        private class LinkTarget : Target
        {
            private readonly VoodooDoll _doll;
            public LinkTarget(VoodooDoll doll) : base(12, false, TargetFlags.None) { _doll = doll; }
            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_doll.Deleted)
                    return;

                if (!(targeted is Mobile m))
                {
                    from.SendMessage("To nie jest prawidłowy cel.");
                    return;
                }

                if (m is PlayerMobile)
                {
                    Head head = _doll.Items.OfType<Head>().FirstOrDefault(h => h.Owner == m);
                    if (head == null)
                    {
                        from.SendMessage("Musisz najpierw umieścić głowę tego gracza w lalce guślarza.");
                        return;
                    }
                    head.Delete();
                }

                _doll._linkedTarget = m;
                _doll._linkExpire   = DateTime.UtcNow.AddMinutes(5);
                from.SendMessage($"Połączenie z {m.Name} nawiązane na 5 minut.");
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendMessage("Cel wyszedł poza zasięg.");
            }
        }

        private class StabDOTTimer : Timer
        {
            private readonly Mobile _target;
            private int _ticks, _dmg;
            private readonly Mobile _source;

            public StabDOTTimer(Mobile target, int ticks, int dmg, Mobile source)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                _target = target;
                _ticks  = ticks;
                _dmg    = dmg;
                _source = source;
            }

            protected override void OnTick()
            {
                if (_ticks-- <= 0 || _target.Deleted)
                {
                    Stop();
                    return;
                }
                AOS.Damage(_target, _source, _dmg, 100, 0, 0, 0, 0);
                _target.FixedEffect(0x376A, 10, 15, 1161, 0);
                _target.PlaySound(0x1E0);
            }
        }

        private class DiseaseSourceTarget : Target
        {
            private readonly VoodooDoll _doll;
            private Poison _poisonToTransfer;

            public DiseaseSourceTarget(VoodooDoll doll) : base(12, false, TargetFlags.None) { _doll = doll; }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is Mobile src) || src.Poison == null)
                {
                    from.SendMessage("Ten cel nie jest zatruty.");
                    return;
                }
                _poisonToTransfer = src.Poison;
                src.Poison = null;
                from.SendMessage("Trucizna pobrana. Wskaż cel, na którego chcesz przenieść truciznę.");
                from.Target = new DiseaseDestTarget(_doll, _poisonToTransfer);
            }
        }

        private class DiseaseDestTarget : Target
        {
            private readonly VoodooDoll _doll;
            private readonly Poison    _poison;

            public DiseaseDestTarget(VoodooDoll doll, Poison poison) : base(12, false, TargetFlags.Harmful)
            {
                _doll   = doll;
                _poison = poison;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is Mobile dst))
                {
                    from.SendMessage("Nieprawidłowy cel.");
                    return;
                }

                dst.Poison = _poison;
                dst.FixedEffect(0x376A, 10, 15, 1161, 0);
                dst.PlaySound(1110);
                from.SendMessage($"Przeniesiono truciznę na {dst.Name}.");
                _doll.Consume();
            }
        }

        // --- Rejestracja eventu zabicia ---
        public static void Initialize()
        {
            EventSink.OnKilledBy += OnKilledByHandler;
        }

        private static void OnKilledByHandler(OnKilledByEventArgs e)
        {
            var dead = e.Killed;
            if (dead == null) return;

            foreach (var doll in World.Items.OfType<VoodooDoll>())
            {
                if (doll._linkedTarget == dead)
                    doll.ResetLink();
            }
        }

        /// <summary>
        /// Resetuje połączenie, gdy cel zginie.
        /// </summary>
        public void ResetLink()
        {
            _linkedTarget = null;
            _linkExpire   = DateTime.MinValue;
            _binder?.SendMessage("Połączenie zostało zresetowane – powiązany cel zginął lub został usunięty.");
        }
    }
}
