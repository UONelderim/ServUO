#region References

using Server.Spells;
using Server.Spells.Necromancy;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericPurgeSpell : ClericSpell
	{
		private static readonly SpellInfo m_Info = new (
			"Czystka", "Repurgo",
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.Sixth;
		public override int RequiredTithing => 5;
		public override double RequiredSkill => 70.0;
		public override int RequiredMana => 20;

		public ClericPurgeSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			Caster.BeginTarget(12,
				true,
				TargetFlags.None,
				(_, o) =>
				{
					if (o is Mobile)
					{
						Target((Mobile)o);
					}
				});
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (CheckBSequence(m, false))
			{
				SpellHelper.Turn(Caster, m);

				m.PlaySound(0xF6);
				m.PlaySound(0x1F7);
				m.FixedParticles(0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head);

				IEntity from = new Entity(Serial.Zero, new Point3D(m.X, m.Y, m.Z - 10), Caster.Map);
				IEntity to = new Entity(Serial.Zero, new Point3D(m.X, m.Y, m.Z + 50), Caster.Map);
				Effects.SendMovingParticles(from, to, 0x2255, 1, 0, false, false, 13, 3, 9501, 1, 0, EffectLayer.Head,
					0x100);

				StatMod mod;

				mod = m.GetStatMod("[Magic] Str Curse");
				if (mod != null && mod.Offset < 0)
					m.RemoveStatMod("[Magic] Str Curse");

				mod = m.GetStatMod("[Magic] Dex Curse");
				if (mod != null && mod.Offset < 0)
					m.RemoveStatMod("[Magic] Dex Curse");

				mod = m.GetStatMod("[Magic] Int Curse");
				if (mod != null && mod.Offset < 0)
					m.RemoveStatMod("[Magic] Int Curse");


				m.Paralyzed = false;
				
				EvilOmenSpell.TryEndEffect(m);
				StrangleSpell.RemoveCurse(m);
				BloodOathSpell.RemoveCurse(m);
				MindRotSpell.ClearMindRotScalar(m);
				CorpseSkinSpell.RemoveCurse(m);

				BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
				BuffInfo.RemoveBuff(m, BuffIcon.FeebleMind);
				BuffInfo.RemoveBuff(m, BuffIcon.Weaken);
				BuffInfo.RemoveBuff(m, BuffIcon.Curse);
				BuffInfo.RemoveBuff(m, BuffIcon.MassCurse);
				BuffInfo.RemoveBuff(m, BuffIcon.Mindrot);
			}

			FinishSequence();
		}
	}
}
