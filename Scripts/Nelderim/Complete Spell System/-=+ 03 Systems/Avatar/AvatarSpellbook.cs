#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarSpellbook : CSpellbook
	{
		public override School School { get { return School.Avatar; } }

		[Constructable]
		public AvatarSpellbook(Mobile owner) : this(owner, 0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public AvatarSpellbook(Mobile owner, bool full) : this(owner, 0, full)
		{
		}

		[Constructable]
		public AvatarSpellbook(Mobile owner, ulong content, bool full) : base(owner, content, 0xEFA, full)
		{
			Hue = 1174;
			Name = "Księga Zaklęć Mnicha";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Ta księga musi byc w Twoim głównym plecaku, byś mógł ją otworzyć.");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(AvatarSpellbookGump));
			from.SendGump(new AvatarSpellbookGump(this));
		}

		public AvatarSpellbook(Serial serial) : base(serial)
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
