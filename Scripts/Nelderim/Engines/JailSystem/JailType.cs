/// <summary>
///  LogoS :: 4.03.2005
/// </summary>

#region References

using Server;

#endregion

namespace Arya.Jail
{
	public class JailType
	{
		public JailType(bool active, string name, Point3D[] jailcells, Point3D jailexit, Point3D jailenter,
			Rectangle3D[] jailregion)
		{
			Active = active;
			JailCells = jailcells;
			JailExit = jailexit;
			JailEnter = jailenter;
			JailRegion = jailregion;
			Name = name;
		}

		public JailType()
		{
			Active = true;
			JailCells = new Point3D[] { };
			JailRegion = new Rectangle3D[] { };
		}

		public bool Active { get; set; }

		public string Name { get; set; }

		public Point3D[] JailCells { get; set; }

		public Point3D JailEnter { get; set; }

		public Point3D JailExit { get; set; }

		public Rectangle3D[] JailRegion { get; set; }

		public bool IsInJail(Mobile m)
		{
			for (int i = 0; i < JailRegion.Length; i++)
			{
				Rectangle3D jailregion = JailRegion[i];

				if (jailregion.Contains(m.Location))
					return true;
			}

			return false;
		}

		public JailType WhichJail(Mobile m)
		{
			for (int i = 0; i < JailRegion.Length; i++)
			{
				Rectangle3D jailregion = JailRegion[i];

				if (jailregion.Contains(m.Location))
					return this;
			}

			return null;
		}
	}
}
