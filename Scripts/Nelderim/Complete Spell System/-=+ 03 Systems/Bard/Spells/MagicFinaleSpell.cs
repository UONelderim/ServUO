#region References

using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagicFinaleSpell : BardSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Magiczny Finał", "Dispersus",
			//SpellCircle.First,
			212, 9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 3;
		public override double RequiredSkill => 80.0;
		public override int RequiredMana => 15;

		public BardMagicFinaleSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				ArrayList targets = new ArrayList();

				var eable = Caster.GetMobilesInRange(4);
				foreach (Mobile m in eable)
				{
					if (m is BaseCreature && ((BaseCreature)m).Summoned)
						targets.Add(m);
				}
				eable.Free();

				Caster.FixedParticles(0x3709, 1, 30, 9965, 5, 7, EffectLayer.Waist);

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];

					Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
						0x3728, 8, 20, 5042);

					m.Delete();
				}
			}

			FinishSequence();
		}
	}
}
