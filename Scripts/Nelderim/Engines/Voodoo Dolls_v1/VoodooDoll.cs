using System;
using System.Linq;
using Server;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Gumps;

namespace Server.Items
{
    public class VoodooDoll : BaseImprisonedMobile
    {
        public override BaseCreature Summon => new AnimatedVoodooDoll(m_CursedPerson, true);

        // === Pola do guślarstwa ===
        private Mobile _binder;
        private Mobile _linkedTarget;
        private DateTime _linkExpire;
        private DateTime _lastCastTime = DateTime.MinValue;

        private int m_Animated;
        [CommandProperty(AccessLevel.GameMaster)]
        public int Animated
        {
            get => m_Animated;
            set { m_Animated = value; InvalidateProperties(); }
        }

        private int m_TimesPoked;
        [CommandProperty(AccessLevel.GameMaster)]
        public int TimesPoked
        {
            get => m_TimesPoked;
            set { m_TimesPoked = value; InvalidateProperties(); }
        }

        private Mobile m_CursedPerson;
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile CursedPerson
        {
            get => m_CursedPerson;
            set { m_CursedPerson = value; InvalidateProperties(); }
        }

        public VoodooDoll(Mobile m) : base(8457)
        {
            Weight = 3.0;
            Hue = 2983;
            Stackable = false;
            CursedPerson = m;
            TimesPoked = m_TimesPoked;
            Animated = m_Animated;
            InvalidateProperties();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            string targetName = (m_CursedPerson != null && m_CursedPerson.Name != null)
                ? m_CursedPerson.Name
                : "(nieznana)";

            if (m_Animated >= 1)
                list.Add("lalka guślarza - {0} [aby animować, potrzebujesz mikstury]", targetName);
            else
                list.Add("lalka guślarza - {0}", targetName);

            base.GetProperties(list);

            if (m_TimesPoked >= 0 && m_TimesPoked <= 9)
                list.Add($"<BASEFONT COLOR=#cc33ff>Poked: <BASEFONT COLOR=#ff0000>{m_TimesPoked}");
            else if (m_TimesPoked >= 10)
                list.Add("<BASEFONT COLOR=#cc33ff>Nie da się ukłuć");
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            InvalidateProperties();
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Podwójne kliknięcie: jeśli brak ofiary wskaż nową ofiarę; jeśli masz ofiarę i lalka nie została animowana, użyj mikstury animacji; jeśli już jest animowana, nawiąż połączenie.");

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            if (m_CursedPerson == null)
            {
                from.SendMessage("Wskaż potwora lub postać jako ofiarę laleczki.");
                from.Target = new CursedPersonTarget(this);
                return;
            }

            if (Animated < 1)
            {
                from.SendMessage("Tego nie da się wskrzesic.");
                return;
            }

            if (from.Skills[SkillName.TasteID].Value < 100.0)
            {
                from.SendMessage("Potrzeba 100.0 guślarstwa, by to zrobić.");
                return;
            }

            Container pack = from.Backpack;
            if (pack == null || pack.ConsumeTotal(new Type[]{ typeof(AnimatedVDoll) }, new int[]{ 1 }) == 0)
            {
                from.SendMessage("Potrzebujesz mikstury animacji.");
                return;
            }

            TimeSpan duration = TimeSpan.FromMinutes(60);
            SpellHelper.Summon(new AnimatedVoodooDoll(m_CursedPerson, true), from, 0x217, duration, false, false);
            from.SendMessage("Laleczka ożywa!");

            if (_linkedTarget == null || DateTime.UtcNow > _linkExpire)
                StartLinking(from);
            else
                from.SendGump(new VoodooSpellGump(from, this));
        }

        public void TryLink(Mobile from)
        {
            if (_linkedTarget != null && !_linkedTarget.Deleted && _linkedTarget.Alive)
            {
                from.SendMessage("Nie możesz zmienić połączenia, dopóki obecny cel jest nadal żywy.");
                return;
            }

            _linkExpire = DateTime.UtcNow.AddMinutes(1);
            double chance = from.Skills[SkillName.TasteID].Value / 100.0;
            if (Utility.RandomDouble() > chance)
            {
                from.SendMessage("Próba nawiązania połączenia nie powiodła się.");
                return;
            }

            _binder = from;
            from.SendMessage("Udana inkantacja! Wskaż postać lub stworzenie, do którego chcesz przywiązać swego ducha.");
            from.Target = new LinkTarget(this);
        }

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

        private class LinkTarget : Target
        {
	        private readonly VoodooDoll _doll;

	        public LinkTarget(VoodooDoll doll)
		        : base(12, false, TargetFlags.None)
	        {
		        _doll = doll;
	        }

	        protected override void OnTarget(Mobile from, object targeted)
	        {
		        if (_doll.Deleted)
			        return;

		        // Wyraźne rzutowanie celu na Mobile
		        Mobile m = targeted as Mobile;
		        if (m == null)
		        {
			        from.SendMessage("To nie jest prawidłowy cel.");
			        return;
		        }

		        // Jeśli cel to gracz, wymagamy najpierw wrzuconej głowy tego gracza do laleczki
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

		        // Ustawienie linku i czasu wygaśnięcia
		        _doll._linkedTarget = m;
		        _doll._linkExpire   = DateTime.UtcNow.AddMinutes(5);

		        from.SendMessage($"Połączenie z {m.Name} nawiązane na 5 minut.");
	        }

	        protected override void OnTargetOutOfRange(Mobile from, object targeted)
	        {
		        from.SendMessage("Cel wyszedł poza zasięg.");
	        }
        }


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

        public void ResetLink()
        {
            _linkedTarget = null;
            _linkExpire   = DateTime.MinValue;
            if (_binder != null && !_binder.Deleted)
                _binder.SendMessage("Połączenie zostało zresetowane – powiązany cel zginął lub został usunięty.");
        }

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

            Mobile target = _linkedTarget;
            var pin = from.Backpack.FindItemByType<VoodooPin>();
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
                    double resistChance = target.Skills[SkillName.MagicResist].Value / 100.0;
                    if (Utility.RandomDouble() < resistChance)
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
                    double resistChance2 = target.Skills[SkillName.MagicResist].Value / 100.0;
                    if (Utility.RandomDouble() < resistChance2)
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

        private void ApplyStab(Mobile from)
        {
            int maxDuration = (_linkedTarget is PlayerMobile) ? 10 : 30;
            int steps       = (int)from.Skills[SkillName.TasteID].Value / 10;
            int ticks       = steps * (maxDuration / 10);
            if (ticks <= 0)
            {
                from.SendMessage("Brak wystarczającej mocy umiejętności, aby odnieść efekt ukłucia.");
                return;
            }
            int dmgPerTick = 2;
            new StabDOTTimer(_linkedTarget, ticks, dmgPerTick, from).Start();
            from.SendMessage($"Ukłucie – cel otrzymuje {dmgPerTick} obrażeń fizycznych co sekundę przez {ticks}s.");
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

        private void ApplyCurse(Mobile from)
        {
            int maxDuration = (_linkedTarget is PlayerMobile) ? 10 : 30;
            int steps       = (int)from.Skills[SkillName.TasteID].Value / 10;
            int durationSec = steps * (maxDuration / 10);
            if (durationSec <= 0)
            {
                from.SendMessage("Brak wystarczającej mocy umiejętności, aby rzucić klątwę.");
                return;
            }
            int penalty = 20;
            TimeSpan duration = TimeSpan.FromSeconds(durationSec);
            string key = $"VoodooCurse_{Serial.Value}";

            _linkedTarget.AddStatMod(new StatMod(StatType.Str, key, -penalty, duration));
            _linkedTarget.AddStatMod(new StatMod(StatType.Dex, key, -penalty, duration));
            _linkedTarget.AddStatMod(new StatMod(StatType.Int, key, -penalty, duration));

            _linkedTarget.FixedEffect(0x376A, 10, 15, 1161, 0);
            _linkedTarget.PlaySound(1099);

            from.SendMessage($"Rzucono klątwę – statystyki obniżone na {durationSec}s.");
            this.Delete();
        }

        private void ApplyDisease(Mobile from)
        {
            from.SendMessage("Wskaż postać, z której chcesz pobrać truciznę.");
            from.Target = new DiseaseSourceTarget(this);
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


		/// <summary>
		/// Wybór celu przeniesienia trucizny.
		/// </summary>
		private class DiseaseDestTarget : Target
		{
			private readonly VoodooDoll _doll;
			private readonly Poison _poison;

			public DiseaseDestTarget(VoodooDoll doll, Poison poison) : base(12, false, TargetFlags.Harmful)
			{
				_doll = doll;
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
				// Efekt wizualny: wirujące cząstki na celu
				dst.FixedEffect(0x376A, 10, 15, 1161, 0);

				// Dźwięk przeniesienia
				dst.PlaySound(1110);
				from.SendMessage($"Przeniesiono truciznę na {dst.Name}.");
			}
		}



		// Serialization
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
			writer.Write(m_Animated);
			writer.Write(m_TimesPoked);
			writer.Write(m_CursedPerson);
			writer.Write(_linkExpire.ToBinary());
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Animated = reader.ReadInt();
			m_TimesPoked = reader.ReadInt();
			m_CursedPerson = reader.ReadMobile();
			_linkExpire = DateTime.FromBinary(reader.ReadLong());
		}

		public VoodooDoll() : this(null) { }
		public VoodooDoll(Serial serial) : base(serial) { }
	}

	[Flipable(0x13CA, 0x13D1)]
	public class Doll : Item
	{
		private Mobile m_CursedPerson;
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile CursedPerson { get => m_CursedPerson; set => m_CursedPerson = value; }

		[Constructable]
		public Doll() : base(0x13CA)
		{
			Name = "laleczka";
			Weight = 3.0;
			Hue = 1096;
			Stackable = false;
		}

		public Doll(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!Movable) return;
			from.Target = new InternalTarget(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
			writer.Write(m_CursedPerson);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_CursedPerson = reader.ReadMobile();
		}

		private class InternalTarget : Target
		{
			private readonly Doll _doll;
			public InternalTarget(Doll doll) : base(1, false, TargetFlags.None) { _doll = doll; }
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (_doll.Deleted) return;
				if (targeted is Head head && head.Owner != null)
				{
					if (_doll.CursedPerson == null)
					{
						_doll.CursedPerson = head.Owner;
						_doll.Hue = 2782;
						_doll.Name = "zakrwawiona laleczka";
						_doll.InvalidateProperties();
						head.Consume();
						from.SendMessage("Umieściłeś głowę na lalce.");
						from.Karma -= 25;
						from.SendLocalizedMessage(1019063);
					}
					else
					{
						from.SendMessage("Na lalce jest już głowa!");
					}
					return;
				}

				if (targeted is Item itm && CraftSystem.VoodooUtils.IsHeatSource(itm))
				{
					if (from.BeginAction(typeof(CookableFood)))
					{
						from.PlaySound(0x225);
						_doll.Consume();
						new CookTimer(from, itm as IPoint3D, from.Map, _doll).Start();
					}
					else
					{
						from.SendLocalizedMessage(500119);
					}
				}
			}

			private class CookTimer : Timer
			{
				private Mobile _from;
				private IPoint3D _point;
				private Map _map;
				private Doll _doll;

				public CookTimer(Mobile from, IPoint3D p, Map map, Doll doll) : base(TimeSpan.FromSeconds(5.0))
				{
					_from = from;
					_point = p;
					_map = map;
					_doll = doll;
				}

				protected override void OnTick()
				{
					_from.EndAction(typeof(CookableFood));
					if (_from.Map != _map || (_point != null && _from.GetDistanceToSqrt(_point) > 3))
					{
						_from.SendMessage("Lalka uciekla!");
						return;
					}

					if (_from.CheckSkill(SkillName.TasteID, 90, 100))
					{
						_from.SendMessage("Stworzyłeś lalke guslarza");
						if (_from.AddToBackpack(new VoodooDoll(_doll.CursedPerson)))
							_from.PlaySound(0x57);
					}
					else
					{
						_from.SendMessage("Lalka wydała donośny krzyk i uciekła");
					}
				}
			}
		}
	}
}
