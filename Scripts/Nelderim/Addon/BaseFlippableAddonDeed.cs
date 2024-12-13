using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public record FlippableAddonEntry(int LabelNumber, Func<BaseAddon> AddonAction);

	public abstract class BaseFlippableAddonDeed : BaseAddonDeed
	{
		public abstract List<FlippableAddonEntry> Entries { get; }

		private Func<BaseAddon> _AddonAction;
		public override BaseAddon Addon => _AddonAction?.Invoke();

		protected BaseFlippableAddonDeed()
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // Must be in your backpack to use.
				return;
			}

			from.SendGump(new FlippableAddonGump(from, this));
		}

		public void CreateAddon(Mobile from, int index)
		{
			if (index >= Entries.Count)
			{
				Console.WriteLine($"BaseFlippableAddonDeed: Invalid index {index} for {GetType().Name}");
				return;
			}

			_AddonAction = Entries[index].AddonAction;

			base.OnDoubleClick(from); //This prompts target and creates BaseAddonDeed.Addon
		}

		public BaseFlippableAddonDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class FlippableAddonGump : Gump
	{
		private readonly Mobile m_From;
		private readonly BaseFlippableAddonDeed m_Deed;

		public FlippableAddonGump(Mobile from, BaseFlippableAddonDeed deed) : base(50, 50)
		{
			m_From = from;
			m_Deed = deed;

			AddPage(0);
			AddBackground(0, 0, 300, 60 + deed.Entries.Count * 30, 9270); // Increased size for longer names
			AddAlphaRegion(10, 10, 280, 40 + deed.Entries.Count * 30);

			AddLabel(50, 20, 1152, "Wybierz orientacje przedmiotu:");

			var y = 50;
			for (var index = 0; index < deed.Entries.Count; index++)
			{
				var entry = deed.Entries[index];
				AddButton(30, y, 4005, 4007, index + 1, GumpButtonType.Reply, 0);
				AddHtmlLocalized(70, y, 230, 30, entry.LabelNumber, 0xFFFFFF, false, false);
				y += 30;
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (m_Deed.Deleted || !m_Deed.IsChildOf(m_From.Backpack))
				return;

			if (info.ButtonID == 0)
			{
				m_From.SendMessage("Zdecydowales nie uzywac przedmiotu.");
				return;
			}

			m_Deed.CreateAddon(sender.Mobile, info.ButtonID - 1);
		}
	}
}
