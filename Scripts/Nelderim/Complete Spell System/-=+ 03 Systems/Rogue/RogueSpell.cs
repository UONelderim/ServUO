#region References

using System;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Rogue
{
	public abstract class RogueSpell : CSpell
	{
		public RogueSpell(Mobile caster, Item scroll, SpellInfo info)
			: base(caster, scroll, info)
		{
		}

		public abstract SpellCircle Circle { get; }

		public override SkillName CastSkill { get { return SkillName.Hiding; } }
		public override SkillName DamageSkill { get { return SkillName.Ninjitsu; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }
		public override bool ClearHandsOnCast { get { return false; } }

		public override void GetCastSkills(out double min, out double max)
		{
			min = RequiredSkill;
			max = RequiredSkill;
		}
		
				public override bool CheckCast()
        		{
        			if (!base.CheckCast())
        				return false;
                    
        			if (Caster is PlayerMobile && !((PlayerMobile)Caster).SpecialSkills.Rogue)
        			{
        				Caster.SendLocalizedMessage(3060182); // Aby korzystac z tych zaklec, musisz wykonac odpowiednie zadanie..
        				return false;
        			}
        
        			return true;
        		}

		public override int GetMana()
		{
			return RequiredMana;
		}

		public override bool ConsumeReagents()
		{
			return true;
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds(CastDelay);
		}
	}
}
