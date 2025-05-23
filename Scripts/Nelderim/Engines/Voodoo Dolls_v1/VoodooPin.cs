using System;
using System.Drawing;
using System.Linq;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Gumps;
using Server.Engines.Craft;
using Server.Spells;

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
		Sleep
	}

	public class VoodooPin : Item
	{
		private VoodooDoll _doll;
		private PinEffect _effect;
		private int _uses;

		// Delay info
		private DateTime _lastUseTime;
		private PinEffect _lastUseEffect;

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
			Weight = 0.1;
			Name = "Szpilka guslarza";
			Hue = 2783;
			_effect = PinEffect.None;
			_uses = 0;
			_lastUseTime = DateTime.MinValue;
			_lastUseEffect = PinEffect.None;
		}

		public VoodooPin(Serial serial) : base(serial) { }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			if (_effect != PinEffect.None)
				list.Add($"Efekt: {_effect}, Pozostało użyć: {_uses}");
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

			// 1) bind
			if (_doll == null || _doll.Deleted)
			{
				from.SendMessage("Wskaż laleczkę, do której chcesz powiązać tę szpilkę.");
				from.Target = new BindTarget(this);
				return;
			}

			// 2) load
			if (_effect == PinEffect.None)
			{
				if (_uses > 0)
				{
					from.SendMessage("Szpilka już naładowana i posiada jeszcze użycia.");
					return;
				}
				from.SendMessage("Wybierz miksturę lub zwój, aby naładować szpilkę.");
				from.Target = new LoadTarget(this);
				return;
			}

			// 3) use
			if (_uses <= 0)
			{
				_effect = PinEffect.None;
				from.SendMessage("Szpilka straciła efekt i wymaga ponownego załadowania.");
				return;
			}

			from.SendMessage("Wskaż cel, na którym chcesz użyć efektu szpilki.");
			from.Target = new PinTarget(this);
		}

		#region BindTarget
		private class BindTarget : Target
		{
			private readonly VoodooPin _pin;
			public BindTarget(VoodooPin pin) : base(1, false, TargetFlags.None) { _pin = pin; }
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (_pin.Deleted) return;
				if (targeted is VoodooDoll doll && !doll.Deleted)
				{
					_pin._doll = doll;
					from.SendMessage($"Szpilka powiązana z laleczką {doll.CursedPerson?.Name ?? "?"}.");
				}
				else if (targeted is Doll blood && !blood.Deleted)
				{
					Mobile cursed = blood.CursedPerson;
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
			public LoadTarget(VoodooPin pin) : base(1, false, TargetFlags.None) { _pin = pin; }
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
					_pin._effect = PinEffect.Refresh; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterAgilityPotion) != null || (bp = targeted as NGreaterAgilityPotion) != null || (bp = targeted as AgilityPotion) != null)
				{
					_pin.Hue = 2620; _pin.Name = "zaklęta szpilka zręczności";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Agility; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterStrengthPotion) != null || (bp = targeted as NGreaterStrengthPotion) != null || (bp = targeted as StrengthPotion) != null)
				{
					_pin.Hue = 2701; _pin.Name = "zaklęta szpilka wzmocnienia";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Strengthen; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterHealPotion) != null || (bp = targeted as HealPotion) != null || (bp = targeted as LesserHealPotion) != null)
				{
					_pin.Hue = 2997; _pin.Name = "zaklęta szpilka leczenia";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Heal; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterCurePotion) != null || (bp = targeted as CurePotion) != null || (bp = targeted as LesserCurePotion) != null)
				{
					_pin.Hue = 1761; _pin.Name = "zaklęta szpilka oczyszczenia";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Cure; _pin._uses = 50;
				}
				else if ((ss = targeted as CurseScroll) != null)
				{
					_pin.Hue = 1154; _pin.Name = "zaklęta szpilka klątwy";
					from.PlaySound(0x1F2); ss.Consume();
					_pin._effect = PinEffect.Curse; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterPoisonPotion) != null || (bp = targeted as PoisonPotion) != null || (bp = targeted as LesserPoisonPotion) != null || (bp = targeted as DeadlyPoisonPotion) != null)
				{
					_pin.Hue = 2857; _pin.Name = "zaklęta szpilka zatrucia";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Poison; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterConflagrationPotion) != null || (bp = targeted as ConflagrationPotion) != null)
				{
					_pin.Hue = 2197; _pin.Name = "zaklęta szpilka podpalenia";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.Conflagration; _pin._uses = 50;
				}
				else if ((ss = targeted as ParalyzeScroll) != null)
				{
					_pin.Hue = 1342; _pin.Name = "zaklęta szpilka paraliżu";
					from.PlaySound(0x1F2); ss.Consume();
					_pin._effect = PinEffect.Paralyse; _pin._uses = 50;
				}
				else if ((bp = targeted as GreaterMaskOfDeathPotion) != null || (bp = targeted as MaskOfDeathPotion) != null)
				{
					_pin.Hue = 1166; _pin.Name = "zaklęta szpilka maski śmierci";
					from.PlaySound(0x240); from.AddToBackpack(new Bottle()); bp.Consume();
					_pin._effect = PinEffect.MaskOfDeath; _pin._uses = 50;
				}
				else if ((ss = targeted as InvisibilityScroll) != null || (bp = targeted as InvisibilityPotion) != null)
				{
					_pin.Hue = 2103; _pin.Name = "zaklęta szpilka niewidzialności";
					from.PlaySound(0x1F1);
					if (ss != null) ss.Consume(); else bp.Consume();
					_pin._effect = PinEffect.Invisibility; _pin._uses = 50;
				}
				else if ((ss = targeted as SleepScroll) != null)
				{
					_pin.Hue = 2150; _pin.Name = "zaklęta szpilka snu";
					from.PlaySound(0x1F1); ss.Consume();
					_pin._effect = PinEffect.Sleep; _pin._uses = 50;
				}
				else
				{
					from.SendMessage("Nie możesz tego przypiąć do szpilki!");
					return;
				}

				from.SendMessage($"Szpilka naładowana: {_pin._effect}, użyć pozostało: {_pin._uses}.");
				_pin.InvalidateProperties();
			}
		}
		
		private static string GetEffectName(PinEffect eff)
		{
			switch (eff)
			{
				case PinEffect.Refresh:       return "odświeżenie";
				case PinEffect.Agility:       return "zręczność";
				case PinEffect.Strengthen:    return "wzmocnienie";
				case PinEffect.Heal:          return "leczenie";
				case PinEffect.Cure:          return "oczyszczenie";
				case PinEffect.Curse:         return "klątwa";
				case PinEffect.Poison:        return "zatrucie";
				case PinEffect.Conflagration: return "podpalenie";
				case PinEffect.Paralyse:      return "paraliż";
				case PinEffect.MaskOfDeath:   return "maska śmierci";
				case PinEffect.Invisibility:  return "niewidzialność";
				case PinEffect.Sleep:         return "sen";
				default:                      return eff.ToString().ToLower();
			}
		}
		#endregion

		#region PinTarget
		private class PinTarget : Target
		{
			private readonly VoodooPin _pin;
			public PinTarget(VoodooPin pin) : base(12, false, TargetFlags.Harmful) { _pin = pin; }
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (_pin.Deleted || _pin._doll == null || _pin._doll.Deleted) return;
				if (!(targeted is Mobile origin)) { from.SendMessage("Musisz wskazać postać lub potwora."); return; }

				DateTime now = DateTime.UtcNow;
				PinEffect eff = _pin._effect;
				double elapsed = (now - _pin._lastUseTime).TotalSeconds;

				if (_pin._lastUseEffect == eff)
				{
					if (elapsed < 5.0) { from.SendMessage($"Musisz odczekać jeszcze {Math.Ceiling(5.0 - elapsed)}s."); return; }
				}
				else if (_pin._lastUseEffect != PinEffect.None)
				{
					if (elapsed < 7.0) { from.SendMessage($"Musisz odczekać jeszcze {Math.Ceiling(7.0 - elapsed)}s."); return; }
				}

				Mobile[] targets;
				if (origin is PlayerMobile p) targets = new[] { (Mobile)p };
				else if (origin is BaseCreature bc)
				{
					Type t = bc.GetType();
					targets = bc.Map.GetMobilesInRange(bc.Location, 5)
						.OfType<BaseCreature>()
						.Where(x => x.GetType() == t)
						.Take(4)
						.ToArray<Mobile>();
				}
				else { from.SendMessage("Nieprawidłowy cel."); return; }

				foreach (var victim in targets)
				{
					switch (eff)
					{
						case PinEffect.Refresh: _pin.Refresh(from, victim); break;
						case PinEffect.Agility: _pin.Agility(from, victim); break;
						case PinEffect.Strengthen: _pin.Strengthen(from, victim); break;
						case PinEffect.Heal: _pin.Heal(from, victim); break;
						case PinEffect.Cure: _pin.Cure(from, victim); break;
						case PinEffect.Curse: _pin.Curse(from, victim); break;
						case PinEffect.Poison: _pin.Poison(from, victim); break;
						case PinEffect.Conflagration: _pin.Conflagration(from, victim); break;
						case PinEffect.Paralyse: _pin.Paralyse(from, victim); break;
						case PinEffect.MaskOfDeath: _pin.MaskOfDeath(from, victim); break;
						case PinEffect.Invisibility: _pin.Invisibility(from, victim); break;
						case PinEffect.Sleep: _pin.Sleep(from, victim); break;
					}
					_pin._doll.TimesPoked++;
				}

				_pin._lastUseTime = now;
				_pin._lastUseEffect = eff;
				_pin._uses--;
				if (_pin._uses <= 0)
				{
					_pin._effect = PinEffect.None;
					from.SendMessage("Szpilka utraciła efekt.");
				}
				// Pobieramy czytelną nazwę efektu
				string effectName = GetEffectName(eff);

				// Deklinujemy typ celu: dla gracza jednorazowo "gracza", wielokrotnie "graczy",
				// dla mobów jednorazowo "potwora", wielokrotnie "potwory"
				string targetType;
				if (origin is PlayerMobile)
					targetType = (targets.Length == 1 ? "gracza" : "graczy");
				else
					targetType = (targets.Length == 1 ? "potwora" : "potwory");

				// Wysyłamy komunikat
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
			target.SendMessage($"Tajemnicze siły, kierowane przez {from.Name}, odświeżają twoją wigor.");
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
        
		/// <summary>
		/// Zwraca czytelną, polską nazwę efektu szpilki.
		/// </summary>

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
			_effect = (PinEffect)reader.ReadInt();
			_uses = reader.ReadInt();
			_doll = reader.ReadItem() as VoodooDoll;
			if (version >= 1)
			{
				_lastUseTime = DateTime.FromBinary(reader.ReadLong());
				_lastUseEffect = (PinEffect)reader.ReadInt();
			}
		}
	}
}
