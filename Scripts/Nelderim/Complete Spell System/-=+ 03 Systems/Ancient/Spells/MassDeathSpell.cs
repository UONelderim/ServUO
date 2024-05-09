#region References

using System;
using System.Collections;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientMassDeathSpell : AncientSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Mass Death", "Vas Corp",
			233,
			9012,
			false,
			Reagent.Bloodmoss,
			Reagent.Ginseng,
			Reagent.Garlic,
			Reagent.MandrakeRoot,
			Reagent.Nightshade
		);

		public override SpellCircle Circle => SpellCircle.Eighth;

		public AncientMassDeathSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override bool DelayedDamage => false;

		public override void OnCast()
		{
			if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
			{
				if (this.Scroll != null)
					Scroll.Consume();
				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if (map != null)
				{
					var eable = Caster.GetMobilesInRange(1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0));
					foreach (Mobile m in eable)
					{
						if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) &&
						    Caster.CanBeHarmful(m, false) && (Caster.InLOS(m)))
							targets.Add(m);
					}
					eable.Free();
				}

				Caster.PlaySound(0x309);

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];

					double damage = m.Hits - (m.Hits / 3.0);

					if (!m.Player && damage < 10.0)
						damage = 10.0;
					else if (damage > 100.0)
						damage = 100.0;

					Caster.DoHarmful(m);
					SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0);
					m.Kill();
				}
			}

			FinishSequence();
		}
	}
}
