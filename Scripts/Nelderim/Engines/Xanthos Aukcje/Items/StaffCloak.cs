#region References

using Server;
using Server.Items;
using Server.Mobiles;

#endregion

/*
** Allows staff to quickly switch between player and their assigned staff levels by equipping or removing the cloak
** Also allows instant teleportation to a specified destination when double-clicked by the staff member.
** Author unknown.
*/

namespace Arya.Auction
{
	public class StaffCloak : Cloak
	{
		private AccessLevel m_StaffLevel;

		[CommandProperty(AccessLevel.Administrator)]
		public AccessLevel StaffLevel
		{
			get
			{
				return m_StaffLevel;
			}
			set
			{
				m_StaffLevel = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D HomeLoc { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Map HomeMap { get; set; }

		public override void OnAdded(IEntity parent)
		{
			base.OnAdded(parent);

			// delete this if someone without the necessary access level picks it up or tries to equip it
			if (RootParent is PlayerMobile && ((PlayerMobile)RootParent).AccessLevel == AccessLevel.Player)
			{
				((PlayerMobile)RootParent).Emote("*Picks up Staff Cloak and watches it go poof*");
				Delete();
				return;
			}

			Mobile mobile = parent as Mobile;

			// when equipped, change access level to player
			if (null != mobile)
			{
				StaffLevel = mobile.AccessLevel;
				mobile.AccessLevel = AccessLevel.Player;
				mobile.Blessed = false;
			}
		}

		public override void OnRemoved(IEntity parent)
		{
			base.OnRemoved(parent);

			Mobile mobile = parent as Mobile;

			// restore access level to the specified level
			if (null != mobile && !Deleted)
			{
				mobile.AccessLevel = StaffLevel;
				StaffLevel = AccessLevel.Player;
				mobile.Blessed = true;
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (HomeMap != Map.Internal && HomeMap != null && from.AccessLevel > AccessLevel.Player)
			{
				from.MoveToWorld(HomeLoc, HomeMap);
			}
		}

		[Constructable]
		public StaffCloak()
		{
			StaffLevel = AccessLevel.Player;
			LootType = LootType.Blessed;
			Name = "Staff Cloak";
			Weight = 0;
		}

		public StaffCloak(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			// version
			writer.Write(0);
			// version 0
			writer.Write((int)m_StaffLevel);
			writer.Write(HomeLoc);
			string mapname = null;
			if (HomeMap != null)
			{
				mapname = HomeMap.Name;
			}

			writer.Write(mapname);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					m_StaffLevel = (AccessLevel)reader.ReadInt();
					HomeLoc = reader.ReadPoint3D();
					string mapName = reader.ReadString();

					try
					{
						HomeMap = Map.Parse(mapName);
					}
					catch { }

					break;
			}
		}
	}
}
