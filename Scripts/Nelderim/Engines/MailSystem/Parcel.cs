
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class Parcel : BaseContainer, IMailItem
	{
		private string _Sender;
		private string _Recipient;
		private string _SentTime;

		public string Sender
		{
			get => _Sender;
			set
			{
				_Sender = value;
				InvalidateProperties();
			}
		}

		public string Recipient
		{
			get => _Recipient;
			set
			{
				_Recipient = value;
				InvalidateProperties();
			}
		}

		public string SentTime
		{
			get => _SentTime;
			set
			{
				_SentTime = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public Parcel() : base(0x232A)
		{
			Weight = 1.0;
			Name = "Paczka";
			Stackable = false;
		}

		public override int DefaultMaxWeight => 100;

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			IMailItem.AddMailProperties(this, list);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
			list.Add(new EnterAddressContextMenuEntry(from, this));
		}

		public Parcel(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(_Sender);
			writer.Write(_Recipient);
			writer.Write(_SentTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			_Sender = reader.ReadString();
			_Recipient = reader.ReadString();
			_SentTime = reader.ReadString();
		}
	}
}
