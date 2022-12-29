
namespace Server.Mobiles
{
	public delegate void OnSpeechAction(Mobile m, Mobile from);
	
	public class OnSpeechEntry
	{
		public string Keyword { get; }
		public bool Exact { get; }
		public OnSpeechAction Action { get; }

		public OnSpeechEntry(string keyword, OnSpeechAction action, bool exact = false)
		{
			Keyword = keyword;
			Exact = exact;
			Action = action;
		}
	}
}
