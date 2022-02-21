#region References

using System;
using System.Collections;

#endregion

namespace Server.Items
{
	public abstract class NReward : IComparable
	{
		public int Value { get; }

		public NReward(int value)
		{
			Value = value;
		}

		public int CompareTo(object o)
		{
			if (o == null)
				return 1;

			if (!(o is NReward))
				throw new ArgumentException();

			NReward r = (NReward)o;

			if (this.Value > r.Value)
				return 1;

			if (this.Value < r.Value)
				return -1;

			return 0;
		}

		public abstract Item Generate(Mobile from);
	}

	public class PowerScrollReward : NReward
	{
		private readonly int m_PSClass;

		public PowerScrollReward(int PSclass, int value) : base(value)
		{
			m_PSClass = PSclass;
		}

		public override Item Generate(Mobile from)
		{
			PowerScroll ps = PowerScroll.CreateRandomNoCraft(m_PSClass, m_PSClass);
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
			PowderOfTranslocation ps = new PowderOfTranslocation(5);


			return ps;
		}
	}

	/*public class SilverReward : NReward
    {


        public SilverReward( int value ) : base( value )
        {

        }
    
        public override Item Generate( Mobile from )
        {
            Silver ps = new Silver( 1 );


            return ps;
        }
    }*/
	public class PowderOfTemperamentReward : NReward
	{
		public PowderOfTemperamentReward(int value) : base(value)
		{
		}

		public override Item Generate(Mobile from)
		{
			PowderOfTemperament ps = new PowderOfTemperament(5);


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
			BallOfSummoning ps = new BallOfSummoning();


			return ps;
		}
	}

	public class DedicatedPowerScrollReward : NReward
	{
		private readonly int m_PSClass;

		public DedicatedPowerScrollReward(int PSclass, int value) : base(value)
		{
			m_PSClass = PSclass;
		}

		public override Item Generate(Mobile from)
		{
			ArrayList candidates = new ArrayList();
			PowerScroll ps = PowerScroll.CreateRandomNoCraft(m_PSClass, m_PSClass);

			for (int i = 0; i < PowerScroll.Skills.Count; i++)
			{
				SkillName skillName = PowerScroll.Skills[i];
				if (skillName == SkillName.Tailoring || skillName == SkillName.Blacksmith ||
				    skillName == SkillName.Fletching) continue;
				Skill skill = from.Skills[skillName];

				if (skill.Lock == SkillLock.Up && skill.Cap < 100 + m_PSClass)
					candidates.Add(skill);
			}

			if (candidates.Count > 0)
			{
				Skill s = candidates[Utility.Random(candidates.Count)] as Skill;
				ps = new PowerScroll(s.SkillName, 100 + m_PSClass);
			}

			ps.LootType = LootType.Cursed;
			from.SendLocalizedMessage(1049524); // You have received a scroll of power!
			return ps;
		}
	}

	public class MinorArtifactScrollReward : NReward
	{
		private readonly bool m_Chosen;

		public MinorArtifactScrollReward(bool chosen, int value) : base(value)
		{
			m_Chosen = chosen;
		}

		public override Item Generate(Mobile from)
		{
			MinorArtifactRewardScroll mrs = new MinorArtifactRewardScroll(m_Chosen);
			mrs.LootType = LootType.Cursed;
			return mrs;
		}
	}

	public class MajorArtifactScrollReward : NReward
	{
		private readonly bool m_Chosen;

		public MajorArtifactScrollReward(bool chosen, int value) : base(value)
		{
			m_Chosen = chosen;
		}

		public override Item Generate(Mobile from)
		{
			MajorArtifactRewardScroll mrs = new MajorArtifactRewardScroll(m_Chosen);
			mrs.LootType = LootType.Cursed;
			return mrs;
		}
	}
}
