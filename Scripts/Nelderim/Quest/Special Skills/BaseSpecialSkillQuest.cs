using System.Linq;

namespace Server.Engines.Quests
{
	public class BaseSpecialSkillQuest : BaseQuest
	{
		public override bool CanOffer()
		{
			//Won't offer if have any special skill quest in progress
			if (Owner.Quests
			    .Where(s => GetType().IsSubclassOf(typeof(BaseSpecialSkillQuest)))
			    .All(q => q.Completed || q.Failed))
			{
				return false;
			}

			return base.CanOffer();
		}

		public override void OnAccept()
		{
			Owner.SpecialSkills.Clear();
			base.OnAccept();
		}
	}
}
