using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
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

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(0.5);
		public override int RequiredTithing => 42;
		public override double RequiredSkill => 30.0;
		public override bool BlocksMovement => false;
		public override int RequiredMana => 28;

		public GrimReaperSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				Caster.PlaySound(0x0F5);
				Caster.PlaySound(0x1ED);
				Caster.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
				Caster.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);

				double delaySeconds = (double)ComputePowerValue(1) / 60;

				// TODO: Should caps be applied?
				if (delaySeconds < 1.5)
					delaySeconds = 1.5;
				else if (delaySeconds > 3.5)
					delaySeconds = 3.5;

				TimeSpan delay = TimeSpan.FromSeconds(delaySeconds);

				Timer timer = Timer.DelayCall(delay, EnemyOfOneSpell.RemoveEffect, Caster);

				DateTime expire = DateTime.UtcNow + delay;

				EnemyOfOneContext context = new EnemyOfOneContext(Caster, timer, expire);
				context.OnCast();
				EnemyOfOneSpell.AddContext(Caster, context);

				BuffInfo.AddBuff(Caster,
					new BuffInfo(BuffIcon.EnemyOfOne, 1044119, 1044118, TimeSpan.FromMinutes(delaySeconds), Caster));
			}

			FinishSequence();
		}
	}
}
