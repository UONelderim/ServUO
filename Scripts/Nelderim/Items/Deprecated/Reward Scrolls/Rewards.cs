using System;
using System.Collections;

namespace Server.Items
{
	public abstract class NReward : IComparable
	{
		private int m_Value;
		public int Value => m_Value;

		public NReward(int value)
		{
			m_Value = value;
		}

		public int CompareTo(object o)
		{
			if (o == null)
				return 1;

			if (!(o is NReward))
				throw new ArgumentException();

			var r = (NReward)o;

			if (Value > r.Value)
				return 1;

			if (Value < r.Value)
				return -1;

			return 0;
		}

		public abstract Item Generate(Mobile from);
	}

	public class PowerScrollReward : NReward
	{
		private int m_PSClass;

		public PowerScrollReward(int PSclass, int value) : base(value)
		{
			m_PSClass = PSclass;
		}

		public override Item Generate(Mobile from)
		{
			var ps = PowerScroll.CreateRandomNoCraft(m_PSClass, m_PSClass);
			ps.LootType = LootType.Cursed;
			from.SendLocalizedMessage(1049524); // You have received a scroll of power!
			return ps;
		}
	}

	public class PowderOfTranslocationReward : NReward
	{
		public PowderOfTranslocationReward(int value) : base(value)
		{
		}

		public override Item Generate(Mobile from)
		{
			var ps = new PowderOfTranslocation(5);


			return ps;
		}
	}

	public class PowderOfTemperamentReward : NReward
	{
		public PowderOfTemperamentReward(int value) : base(value)
		{
		}

		public override Item Generate(Mobile from)
		{
			var ps = new PowderOfTemperament(5);


			return ps;
		}
	}

	public class BallOfSummoningReward : NReward
	{
		public BallOfSummoningReward(int value) : base(value)
		{
		}

		public override Item Generate(Mobile from)
		{
			var ps = new BallOfSummoning();


			return ps;
		}
	}

	public class DedicatedPowerScrollReward : NReward
	{
		private int m_PSClass;

		public DedicatedPowerScrollReward(int PSclass, int value) : base(value)
		{
			m_PSClass = PSclass;
		}

		public override Item Generate(Mobile from)
		{
			var candidates = new ArrayList();
			var ps = PowerScroll.CreateRandomNoCraft(m_PSClass, m_PSClass);

			for (var i = 0; i < PowerScroll.Skills.Count; i++)
			{
				var skillName = PowerScroll.Skills[i];
				if (skillName == SkillName.Tailoring || skillName == SkillName.Blacksmith ||
				    skillName == SkillName.Fletching) continue;
				var skill = from.Skills[skillName];

				if (skill.Lock == SkillLock.Up && skill.Cap < 100 + m_PSClass)
					candidates.Add(skill);
			}

			if (candidates.Count > 0)
			{
				var s = candidates[Utility.Random(candidates.Count)] as Skill;
				ps = new PowerScroll(s.SkillName, 100 + m_PSClass);
			}

			ps.LootType = LootType.Cursed;
			from.SendLocalizedMessage(1049524); // You have received a scroll of power!
			return ps;
		}
	}

	public class MinorArtifactScrollReward : NReward
	{
		private bool m_Chosen;

		public MinorArtifactScrollReward(bool chosen, int value) : base(value)
		{
			m_Chosen = chosen;
		}

		public override Item Generate(Mobile from)
		{
			var mrs = new MinorArtifactRewardScroll(m_Chosen);
			mrs.LootType = LootType.Cursed;
			return mrs;
		}
	}

	public class MajorArtifactScrollReward : NReward
	{
		private bool m_Chosen;

		public MajorArtifactScrollReward(bool chosen, int value) : base(value)
		{
			m_Chosen = chosen;
		}

		public override Item Generate(Mobile from)
		{
			var mrs = new MajorArtifactRewardScroll(m_Chosen);
			mrs.LootType = LootType.Cursed;
			return mrs;
		}
	}
}
