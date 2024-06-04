using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public partial class AnimalTrainer
	{
		private static List<OnSpeechEntry> _OnSpeechActions = new()
		{
			new OnSpeechEntry("opiek", (m, from) =>
			{
				if (m is AnimalTrainer at)
				{
					at.CloseClaimList(from);
					at.BeginStable(from);
				}
			}),
			new OnSpeechEntry("oddaj.*wszyst", (m, from) =>
			{
				if (m is AnimalTrainer at)
				{
					at.CloseClaimList(from);
					at.Claim(from);
				}
			}, true),
			new OnSpeechEntry("oddaj", (m, from) =>
			{
				if (m is AnimalTrainer at)
				{
					at.CloseClaimList(from);
					at.BeginClaimList(from);
				}
			}),
			new OnSpeechEntry("zmniejsz", (m, from) =>
			{
				if (m is AnimalTrainer at)
				{
					at.CloseClaimList(from);
					from.Target = new ShrinkTarget(at);
				}
			})
		};

		public override List<OnSpeechEntry> OnSpeechActions => _OnSpeechActions;

		private void NInitSBInfo()
		{
			if (Race == Race.NDrow)
			{
				m_SBInfos.Add(new SBDrowAnimalTrainer());
			}
		}
	}
}
