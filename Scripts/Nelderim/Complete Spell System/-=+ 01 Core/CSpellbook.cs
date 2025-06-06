#region References

using System;
using System.Collections;
using Server.Commands;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

#endregion

namespace Server.ACC.CSS
{
	public class CSpellbook : Item
	{
		public static void Initialize()
		{
			CommandSystem.Register("FillBook", ACC.GlobalMinimumAccessLevel, FillBook_OnCommand);
		}

		[Usage("FillBook")]
		[Description("Completely fills a targeted spellbook with scrolls.")]
		private static void FillBook_OnCommand(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, FillBook_OnTarget);
			e.Mobile.SendMessage("Target the spellbook to fill.");
		}

		private static void FillBook_OnTarget(Mobile from, object obj)
		{
			if (obj is CSpellbook)
			{
				CSpellbook book = (CSpellbook)obj;

				if (book == null || book.Deleted)
					return;

				book.Fill();
				book.Full = true;

				from.SendMessage("The spellbook has been filled.");

				CommandLogging.WriteLine(from, "{0} {1} filling spellbook {2}", from.AccessLevel,
					CommandLogging.Format(from), CommandLogging.Format(book));
			}
			else
			{
				from.BeginTarget(-1, false, TargetFlags.None, FillBook_OnTarget);
				from.SendMessage("That is not a spellbook. Try again.");
			}
		}

		public override bool AllowEquipedCast(Mobile from) { return true; }
		public override bool DisplayLootType { get { return true; } }

		public virtual School School { get { return School.Invalid; } }
		public virtual ArrayList SchoolSpells { get { return SpellInfoRegistry.GetSpellsForSchool(this.School); } }
		public virtual int BookCount { get { return SchoolSpells.Count; } }

		private ulong m_Content;

		[CommandProperty(AccessLevel.GameMaster)]
		public int SpellCount { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public ulong Content
		{
			get { return m_Content; }
			set
			{
				if (m_Content != value)
				{
					m_Content = value;

					SpellCount = 0;

					while (value > 0)
					{
						SpellCount += (int)(value & 0x1);
						value >>= 1;
					}

					InvalidateProperties();
				}
			}
		}

		//public int Mark{ get{ return m_Mark; } set{ m_Mark = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Full { get; set; }

		public CSpellbook()
			: this(0, CSSettings.FullSpellbooks)
		{
		}

		public CSpellbook(bool full)
			: this(0, full)
		{
		}

		public CSpellbook(ulong content, bool full)
			: this(content, 0xEFA, full)
		{
		}

		public CSpellbook(ulong content, int itemID, bool full)
			: base(itemID)
		{
			Name = "Arcane Tome";

			Weight = 3.0;
			Layer = Layer.OneHanded;
			LootType = LootType.Blessed;

			Content = content;

			if (full)
				Fill();
		}

		public CSpellbook(Serial serial)
			: base(serial)
		{
		}

		public void Fill()
		{
			Full = true;

			if (this.BookCount == 64)
				this.Content = UInt64.MaxValue;
			else
				this.Content = (1ul << this.BookCount) - 1;
		}

		public bool AddSpell(Type type)
		{
			if (!SchoolSpells.Contains(type))
				return false;

			m_Content |= (ulong)1 << SchoolSpells.IndexOf(type);
			++SpellCount;

			InvalidateProperties();
			return true;
		}

		public bool HasSpell(Type type)
		{
			if (SchoolSpells.Contains(type) && SpellInfoRegistry.CheckRegistry(this.School, type))
				return ((m_Content & ((ulong)1 << SchoolSpells.IndexOf(type))) != 0);

			return false;
		}

		public static bool MobileHasSpell(Mobile m, School school, Type type)
		{
			if (m == null || m.Deleted || m.Backpack == null || school == School.Invalid || type == null)
				return false;

			foreach (Item i in m.Backpack.Items)
			{
				if (i is CSpellbook)
				{
					CSpellbook book = (CSpellbook)i;
					if (book.School == school && book.HasSpell(type))
						return true;
				}
			}

			Item layer = m.FindItemOnLayer(Layer.OneHanded);
			if (layer is CSpellbook)
			{
				CSpellbook book = (CSpellbook)layer;
				if (book.School == school && book.HasSpell(type))
					return true;
			}

			layer = m.FindItemOnLayer(Layer.FirstValid);
			if (layer is CSpellbook)
			{
				CSpellbook book = (CSpellbook)layer;
				if (book.School == school && book.HasSpell(type))
					return true;
			}

			return false;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1042886, SpellCount.ToString()); // ~1_NUMBERS_OF_SPELLS~ Spells
		}

		public override void OnAosSingleClick(Mobile from)
		{
			base.OnAosSingleClick(from);
			this.LabelTo(from, 1042886, SpellCount.ToString());
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is CSpellScroll && dropped.Amount == 1)
			{
				CSpellScroll scroll = (CSpellScroll)dropped;

				if (!SchoolSpells.Contains(scroll.SpellType))
				{
					return false;
				}

				if (HasSpell(scroll.SpellType))
				{
					from.SendLocalizedMessage(500179); // That spell is already present in that spellbook.
					return false;
				}

				AddSpell(scroll.SpellType);
				scroll.Delete();

				from.Send(new PlaySound(0x249, GetWorldLocation()));
				return true;
			}

			if (dropped is SpellScroll && dropped.Amount == 1)
			{
				SpellScroll scroll = (SpellScroll)dropped;

				Type type = SpellRegistry.Types[scroll.SpellID];

				if (!SchoolSpells.Contains(type))
				{
					return false;
				}

				if (HasSpell(type))
				{
					from.SendLocalizedMessage(500179); // That spell is already present in that spellbook.
					return false;
				}

				AddSpell(type);
				scroll.Delete();

				@from.Send(new PlaySound(0x249, GetWorldLocation()));
				return true;
			}

			return false;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(Full);
			writer.Write(m_Content);
			writer.Write(SpellCount);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					Full = reader.ReadBool();
					m_Content = reader.ReadULong();
					SpellCount = reader.ReadInt();

					break;
				}
			}
		}
	}
}
