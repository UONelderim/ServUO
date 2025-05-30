using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Items
{
	public interface IMailItem
	{
		public string Sender { get; set; }
		public string Recipient { get; set; }
		public string SentTime { get; set; }
		
		public static void AddMailProperties(IMailItem mail, ObjectPropertyList list)
		{
			if (mail.Sender != null)
				list.Add(1060658, "{0}\t{1}", "Od", mail.Sender);
			if (mail.Recipient != null)
				list.Add(1060659, "{0}\t{1}", "Do", mail.Recipient);
			if (mail.SentTime != null)
				list.Add(1060660, "{0}\t{1}", "Wyslano", mail.SentTime);
		}
	}

	public class EnterAddressContextMenuEntry : ContextMenuEntry
	{
		private Mobile _From;
		private Item _Item;
		
		public EnterAddressContextMenuEntry(Mobile from, Item item) : base(3070074)
		{
			_From = from;
			_Item = item;
		}

		public override void OnClick()
		{
			if (_From.CheckAlive() && _Item.IsChildOf(_From.Backpack) && _Item is IMailItem mail)
			{
				_From.SendGump(new EnterAddressGump(mail));
			}
		}
	}
}
