using System;
using Server.Network;

namespace Server.Gumps
{
	public class ArtChooseGump : Gump
	{
		private Type[] m_ArtList;
		private string[] m_ArtName;
		private Item m_Scroll;
		private int m_Page;

		public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
		public static readonly int TextOffsetX = PropsConfig.TextOffsetX;
		public static readonly int EntryHeight = PropsConfig.EntryHeight;


		public const int GumpWidth = 300;
		public const int TitleHeight = 20;
		public const int TitleWidth = 280;

		public readonly int GumpBaseHeight = 30 + TitleHeight + EntryHeight;

		//public const int MainHeight = 310;
		public const int ButtonWidth = 30;

		public ArtChooseGump(Mobile owner, Type[] list, string[] names, Item scroll, int page) : base(GumpOffsetX,
			GumpOffsetY)
		{
			owner.CloseGump(typeof(ArtChooseGump));
			m_ArtList = list;
			m_ArtName = names;
			m_Scroll = scroll;
			m_Page = page;

			Draw();
		}

		public ArtChooseGump(Mobile owner, Type[] list, string[] names, Item scroll) : this(owner,
			list,
			names,
			scroll,
			0)
		{
		}

		public void Draw()
		{
			var from = m_Page * 10;
			bool NextPage, PrevPage;

			if (m_ArtList.Length <= 10 && from == 0)
				NextPage = PrevPage = false;
			else if (m_ArtList.Length > 10 && from == 0)
			{
				PrevPage = false;
				NextPage = true;
			}
			else if (from > 0 && m_ArtList.Length - from > 10)
			{
				NextPage = true;
				PrevPage = true;
			}
			else if (from > 0 && m_ArtList.Length - from <= 10)
			{
				NextPage = false;
				PrevPage = true;
			}
			else
				NextPage = PrevPage = false;


			AddPage(0);
			var GumpHeight = GumpBaseHeight + (10 * EntryHeight) + 10;
			AddBackground(0, 0, GumpWidth, GumpHeight, 9200);
			AddAlphaRegion(10, 10, TitleWidth, TitleHeight);

			var x = 10;
			var y = 10;

			AddLabel(x + TextOffsetX, y, 1952, "WYBIERZ NAGRODE");

			y += TitleHeight + 10;

			AddAlphaRegion(x, y, TitleWidth, GumpHeight - GumpBaseHeight);

			//Itemy
			for (var i = from; i < from + 10 && i < m_ArtList.Length; i++)
			{
				AddButton(x + 5, y + 5, 4005, 4006, i + 3, GumpButtonType.Reply, 0);
				AddLabel(x + 5 + ButtonWidth, y + 5, 1952, m_ArtName[i]);
				y += EntryHeight;
			}

			var PosY = 35 + TitleHeight + (10 * EntryHeight);
			if (PrevPage)
			{
				AddButton(x, PosY, 4014, 4015, 1, GumpButtonType.Reply, 0);
				AddLabel(x + ButtonWidth, PosY, 1952, "POPRZEDNIE");
			}

			if (NextPage)
			{
				AddButton(x + 105, PosY, 4005, 4006, 2, GumpButtonType.Reply, 0);
				AddLabel(x + 105 + ButtonWidth, PosY, 1952, "NASTEPNE");
			}

			AddButton(x + 205, PosY, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(x + 205 + ButtonWidth, PosY, 1952, "ANULUJ");
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (m_Scroll.Deleted || !m_Scroll.IsChildOf(state.Mobile.Backpack)) return;
			var from = state.Mobile;

			if (info.ButtonID <= 0)
				return;
			else if (info.ButtonID == 1)
			{
				state.Mobile.CloseGump(typeof(ArtChooseGump));
				state.Mobile.SendGump(new ArtChooseGump(state.Mobile, m_ArtList, m_ArtName, m_Scroll, m_Page - 1));
			}
			else if (info.ButtonID == 2)
			{
				state.Mobile.CloseGump(typeof(ArtChooseGump));
				state.Mobile.SendGump(new ArtChooseGump(state.Mobile, m_ArtList, m_ArtName, m_Scroll, m_Page + 1));
			}
			else
			{
				var i = info.ButtonID - 3;

				var art = (Item)Activator.CreateInstance(m_ArtList[i]);
				from.Backpack.DropItem(art);
				from.SendLocalizedMessage(505596); // Nagroda zmaterializwoała się w plecaku.

				from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
				from.PlaySound(0x1E0);

				m_Scroll.Delete();
			}
		}
	}
}
