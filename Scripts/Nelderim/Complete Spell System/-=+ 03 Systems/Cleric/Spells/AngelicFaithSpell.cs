#region References

using System;
using System.Collections;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericAngelicFaithSpell : ClericSpell, ITransformationSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Anielska Wiara", "Angelus Terum",
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.Eighth;

		public override int RequiredTithing => 100;
		public override double RequiredSkill => 80.0;

		public override int RequiredMana => 50;

		private static readonly Hashtable m_Table = new Hashtable();

		public ClericAngelicFaithSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public int Body => 123;

		public int Hue => 2705;

		public int PhysResistOffset => 0;

		public int FireResistOffset => 0;
		public int ColdResistOffset => 0;
		public int PoisResistOffset => 0;
		public int NrgyResistOffset => 0;

		public double TickRate => Caster.Skills[DamageSkill].Value / 10 * 60;

		public void OnTick(Mobile m)
		{
			RemoveEffect(m);
		}

		public void DoEffect(Mobile m)
		{
			object[] mods =
			{
				new StatMod(StatType.Str, "[Cleric] Str Offset", 10, TimeSpan.Zero),
				new StatMod(StatType.Dex, "[Cleric] Dex Offset", 10, TimeSpan.Zero),
				new StatMod(StatType.Int, "[Cleric] Int Offset", 10, TimeSpan.Zero),
				new DefaultSkillMod(SkillName.Anatomy, true, 20)
			};

			m_Table[Caster] = mods;

			Caster.AddStatMod((StatMod)mods[0]);
			Caster.AddStatMod((StatMod)mods[1]);
			Caster.AddStatMod((StatMod)mods[2]);
			Caster.AddSkillMod((SkillMod)mods[3]);

			Caster.PlaySound(0x165);
			Caster.FixedParticles(0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head);
		}

		public void RemoveEffect(Mobile m)
		{
			object[] mods = (object[])m_Table[m];

			if (mods != null)
			{
				m.RemoveStatMod(((StatMod)mods[0]).Name);
				m.RemoveStatMod(((StatMod)mods[1]).Name);
				m.RemoveStatMod(((StatMod)mods[2]).Name);
				m.RemoveSkillMod((SkillMod)mods[3]);
			}

			m_Table.Remove(m);
		}

		public override bool CheckCast()
		{
			if (!TransformationSpellHelper.CheckCast(Caster, this))
				return false;

			return base.CheckCast();
		}

		public override void OnCast()
		{
			TransformationSpellHelper.OnCast(Caster, this);

			FinishSequence();
		}
	}
}
