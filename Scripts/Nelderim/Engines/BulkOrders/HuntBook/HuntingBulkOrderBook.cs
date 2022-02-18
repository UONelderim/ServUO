// 08.03.16 :: emfor :: bulki mysliwego

#region References

using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;

#endregion

namespace Server.Engines.BulkOrders
{
	public class HuntingBulkOrderBook : Item, ISecurable, IUsesRemaining
	{
		private string m_BookName;

		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining { get; set; }

		public bool ShowUsesRemaining { get { return false; } set { } }

		[CommandProperty(AccessLevel.GameMaster)]
		public string BookName
		{
			get { return m_BookName; }
			set
			{
				m_BookName = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public SecureLevel Level { get; set; }

		public ArrayList Entries { get; private set; }

		public HuntingBOBFilter Filter { get; private set; }

		[Constructable]
		public HuntingBulkOrderBook() : base(0x2259)
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			Name = "Ksiega zlecen Mysliwego";
			Hue = 2702;

			Entries = new ArrayList();
			Filter = new HuntingBOBFilter();

			Level = SecureLevel.CoOwners;

			UsesRemaining = 1500;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			else if (Entries.Count == 0)
				from.SendLocalizedMessage(1062381); // The book is empty.
			else if (from is PlayerMobile)
				from.SendGump(new HuntingBOBGump((PlayerMobile)from, this));
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is LargeHunterBOD)
			{
				if (!IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(1062385); // You must have the book in your backpack to add deeds to it.
					return false;
				}

				if (Entries.Count < 500)
				{
					Entries.Add(new HuntingBOBLargeEntry((LargeHunterBOD)dropped));
					InvalidateProperties();

					from.SendLocalizedMessage(1062386); // Deed added to book.

					if (from is PlayerMobile)
						from.SendGump(new HuntingBOBGump((PlayerMobile)from, this));

					dropped.Delete();
					return true;
				}

				from.SendLocalizedMessage(1062387); // The book is full of deeds.
				return false;
			}

			if (dropped is SmallHunterBOD)
			{
				if (!IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(1062385); // You must have the book in your backpack to add deeds to it.
					return false;
				}

				if (Entries.Count < 500)
				{
					Entries.Add(new HuntingBOBSmallEntry((SmallHunterBOD)dropped));
					InvalidateProperties();

					from.SendLocalizedMessage(1062386); // Deed added to book.

					if (from is PlayerMobile)
						from.SendGump(new HuntingBOBGump((PlayerMobile)from, this));

					dropped.Delete();
					return true;
				}

				@from.SendLocalizedMessage(1062387); // The book is full of deeds.
				return false;
			}

			from.SendLocalizedMessage(1062388); // That is not a bulk order deed.
			return false;
		}

		public HuntingBulkOrderBook(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version

			writer.Write(UsesRemaining);

			writer.Write((int)Level);

			writer.Write(m_BookName);

			Filter.Serialize(writer);

			writer.WriteEncodedInt(Entries.Count);

			for (int i = 0; i < Entries.Count; ++i)
			{
				object obj = Entries[i];

				if (obj is HuntingBOBLargeEntry)
				{
					writer.WriteEncodedInt(0);
					((HuntingBOBLargeEntry)obj).Serialize(writer);
				}
				else if (obj is HuntingBOBSmallEntry)
				{
					writer.WriteEncodedInt(1);
					((HuntingBOBSmallEntry)obj).Serialize(writer);
				}
				else
				{
					writer.WriteEncodedInt(-1);
				}
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				{
					UsesRemaining = reader.ReadInt();
					Level = (SecureLevel)reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_BookName = reader.ReadString();

					Filter = new HuntingBOBFilter(reader);

					int count = reader.ReadEncodedInt();

					Entries = new ArrayList(count);

					for (int i = 0; i < count; ++i)
					{
						int v = reader.ReadEncodedInt();

						switch (v)
						{
							case 0:
								Entries.Add(new HuntingBOBLargeEntry(reader));
								break;
							case 1:
								Entries.Add(new HuntingBOBSmallEntry(reader));
								break;
						}
					}

					break;
				}
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (this is IUsesRemaining && ((IUsesRemaining)this).ShowUsesRemaining)
				list.Add(1060584, ((IUsesRemaining)this).UsesRemaining.ToString()); // uses remaining: ~1_val~

			list.Add(1062344, Entries.Count.ToString()); // Deeds in book: ~1_val~

			if (m_BookName != null && m_BookName.Length > 0)
				list.Add(1062481, m_BookName); // Book Name: ~1_val~
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (from.CheckAlive() && IsChildOf(from.Backpack))
				list.Add(new NameBookEntry(from, this));

			SetSecureLevelEntry.AddTo(from, this, list);
		}

		private class NameBookEntry : ContextMenuEntry
		{
			private readonly Mobile m_From;
			private readonly HuntingBulkOrderBook m_Book;

			public NameBookEntry(Mobile from, HuntingBulkOrderBook book) : base(6216)
			{
				m_From = from;
				m_Book = book;
			}

			public override void OnClick()
			{
				if (m_From.CheckAlive() && m_Book.IsChildOf(m_From.Backpack))
				{
					m_From.Prompt = new NameBookPrompt(m_Book);
					m_From.SendLocalizedMessage(1062479); // Type in the new name of the book:
				}
			}
		}

		private class NameBookPrompt : Prompt
		{
			private readonly HuntingBulkOrderBook m_Book;

			public NameBookPrompt(HuntingBulkOrderBook book)
			{
				m_Book = book;
			}

			public override void OnResponse(Mobile from, string text)
			{
				if (text.Length > 40)
					text = text.Substring(0, 40);

				if (from.CheckAlive() && m_Book.IsChildOf(from.Backpack))
				{
					m_Book.BookName = Utility.FixHtml(text.Trim());

					from.SendLocalizedMessage(1062480); // The bulk order book's name has been changed.
				}
			}

			public override void OnCancel(Mobile from)
			{
			}
		}
	}
}
