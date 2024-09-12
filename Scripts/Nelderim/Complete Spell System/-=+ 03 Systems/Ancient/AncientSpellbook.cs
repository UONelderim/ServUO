#region References

using Server.Items;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientSpellbook : CSpellbook
	{
		public override School School { get { return School.Ancient; } }

		[Constructable]
		public AncientSpellbook(Mobile owner)
			: this(owner, 0, CSSettings.FullSpellbooks)
		{
		}

		[Constructable]
		public AncientSpellbook(Mobile owner, bool full)
			: this(owner, 0, full)
		{
		}

		[Constructable]
		public AncientSpellbook(Mobile owner, ulong content, bool full)
			: base(owner, content, 0xEFA, full)
		{
			Hue = 1355;
			Name = "Księga Starożytnych Zaklęć";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel == AccessLevel.Player)
			{
				Container pack = from.Backpack;
				if (!(Parent == from || (pack != null && Parent == pack)))
				{
					from.SendMessage("Ta ksiega musi znajdowac sie Twoim glownym plecaku");
					return;
				}

				if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(@from, this.School))
				{
					return;
				}
			}

			from.CloseGump(typeof(AncientSpellbookGump));
			from.SendGump(new AncientSpellbookGump(this));
		}

		public AncientSpellbook(Serial serial)
			: base(serial)
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
