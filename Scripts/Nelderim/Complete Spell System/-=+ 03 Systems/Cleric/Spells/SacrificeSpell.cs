#region References

using Server.Items;
using Server.Spells;
using PSys = Server.Engines.PartySystem;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSacrificeSpell : ClericSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Poświęcenie", "Adoleo",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override int RequiredTithing => 5;
		public override double RequiredSkill => 5.0;

		public override int RequiredMana => 4;

		public static void Initialize()
		{
			PlayerEvent.HitByWeapon += InternalCallback;
		}

		public ClericSacrificeSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				if (!Caster.CanBeginAction(typeof(ClericSacrificeSpell)))
				{
					Caster.EndAction(typeof(ClericSacrificeSpell));
					Caster.PlaySound(0x244);
					Caster.FixedParticles(0x3709, 1, 30, 9965, 1152, 0, EffectLayer.Waist);
					Caster.FixedParticles(0x376A, 1, 30, 9502, 1152, 0, EffectLayer.Waist);
					Caster.SendMessage("Przestajesz poświęcać się dla innych.");
				}
				else
				{
					Caster.BeginAction(typeof(ClericSacrificeSpell));
					Caster.FixedParticles(0x3709, 1, 30, 9965, 1153, 7, EffectLayer.Waist);
					Caster.FixedParticles(0x376A, 1, 30, 9502, 1153, 3, EffectLayer.Waist);
					Caster.PlaySound(0x244);
					Caster.SendMessage("Zaczynasz poświęcać się dla innych.");
				}
			}

			FinishSequence();
		}

		private static void InternalCallback(Mobile attacker, Mobile defender, int damage, WeaponAbility a)
		{
			if (!defender.CanBeginAction(typeof(ClericSacrificeSpell)))
			{
				PSys.Party p = PSys.Party.Get(defender);

				if (p != null)
				{
					foreach (PSys.PartyMemberInfo info in p.Members)
					{
						Mobile m = info.Mobile;

						if (m != defender && m != attacker && !m.Poisoned)
						{
							m.Heal(damage / 3);
							m.PlaySound(0x202);
							m.FixedParticles(0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist);
							m.FixedParticles(0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist);
						}
					}
				}
			}
		}
	}
}
