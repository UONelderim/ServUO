#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardSpellbook : CSpellbook
	{
		public override School School { get { return School.Bard; } }

		[Constructable]
		public BardSpellbook(Mobile owner) : this(owner, 0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public BardSpellbook(Mobile owner, bool full) : this(owner, 0, full)
		{
		}

		[Constructable]
		public BardSpellbook(Mobile owner, ulong content, bool full) : base(owner, content, 0xEFA, full)
		{
			Name = "Księga Pieśni Bojowych";
			Hue = 0x96;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Musisz miec tę księgę w Twoim głównym plecaku, by móc jej używać.");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(BardSpellbookGump));
			from.SendGump(new BardSpellbookGump(this));
		}

		public BardSpellbook(Serial serial) : base(serial)
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
