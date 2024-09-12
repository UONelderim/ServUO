#region References

using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public abstract class ClericSpell : CSpell
	{
		public abstract SpellCircle Circle { get; }

		public ClericSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
		{
		}

		public override SkillName CastSkill { get { return SkillName.Healing; } }
		public override SkillName DamageSkill { get { return SkillName.Anatomy; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }
		public override bool ClearHandsOnCast { get { return false; } }

		public override int GetMana()
		{
			return RequiredMana;
		}

		public override bool CheckCast()
		{
			if (!base.CheckCast())
				return false;
			
						
			if (Caster is PlayerMobile && !((PlayerMobile)Caster).SpecialSkills.Cleric)
			{
				Caster.SendLocalizedMessage(3060182); // Aby korzystac z tych zaklec, musisz wykonac odpowiednie zadanie..
				return false;
			}

			if (Caster.Skills[CastSkill].Value < RequiredSkill)
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + RequiredSkill +
				                   " Leczenia by aktywować tę umiejętnosć.");
				return false;
			}

			if (Caster.TithingPoints < RequiredTithing)
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + RequiredTithing +
				                   " dziesięciny by aktywować tę umiejętnosć.");
				return false;
			}

			if (Caster.Mana < ScaleMana(GetMana()))
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + GetMana() + " Many by aktywować tę umiejętnosć.");
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			if (!base.CheckFizzle())
				return false;

			int tithing = RequiredTithing;
			double min, max;

			GetCastSkills(out min, out max);

			if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
				tithing = 0;

			int mana = ScaleMana(GetMana());

			if (Caster.Skills[CastSkill].Value < RequiredSkill)
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + RequiredSkill +
				                   " dziesięciny by aktywować tę umiejętnosć..");
				return false;
			}

			if (Caster.TithingPoints < tithing)
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + tithing + " dziesięciny by aktywować tę umiejętnosć.");
				return false;
			}

			if (Caster.Mana < mana)
			{
				Caster.SendMessage("Musisz mieć przynajmniej " + mana + " Many by aktywować tę umiejętnosć.");
				return false;
			}

			Caster.TithingPoints -= tithing;

			return true;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, Info.Mantra);
			Caster.PlaySound(0x24A);
		}

		public override void DoFizzle()
		{
			Caster.PlaySound(0x1D6);
			Caster.NextSpellTime = Core.TickCount;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound(0x1D6);
		}

		public override void OnDisturb(DisturbType type, bool message)
		{
			base.OnDisturb(type, message);

			if (message)
				Caster.PlaySound(0x1D6);
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect(0x37C4, 10, 42, 4, 3);
		}

		public override void GetCastSkills(out double min, out double max)
		{
			min = RequiredSkill;
			max = RequiredSkill + 40.0;
		}
	}
}
