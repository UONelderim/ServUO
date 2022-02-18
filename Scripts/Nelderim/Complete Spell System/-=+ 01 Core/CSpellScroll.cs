#region References

using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Multis;
using Server.Spells;

#endregion

namespace Server.ACC.CSS
{
	public class CSpellScroll : Item
	{
		public Type SpellType { get; private set; }

		public CSpellScroll(Serial serial)
			: base(serial)
		{
		}

		[Constructable]
		public CSpellScroll(Type spellType, int itemID)
			: this(spellType, itemID, 1)
		{
		}

		[Constructable]
		public CSpellScroll(Type spellType, int itemID, int amount)
			: base(itemID)
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;

			SpellType = spellType;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version

			writer.Write(SpellType.Name);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				{
					SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());

					break;
				}
				case 0:
				{
					SpellType = null;
					int bad = reader.ReadInt();

					break;
				}
			}
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!DesignContext.Check(from))
				return; // They are customizing

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			Spell spell = SpellInfoRegistry.NewSpell(SpellType, School.Invalid, from, this);

			if (spell != null)
				spell.Cast();
			else
				from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
		}
	}
}
