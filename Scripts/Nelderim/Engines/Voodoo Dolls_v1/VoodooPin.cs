using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public enum PinEffect
    {
        None,
        Refresh,
        Agility,
        Strengthen,
        Heal,
        Cure,
        Curse,
        Poison,
        Conflagration,
        Paralyse,
        MaskOfDeath,
        Invisibility,
        Sleep,
        Disease,
        Stab
    }

    public class VoodooPin : Item
    {
        private VoodooDoll _doll;
        private PinEffect  _effect;
        private int        _uses;

        private DateTime   _lastUseTime;
        private PinEffect  _lastUseEffect;

        private enum PinAction { None, Bind, Load, Use }
        private PinAction _lastAction = PinAction.None;

        [CommandProperty(AccessLevel.GameMaster)]
        public PinEffect Effect
        {
            get => _effect;
            set { _effect = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get => _uses;
            set { _uses = value; InvalidateProperties(); }
        }

        [Constructable]
        public VoodooPin() : base(3922)
        {
            Weight          = 0.1;
            Name            = "Szpilka guślarza";
            Hue             = 2783;

            _effect         = PinEffect.None;
            _uses           = 0;
            _lastUseTime    = DateTime.MinValue;
            _lastUseEffect  = PinEffect.None;
        }

        public VoodooPin(Serial serial) : base(serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
	        base.GetProperties(list);

	        if (_effect != PinEffect.None)
		        list.Add($"Efekt: {GetEffectName(_effect)}, pozostało użyć: {_uses}");
	        else
		        list.Add("Szpilka niezaładowana");
        }


        public override void OnDoubleClick(Mobile from)
        {
            from.RevealingAction();
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            switch (_lastAction)
            {
                case PinAction.Bind:
                    from.SendMessage("Powiązywanie z laleczką...");
                    from.Target = new BindTarget(this);
                    break;
                case PinAction.Load:
                    from.SendMessage("Ładowanie efektu szpilki...");
                    from.Target = new LoadTarget(this);
                    break;
                case PinAction.Use:
                    if (_effect != PinEffect.None && _uses > 0)
                    {
                        from.SendMessage("Używanie efektu szpilki...");
                        from.Target = new PinTarget(this);
                    }
                    else
                    {
                        from.SendMessage("Szpilka nie jest załadowana.");
                    }
                    break;
                default:
                    from.SendMessage("Musisz najpierw wybrać akcję z menu kontekstowego.");
                    break;
            }
        }

        #region Context Menu
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (!IsChildOf(from.Backpack))
                return;

            list.Add(new BindEntry(from, this));  // 3070076
            list.Add(new LoadEntry(from, this));  // 3070075
            list.Add(new UseEntry(from, this));   // 3070074
        }

        private class BindEntry : ContextMenuEntry
        {
            private readonly Mobile   _from;
            private readonly VoodooPin _pin;

            public BindEntry(Mobile from, VoodooPin pin) : base(3070076, 12)
            {
                _from = from;
                _pin  = pin;
            }
            public override void OnClick()
            {
                if (_pin.Deleted) return;
                _pin._lastAction = PinAction.Bind;
                _from.SendMessage("Wybrano: Powiąż cel z laleczką. Teraz podwójne kliknięcie wykona tę akcję.");
            }
        }

        private class LoadEntry : ContextMenuEntry
        {
	        private readonly Mobile   _from;
	        private readonly VoodooPin _pin;

	        public LoadEntry(Mobile from, VoodooPin pin) : base(3070075, 12)
	        {
		        _from = from;
		        _pin  = pin;
	        }
	        public override void OnClick()
	        {
		        if (_pin.Deleted || _pin._doll == null || _pin._doll.Deleted)
		        {
			        _from.SendMessage("Brak powiązanej laleczki.");
			        return;
		        }

		        _from.SendMessage("Wybierz miksturę lub zwój, aby naładować szpilkę.");
		        _from.Target = new LoadTarget(_pin);
	        }
        }


        private class UseEntry : ContextMenuEntry
        {
            private readonly Mobile   _from;
            private readonly VoodooPin _pin;

            public UseEntry(Mobile from, VoodooPin pin) : base(3070074, 12)
            {
                _from = from;
                _pin  = pin;
            }
            public override void OnClick()
            {
                if (_pin.Deleted) return;
                _pin._lastAction = PinAction.Use;
                _from.SendMessage("Wybrano: Użyj efektu. Teraz podwójne kliknięcie wykona tę akcję.");
            }
        }
        #endregion

        #region BindTarget
        private class BindTarget : Target
        {
            private readonly VoodooPin _pin;

            public BindTarget(VoodooPin pin) : base(12, false, TargetFlags.None)
            {
                _pin = pin;
            }
            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_pin.Deleted) return;
                if (targeted is VoodooDoll doll && !doll.Deleted)
                {
                    _pin._doll = doll;
                    from.SendMessage($"Szpilka powiązana z laleczką {doll.CursedPerson?.Name ?? "(?)"}.");
                }
                else if (targeted is Doll blood && !blood.Deleted)
                {
                    var cursed = blood.CursedPerson;
                    blood.Delete();
                    var newDoll = new VoodooDoll(cursed);
                    newDoll.MoveToWorld(from.Location, from.Map);
                    _pin._doll = newDoll;
                    from.SendMessage("Zakrwawiona laleczka została przekształcona i powiązana szpilką.");
                }
                else
                {
                    from.SendMessage("Musisz wskazać prawidłową laleczkę guslarza.");
                }
            }
        }
        #endregion

        #region LoadTarget
        private class LoadTarget : Target
        {
            private readonly VoodooPin _pin;
            public LoadTarget(VoodooPin pin) : base(12, false, TargetFlags.None)
            {
                _pin = pin;
            }
            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_pin.Deleted || _pin._doll == null || _pin._doll.Deleted) return;
                if (!from.InRange(_pin, 1)) { from.SendLocalizedMessage(500446); return; }

                BasePotion bp;
                SpellScroll ss;

                if ((bp = targeted as TotalRefreshPotion) != null || (bp = targeted as RefreshPotion) != null)
                {
                    _pin.Hue = 2980; _pin.Name = "zaklęta szpilka odświeżająca";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Refresh; _pin.Uses = 50;
                }
                else if ((bp = targeted as GreaterAgilityPotion) != null ||
                         (bp = targeted as NGreaterAgilityPotion) != null ||
                         (bp = targeted as AgilityPotion) != null)
                {
                    _pin.Hue = 2620; _pin.Name = "zaklęta szpilka zręczności";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Agility; _pin.Uses = 50;
                }
                else if ((bp = targeted as GreaterStrengthPotion) != null ||
                         (bp = targeted as NGreaterStrengthPotion) != null ||
                         (bp = targeted as StrengthPotion) != null)
                {
                    _pin.Hue = 2701; _pin.Name = "zaklęta szpilka wzmocnienia";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Strengthen; _pin.Uses = 50;
                }
                else if ((bp = targeted as GreaterHealPotion) != null ||
                         (bp = targeted as HealPotion) != null ||
                         (bp = targeted as LesserHealPotion) != null)
                {
                    _pin.Hue = 2997; _pin.Name = "zaklęta szpilka leczenia";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Heal; _pin.Uses = 50;
                }
                else if ((bp = targeted as GreaterCurePotion) != null ||
                         (bp = targeted as CurePotion) != null ||
                         (bp = targeted as LesserCurePotion) != null)
                {
                    _pin.Hue = 1761; _pin.Name = "zaklęta szpilka oczyszczenia";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Cure; _pin.Uses = 50;
                }
                else if ((ss = targeted as CurseScroll) != null)
                {
                    _pin.Hue = 1154; _pin.Name = "zaklęta szpilka klątwy";
                    from.PlaySound(0x1F2); ss.Consume();
                    _pin.Effect = PinEffect.Curse; _pin.Uses = 50;
                }
                else if ((bp = targeted as PoisonPotion) != null ||
                         (bp = targeted as LesserPoisonPotion) != null ||
                         (bp = targeted as GreaterPoisonPotion) != null ||
                         (bp = targeted as DeadlyPoisonPotion) != null)
                {
                    _pin.Hue = 2857; _pin.Name = "zaklęta szpilka zatrucia";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Poison; _pin.Uses = 50;
                }
                else if ((bp = targeted as ConflagrationPotion) != null ||
                         (bp = targeted as GreaterConflagrationPotion) != null)
                {
                    _pin.Hue = 2197; _pin.Name = "zaklęta szpilka podpalenia";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.Conflagration; _pin.Uses = 50;
                }
                else if ((ss = targeted as ParalyzeScroll) != null)
                {
                    _pin.Hue = 1342; _pin.Name = "zaklęta szpilka paraliżu";
                    from.PlaySound(0x1F2); ss.Consume();
                    _pin.Effect = PinEffect.Paralyse; _pin.Uses = 50;
                }
                else if ((bp = targeted as MaskOfDeathPotion) != null ||
                         (bp = targeted as GreaterMaskOfDeathPotion) != null)
                {
                    _pin.Hue = 1166; _pin.Name = "zaklęta szpilka maski śmierci";
                    from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
                    _pin.Effect = PinEffect.MaskOfDeath; _pin.Uses = 50;
                }
                else if ((ss = targeted as InvisibilityScroll) != null ||
                         (bp = targeted as InvisibilityPotion) != null)
                {
                    _pin.Hue = 2103; _pin.Name = "zaklęta szpilka niewidzialności";
                    from.PlaySound(0x1F1);
                    if (ss != null) ss.Consume();
                    else bp.Consume();
                    _pin.Effect = PinEffect.Invisibility; _pin.Uses = 50;
                }
                else if ((ss = targeted as SleepScroll) != null)
                {
                    _pin.Hue = 2150; _pin.Name = "zaklęta szpilka snu";
                    from.PlaySound(0x1F1); ss.Consume();
                    _pin.Effect = PinEffect.Sleep; _pin.Uses = 50;
                }
                else
                {
                    from.SendMessage("Nie możesz tego przypiąć do szpilki!");
                    return;
                }

                from.SendMessage($"Szpilka naładowana: {GetEffectName(_pin.Effect)}, pozostało użyć: {_pin.Uses}.");
                _pin.InvalidateProperties();
            }
        }
        #endregion

        #region PinTarget
        private class PinTarget : Target
        {
            private readonly VoodooPin _pin;

            public PinTarget(VoodooPin pin) : base(12, false, TargetFlags.Harmful)
            {
                _pin = pin;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_pin.Deleted || _pin._doll == null || _pin._doll.Deleted)
                    return;

                if (!(targeted is Mobile origin))
                {
                    from.SendMessage("Musisz wskazać postać lub potwora.");
                    return;
                }

                DateTime now = DateTime.UtcNow;
                PinEffect eff = _pin._effect;
                double elapsed = (now - _pin._lastUseTime).TotalSeconds;

                if (_pin._lastUseEffect == eff)
                {
                    if (elapsed < 5.0)
                    {
                        from.SendMessage($"Musisz odczekać jeszcze {Math.Ceiling(5.0 - elapsed)}s.");
                        return;
                    }
                }
                else if (_pin._lastUseEffect != PinEffect.None)
                {
                    if (elapsed < 7.0)
                    {
                        from.SendMessage($"Musisz odczekać jeszcze {Math.Ceiling(7.0 - elapsed)}s.");
                        return;
                    }
                }

                Mobile[] targets;
                if (origin is PlayerMobile p)
                    targets = new[] { (Mobile)p };
                else if (origin is BaseCreature bc)
                {
                    Type t = bc.GetType();
                    targets = bc.Map.GetMobilesInRange(bc.Location, 5)
                                    .OfType<BaseCreature>()
                                    .Where(x => x.GetType() == t)
                                    .Take(4)
                                    .ToArray<Mobile>();
                }
                else
                {
                    from.SendMessage("Nieprawidłowy cel.");
                    return;
                }

                foreach (var victim in targets)
                {
                    switch (eff)
                    {
                        case PinEffect.Refresh:       _pin.Refresh(from, victim); break;
                        case PinEffect.Agility:       _pin.Agility(from, victim); break;
                        case PinEffect.Strengthen:    _pin.Strengthen(from, victim); break;
                        case PinEffect.Heal:          _pin.Heal(from, victim); break;
                        case PinEffect.Cure:          _pin.Cure(from, victim); break;
                        case PinEffect.Curse:         _pin.Curse(from, victim); break;
                        case PinEffect.Poison:        _pin.Poison(from, victim); break;
                        case PinEffect.Conflagration: _pin.Conflagration(from, victim); break;
                        case PinEffect.Paralyse:      _pin.Paralyse(from, victim); break;
                        case PinEffect.MaskOfDeath:   _pin.MaskOfDeath(from, victim); break;
                        case PinEffect.Invisibility:  _pin.Invisibility(from, victim); break;
                        case PinEffect.Sleep:         _pin.Sleep(from, victim); break;
                        // Disease i Stab mogą wymagać dodatkowej obsługi
                        default:
                            from.SendMessage("Ten efekt nie jest zaimplementowany.");
                            break;
                    }

                    _pin._doll.TimesPoked++;
                }

                _pin._lastUseTime   = now;
                _pin._lastUseEffect = eff;
                _pin._uses--;
                if (_pin._uses <= 0)
                {
                    _pin._effect = PinEffect.None;
                    from.SendMessage("Szpilka utraciła efekt.");
                }

                string effectName = GetEffectName(eff);
                string targetType = (origin is PlayerMobile)
                    ? (targets.Length == 1 ? "gracza" : "graczy")
                    : (targets.Length == 1 ? "potwora" : "potwory");

                from.SendMessage($"Efekt {effectName} zastosowany na {targets.Length} {targetType}.");
                _pin.InvalidateProperties();
            }
        }
        #endregion

        #region Effects
        public void Refresh(Mobile from, Mobile target)
        {
            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x202);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, odświeżają twój wigor.");
        }

        public void Agility(Mobile from, Mobile target)
        {
            target.FixedParticles(0x3779, 9, 32, 5031, EffectLayer.Waist);
            target.PlaySound(0x208);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, zwiększają twoją zręczność.");
        }

        public void Strengthen(Mobile from, Mobile target)
        {
            target.FixedParticles(0x3790, 9, 32, 5032, EffectLayer.Waist);
            target.PlaySound(0x209);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, wzmacniają cię.");
        }

        public void Heal(Mobile from, Mobile target)
        {
            int amount = Utility.Random(1, (int)(from.Skills[SkillName.TasteID].Value / 4));
            target.Heal(amount);
            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x202);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, leczą cię nieco.");
        }

        public void Cure(Mobile from, Mobile target)
        {
            target.CurePoison(from);
            target.FixedParticles(0x3709, 10, 15, 5033, EffectLayer.Waist);
            target.PlaySound(0x1E5);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, oczyszczają twoją krew.");
        }

        public void Curse(Mobile from, Mobile target)
        {
            target.FixedParticles(0x375A, 9, 20, 5034, EffectLayer.Head);
            target.PlaySound(0x204);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, rzucają klątwę na ciebie.");
        }

        public void Poison(Mobile from, Mobile target)
        {
            target.ApplyPoison(from, Server.Poison.Deadly);
            target.FixedParticles(0x38F5, 10, 32, 5035, EffectLayer.Waist);
            target.PlaySound(0x204);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, zatruwają cię.");
        }

        public void Conflagration(Mobile from, Mobile target)
        {
            int damage = Utility.RandomMinMax(5, (int)(from.Skills[SkillName.TasteID].Value / 5));
            AOS.Damage(target, from, damage, 0, 100, 0, 0, 0);
            target.FixedParticles(0x3709, 10, 30, 5036, EffectLayer.Waist);
            target.PlaySound(0x208);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, podpaliły cię.");
        }

        public void Paralyse(Mobile from, Mobile target)
        {
            target.Paralyze(TimeSpan.FromSeconds(5.0));
            target.FixedParticles(0x376A, 9, 32, 5037, EffectLayer.Waist);
            target.PlaySound(0x204);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, paraliżują cię.");
        }

        public void MaskOfDeath(Mobile from, Mobile target)
        {
            int damage = Utility.RandomMinMax(10, (int)(from.Skills[SkillName.TasteID].Value / 3));
            AOS.Damage(target, from, damage, 100, 0, 0, 0, 0);
            target.FixedParticles(0x376A, 9, 32, 5038, EffectLayer.Head);
            target.PlaySound(0x1F1);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, przywołują maskę śmierci.");
        }

        public void Invisibility(Mobile from, Mobile target)
        {
            target.Hidden = true;
            target.FixedParticles(0x3779, 9, 32, 5039, EffectLayer.Head);
            target.PlaySound(0x246);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, czynią cię niewidzialnym.");
        }

        public void Sleep(Mobile from, Mobile target)
        {
            target.Freeze(TimeSpan.FromSeconds(8.0));
            target.PlaySound(0x1FE);
            target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, usypiają cię.");
        }

        private static string GetEffectName(PinEffect eff)
        {
            return eff switch
            {
                PinEffect.Refresh       => "odświeżenie",
                PinEffect.Agility       => "zręczność",
                PinEffect.Strengthen    => "wzmocnienie",
                PinEffect.Heal          => "leczenie",
                PinEffect.Cure          => "oczyszczenie",
                PinEffect.Curse         => "klątwa",
                PinEffect.Poison        => "zatrucie",
                PinEffect.Conflagration => "podpalenie",
                PinEffect.Paralyse      => "paraliż",
                PinEffect.MaskOfDeath   => "maska śmierci",
                PinEffect.Invisibility  => "niewidzialność",
                PinEffect.Sleep         => "sen",
                PinEffect.Disease       => "choroba",
                PinEffect.Stab          => "ukłucie",
                _                       => eff.ToString().ToLower(),
            };
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
            writer.Write((int)_effect);
            writer.Write(_uses);
            writer.Write(_doll);
            writer.Write(_lastUseTime.ToBinary());
            writer.Write((int)_lastUseEffect);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _effect        = (PinEffect)reader.ReadInt();
            _uses          = reader.ReadInt();
            _doll          = reader.ReadItem() as VoodooDoll;
            _lastUseTime   = DateTime.FromBinary(reader.ReadLong());
            _lastUseEffect = (PinEffect)reader.ReadInt();
        }
    }
}
