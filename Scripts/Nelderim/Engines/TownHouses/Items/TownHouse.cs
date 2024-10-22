#region References

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Multis;

#endregion

namespace Knives.TownHouses
{
	public class TownHouse : BaseHouse
	{
		private static TimeSpan TimeUntilCondemned = TimeSpan.FromDays(30); 
		
		public static ArrayList AllTownHouses { get; } = new ArrayList();

		private Item c_Hanger;
		private readonly ArrayList c_Sectors = new ArrayList();

		public TownHouseSign ForSaleSign { get; private set; }

		public override Point3D BaseBanLocation { get { return Point3D.Zero; } }

		public MultiComponentList _Components = new([]);
		public override MultiComponentList Components => _Components;

		public Item Hanger
		{
			get
			{
				if (c_Hanger == null)
				{
					c_Hanger = new Item(0xB98);
					c_Hanger.Movable = false;
					c_Hanger.Location = Sign.Location;
					c_Hanger.Map = Sign.Map;
				}

				return c_Hanger;
			}
			set { c_Hanger = value; }
		}

		public TownHouse(Mobile m, TownHouseSign sign, int locks, int secures) : base(0x1DD6 | 0x4000, m, locks,
			secures)
		{
			ForSaleSign = sign;

			SetSign(0, 0, 0);

			AllTownHouses.Add(this);
		}

		public void InitSectorDefinition()
		{
			if (ForSaleSign == null || ForSaleSign.Blocks.Count == 0)
				return;

			int minX = ((Rectangle2D)ForSaleSign.Blocks[0]).Start.X;
			int minY = ((Rectangle2D)ForSaleSign.Blocks[0]).Start.Y;
			int maxX = ((Rectangle2D)ForSaleSign.Blocks[0]).End.X;
			int maxY = ((Rectangle2D)ForSaleSign.Blocks[0]).End.Y;

			foreach (Rectangle2D rect in ForSaleSign.Blocks)
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

			foreach (Sector sector in c_Sectors)
				sector.OnMultiLeave(this);

			c_Sectors.Clear();
			for (int x = minX; x < maxX; ++x)
			for (int y = minY; y < maxY; ++y)
				if (!c_Sectors.Contains(Map.GetSector(new Point2D(x, y))))
					c_Sectors.Add(Map.GetSector(new Point2D(x, y)));

			foreach (Sector sector in c_Sectors)
				sector.OnMultiEnter(this);
			
			Components.Resize(maxX - minX, maxY - minY);
			Components.Add(0x520, Components.Width - 1, Components.Height - 1, -5);
		}

		public override Rectangle2D[] Area
		{
			get
			{
				if (ForSaleSign == null)
					return new Rectangle2D[100];

				Rectangle2D[] rects = new Rectangle2D[ForSaleSign.Blocks.Count];

				for (int i = 0; i < ForSaleSign.Blocks.Count && i < rects.Length; ++i)
					rects[i] = (Rectangle2D)ForSaleSign.Blocks[i];

				return rects;
			}
		}

		public override bool IsInside(Point3D p, int height)
		{
			if (ForSaleSign == null)
				return false;

			if (Map == null || Region == null)
			{
				Delete();
				return false;
			}

			Sector sector = null;

			try
			{
				if (ForSaleSign is RentalContract && Region.Contains(p))
					return true;

				sector = Map.GetSector(p);

				foreach (BaseMulti m in sector.Multis)
				{
					if (m != this
					    && m is TownHouse
					    && ((TownHouse)m).ForSaleSign is RentalContract
					    && ((TownHouse)m).IsInside(p, height))
						return false;
				}

				return Region.Contains(p);
			}
			catch (Exception e)
			{
				Errors.Report("Error occured in IsInside().  More information on the console.");
				Console.WriteLine("Info:{0}, {1}, {2}", Map, sector, Region,
					sector != null ? "" + sector.Multis : "**");
				Console.WriteLine(e.Source);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				return false;
			}
		}

		public override int GetVendorSystemMaxVendors()
		{
			return 50;
		}

		public override int GetAosMaxSecures()
		{
			return MaxSecures;
		}

		public override int GetAosMaxLockdowns()
		{
			return MaxLockDowns;
		}

		public override void OnMapChange()
		{
			base.OnMapChange();

			if (c_Hanger != null)
				c_Hanger.Map = Map;
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			base.OnLocationChange(oldLocation);

			if (c_Hanger != null)
				c_Hanger.Location = Sign.Location;
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			//
			// Wylaczona obsluga komend glosowych
			//
			//if (e.Mobile != Owner || !IsInside(e.Mobile)) {
			//	return;
			//}

			//if (e.Speech.ToLower() == "check house rent")
			//c_Sign.CheckRentTimer();
		}

		public override void OnDelete()
		{
			if (c_Hanger != null)
				c_Hanger.Delete();

			var eable = Sign.GetItemsInRange(0);
			foreach (Item item in eable)
				if (item != Sign)
					item.Visible = true;
			eable.Free();

			ForSaleSign.ClearHouse();
			Doors.Clear();

			AllTownHouses.Remove(this);

			base.OnDelete();
		}

		public override DecayType DecayType
		{
			get
			{
				if (DateTime.UtcNow - TimeUntilCondemned > Owner.Account?.LastLogin)
				{
					return DecayType.Condemned;
				}

				return base.DecayType;
			}
		}

		public TownHouse(Serial serial)
			: base(serial)
		{
			AllTownHouses.Add(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(3);

			// Version 2

			writer.Write(c_Hanger);

			// Version 1

			writer.Write(ForSaleSign);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version >= 2)
				c_Hanger = reader.ReadItem();

			ForSaleSign = (TownHouseSign)reader.ReadItem();

			if (version <= 2)
			{
				int count = reader.ReadInt();
				for (int i = 0; i < count; ++i)
					reader.ReadRect2D();
			}

			if (Price == 0)
				Price = 1;

			ItemID = 0x1DD6 | 0x4000;
		}
	}
}
