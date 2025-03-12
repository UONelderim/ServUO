#region References

using System;
using System.Collections;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardEnchantingEtudeSpell : BardSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Wzmacniająca Etiuda", "Enchantendre",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 3;
		public override double RequiredSkill => 80.0;
		public override int RequiredMana => 40;

		public BardEnchantingEtudeSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				ArrayList targets = new ArrayList();

				var eable = Caster.GetMobilesInRange(3);
				foreach (Mobile m in eable)
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}
				eable.Free();

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];

					int amount = (int)(Caster.Skills[CastSkill].Base * 0.1);
					string intt = "int";

					double duration = (Caster.Skills[SkillName.Musicianship].Base * 5.0);

					StatMod mod = new StatMod(StatType.Int, intt, +amount, TimeSpan.FromSeconds(duration));

					m.AddStatMod(mod);

					if (m.Hidden == false)
					{
						m.FixedParticles(0x375A, 10, 15, 5017, 0x1F8, 3, EffectLayer.Waist);
					}
				}
			}

			FinishSequence();
		}
	}
}
