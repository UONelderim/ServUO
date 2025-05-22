using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Mail;

namespace Server.Gumps
{
	public class WriteLetterGump : Gump
	{
		private Mobile m_Owner;
		private Mobile m_Recipient;

		public WriteLetterGump(Mobile owner, Mobile recipient) : base(25, 25)
		{
			m_Owner = owner;
			m_Recipient = recipient;

			owner.CloseGump(typeof(WriteLetterGump));

			AddPage(0);
			AddBackground(0, 0, 260, 300, 5054);
			AddLabel(20, 20, 1152, "Napisz list do:");
			AddLabel(140, 20, 1152, m_Recipient.Name);
			AddTextEntry(20, 50, 220, 140, 0, 1, "");
			AddButton(20, 200, 0xFAE, 0xFAF, 1, GumpButtonType.Reply, 0);
			AddLabel(50, 200, 1152, "Wyślij list");
			AddButton(20, 230, 0xFAB, 0xFAC, 2, GumpButtonType.Reply, 0);
			AddLabel(50, 230, 1152, "Dołącz paczkę");
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1: // Wyślij list
					var textEntry = info.GetTextEntry(1);
					string body = textEntry?.Text?.Trim() ?? "";

					var letter = new PlayerLetter { BodyText = body, Sender = m_Owner, Recipient = m_Recipient };

					var options = MailUtility.GetAllDestinations(letter.Recipient, letter.Sender);

					if (options.Count > 1)
					{
						from.SendGump(new DestinationChoiceGump(from, letter, options));
					}
					else
					{
						letter.Destination = options[0];
						letter.ProcessSend();
					}

					break;

				case 2: // Dołącz paczkę
					from.Target = new ParcelTarget(m_Owner, m_Recipient);
					break;
			}
		}

		private class ParcelTarget : Target
		{
			private Mobile m_From;
			private Mobile m_To;

			public ParcelTarget(Mobile from, Mobile to) : base(10, false, TargetFlags.None)
			{
				m_From = from;
				m_To = to;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (!(target is Item item && item.IsChildOf(from.Backpack)))
				{
					from.SendMessage("Nieprawidłowy przedmiot do wysłania.");
					return;
				}

				var parcel = new Parcel { Sender = m_From, Recipient = m_To };
				parcel.AddItem(item);

				var options = MailUtility.GetAllDestinations(parcel.Recipient, parcel.Sender);

				if (options.Count > 1)
				{
					from.SendGump(new DestinationChoiceGump(from, parcel, options));
				}
				else
				{
					parcel.Destination = options[0];
					parcel.ProcessSend();
				}
			}
		}
	}
}
