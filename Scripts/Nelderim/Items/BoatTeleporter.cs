using System.Collections.Generic;
using Server.Multis;

namespace Server.Items
{
	public class BoatTeleporter : Teleporter
	{
		private BoatTeleportRegion m_Region;
		private int _RegionRange = 2;
		private bool _showBounds;

		[Constructable]
		public BoatTeleporter()
		{
		}

		private void RegisterRegion()
		{
			m_Region = new BoatTeleportRegion(this, GetRegionBounds());
			m_Region.Register();
		}

		private Rectangle2D GetRegionBounds()
		{
			return new Rectangle2D(Location.X - _RegionRange, Location.Y - _RegionRange, _RegionRange * 2, _RegionRange * 2);
		}
		
		private void UnregisterRegion()
		{
			if (m_Region != null)
			{
				m_Region.ClearBounds();
				m_Region.Unregister();
				m_Region = null;
			}
		}

		private void UpdateBounds()
		{
			if (m_Region != null)
			{
				if (_showBounds)
					m_Region.MarkBounds();
				else
					m_Region.ClearBounds();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool ShowBounds
		{
			get => _showBounds;
			set
			{
				_showBounds = value;
				UpdateBounds();				
			}
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int RegionRange
		{
			get => _RegionRange;
			set
			{
				UnregisterRegion();
				_RegionRange = value;
				RegisterRegion();
				UpdateBounds();
			}
		}

		public override bool IsVirtualItem => true;

		public override void OnLocationChange(Point3D oldLocation)
		{
			UnregisterRegion();
			RegisterRegion();
			UpdateBounds();
		}

		public override void OnMapChange()
		{
			UnregisterRegion();
			RegisterRegion();
			UpdateBounds();
		}

		public override void OnDelete()
		{
			base.OnDelete();
			UnregisterRegion();
		}

		[CommandProperty(AccessLevel.GameMaster, true)]
		public override Map MapDest => Map;

		public override bool OnMoveOver(Mobile m)
		{
			return true; //Do nothing
		}

		public BoatTeleporter(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write(_RegionRange);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			_RegionRange = reader.ReadInt();
			RegisterRegion();
		}
	}

	public class BoatTeleportRegion : Region
	{
		private BoatTeleporter _Teleporter;
		private List<Item> m_Markers;

		public BoatTeleportRegion(BoatTeleporter teleporter, Rectangle2D rec)
			: base($"BoatTeleportRegion-{teleporter.Serial}", teleporter.Map, DefaultPriority, rec)
		{
			_Teleporter = teleporter;
		}

		public void MarkBounds()
		{
			ClearBounds();
			m_Markers = new List<Item>();

			int w = Bounds.X + Bounds.Width;
			int h = Bounds.Y + Bounds.Height;

			for (int x = Bounds.X; x <= w; x++)
			{
				for (int y = Bounds.Y; y <= h; y++)
				{
					if (x == Bounds.X || x == Bounds.X + Bounds.Width || y == Bounds.Y || y == Bounds.Y + Bounds.Height)
					{
						MarkerItem i = new MarkerItem();
						i.MoveToWorld(new Point3D(x, y, -5), Map);
						m_Markers.Add(i);
					}
				}
			}
		}

		public void ClearBounds()
		{
			if (m_Markers == null)
				return;

			foreach (Item i in m_Markers)
				i.Delete();

			m_Markers.Clear();
		}

		public override void OnUnregister()
		{
			ClearBounds();
		}

		public void CheckEnter(BaseBoat boat)
		{
			if (boat == null || Map == null || Map == Map.Internal)
				return;
			
			Region r = Find(boat.Location, boat.Map);
			if (r != null && !r.IsPartOf(this))
				return;
			
			int x = boat.X - Bounds.Start.X;
			int y = boat.Y - Bounds.Start.Y;
			int z = Map.GetAverageZ(x, y);

			Point3D ePnt = _Teleporter.PointDest;

			int offsetX = ePnt.X - boat.X;
			int offsetY = ePnt.Y - boat.Y;
			int offsetZ = Map.GetAverageZ(ePnt.X, ePnt.Y) - boat.Z;

			if (boat.CanFit(ePnt, Map, boat.ItemID))
			{
				boat.Teleport(offsetX, offsetY, offsetZ);
			}
			else
			{
				boat.StopMove(true);
				boat.SendMessageToAllOnBoard("Lodz utknela na rafie koralowej!");
			}
		}
	}

	public class MarkerItem : Static
	{
		public MarkerItem() : base(0x3709)
		{
			Visible = false;
		}

		public override bool IsVirtualItem => true;

		public MarkerItem(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			Delete();
		}
	}
}
