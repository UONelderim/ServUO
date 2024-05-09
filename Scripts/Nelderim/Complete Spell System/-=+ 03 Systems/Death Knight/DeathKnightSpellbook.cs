using Server.ACC.CSS;
using Server.Gumps;

namespace Server.Items
{
	public class DeathKnightSpellbook : CSpellbook
	{
		public override School School => School.DeathKnight;

		[Constructable]
		public DeathKnightSpellbook() : this(0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public DeathKnightSpellbook(bool full) : this(0, full)
		{
		}

		[Constructable]
		public DeathKnightSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
		{
			Hue = 2001;
			Name = "Księga Mrocznego Rycerza";
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

			from.CloseGump(typeof(DeathKnightSpellbookGump));
			from.SendGump(new DeathKnightSpellbookGump(this));
		}

		public DeathKnightSpellbook(Serial serial) : base(serial)
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
