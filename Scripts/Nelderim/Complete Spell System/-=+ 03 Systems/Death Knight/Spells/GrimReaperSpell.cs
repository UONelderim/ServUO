using System;
using Server.Spells.Chivalry;

namespace Server.Spells.DeathKnight
{
	public class GrimReaperSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Ponury Zniwiarz", "Astaroth Mortem",
				-1,
				9002
			);

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds( 0.5 );
		public override double RequiredSkill => 30.0;
		public override int RequiredMana => 28;
		public override int RequiredTithing => 42;
		public override bool BlocksMovement => false;

		public GrimReaperSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			TimeSpan delay = base.GetCastDelay();

			if (EnemyOfOneSpell.UnderEffect(Caster))
			{
				double milliseconds = delay.TotalMilliseconds / 2;

				delay = TimeSpan.FromMilliseconds(milliseconds);
			}

			return delay;
		}

		public override void OnCast()
		{
			if (EnemyOfOneSpell.UnderEffect(Caster))
			{
				PlayEffects();

				// As per Pub 71, Enemy of one has now been changed to a Spell Toggle. You can remove the effect
				// before the duration expires by recasting the spell.
				EnemyOfOneSpell.RemoveEffect(Caster);
			}
			else if (CheckSequence())
			{
				PlayEffects();

				// TODO: validate formula
				int seconds = ComputePowerValue(1);
				Utility.FixMinMax(ref seconds, 67, 228);

				TimeSpan delay = TimeSpan.FromSeconds(seconds);

				Timer timer = Timer.DelayCall(delay, EnemyOfOneSpell.RemoveEffect, Caster);

				DateTime expire = DateTime.UtcNow + delay;

				EnemyOfOneContext context = new EnemyOfOneContext(Caster, timer, expire);
				context.OnCast();
				EnemyOfOneSpell.AddContext(Caster, context);
			}

			FinishSequence();
		}

		private void PlayEffects()
		{
			Caster.PlaySound(0x0F5);
			Caster.PlaySound(0x1ED);

			Caster.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
			Caster.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);
		}
	}
}
