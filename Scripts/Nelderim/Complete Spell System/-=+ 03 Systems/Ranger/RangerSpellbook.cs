#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerSpellbook : CSpellbook
	{
		public override School School { get { return School.Ranger; } }

		[Constructable]
		public RangerSpellbook() : this(0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public RangerSpellbook(bool full) : this(0, full)
		{
		}

		[Constructable]
		public RangerSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
		{
			Hue = 2001;
			Name = "Poradnik Strażnika Leśnego";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Ta księga musi znajdować się w Twoim głównym plecaku.");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(RangerSpellbookGump));
			from.SendGump(new RangerSpellbookGump(this));
		}

		public RangerSpellbook(Serial serial) : base(serial)
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
