using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Server.Nelderim;

namespace Server.Mobiles
{
	public partial class VendorAI
	{
		private static readonly List<OnSpeechEntry> _KeywordActions =
			new List<OnSpeechEntry>
			{
				new OnSpeechEntry("sprzed" , (m, from) =>
				{
					if (m is BaseVendor bv)
					{
						bv.VendorSell(from);
					}
				}),
				new OnSpeechEntry("kup" , (m, from) =>
				{
					if (m is BaseVendor bv)
					{
						bv.VendorBuy(from);
					}
				}),
				new OnSpeechEntry("plotk" , (m, from) =>
				{
					if (m is BaseVendor bv)
					{
						bv.SayAboutRumors(from);
					}
				}),
				new OnSpeechEntry("zlecen" , (m, from) =>
				{
					if (!(m is BaseVendor bv)) return;
					if (!bv.CheckVendorAccess(from)) return;
					if (!bv.IsAssignedBuildingWorking()) return;
						
					new BaseVendor.BulkOrderInfoEntry(from, bv).OnClick();
				}),
				new OnSpeechEntry("lapowk" , (m, from) =>
				{
					if (!(m is BaseVendor bv)) return;
					if (!bv.CheckVendorAccess(from)) return;
						
					new BaseVendor.BribeEntry(from, bv).OnClick();
				}),
				new OnSpeechEntry("nagrod" , (m, from) =>
				{
					if (!(m is BaseVendor bv)) return;
					if (!bv.CheckVendorAccess(from)) return;
						
					new BaseVendor.ClaimRewardsEntry(from, bv).OnClick();
				})
			};
		
		private void NelderimOnSpeech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;

			if (m_Mobile is BaseVendor bv && from.InRange(m_Mobile, 2) && !e.Handled)
			{
				foreach (var entry in _KeywordActions.Concat(bv.OnSpeechActions))
				{
					var keyword = entry.Keyword;
					var regex = new Regex(keyword, RegexOptions.IgnoreCase);
					if (regex.IsMatch(e.Speech))
					{
						e.Handled = true;
						var lazyRegex = new Regex($"^{keyword}..?$", RegexOptions.IgnoreCase);
						if(!entry.Exact && String.Equals(e.Speech, keyword, StringComparison.CurrentCultureIgnoreCase) || lazyRegex.IsMatch(e.Speech))
						{
							bv.OnLazySpeech();
						}
						else
						{
							entry.Action.Invoke(bv, from);
						}
						bv.FocusMob = from;
						break;
					}
				}

				if (!e.Handled)
				{
					bv.SayRumor(from, e);
				}
				
				if (from is PlayerMobile && !from.Hidden && from.Alive && Utility.RandomDouble() < m_Mobile.GetRumorsActionPropability())
					m_Mobile.AnnounceRandomRumor(PriorityLevel.Low);
			}
		}
	}
}
