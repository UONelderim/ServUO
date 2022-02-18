#region References

using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Items
{
	public class GravestoneAddon : Item, IAddon, IChopable
	{
		[Constructable]
		public GravestoneAddon(int itemID)
			: base(itemID)
		{
			LootType = LootType.Blessed;
			Movable = false;
		}

		public GravestoneAddon(Serial serial)
			: base(serial)
		{
		}

		public Item Deed
		{
			get
			{
				GravestoneDeed deed = new GravestoneDeed();

				return deed;
			}
		}

		void IChopable.OnChop(Mobile user)
		{
			OnDoubleClick(user);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.InRange(Location, 2))
			{
				BaseHouse house = BaseHouse.FindHouseAt(this);

				if (house != null && house.IsOwner(from))
				{
					from.CloseGump(typeof(RewardDemolitionGump));
					from.SendGump(new RewardDemolitionGump(this, 1018318)); // Do you wish to re-deed this banner?
				}
				else
					from.SendLocalizedMessage(
						1018330); // You can only re-deed a banner if you placed it or you are the owner of the house.
			}
			else
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}

		public bool CouldFit(IPoint3D p, Map map)
		{
			if (map == null || !map.CanFit(p.X, p.Y, p.Z, ItemData.Height))
				return false;

			return true;
		}
	}

	public class GravestoneDeed : Item
	{
		[Constructable]
		public GravestoneDeed()
			: base(0x14F0)
		{
			Name = "Kamien Grobowy";
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public GravestoneDeed(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);

				if (house != null && house.IsOwner(from))
				{
					from.CloseGump(typeof(InternalGump));
					from.SendGump(new InternalGump(this));
				}
				else
					from.SendLocalizedMessage(502092); // You must be in your house to do 
			}
			else
				from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.          	
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}

		private class InternalGump : Gump
		{
			public const int Start = 0x1165;
			public const int End = 0x1183;
			private readonly GravestoneDeed m_Gravestone;

			public InternalGump(GravestoneDeed gravestone)
				: base(100, 200)
			{
				m_Gravestone = gravestone;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage(0);

				AddBackground(25, 0, 520, 230, 0xA28);
				AddLabel(70, 12, 0x3E3, "Wybierz kamien grobowy:");

				int itemID = Start;

				for (int i = 1; i <= 5; i++)
				{
					AddPage(i);

					for (int j = 0; j < 8; j++, itemID += 2)
					{
						AddItem(50 + 60 * j, 70, itemID);
						AddButton(50 + 60 * j, 50, 0x845, 0x846, itemID, GumpButtonType.Reply, 0);

						if (itemID >= End)
							break;
					}

					if (i > 1)
						AddButton(75, 198, 0x8AF, 0x8AF, 0, GumpButtonType.Page, i - 1);

					if (i < 5)
						AddButton(475, 198, 0x8B0, 0x8B0, 0, GumpButtonType.Page, i + 1);
				}
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (m_Gravestone == null || m_Gravestone.Deleted)
					return;

				Mobile m = sender.Mobile;

				if (info.ButtonID >= Start && info.ButtonID <= End)
				{
					//if ((info.ButtonID & 0x1) == 0)
					//{
					m.SendMessage("Gdzie chcesz go umiescic?"); // Where would you like to place this banner?
					m.Target = new InternalTarget(m_Gravestone, info.ButtonID);
					//}
				}
			}
		}

		private class InternalTarget : Target
		{
			private readonly GravestoneDeed m_Gravestone;
			private readonly int m_ItemID;

			public InternalTarget(GravestoneDeed gravestone, int itemID)
				: base(-1, true, TargetFlags.None)
			{
				m_Gravestone = gravestone;
				m_ItemID = itemID;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Gravestone == null || m_Gravestone.Deleted)
					return;

				if (m_Gravestone.IsChildOf(from.Backpack))
				{
					BaseHouse house = BaseHouse.FindHouseAt(from);

					if (house != null && house.IsOwner(from))
					{
						IPoint3D p = targeted as IPoint3D;
						Map map = from.Map;

						if (p == null || map == null)
							return;

						Point3D p3d = new Point3D(p);
						ItemData id = TileData.ItemTable[m_ItemID & TileData.MaxItemValue];

						if (map.CanFit(p3d, id.Height))
						{
							house = BaseHouse.FindHouseAt(p3d, map, id.Height);

							if (house != null && house.IsOwner(from))
							{
								//bool north = BaseAddon.IsWall(p3d.X, p3d.Y - 1, p3d.Z, map); //NoWall or Valid
								//bool west = BaseAddon.IsWall(p3d.X - 1, p3d.Y, p3d.Z, map);

								//if (north && west)
								//{
								from.CloseGump(typeof(FacingGump));
								from.SendGump(new FacingGump(m_Gravestone, m_ItemID, p3d, house));
								//}
								// else if (north || west)
								//{
								//GravestoneAddon gravestone = null;

								//if (north)
								//gravestone = new GravestoneAddon(m_ItemID);
								//else if (west)
								//gravestone = new GravestoneAddon(m_ItemID + 1);

								//house.Addons[gravestone] = from;

								//gravestone.IsRewardItem = m_Gravestone.IsRewardItem;
								//gravestone.MoveToWorld(p3d, map);

								//m_Gravestone.Delete();
								//}
								//else
								//from.SendLocalizedMessage(1042039); // The banner must be placed next to a wall.								
							}
							else
								from.SendLocalizedMessage(1042036); // That location is not in your house.
						}
						else
							from.SendLocalizedMessage(500269); // You cannot build that there.		
					}
					else
						from.SendLocalizedMessage(502092); // You must be in your house to do 
				}
				else
					from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.     
			}


			private class FacingGump : Gump
			{
				private readonly GravestoneDeed m_Gravestone;
				private readonly int m_ItemID;
				private readonly Point3D m_Location;
				private readonly BaseHouse m_House;

				public FacingGump(GravestoneDeed gravestone, int itemID, Point3D location, BaseHouse house)
					: base(150, 50)
				{
					m_Gravestone = gravestone;
					m_ItemID = itemID;
					m_Location = location;
					m_House = house;

					Closable = true;
					Disposable = true;
					Dragable = true;
					Resizable = false;

					AddPage(0);

					AddBackground(0, 0, 300, 150, 0xA28);

					AddItem(90, 30, itemID);
					AddItem(180, 30, itemID + 1);

					AddButton(50, 35, 0x868, 0x869, (int)Buttons.West, GumpButtonType.Reply, 0);
					AddButton(145, 35, 0x868, 0x869, (int)Buttons.North, GumpButtonType.Reply, 0);
				}

				private enum Buttons
				{
					Cancel,
					North,
					West
				}

				public override void OnResponse(NetState sender, RelayInfo info)
				{
					if (m_Gravestone == null || m_Gravestone.Deleted || m_House == null)
						return;

					GravestoneAddon gravestone = null;

					if (info.ButtonID == (int)Buttons.North)
						gravestone = new GravestoneAddon(m_ItemID + 1);
					if (info.ButtonID == (int)Buttons.West)
						gravestone = new GravestoneAddon(m_ItemID);

					if (gravestone != null)
					{
						m_House.Addons[gravestone] = sender.Mobile;

						gravestone.MoveToWorld(m_Location, sender.Mobile.Map);

						m_Gravestone.Delete();
					}
				}
			}
		}
	}
}
