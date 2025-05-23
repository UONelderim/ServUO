using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Items
{
	public class Letter : Item, IMailItem
	{
		private string _Sender;
		private string _Recipient;
		private bool _Sealed;
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

		public bool Sealed
		{
			get => _Sealed;
			set
			{
				_Sealed = value;
				ItemID = _Sealed ? 0x0E35 : 0x0E34;
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
		
		private string _Text;

		public string Text
		{
			get => _Text;
			set
			{
				_Text = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public Letter() : base(0xE34)
		{
			Weight = 1.0;
			Movable = true;
			Name = "List";
			Stackable = false;
		}

		public Letter(Serial serial) : base(serial) { }


		public override void OnDoubleClick(Mobile from)
		{
			from.SendGump(new LetterGump(this));
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			IMailItem.AddMailProperties(this, list);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
			IMailItem.AddMailContextMenuEntries(this, from, list);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(_Text);
			writer.Write(_Sender);
			writer.Write(_Recipient);
			writer.Write(_Sealed);
			writer.Write(_SentTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			_Text = reader.ReadString();
			_Sender = reader.ReadString();
			_Recipient = reader.ReadString();
			_Sealed = reader.ReadBool();
			_SentTime = reader.ReadString();
		}
	}
}
