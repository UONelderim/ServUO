using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public static class VoodooManager
    {
        private static readonly Dictionary<PinEffect, Action<Mobile, Mobile>> _pinEffects;
        private static readonly Dictionary<VoodooSpellType, Action<VoodooDoll, Mobile>> _spellEffects;

        static VoodooManager()
        {
            // Rejestracja efektów szpilki
            _pinEffects = new Dictionary<PinEffect, Action<Mobile, Mobile>>
            {
                { PinEffect.Stab,    ApplyStabEffect    },
                { PinEffect.Curse,   ApplyCurseEffect   },
                { PinEffect.Disease, ApplyDiseaseEffect }
            };

            // Rejestracja zaklęć laleczki – Link używa TryLink, pozostałe idą przez CastVoodooSpell
            _spellEffects = new Dictionary<VoodooSpellType, Action<VoodooDoll, Mobile>>
            {
                { VoodooSpellType.Stab,    (doll, from) => doll.CastVoodooSpell(from, VoodooSpellType.Stab)    },
                { VoodooSpellType.Curse,   (doll, from) => doll.CastVoodooSpell(from, VoodooSpellType.Curse)   },
                { VoodooSpellType.Disease, (doll, from) => doll.CastVoodooSpell(from, VoodooSpellType.Disease) },
                { VoodooSpellType.Link,    (doll, from) => doll.TryLink(from)                                 }
            };

            EventSink.OnKilledBy += OnKilledByHandler;
        }

        /// <summary>
        /// Rzuca zaklęcie voodoo wybrane w gumpie.
        /// </summary>
        public static void CastSpell(VoodooSpellType type, VoodooDoll doll, Mobile from)
        {
	        switch (type)
	        {
		        case VoodooSpellType.Stab:
			        doll.ApplyStab(from);
			        break;
		        case VoodooSpellType.Curse:
			        doll.ApplyCurse(from);
			        break;
		        case VoodooSpellType.Disease:
			        doll.ApplyDisease(from);
			        break;
		        case VoodooSpellType.Link:
			        doll.TryLink(from);
			        break;
		        default:
			        from.SendMessage("Nieznany efekt guślarstwa.");
			        break;
	        }
        }


        /// <summary>
        /// Wykonuje efekt szpilki voodoo na celu.
        /// </summary>
        public static void ApplyPinEffect(PinEffect effect, Mobile from, Mobile to)
        {
            if (!_pinEffects.TryGetValue(effect, out var action))
            {
                from.SendMessage("Nieznany efekt voodoo szpilki.");
                return;
            }
            action(from, to);
        }

        private static void OnKilledByHandler(OnKilledByEventArgs e)
        {
	        var dead = e.Killed;
	        if (dead == null)
		        return;

	        foreach (var doll in World.Items.OfType<VoodooDoll>())
	        {
		        if (doll.HasActiveLink && doll.LinkedTarget == dead)
			        doll.ResetLink();
	        }
        }


        private static void ApplyStabEffect(Mobile from, Mobile target)
        {
            int dmg   = 2;
            int steps = (int)(from.Skills[SkillName.TasteID].Value / 10);
            if (steps <= 0)
            {
                from.SendMessage("Brak wystarczającej mocy Guślarstwa do ukłucia.");
                return;
            }

            new StabDOTTimer(target, steps, dmg, from).Start();
            from.SendMessage($"Ukłucie – {dmg} obrażeń co sekundę przez {steps}s.");
        }

        private static void ApplyCurseEffect(Mobile from, Mobile target)
        {
            target.FixedParticles(0x375A, 9, 20, 5034, EffectLayer.Head);
            target.PlaySound(0x204);
            target.SendMessage($"Rzucono klątwę na ciebie, rzucając {from.Name}.");
            from.SendMessage("Klątwa nałożona.");
        }

        private static void ApplyDiseaseEffect(Mobile from, Mobile target)
        {
            from.SendMessage("Wskaż postać, z której chcesz pobrać truciznę.");
            from.Target = new DiseaseSourceTarget(target);
        }

        #region Timers i Targety

        private class StabDOTTimer : Timer
        {
            private readonly Mobile _target;
            private int             _ticks;
            private readonly int    _dmg;
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
            private readonly Mobile _initialTarget;
            private Poison          _poisonToTransfer;

            public DiseaseSourceTarget(Mobile target)
                : base(12, false, TargetFlags.Harmful)
            {
                _initialTarget = target;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is Mobile src) || src.Poison == null)
                {
                    from.SendMessage("Ten cel nie jest zatruty.");
                    return;
                }

                _poisonToTransfer = src.Poison;
                src.Poison        = null;

                from.SendMessage("Trucizna pobrana. Wskaż, na kogo chcesz ją przenieść.");
                from.Target = new DiseaseDestTarget(_initialTarget, _poisonToTransfer);
            }
        }

        private class DiseaseDestTarget : Target
        {
            private readonly Mobile _to;
            private readonly Poison _poison;

            public DiseaseDestTarget(Mobile to, Poison poison)
                : base(12, false, TargetFlags.Harmful)
            {
                _to     = to;
                _poison = poison;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is Mobile dst))
                {
                    from.SendMessage("Nieprawidłowy cel.");
                    return;
                }

                dst.Poison     = _poison;
                dst.FixedEffect(0x376A, 10, 15, 1161, 0);
                dst.PlaySound(1110);
                from.SendMessage($"Przeniesiono truciznę na {dst.Name}.");
            }
        }

        #endregion
    }
}
