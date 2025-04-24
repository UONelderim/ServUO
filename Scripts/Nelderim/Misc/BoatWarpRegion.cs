using Server.Items;
using Server.Mobiles;
using Server.Multis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Regions
{
    /// <summary>
    /// A region-based system that teleports boats to predefined locations when they enter specific areas.
    /// This implementation works with the existing BaseBoat and BaseGalleon classes without requiring modifications.
    /// </summary>
    public class BoatWarpRegion : Region
    {
        // Static collection of destination points, indexed by destination ID
        private static readonly Dictionary<int, Point3D> _destinationPoints = new Dictionary<int, Point3D>();

        // List of markers showing the region boundaries (when enabled)
        private List<Item> _boundaryMarkers;

        // Index of the destination in WarpLocations array
        private readonly int _destinationIndex;

        // Whether to show visual region boundaries
        private readonly bool _showBoundaries;

        // Timer for periodically checking for boats
        private Timer _checkTimer;

        /// <summary>
        /// Predefined warp locations where boats can be teleported to.
        /// Each rectangle represents a potential destination area.
        /// </summary>
        private static readonly Rectangle2D[] _warpLocations =
        {
            new Rectangle2D(5534, 3655, 10, 10), // Destination 0
            new Rectangle2D(2252, 29, 10, 10)    // Destination 1 (return)
        };

        /// <summary>
        /// Gets the array of warp destination locations.
        /// </summary>
        public static Rectangle2D[] WarpLocations => _warpLocations;

        /// <summary>
        /// Creates a new boat warp region.
        /// </summary>
        /// <param name="name">Name of the region</param>
        /// <param name="map">Map where the region is located</param>
        /// <param name="bounds">Region boundary</param>
        /// <param name="destinationIndex">Index of the destination in WarpLocations array</param>
        /// <param name="showBoundaries">Whether to show region boundaries with marker items</param>
        public BoatWarpRegion(string name, Map map, Rectangle2D bounds, int destinationIndex, bool showBoundaries = true)
            : base(name, map, DefaultPriority, bounds)
        {
            _destinationIndex = destinationIndex;
            _showBoundaries = showBoundaries;

            // Validate destination index
            if (destinationIndex < 0 || destinationIndex >= _warpLocations.Length)
                throw new ArgumentOutOfRangeException(nameof(destinationIndex), "Destination index must be between 0 and " + (_warpLocations.Length - 1));

            // Initialize destination point for this region if not already set
            if (!_destinationPoints.ContainsKey(destinationIndex))
            {
                Rectangle2D destRect = _warpLocations[destinationIndex];
                Point3D center = new Point3D(
                    destRect.X + (destRect.Width / 2),
                    destRect.Y + (destRect.Height / 2),
                    0);
                _destinationPoints[destinationIndex] = center;
            }

            if (showBoundaries)
                MarkBounds(bounds);

            // Start a timer to periodically check for boats in the region
            _checkTimer = Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), CheckForBoats);
        }

        /// <summary>
        /// Creates visual markers around the region boundaries.
        /// </summary>
        /// <param name="rectangle">The rectangle defining the region bounds</param>
        public void MarkBounds(Rectangle2D rectangle)
        {
            _boundaryMarkers = new List<Item>();
            int markerSpacing = 10;
            int markerCount = 0;

            for (int x = rectangle.X; x <= rectangle.X + rectangle.Width; x++)
            {
                for (int y = rectangle.Y; y <= rectangle.Y + rectangle.Height; y++)
                {
                    if (x == rectangle.X || x == rectangle.X + rectangle.Width ||
                        y == rectangle.Y || y == rectangle.Y + rectangle.Height)
                    {
                        if (markerCount >= markerSpacing)
                        {
                            BoundaryMarker marker = new BoundaryMarker(14089);
                            marker.MoveToWorld(new Point3D(x, y, -5), Map);
                            _boundaryMarkers.Add(marker);
                            markerCount = 0;
                        }
                        else
                        {
                            markerCount++;
                        }
                    }
                }
            }
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (m is PlayerMobile)
            {
                BaseBoat boat = FindBoatAt(m.Location, m.Map);
                if (boat != null)
                {
                    TeleportBoat(boat);
                }
            }
        }

        private void CheckForBoats()
        {
            if (Map == null || Map == Map.Internal)
                return;

            foreach (BaseBoat boat in BaseBoat.Boats)
            {
                if (boat.Map == Map && Contains(boat.Location))
                {
                    bool hasPlayers = false;

                    IPooledEnumerable mobiles = boat.GetMobilesInRange(0);
                    foreach (Mobile mobile in mobiles)
                    {
                        if (mobile is PlayerMobile)
                        {
                            hasPlayers = true;
                            break;
                        }
                    }
                    mobiles.Free();

                    if (hasPlayers)
                    {
                        TeleportBoat(boat);
                    }
                }
            }
        }

        public override void OnUnregister()
        {
            if (_checkTimer != null)
            {
                _checkTimer.Stop();
                _checkTimer = null;
            }

            if (_boundaryMarkers != null)
            {
                foreach (Item marker in _boundaryMarkers)
                    marker.Delete();

                _boundaryMarkers.Clear();
            }

            base.OnUnregister();
        }

        private static BaseBoat FindBoatAt(Point3D location, Map map)
        {
            if (map == null || map == Map.Internal)
                return null;

            foreach (BaseBoat boat in BaseBoat.Boats)
            {
                if (boat.Map == map && boat.Contains(location))
                    return boat;
            }

            return null;
        }

        public void TeleportBoat(BaseBoat boat)
        {
            if (boat == null || Map == null || Map == Map.Internal)
                return;

            Region currentRegion = Region.Find(boat.Location, boat.Map);
            if (currentRegion == null || !currentRegion.IsPartOf(this))
                return;

            if (!_destinationPoints.TryGetValue(_destinationIndex, out Point3D destination))
            {
                Rectangle2D destRect = _warpLocations[_destinationIndex];
                destination = new Point3D(
                    destRect.X + (destRect.Width / 2),
                    destRect.Y + (destRect.Height / 2),
                    0);
            }

            int offsetX = destination.X - boat.X;
            int offsetY = destination.Y - boat.Y;
            int offsetZ = 0 - boat.Z;

            if (boat.CanFit(destination, Map, boat.ItemID))
            {
                boat.Teleport(offsetX, offsetY, offsetZ);

                if (boat.Z != 0)
                    boat.Z = 0;

                if (boat.TillerMan != null)
                    boat.TillerManSay(501425); // Ar, turbulent water!
            }
            else
            {
                boat.StopMove(true);
                boat.SendMessageToAllOnBoard("Łódź utknęła na rafie koralowej!");
            }
        }

        /// <summary>
        /// Static method to register both forward and return warp regions.
        /// </summary>
        public static void RegisterBoatWarpRegions()
        {
            Region regionAtoB = new BoatWarpRegion("BoatWarpAtoB", Map.Felucca, new Rectangle2D(2252, 29, 10, 10), 0, true);
            Region regionBtoA = new BoatWarpRegion("BoatWarpBtoA", Map.Felucca, new Rectangle2D(5534, 3655, 10, 10), 1, true);
        }
    }

    public class BoundaryMarker : Static
    {
        public BoundaryMarker(int itemID) : base(itemID)
        {
            Hue = 1234;
        }

        public BoundaryMarker(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Delete();
        }
    }
}
