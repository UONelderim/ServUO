using System;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public interface IMailItem
	{
		public string Sender { get; set; }
		public string Recipient { get; set; }
		public bool Sealed { get; set; }
		public string SentTime { get; set; }
		
		public static void AddMailProperties(IMailItem mail, ObjectPropertyList list)
		{
			if (mail.Sender != null)
				list.Add(1060658, "{0}\t{1}", "Od", mail.Sender);
			if (mail.Recipient != null)
				list.Add(1060659, "{0}\t{1}", "Do", mail.Recipient);
			if(mail.Sealed)
				list.Add("Zapieczętowane");
			if (mail.SentTime != null)
				list.Add(1060660, "{0}\t{1}", "Wyslano", mail.SentTime);
		}
		
		public static void AddMailContextMenuEntries(Item item, Mobile from, List<ContextMenuEntry> list)
		{
			if(item is IMailItem mail && !mail.Sealed)
				list.Add(new EnterAddressContextMenuEntry(from, item));
			list.Add(new SealContextMenuEntry(from, item));
		}
	}

	public class EnterAddressContextMenuEntry : ContextMenuEntry
	{
		private Mobile _From;
		private Item _Mail;
		
		public EnterAddressContextMenuEntry(Mobile from, Item mail) : base(3070074)
		{
			_From = from;
			_Mail = mail;
		}

		public override void OnClick()
		{
			if (_From.CheckAlive() && _Mail.IsChildOf(_From.Backpack))
			{
				
			}
			base.OnClick();
		}
	}
	
	

	public class SealContextMenuEntry : ContextMenuEntry
	{
		private Mobile _From;
		private Item _Item;
		
		public SealContextMenuEntry(Mobile from, Item item) : base(3070075)
		{
			_From = from;
			_Item = item;
		}

		public override void OnClick()
		{
			if (_From.CheckAlive() && _Item.IsChildOf(_From.Backpack) && _Item is IMailItem mail)
			{
				var wax = _From.Backpack.FindItem(i => i is Beeswax or RawBeeswax or PureRawBeeswax);
				if (wax == null)
				{
					_From.SendMessage("Potrzebujesz wosku aby to zapieczętować");
					return;
				}
				wax.Consume(1);
				mail.Sealed = true;
			}
			base.OnClick();
		}
	}
	
}
