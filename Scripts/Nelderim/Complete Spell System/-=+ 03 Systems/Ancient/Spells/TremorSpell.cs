#region References

using System;
using System.Collections;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientTremorSpell : AncientSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Tremor", "Vas Por Ylem",
			233,
			9012,
			false,
			Reagent.Bloodmoss,
			Reagent.MandrakeRoot,
			Reagent.SulfurousAsh
		);

		public override SpellCircle Circle => SpellCircle.Sixth;

		public AncientTremorSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override bool DelayedDamage => false;

		public override void OnCast()
		{
			if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
			{
				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				if (map != null)
				{
					var eable = Caster.GetMobilesInRange(1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0));
					foreach (Mobile m in eable)
					{
						if (Caster != m && m.AccessLevel == AccessLevel.Player &&
						    SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) &&
						    Caster.InLOS(m))
							targets.Add(m);
					}
					eable.Free();
				}

				Caster.PlaySound(0x2F3);

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];

					m.Stam = 0;
					m.Mana = 0;
					m.SendMessage("A powerful tremor in the ground makes you stumble and tremble!");
					int damage = (Utility.Random(10, 15));
					Caster.DoHarmful(m);
					SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0);
				}
			}

			FinishSequence();
		}
	}
}
