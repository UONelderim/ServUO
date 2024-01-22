#region References

using System;
using System.Collections.Generic;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerHuntersAimSpell : RangerSpell
	{
		private static readonly SpellInfo m_Info = new (
			"Celność łowcy", "Cu Ner Sinta",
			212,
			9041,
			Reagent.Nightshade,
			CReagent.SpringWater,
			Reagent.Bloodmoss
		);

		public override SpellCircle Circle => SpellCircle.Fourth;
		public override double CastDelay => 3.0;
		public override int RequiredMana => 25;
		public override double RequiredSkill => 50;

		private static readonly List<Mobile> _Table = new();

		public RangerHuntersAimSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence() && Caster.BeginAction(typeof(RangerHuntersAimSpell)))
			{
				var scalar = Caster.Skills[DamageSkill].Value / 100;
				var duration = TimeSpan.FromMinutes(2 * scalar);
				Caster.AddStatMod(new(StatType.Dex, "[Ranger] Dex Offset", 5, duration));
				Caster.AddStatMod(new(StatType.Str, "[Ranger] Str Offset", 5, duration));
				Caster.AddSkillMod(new TimedSkillMod(SkillName.Archery, true, 20, duration));
				Caster.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, 20, duration));

				Timer.DelayCall(duration, () =>
				{
					_Table.Remove(Caster);
					Caster.EndAction(typeof(RangerHuntersAimSpell));
				});
			}
			else
			{
				Caster.SendLocalizedMessage(1005559);
			}
		}
	}
}
