#region References

using System;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidPackOfBeastSpell : DruidSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Leśne Bestyje", "En Sec Ohm Ess Sepa",
			//SpellCircle.Third,
			266,
			9040,
			false,
			Reagent.BlackPearl,
			Reagent.Bloodmoss,
			CReagent.PetrafiedWood
		);

		public override SpellCircle Circle => SpellCircle.Third;

		public override double CastDelay => 1.0;
		public override double RequiredSkill => 40.0;
		public override int RequiredMana => 45;

		public DruidPackOfBeastSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		private static readonly Type[] m_Types =
		{
			typeof(BrownBear), typeof(TimberWolf), typeof(Panther), typeof(GreatHart), typeof(Hind),
			typeof(Alligator), typeof(Boar), typeof(GiantRat)
		};

		public override void OnCast()
		{
			if (CheckSequence())
			{
				try
				{
					Type beasttype = (m_Types[Utility.Random(m_Types.Length)]);

					BaseCreature creaturea = (BaseCreature)Activator.CreateInstance(beasttype);
					BaseCreature creatureb = (BaseCreature)Activator.CreateInstance(beasttype);
					BaseCreature creaturec = (BaseCreature)Activator.CreateInstance(beasttype);
					BaseCreature creatured = (BaseCreature)Activator.CreateInstance(beasttype);


					SpellHelper.Summon(creaturea, Caster, 0x215,
						TimeSpan.FromSeconds(4.0 * Caster.Skills[CastSkill].Value), false, false);
					SpellHelper.Summon(creatureb, Caster, 0x215,
						TimeSpan.FromSeconds(4.0 * Caster.Skills[CastSkill].Value), false, false);

					double morebeast = 0;

					morebeast = Utility.Random(10) + (Caster.Skills[CastSkill].Value * 0.1);


					if (morebeast > 11)
					{
						SpellHelper.Summon(creaturec, Caster, 0x215,
							TimeSpan.FromSeconds(4.0 * Caster.Skills[CastSkill].Value), false, false);
					}

					if (morebeast > 18)
					{
						SpellHelper.Summon(creatured, Caster, 0x215,
							TimeSpan.FromSeconds(4.0 * Caster.Skills[CastSkill].Value), false, false);
					}
				}
				catch
				{
				}
			}

			FinishSequence();
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds(7.5);
		}
	}
}
