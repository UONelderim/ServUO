// 2005.01.04 :: LogoS :: RaceTeleporter

#region References

using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class RaceTeleporter : Item
	{
		private Point3D m_TamaelLoc;
		private Point3D m_JarlingLoc;
		private Point3D m_NaurLoc;
		private Point3D m_ElfLoc;
		private Point3D m_DrowLoc;
		private Point3D m_KrasnoludLoc;

		private Map m_WorldMap;
		private bool m_Active;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get { return m_Active; }
			set
			{
				m_Active = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D TamaelLoc
		{
			get { return m_TamaelLoc; }
			set
			{
				m_TamaelLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D JarlingLoc
		{
			get { return m_JarlingLoc; }
			set
			{
				m_JarlingLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D NaurLoc
		{
			get { return m_NaurLoc; }
			set
			{
				m_NaurLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D ElfLoc
		{
			get { return m_ElfLoc; }
			set
			{
				m_ElfLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D DrowLoc
		{
			get { return m_DrowLoc; }
			set
			{
				m_DrowLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D KrasnoludLoc
		{
			get { return m_KrasnoludLoc; }
			set
			{
				m_KrasnoludLoc = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Map WorldMap
		{
			get { return m_WorldMap; }
			set
			{
				m_WorldMap = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RaceTeleporter() : base(0x1BC3)
		{
			Name = "Nelderim";
			Movable = false;
			Visible = false;
			m_Active = true;
		}

		public RaceTeleporter(Serial serial) : base(serial)
		{
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m_Active)
			{
				StartTeleport(m);
				return false;
			}

			return true;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Active)
				list.Add(1060742); // active
			else
				list.Add(1060743); // inactive

			if (m_WorldMap != null)
				list.Add(1060658, "Map\t{0}", m_WorldMap);
		}

		public virtual void StartTeleport(Mobile m)
		{
			Map map = m_WorldMap;

			if (map == null || map == Map.Internal)
				map = m.Map;

			Point3D p = Point3D.Zero;

			if (m.Race == Race.NTamael)
				p = m_TamaelLoc;
			else if (m.Race == Race.NJarling)
				p = m_JarlingLoc;
			else if (m.Race == Race.NNaur)
				p = m_NaurLoc;
			else if (m.Race == Race.NElf)
				p = m_ElfLoc;
			else if (m.Race == Race.NDrow)
				p = m_DrowLoc;
			else if (m.Race == Race.NKrasnolud)
				p = m_KrasnoludLoc;

			if (p == Point3D.Zero)
			{
				m.SendMessage("Nie wybrales zadnej z ras, wroc sie i dokonaj wyboru!");
				return;
			}

			BaseCreature.TeleportPets(m, p, map);


			m.Map = map;
			m.Location = p;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write(m_TamaelLoc);
			writer.Write(m_JarlingLoc);
			writer.Write(m_NaurLoc);
			writer.Write(m_ElfLoc);
			writer.Write(m_DrowLoc);
			writer.Write(m_KrasnoludLoc);
			writer.Write(m_Active);
			writer.Write(m_WorldMap);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					m_TamaelLoc = reader.ReadPoint3D();
					m_JarlingLoc = reader.ReadPoint3D();
					m_NaurLoc = reader.ReadPoint3D();
					m_ElfLoc = reader.ReadPoint3D();
					m_DrowLoc = reader.ReadPoint3D();
					m_KrasnoludLoc = reader.ReadPoint3D();

					m_Active = reader.ReadBool();
					m_WorldMap = reader.ReadMap();

					break;
				}
			}
		}
	}
}
