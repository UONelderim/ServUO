using Server.Mobiles;

namespace Server.Nelderim
{
	public class FactionTeleporter : Item
	{
		private bool m_Active;
		private Map m_WorldMap;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get => m_Active;
			set
			{
				m_Active = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D EastLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D WestLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D KompaniaHandlowaLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D VoxPopuliLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Map WorldMap
		{
			get => m_WorldMap;
			set
			{
				m_WorldMap = value;
				InvalidateProperties();
			}
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public string UknFactionMsg { get; set; }

		[Constructable]
		public FactionTeleporter() : base(0x1BC3)
		{
			Name = "Teleporter Frakcyjny";
			Movable = false;
			Visible = false;
			m_Active = true;
		}

		public FactionTeleporter(Serial serial) : base(serial)
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

			Point3D p = m.Faction switch
			{
				East _ => EastLoc,
				West _ => WestLoc, 
				KompaniaHandlowa _ => KompaniaHandlowaLoc,
				VoxPopuli _ => VoxPopuliLoc,
				_ => Point3D.Zero
			};

			if (p == Point3D.Zero)
			{
				m.SendMessage(UknFactionMsg);
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
			writer.Write(EastLoc);
			writer.Write(WestLoc);
			writer.Write(KompaniaHandlowaLoc);
			writer.Write(VoxPopuliLoc);
			writer.Write(m_Active);
			writer.Write(m_WorldMap);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			EastLoc = reader.ReadPoint3D();
			WestLoc = reader.ReadPoint3D();
			KompaniaHandlowaLoc = reader.ReadPoint3D();
			VoxPopuliLoc = reader.ReadPoint3D();
			m_Active = reader.ReadBool();
			m_WorldMap = reader.ReadMap();
		}
	}
}
