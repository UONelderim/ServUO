using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class LetterGump : Gump
	{
		private Mobile m_Owner;
		private Mobile m_From;
		private string m_Letter;
		private PlayerLetter m_LetterItem;

		public LetterGump(Mobile owner, string text, Mobile from, PlayerLetter letter) : base(25, 25)
		{
			m_Owner = owner;
			m_From = from;
			m_Letter = text;
			m_LetterItem = letter;

			AddButton(245, 253, 0xFAE, 0xFB0, 1, GumpButtonType.Reply, 0);
			AddLabel(210, 255, 0, "Odpisz");
			AddHtml(30, 37, 234, 200, m_Letter, false, true);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1 && !m_LetterItem.Replied)
			{
				state.Mobile.SendGump(new WriteLetterGump(m_Owner, m_From));
				m_LetterItem.Replied = true;
			}
		}
	}
}
