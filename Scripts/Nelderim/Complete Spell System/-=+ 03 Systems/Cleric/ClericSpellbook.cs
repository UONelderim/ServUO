#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSpellbook : CSpellbook
	{
		public override School School { get { return School.Cleric; } }

		[Constructable]
		public ClericSpellbook(Mobile owner) : this(owner, 0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public ClericSpellbook(Mobile owner, bool full) : this(owner,0, full)
		{
		}

		[Constructable]
		public ClericSpellbook(Mobile owner, ulong content, bool full) : base(owner, content, 0xEFA, full)
		{
			Hue = 0x1F0;
			Name = "Księga Herdeizmu";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Ta ksiega musi znajdowac sie w Twoim glownym plecaku.");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(ClericSpellbookGump));
			from.SendGump(new ClericSpellbookGump(this));
		}

		public ClericSpellbook(Serial serial) : base(serial)
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
