#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueSpellbook : CSpellbook
	{
		public override School School { get { return School.Rogue; } }

		[Constructable]
		public RogueSpellbook(Mobile owner) : this(owner, 0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public RogueSpellbook(Mobile owner, bool full) : this(owner, 0, full)
		{
		}

		[Constructable]
		public RogueSpellbook(Mobile owner, ulong content, bool full) : base(owner, content, 0xEFA, full)
		{
			Hue = 0x20;
			Name = "Księga Podstępnych Sztuczek";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Ta księga musi znajdować sie w Twoim głównym plecaku.");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(RogueSpellbookGump));
			from.SendGump(new RogueSpellbookGump(this));
		}

		public RogueSpellbook(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
