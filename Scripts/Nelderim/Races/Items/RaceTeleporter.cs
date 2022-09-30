#region References

using Nelderim.Races;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class RaceTeleporter : Item
	{
		private Map m_WorldMap;
		private bool m_Active;

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
		public Point3D TamaelLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D JarlingLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D NaurLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D ElfLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D DrowLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D KrasnoludLoc { get; set; }

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
		public string UnkRaceMsg { get; set; }

		[Constructable]
		public RaceTeleporter() : base(0x1BC3)
		{
			Name = "Teleporter Rasowy";
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

			Point3D p = m.Race switch
			{
				NTamael _ => TamaelLoc,
				NJarling _ => JarlingLoc,
				NNaur _ => NaurLoc,
				NElf _ => ElfLoc,
				NDrow _ => DrowLoc,
				NKrasnolud _ => KrasnoludLoc,
				_ => Point3D.Zero
			};

			if (p == Point3D.Zero)
			{
				m.SendMessage(UnkRaceMsg);
				return;
			}

			BaseCreature.TeleportPets(m, p, map);

			m.Map = map;
			m.Location = p;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
			writer.Write(UnkRaceMsg);
			writer.Write(TamaelLoc);
			writer.Write(JarlingLoc);
			writer.Write(NaurLoc);
			writer.Write(ElfLoc);
			writer.Write(DrowLoc);
			writer.Write(KrasnoludLoc);
			writer.Write(m_Active);
			writer.Write(m_WorldMap);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					UnkRaceMsg = reader.ReadString();
					goto case 0;
				case 0:
				{
					TamaelLoc = reader.ReadPoint3D();
					JarlingLoc = reader.ReadPoint3D();
					NaurLoc = reader.ReadPoint3D();
					ElfLoc = reader.ReadPoint3D();
					DrowLoc = reader.ReadPoint3D();
					KrasnoludLoc = reader.ReadPoint3D();

					m_Active = reader.ReadBool();
					m_WorldMap = reader.ReadMap();

					break;
				}
			}
		}
	}
}
