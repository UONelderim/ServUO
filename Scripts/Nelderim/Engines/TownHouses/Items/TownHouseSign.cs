#region References

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Nelderim;
using Server.Network;

#endregion

namespace Knives.TownHouses
{
	public enum Intu { Neither, No, Yes }

	public enum SignIDs
	{
		TownHouseSignWE = 0xC0B,
		TownHouseSignNS = 0xC0C,
		HouseSignWE = 0xBD1,
		HouseSignNS = 0xBD2,
		SignHangerWE = 0xB97,
		SignHangerNS = 0xB98,
	}

	[Flipable(0xC0B, 0xC0C)]
	public class TownHouseSign : Item
	{
		public static ArrayList AllSigns { get; } = new ArrayList();

		private Point3D c_BanLoc, c_SignLoc;

		private int c_Locks,
			c_Secures,
			c_Price,
			c_MinZ,
			c_MaxZ,
			c_MinTotalSkill,
			c_MaxTotalSkill,
			c_ItemsPrice,
			c_RTOPayments;

		private bool c_YoungOnly,
			c_RecurRent,
			c_KeepItems,
			c_LeaveItems,
			c_RentToOwn,
			c_Free,
			c_ForcePrivate,
			c_ForcePublic,
			c_NoTrade;

		private string c_Skill;
		private double c_SkillReq;
		private ArrayList c_DecoreItemInfos, c_PreviewItems;
		private Timer c_RentTimer, c_PreviewTimer;
		private TimeSpan c_RentByTime, c_OriginalRentTime;
		private Intu c_Murderers;

		public Point3D BanLoc
		{
			get { return c_BanLoc; }
			set
			{
				c_BanLoc = value;
				InvalidateProperties();
				if (Owned)
					House.Region.GoLocation = value;
			}
		}

		public Point3D SignLoc
		{
			get { return c_SignLoc; }
			set
			{
				c_SignLoc = value;
				InvalidateProperties();

				if (Owned)
				{
					House.Sign.Location = value;
					House.Hanger.Location = value;
				}
			}
		}

		public int Locks
		{
			get { return c_Locks; }
			set
			{
				c_Locks = value;
				InvalidateProperties();
				if (Owned)
					House.MaxLockDowns = value;
			}
		}

		public int Secures
		{
			get { return c_Secures; }
			set
			{
				c_Secures = value;
				InvalidateProperties();
				if (Owned)
					House.MaxSecures = value;
			}
		}

		public int Price
		{
			get { return c_Price; }
			set
			{
				c_Price = value;
				InvalidateProperties();
			}
		}

		public int MinZ
		{
			get { return c_MinZ; }
			set
			{
				if (value > c_MaxZ)
					c_MaxZ = value + 1;

				c_MinZ = value;
				if (Owned)
					UpdateRegion();
			}
		}

		public int MaxZ
		{
			get { return c_MaxZ; }
			set
			{
				if (value < c_MinZ)
					value = c_MinZ;

				c_MaxZ = value;
				if (Owned)
					UpdateRegion();
			}
		}

		public int MinTotalSkill
		{
			get { return c_MinTotalSkill; }
			set
			{
				if (value > c_MaxTotalSkill)
					value = c_MaxTotalSkill;

				c_MinTotalSkill = value;
				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public int MaxTotalSkill
		{
			get { return c_MaxTotalSkill; }
			set
			{
				if (value < c_MinTotalSkill)
					value = c_MinTotalSkill;

				c_MaxTotalSkill = value;
				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public bool YoungOnly
		{
			get { return c_YoungOnly; }
			set
			{
				c_YoungOnly = value;

				if (c_YoungOnly)
					c_Murderers = Intu.Neither;

				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public DateTime RentTime { get; private set; }

		public TimeSpan RentByTime
		{
			get { return c_RentByTime; }
			set
			{
				c_RentByTime = value;
				c_OriginalRentTime = value;

				if (value == TimeSpan.Zero)
					ClearRentTimer();
				else
				{
					ClearRentTimer();
					BeginRentTimer(value);
				}

				InvalidateProperties();
			}
		}

		public bool RecurRent
		{
			get { return c_RecurRent; }
			set
			{
				c_RecurRent = value;

				if (!value)
					c_RentToOwn = value;

				InvalidateProperties();
			}
		}

		public bool KeepItems
		{
			get { return c_KeepItems; }
			set
			{
				c_LeaveItems = false;
				c_KeepItems = value;
				InvalidateProperties();
			}
		}

		public bool Free
		{
			get { return c_Free; }
			set
			{
				c_Free = value;
				c_Price = 1;
				InvalidateProperties();
			}
		}

		public Intu Murderers
		{
			get { return c_Murderers; }
			set
			{
				c_Murderers = value;

				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public ArrayList Blocks { get; set; }

		public string Skill
		{
			get { return c_Skill; }
			set
			{
				c_Skill = value;
				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public double SkillReq
		{
			get { return c_SkillReq; }
			set
			{
				c_SkillReq = value;
				ValidateOwnership();
				InvalidateProperties();
			}
		}

		public bool LeaveItems
		{
			get { return c_LeaveItems; }
			set
			{
				c_LeaveItems = value;
				InvalidateProperties();
			}
		}

		public bool RentToOwn
		{
			get { return c_RentToOwn; }
			set
			{
				c_RentToOwn = value;
				InvalidateProperties();
			}
		}

		public bool Relock { get; set; }

		public int ItemsPrice
		{
			get { return c_ItemsPrice; }
			set
			{
				c_ItemsPrice = value;
				InvalidateProperties();
			}
		}

		public TownHouse House { get; set; }

		public Timer DemolishTimer { get; private set; }
		public DateTime DemolishTime { get; private set; }

		public bool Owned { get { return House != null && !House.Deleted; } }
		public int Floors { get { return (c_MaxZ - c_MinZ) / 20 + 1; } }

		public bool BlocksReady { get { return Blocks.Count != 0; } }
		public bool FloorsReady { get { return (BlocksReady && MinZ != Int16.MinValue); } }
		public bool SignReady { get { return (FloorsReady && SignLoc != Point3D.Zero); } }
		public bool BanReady { get { return (SignReady && BanLoc != Point3D.Zero); } }
		public bool LocSecReady { get { return (BanReady && Locks != 0 && Secures != 0); } }
		public bool ItemsReady { get { return LocSecReady; } }
		public bool LengthReady { get { return ItemsReady; } }
		public bool PriceReady { get { return (LengthReady && Price != 0); } }

		public string PriceType
		{
			get
			{
				if (c_RentByTime == TimeSpan.Zero)
					return "Sprzedaz";
				if (c_RentByTime == TimeSpan.FromDays(1))
					return "Dziennie";
				if (c_RentByTime == TimeSpan.FromDays(7))
					return "Tygodniowo";
				if (c_RentByTime == TimeSpan.FromDays(30))
					return "Miesiecznie";

				return "Sprzedaz";
			}
		}

		public string PriceTypeShort
		{
			get
			{
				if (c_RentByTime == TimeSpan.Zero)
					return "Sprzedaz";
				if (c_RentByTime == TimeSpan.FromDays(1))
					return "Dzien";
				if (c_RentByTime == TimeSpan.FromDays(7))
					return "Tydzien";
				if (c_RentByTime == TimeSpan.FromDays(30))
					return "Miesiac";

				return "Sprzedaz";
			}
		}

		[Constructable]
		public TownHouseSign() : base(0xC0B)
		{
			Name = "Na sprzedaz";
			Movable = false;

			c_BanLoc = Point3D.Zero;
			c_SignLoc = Point3D.Zero;
			c_Skill = "";
			Blocks = new ArrayList();
			c_DecoreItemInfos = new ArrayList();
			c_PreviewItems = new ArrayList();
			DemolishTime = DateTime.Now;
			RentTime = DateTime.Now;
			c_RentByTime = TimeSpan.Zero;
			c_RecurRent = true;

			c_MinZ = Int16.MinValue;
			c_MaxZ = Int16.MaxValue;

			AllSigns.Add(this);
		}

		private void SearchForHouse()
		{
			foreach (TownHouse house in TownHouse.AllTownHouses)
				if (house.ForSaleSign == this)
					House = house;
		}

		public void UpdateBlocks()
		{
			if (!Owned)
				return;

			if (Blocks.Count == 0)
				UnconvertDoors();

			UpdateRegion();
			ConvertItems(false);
			House.InitSectorDefinition();
		}

		public void UpdateRegion()
		{
			if (House == null)
				return;

			this.House.UpdateRegion();

			Rectangle3D rect = new Rectangle3D(Point3D.Zero, Point3D.Zero);

			for (int i = 0; i < House.Region.Area.Length; ++i)
			{
				rect = House.Region.Area[i];
				rect = new Rectangle3D(new Point3D(rect.Start.X - House.X, rect.Start.Y - House.Y, MinZ),
					new Point3D(rect.End.X - House.X, rect.End.Y - House.Y, MaxZ));
				House.Region.Area[i] = rect;
			}

			House.Region.Unregister();
			House.Region.Register();
			House.Region.GoLocation = BanLoc;
		}

		public void ShowAreaPreview(Mobile m)
		{
			ClearPreview();

			Point2D point = Point2D.Zero;
			ArrayList blocks = new ArrayList();

			foreach (Rectangle2D rect in Blocks)
				for (int x = rect.Start.X; x < rect.End.X; ++x)
				for (int y = rect.Start.Y; y < rect.End.Y; ++y)
				{
					point = new Point2D(x, y);
					if (!blocks.Contains(point))
						blocks.Add(point);
				}

			if (blocks.Count > 500)
			{
				m.SendMessage("Due to size of the area, skipping the preview.");
				return;
			}

			Item item = null;
			int avgz = 0;
			foreach (Point2D p in blocks)
			{
				avgz = Map.GetAverageZ(p.X, p.Y);

				item = new Item(0x1766);
				item.Name = "Area Preview";
				item.Movable = false;
				item.Location = new Point3D(p.X, p.Y, (avgz <= m.Z ? m.Z + 2 : avgz + 2));
				item.Map = Map;

				c_PreviewItems.Add(item);
			}

			c_PreviewTimer = Timer.DelayCall(TimeSpan.FromSeconds(100), ClearPreview);
		}

		public void ShowSignPreview()
		{
			ClearPreview();

			bool northSouth = (ItemID == (int)SignIDs.TownHouseSignNS);

			int signId = (int)(northSouth ? SignIDs.HouseSignNS : SignIDs.HouseSignWE);
			Item sign = new Item(signId);
			sign.Name = "Sign Preview";
			sign.Movable = false;
			sign.Location = SignLoc;
			sign.Map = Map;

			c_PreviewItems.Add(sign);

			int hangerId = (int)(northSouth ? SignIDs.SignHangerNS : SignIDs.SignHangerWE);
			sign = new Item(hangerId);
			sign.Name = "Sign Preview";
			sign.Movable = false;
			sign.Location = SignLoc;
			sign.Map = Map;

			c_PreviewItems.Add(sign);

			c_PreviewTimer = Timer.DelayCall(TimeSpan.FromSeconds(100), ClearPreview);
		}

		public void ShowBanPreview()
		{
			ClearPreview();

			Item ban = new Item(0x17EE);
			ban.Name = "Ban Loc Preview";
			ban.Movable = false;
			ban.Location = BanLoc;
			ban.Map = Map;

			c_PreviewItems.Add(ban);

			c_PreviewTimer = Timer.DelayCall(TimeSpan.FromSeconds(100), ClearPreview);
		}

		public void ShowFloorsPreview(Mobile m)
		{
			ClearPreview();

			Item item = new Item(0x7BD);
			item.Name = "Bottom Floor Preview";
			item.Movable = false;
			item.Location = m.Location;
			item.Z = c_MinZ;
			item.Map = Map;

			c_PreviewItems.Add(item);

			item = new Item(0x7BD);
			item.Name = "Top Floor Preview";
			item.Movable = false;
			item.Location = m.Location;
			item.Z = c_MaxZ;
			item.Map = Map;

			c_PreviewItems.Add(item);

			c_PreviewTimer = Timer.DelayCall(TimeSpan.FromSeconds(100), ClearPreview);
		}

		public void ClearPreview()
		{
			foreach (Item item in new ArrayList(c_PreviewItems))
			{
				c_PreviewItems.Remove(item);
				item.Delete();
			}

			if (c_PreviewTimer != null)
				c_PreviewTimer.Stop();

			c_PreviewTimer = null;
		}

		public void Purchase(Mobile m)
		{
			Purchase(m, false);
		}

		public void Purchase(Mobile m, bool sellitems)
		{
			try
			{
				if (Owned)
				{
					m.SendMessage("Ktos juz kupil ten dom!");
					return;
				}

				if (!PriceReady)
				{
					m.SendMessage("Ten dom nie jest jeszcze gotowy do zakupu.");
					return;
				}

				int price = c_Price + (sellitems ? c_ItemsPrice : 0);

				if (c_Free)
					price = 0;

				if (m.AccessLevel == AccessLevel.Player && !Banker.Withdraw(m, price))
				{
					m.SendMessage("Nie masz tyle pieniedzy w banku, aby wynajac ten dom.");
					return;
				}
				
				foreach (Rectangle2D rect in Blocks)
				{
					var doorOpen = false;
					var eable = Map.GetItemsInBounds(rect.ExtendedBy(1));
					foreach (var item in eable)
					{
						if (item is BaseDoor door && door.Open)
						{
							doorOpen = true;
							break;
						}
					}
					eable.Free();
					if (doorOpen)
					{
						m.SendMessage("Musisz zamknac wszystkie drzwi w domu przed zakupem.");
						return;
					}
				}

				if (m.AccessLevel == AccessLevel.Player)
					m.SendLocalizedMessage(1060398,
						price.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.

				Visible = false;

				int minX = ((Rectangle2D)Blocks[0]).Start.X;
				int minY = ((Rectangle2D)Blocks[0]).Start.Y;
				int maxX = ((Rectangle2D)Blocks[0]).End.X;
				int maxY = ((Rectangle2D)Blocks[0]).End.Y;

				foreach (Rectangle2D rect in Blocks)
				{
					if (rect.Start.X < minX)
						minX = rect.Start.X;
					if (rect.Start.Y < minY)
						minY = rect.Start.Y;
					if (rect.End.X > maxX)
						maxX = rect.End.X;
					if (rect.End.Y > maxY)
						maxY = rect.End.Y;
				}

				bool northSouth = (ItemID == (int)SignIDs.TownHouseSignNS);

				House = new TownHouse(m, this, c_Locks, c_Secures);
				int signId = (int)(northSouth ? SignIDs.HouseSignNS : SignIDs.HouseSignWE);
				House.ChangeSignType(signId);
				
				House.Components.Resize(maxX - minX, maxY - minY);
				House.Components.Add(0x520, House.Components.Width - 1, House.Components.Height - 1, -5);
				
				House.Location = new Point3D(minX, minY, c_MinZ);
				House.Map = Map;
				House.Region.GoLocation = c_BanLoc;
				House.Sign.Location = c_SignLoc;

				int hangerId = (int)(northSouth ? SignIDs.SignHangerNS : SignIDs.SignHangerWE);
				House.Hanger = new Item(hangerId);
				House.Hanger.Location = c_SignLoc;
				House.Hanger.Map = Map;
				House.Hanger.Movable = false;

				if (c_ForcePublic)
					House.Public = true;

				House.Price = (RentByTime == TimeSpan.FromDays(0) ? c_Price : 1);

				UpdateRegion();

				if (House.Price == 0)
					House.Price = 1;

				if (c_RentByTime != TimeSpan.Zero)
					BeginRentTimer(c_RentByTime);

				c_RTOPayments = 1;

				HideOtherSigns();

				c_DecoreItemInfos = new ArrayList();

				ConvertItems(sellitems);
			}
			catch (Exception e)
			{
				Errors.Report("Wystapil blad. Skontaktuj sie z administracja.");
				Console.WriteLine(e.Message);
				Console.WriteLine(e.Source);
				Console.WriteLine(e.StackTrace);
			}
		}

		private void HideOtherSigns()
		{
			var eable = House.Sign.GetItemsInRange(0);
			foreach (Item item in eable)
				if (!(item is HouseSign))
					if (item.ItemID == 0xB95
					    || item.ItemID == 0xB96
					    || item.ItemID == 0xC43
					    || item.ItemID == 0xC44
					    || (item.ItemID > 0xBA3 && item.ItemID < 0xC0E))
						item.Visible = false;
			eable.Free();
		}

		public virtual void ConvertItems(bool keep)
		{
			if (House == null)
				return;

			ArrayList items = new ArrayList();
			foreach (Rectangle2D rect in Blocks)
			{
				var eable = Map.GetItemsInBounds(rect);
				foreach (Item item in eable)
					if (House.Region.Contains(item.Location) && item.RootParent == null && !items.Contains(item))
						items.Add(item);
				eable.Free();
			}

			foreach (Item item in new ArrayList(items))
			{
				if (item is HouseSign
				    || item is BaseMulti
				    || item is BaseAddon
				    || item is AddonComponent
				    || item == House.Hanger
				    || !item.Visible
				    || item.IsLockedDown
				    || item.IsSecure
				    || item.Movable
				    || c_PreviewItems.Contains(item))
					continue;

				if (item is BaseDoor)
					ConvertDoor((BaseDoor)item);
				else if (!c_LeaveItems)
				{
					c_DecoreItemInfos.Add(new DecoreItemInfo(item.GetType().ToString(), item.Name, item.ItemID,
						item.Hue, item.Location, item.Map));

					if (!c_KeepItems || !keep)
						item.Delete();
					else
					{
						item.Movable = true;
						House.LockDown(House.Owner, item, false);
					}
				}
			}
		}

		protected void ConvertDoor(BaseDoor door)
		{
			if (!Owned)
				return;

			if (door is ISecurable)
			{
				door.Locked = false;
				House.Doors.Add(door);
				return;
			}

			door.Open = false;

			GenericHouseDoor newdoor =
				new GenericHouseDoor(0, door.ClosedID, door.OpenedSound, door.ClosedSound);
			newdoor.Offset = door.Offset;
			newdoor.ClosedID = door.ClosedID;
			newdoor.OpenedID = door.OpenedID;
			newdoor.Location = door.Location;
			newdoor.Map = door.Map;

			door.Delete();

			var eable = newdoor.GetItemsInRange(1);
			foreach (Item inneritem in eable)
			{
				if (inneritem is BaseDoor && inneritem != newdoor && inneritem.Z == newdoor.Z)
				{
					((BaseDoor)inneritem).Link = newdoor;
					newdoor.Link = (BaseDoor)inneritem;
				}
			}
			eable.Free();

			House.Doors.Add(newdoor);
		}

		public virtual void UnconvertDoors()
		{
			if (House == null)
				return;

			BaseDoor newdoor = null;

			foreach (BaseDoor door in new ArrayList(House.Doors))
			{
				door.Open = false;

				if (Relock)
					door.Locked = true;

				newdoor = new StrongWoodDoor(0);
				newdoor.ItemID = door.ItemID;
				newdoor.ClosedID = door.ClosedID;
				newdoor.OpenedID = door.OpenedID;
				newdoor.OpenedSound = door.OpenedSound;
				newdoor.ClosedSound = door.ClosedSound;
				newdoor.Offset = door.Offset;
				newdoor.Location = door.Location;
				newdoor.Map = door.Map;

				door.Delete();

				var eable = newdoor.GetItemsInRange(1);
				foreach (Item inneritem in eable)
				{
					if (inneritem is BaseDoor && inneritem != newdoor && inneritem.Z == newdoor.Z)
					{
						((BaseDoor)inneritem).Link = newdoor;
						newdoor.Link = (BaseDoor)inneritem;
					}
				}
				eable.Free();

				House.Doors.Remove(door);
			}
		}

		public void RecreateItems()
		{
			Item item = null;
			foreach (DecoreItemInfo info in c_DecoreItemInfos)
			{
				item = null;

				if (info.TypeString.ToLower().IndexOf("static") != -1)
					item = new Static(info.ItemID);
				else
				{
					try
					{
						item = Activator.CreateInstance(ScriptCompiler.FindTypeByFullName(info.TypeString)) as Item;
					}
					catch { continue; }
				}

				if (item == null)
					continue;

				item.ItemID = info.ItemID;
				item.Name = info.Name;
				item.Hue = info.Hue;
				item.Location = info.Location;
				item.Map = info.Map;
				item.Movable = false;
			}
		}

		public virtual void ClearHouse()
		{
			UnconvertDoors();
			ClearDemolishTimer();
			ClearRentTimer();
			PackUpItems(House);
			RecreateItems();
			House = null;
			Visible = true;

			if (c_RentToOwn)
				c_RentByTime = c_OriginalRentTime;
		}

		public virtual void ValidateOwnership()
		{
			if (!Owned)
				return;

			if (House.Owner == null)
			{
				House.Delete();
				return;
			}

			if (House.Owner.AccessLevel != AccessLevel.Player)
				return;

			if (!CanBuyHouse(House.Owner) && DemolishTimer == null)
				BeginDemolishTimer();
			else
				ClearDemolishTimer();
		}

		public int CalcVolume()
		{
			int floors = 1;
			if (c_MaxZ - c_MinZ < 100)
				floors = 1 + Math.Abs((c_MaxZ - c_MinZ) / 20);

			Point3D point = Point3D.Zero;
			ArrayList blocks = new ArrayList();

			foreach (Rectangle2D rect in Blocks)
				for (int x = rect.Start.X; x < rect.End.X; ++x)
				for (int y = rect.Start.Y; y < rect.End.Y; ++y)
				for (int z = 0; z < floors; z++)
				{
					point = new Point3D(x, y, z);
					if (!blocks.Contains(point))
						blocks.Add(point);
				}

			return blocks.Count;
		}

		private void StartTimers()
		{
			if (DemolishTime > DateTime.Now)
				BeginDemolishTimer(DemolishTime - DateTime.Now);
			else if (c_RentByTime != TimeSpan.Zero && c_RentTimer == null)
				BeginRentTimer(c_RentByTime);
		}

		#region Demolish

		public void ClearDemolishTimer()
		{
			if (DemolishTimer == null)
				return;

			DemolishTimer.Stop();
			DemolishTimer = null;
			DemolishTime = DateTime.Now;

			if (!House.Deleted && Owned)
				House.Owner.SendMessage("Wyburzanie anulowane.");
		}

		public void CheckDemolishTimer()
		{
			if (DemolishTimer == null || !Owned)
				return;

			DemolishAlert();
		}

		protected void BeginDemolishTimer()
		{
			BeginDemolishTimer(TimeSpan.FromHours(24));
		}

		protected void BeginDemolishTimer(TimeSpan time)
		{
			if (!Owned)
				return;

			DemolishTime = DateTime.Now + time;
			DemolishTimer = Timer.DelayCall(time, PackUpHouse);

			DemolishAlert();
		}

		protected virtual void DemolishAlert()
		{
			House.Owner.SendMessage("Juz nie spelniasz wymagan na ten dom, zostaniesz eksmitowany za {0}h:{1}m:{2}s.",
				(DemolishTime - DateTime.Now).Hours, (DemolishTime - DateTime.Now).Minutes,
				(DemolishTime - DateTime.Now).Seconds);
		}

		protected void PackUpHouse()
		{
			if (!Owned || House.Deleted)
				return;

			PackUpItems(House);

			House.Owner.BankBox.DropItem(new BankCheck(House.Price));

			House.Delete();
		}

		public static void PackUpItems(BaseHouse house)
		{
			if (house == null)
				return;


			Container bag = new Bag();
			bag.Name = "Przedmioty z domu miejskiego";

			foreach (KeyValuePair<Item, Mobile> kvp in new Dictionary<Item, Mobile>(house.LockDowns))
			{
				Item item = kvp.Key;
				item.IsLockedDown = false;
				item.Movable = true;
				house.LockDowns.Remove(kvp.Key);
				bag.DropItem(item);
			}

			foreach (SecureInfo info in new ArrayList(house.Secures))
			{
				info.Item.IsLockedDown = false;
				info.Item.IsSecure = false;
				info.Item.Movable = true;
				info.Item.SetLastMoved();
				house.Secures.Remove(info);
				bag.DropItem(info.Item);
			}

			ArrayList unlockedItemsToPack = new ArrayList();
			var addonsToPack = new List<BaseAddon>();
			foreach (Rectangle2D rect in house.Area)
			{
				var eable = house.Map.GetItemsInBounds(rect);
				foreach (Item item in eable)
				{
					if (item is BaseAddon ba)
					{
						addonsToPack.Add(ba);
						continue;
					}

					if (item is HouseSign
					    || item is BaseDoor
					    || item is BaseMulti
					    || item is AddonComponent
					    || !item.Visible
					    || item.IsLockedDown
					    || item.IsSecure
					    || !item.Movable
					    || item.Map != house.Map
					    || !house.Region.Contains(item.Location))
						continue;


					unlockedItemsToPack.Add(item);
				}
				eable.Free();
			}

			foreach (Item item in unlockedItemsToPack)
			{
				bag.DropItem(item);
			}
			
			foreach (var addon in addonsToPack)
			{
				bag.DropItem(addon.Deed);
				addon.Delete();
			}

			if (bag.Items.Count == 0)
			{
				bag.Delete();
				return;
			}

			house.Owner.BankBox.DropItem(bag);
		}

		#endregion

		#region Rent

		public void ClearRentTimer()
		{
			if (c_RentTimer != null)
			{
				c_RentTimer.Stop();
				c_RentTimer = null;
			}

			RentTime = DateTime.Now;
		}

		private void BeginRentTimer()
		{
			BeginRentTimer(TimeSpan.FromDays(1));
		}

		private void BeginRentTimer(TimeSpan time)
		{
			if (!Owned)
				return;

			c_RentTimer = Timer.DelayCall(time, RentDue);
			RentTime = DateTime.Now + time;
		}

		public void CheckRentTimer()
		{
			if (c_RentTimer == null || !Owned)
				return;

			House.Owner.SendMessage("Cykl wynajmu konczy sie za {0} dni, {1}h:{2}m:{3}s.",
				(RentTime - DateTime.Now).Days, (RentTime - DateTime.Now).Hours,
				(RentTime - DateTime.Now).Minutes, (RentTime - DateTime.Now).Seconds);
		}

		private void RentDue()
		{
			if (!Owned || House.Owner == null)
				return;

			if (!c_RecurRent)
			{
				House.Owner.SendMessage("Twoj kontrakt wynajmu minal, wiec bank zajal twoj dom miejski.");
				PackUpHouse();
				return;
			}

			if (!c_Free && House.Owner.AccessLevel == AccessLevel.Player &&
			    !Banker.Withdraw(House.Owner, c_Price))
			{
				House.Owner.SendMessage("Nie stac cie na wynajem, wiec bank zajal twoj dom miejski.");
				PackUpHouse();
				return;
			}

			if (!c_Free)
				House.Owner.SendMessage("Bank pobral {0} sztuk zlota za wynajem domu miejskiego.", c_Price);

			OnRentPaid();

			if (c_RentToOwn)
			{
				c_RTOPayments++;

				bool complete = false;

				if (c_RentByTime == TimeSpan.FromDays(1) && c_RTOPayments >= 60)
				{
					complete = true;
					House.Price = c_Price * 60;
				}

				if (c_RentByTime == TimeSpan.FromDays(7) && c_RTOPayments >= 9)
				{
					complete = true;
					House.Price = c_Price * 9;
				}

				if (c_RentByTime == TimeSpan.FromDays(30) && c_RTOPayments >= 2)
				{
					complete = true;
					House.Price = c_Price * 2;
				}

				if (complete)
				{
					House.Owner.SendMessage("Od teraz wynajmujesz ten dom.");
					c_RentByTime = TimeSpan.FromDays(0);
					return;
				}
			}

			BeginRentTimer(c_RentByTime);
		}

		protected virtual void OnRentPaid()
		{
		}

		public void NextPriceType()
		{
			if (c_RentByTime == TimeSpan.Zero)
				RentByTime = TimeSpan.FromDays(1);
			else if (c_RentByTime == TimeSpan.FromDays(1))
				RentByTime = TimeSpan.FromDays(7);
			else if (c_RentByTime == TimeSpan.FromDays(7))
				RentByTime = TimeSpan.FromDays(30);
			else
				RentByTime = TimeSpan.Zero;
		}

		public void PrevPriceType()
		{
			if (c_RentByTime == TimeSpan.Zero)
				RentByTime = TimeSpan.FromDays(30);
			else if (c_RentByTime == TimeSpan.FromDays(30))
				RentByTime = TimeSpan.FromDays(7);
			else if (c_RentByTime == TimeSpan.FromDays(7))
				RentByTime = TimeSpan.FromDays(1);
			else
				RentByTime = TimeSpan.Zero;
		}

		#endregion

		public bool CanBuyHouse(Mobile m)
		{
			if (c_Skill != "")
			{
				try
				{
					SkillName index = (SkillName)Enum.Parse(typeof(SkillName), c_Skill, true);
					if (m.Skills[index].Value < c_SkillReq)
						return false;
				}
				catch
				{
					return false;
				}
			}

			if (c_MinTotalSkill != 0 && m.SkillsTotal / 10 < c_MinTotalSkill)
				return false;

			if (c_MaxTotalSkill != 0 && m.SkillsTotal / 10 > c_MaxTotalSkill)
				return false;

			if (c_YoungOnly && m.Player && !((PlayerMobile)m).Young)
				return false;

			if (c_Murderers == Intu.Yes && m.Kills < 5)
				return false;

			if (c_Murderers == Intu.No && m.Kills >= 5)
				return false;

			var houseFaction = NelderimRegionSystem.GetRegion(GetRegion().Name).GetFaction();
			if (houseFaction != Faction.None)
			{
				return m.Faction == houseFaction;
			}

			return true;
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (m.AccessLevel != AccessLevel.Player)
				new TownHouseSetupGump(m, this);
			else if (!Visible)
				return;
			else if (!CanBuyHouse(m))
				m.SendMessage("Nie spelniasz wymagan by kupic ten dom.");
			else if (BaseHouse.HasHouse(m))
				m.SendMessage("Nie mozesz miec wiecej domow.");
			else
				new TownHouseConfirmGump(m, this);
		}

		public override void Delete()
		{
			if (House == null || House.Deleted)
				base.Delete();
			else
				PublicOverheadMessage(MessageType.Regular, 0x0, true,
					"Nie mozesz tego usunac, dopoki dom ma wlasciciela.");

			if (this.Deleted)
				AllSigns.Remove(this);
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (c_Free)
				list.Add(1060658, "Cena\tDarmowy");
			else if (c_RentByTime == TimeSpan.Zero)
				list.Add(1060658, "Cena\t{0}{1}", c_Price, c_KeepItems ? " (+" + c_ItemsPrice + " za przedmioty)" : "");
			else if (c_RecurRent)
				list.Add(1060658, "{0}\t{1}\r{2}", PriceType + (c_RentToOwn ? " Wynajem na wlasnosc" : " Cykliczny"),
					c_Price, c_KeepItems ? " (+" + c_ItemsPrice + " za przedmioty)" : "");
			else
				list.Add(1060658, "Jeden {0}\t{1}{2}", PriceTypeShort, c_Price,
					c_KeepItems ? " (+" + c_ItemsPrice + " za przedmioty)" : "");

			list.Add(1060659, "Zablokowanych\t{0}", c_Locks);
			list.Add(1060660, "Zabepizeczonych\t{0}", c_Secures);

			if (c_SkillReq != 0.0)
				list.Add(1060661, "Wymaga\t{0}", c_SkillReq + " w " + c_Skill);
			if (c_MinTotalSkill != 0)
				list.Add(1060662, "Wymaga ponad \t{0} wszystkich skilli", c_MinTotalSkill);
			if (c_MaxTotalSkill != 0)
				list.Add(1060663, "Wymaga mniej niz \t{0} wszystkich skilli", c_MaxTotalSkill);

			if (c_YoungOnly)
				list.Add(1063483, "Musisz miec status\tMlody");
			else if (c_Murderers == Intu.Yes)
				list.Add(1063483, "Musisz byc\tmorderca");
			else if (c_Murderers == Intu.No)
				list.Add(1063483, "Musisz byc\tniewinny");
		}

		public TownHouseSign(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(13);

			// Version 13

			writer.Write(c_ForcePrivate);
			writer.Write(c_ForcePublic);
			writer.Write(c_NoTrade);

			// Version 12

			writer.Write(c_Free);

			// Version 11

			writer.Write((int)c_Murderers);

			// Version 10

			writer.Write(c_LeaveItems);

			// Version 9
			writer.Write(c_RentToOwn);
			writer.Write(c_OriginalRentTime);
			writer.Write(c_RTOPayments);

			// Version 7
			writer.WriteItemList(c_PreviewItems, true);

			// Version 6
			writer.Write(c_ItemsPrice);
			writer.Write(c_KeepItems);

			// Version 5
			writer.Write(c_DecoreItemInfos.Count);
			foreach (DecoreItemInfo info in c_DecoreItemInfos)
				info.Save(writer);

			writer.Write(Relock);

			// Version 4
			writer.Write(c_RecurRent);
			writer.Write(c_RentByTime);
			writer.Write(RentTime);
			writer.Write(DemolishTime);
			writer.Write(c_YoungOnly);
			writer.Write(c_MinTotalSkill);
			writer.Write(c_MaxTotalSkill);

			// Version 3
			writer.Write(c_MinZ);
			writer.Write(c_MaxZ);

			// Version 2
			writer.Write(House);

			// Version 1
			writer.Write(c_Price);
			writer.Write(c_Locks);
			writer.Write(c_Secures);
			writer.Write(c_BanLoc);
			writer.Write(c_SignLoc);
			writer.Write(c_Skill);
			writer.Write(c_SkillReq);
			writer.Write(Blocks.Count);
			foreach (Rectangle2D rect in Blocks)
				writer.Write(rect);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version >= 13)
			{
				c_ForcePrivate = reader.ReadBool();
				c_ForcePublic = reader.ReadBool();
				c_NoTrade = reader.ReadBool();
			}

			if (version >= 12)
				c_Free = reader.ReadBool();

			if (version >= 11)
				c_Murderers = (Intu)reader.ReadInt();

			if (version >= 10)
				c_LeaveItems = reader.ReadBool();

			if (version >= 9)
			{
				c_RentToOwn = reader.ReadBool();
				c_OriginalRentTime = reader.ReadTimeSpan();
				c_RTOPayments = reader.ReadInt();
			}

			c_PreviewItems = new ArrayList();
			if (version >= 7)
				c_PreviewItems = reader.ReadItemList();

			if (version >= 6)
			{
				c_ItemsPrice = reader.ReadInt();
				c_KeepItems = reader.ReadBool();
			}

			c_DecoreItemInfos = new ArrayList();
			if (version >= 5)
			{
				int decorecount = reader.ReadInt();
				DecoreItemInfo info;
				for (int i = 0; i < decorecount; ++i)
				{
					info = new DecoreItemInfo();
					info.Load(reader);
					c_DecoreItemInfos.Add(info);
				}

				Relock = reader.ReadBool();
			}

			if (version >= 4)
			{
				c_RecurRent = reader.ReadBool();
				c_RentByTime = reader.ReadTimeSpan();
				RentTime = reader.ReadDateTime();
				DemolishTime = reader.ReadDateTime();
				c_YoungOnly = reader.ReadBool();
				c_MinTotalSkill = reader.ReadInt();
				c_MaxTotalSkill = reader.ReadInt();
			}

			if (version >= 3)
			{
				c_MinZ = reader.ReadInt();
				c_MaxZ = reader.ReadInt();
			}

			if (version >= 2)
				House = (TownHouse)reader.ReadItem();

			c_Price = reader.ReadInt();
			c_Locks = reader.ReadInt();
			c_Secures = reader.ReadInt();
			c_BanLoc = reader.ReadPoint3D();
			c_SignLoc = reader.ReadPoint3D();
			c_Skill = reader.ReadString();
			c_SkillReq = reader.ReadDouble();

			Blocks = new ArrayList();
			int count = reader.ReadInt();
			for (int i = 0; i < count; ++i)
				Blocks.Add(reader.ReadRect2D());

			if (RentTime > DateTime.Now)
				BeginRentTimer(RentTime - DateTime.Now);

			Timer.DelayCall(TimeSpan.Zero, StartTimers);

			ClearPreview();

			AllSigns.Add(this);
		}
	}
}
