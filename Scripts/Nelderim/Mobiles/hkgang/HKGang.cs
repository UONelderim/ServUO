//
// Hunter Killer Gang v1.0b
// jm (aka x-ray aka âåäüÌÛØ) 
// jm99[at]mail333.com
//
// Some code fixes from MarkC777 and KillerBeeZ,  
// thanks guys !
//

#region References

using System;
using System.Collections;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Engines.HunterKiller
{
	public enum HKState
	{
		Waiting,
		Pursuit,
		Ambush,
		Returning
	}

	public class HKGangSpawn : Item
	{
		private Mobile Leader;
		private ArrayList Killers;
		private Timer timer;
		private DateTime nextRefreshTime;

		[Constructable]
		public HKGangSpawn() : base(0x1f13)
		{
			Visible = false;
			Movable = false;

			Name = "Rzezimieszkowie z Vox Populi";

			Timer.DelayCall(TimeSpan.Zero, AddComponents);
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxRange { get; set; } = 200;

		[CommandProperty(AccessLevel.GameMaster)]
		public HKState State { get; private set; } = HKState.Waiting;

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime NextAction { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Target { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public WayPoint Waypoint { get; private set; }

		public HKGangSpawn(Serial serial) : base(serial)
		{
		}

		public void AddRandomMobile()
		{
			switch (Utility.Random(3))
			{
				case 0:
					AddMobile(false, new HKArcher());
					break;
				case 1:
					AddMobile(false, new HKMage());
					break;
				case 2:
					AddMobile(false, new HKWarrior());
					break;
			}
		}

		public void AddComponents()
		{
			if (Deleted) return;

			Killers = new ArrayList();

			AddMobile(true, new HKLeader());
			AddMobile(false, new HKWarrior());

			AddRandomMobile();
			AddRandomMobile();

			timer = new SliceTimer(this);

			timer.Start();
		}

		public void AddMobile(bool isLeader, Mobile m)
		{
			if (isLeader)
				Leader = m;
			else
				Killers.Add(m);

			Point3D loc = new Point3D(X, Y, Z);

			BaseCreature bc = m as BaseCreature;

			if (bc != null)
			{
				bc.RangeHome = 4;

				bc.Home = loc;
			}

			m.Location = loc;
			m.Map = Map;
		}

		public void ClearWaypoint()
		{
			if (Waypoint != null)
			{
				if (Leader != null && !Leader.Deleted && Leader.Alive) ((HKLeader)Leader).CurrentWayPoint = null;

				foreach (Mobile m in Killers)
				{
					if (!m.Deleted && m.Alive)
					{
						((HKMobile)m).CurrentWayPoint = null;
					}
				}

				Waypoint.Delete();

				Waypoint = null;
			}
		}

		public void AssignWaypoint()
		{
			((HKLeader)Leader).CurrentWayPoint = Waypoint;

			foreach (Mobile m in Killers)
			{
				if (!m.Deleted && m.Alive)
				{
					((HKMobile)m).CurrentWayPoint = Waypoint;
				}
			}
		}

		public void RefreshWaypoint(bool speak)
		{
			if (Leader != null && !Leader.Deleted && Leader.Alive)
			{
				ClearWaypoint();

				if (speak) ((HKLeader)Leader).Speak(0);

				if (Leader.GetDistanceToSqrt(Target) > MaxRange || Map != Target.Map)
				{
					StateReturning();

					return;
				}

				Waypoint = new WayPoint();

				Waypoint.Location = Target.Location;
				Waypoint.Map = Map;

				AssignWaypoint();
			}
			else
			{
				StateWaiting();

//				System.Console.WriteLine("DEBUG: Leader dead");
			}
		}

		public void FindTarget()
		{
			if (Leader == null || Leader.Deleted || !Leader.Alive) return;

			ArrayList list = new ArrayList();

			foreach (Mobile m in this.GetMobilesInRange(MaxRange))
			{
				if (m != null && m.Player && !m.Deleted && m.Alive && !m.Hidden && m.AccessLevel == AccessLevel.Player)
				{
					list.Add(m);
				}
			}

			if (list.Count == 0) return;

			Target = (Mobile)list[Utility.Random(0, list.Count)];

			State = HKState.Pursuit;

			NextAction = DateTime.Now + TimeSpan.FromMinutes(15);

			RefreshWaypoint(true);

//			System.Console.WriteLine("DEBUG: Pursuit, target: {0}", target.Name);
		}

		public void SetHome(Point3D loc)
		{
			((HKLeader)Leader).Home = loc;

			foreach (Mobile m in Killers)
			{
				if (!m.Deleted && m.Alive)
				{
					((HKMobile)m).Home = loc;
				}
			}
		}

		public void SetHome2This()
		{
			foreach (Mobile m in Killers)
			{
				if (!m.Deleted && m.Alive)
				{
					((HKMobile)m).Home = m.Location;
				}
			}
		}

		public bool CheckTarget()
		{
			if (Target != null && !Target.Deleted)
			{
				if (!Target.Alive)
				{
					if (Leader != null && !Leader.Deleted && Leader.Alive)
					{
						ClearWaypoint();

						SetHome(Leader.Location);

						((HKLeader)Leader).Speak(1);

						State = HKState.Ambush;

						NextAction = DateTime.Now + TimeSpan.FromMinutes(8);

//						System.Console.WriteLine("DEBUG: Target dead, Ambush");
					}
					else
					{
//						System.Console.WriteLine("DEBUG: Leader dead");

						StateWaiting();
					}

					return true;
				}

				return false;
			}

			return true;
		}

		public void StateReturning()
		{
			State = HKState.Returning;

			ClearWaypoint();

			SetHome(new Point3D(X, Y, Z));

			NextAction = DateTime.Now + TimeSpan.FromMinutes(10);

//			System.Console.WriteLine("DEBUG: Returning");
		}

		public void StateWaiting()
		{
			State = HKState.Waiting;

			ClearWaypoint();

			SetHome2This();

//			System.Console.WriteLine("DEBUG: Waiting");
		}

		public int GetAlive()
		{
			int count = 0;

			if (Leader != null && !Leader.Deleted && Leader.Alive) count++;

			foreach (Mobile m in Killers)
			{
				if (!m.Deleted && m.Alive)
				{
					count++;
				}
			}

			return count;
		}

		public void OnSlice()
		{
//			System.Console.WriteLine("DEBUG: Tick !");

			if (GetAlive() == 0)
			{
				ClearWaypoint();

				Delete();

//				System.Console.WriteLine("DEBUG: Removing spawn");

				return;
			}

			switch (State)
			{
				case HKState.Waiting:

					if (0.05 < Utility.RandomDouble()) return;

					FindTarget();

					break;

				case HKState.Pursuit:

					if (DateTime.Now >= NextAction)
					{
						if (Leader != null && !Leader.Deleted && Leader.Alive)
						{
							StateReturning();
						}
						else
						{
							StateWaiting();
						}

						return;
					}

					if (CheckTarget()) break;

					if (DateTime.Now >= nextRefreshTime)
					{
						RefreshWaypoint(false);

						nextRefreshTime = DateTime.Now + TimeSpan.FromSeconds(10);
					}

					break;

				case HKState.Ambush:

					if (DateTime.Now >= NextAction)
					{
						if (Leader != null && !Leader.Deleted && Leader.Alive)
						{
							((HKLeader)Leader).Speak(2);

							StateReturning();
						}
						else
						{
							StateWaiting();
						}
					}

					break;

				case HKState.Returning:

					if (DateTime.Now >= NextAction)
					{
						StateWaiting();
					}

					break;
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (Leader != null && !Leader.Deleted) Leader.Delete();

			if (Killers != null)
			{
				foreach (Mobile m in Killers)
				{
					if (!m.Deleted)
					{
						m.Delete();
					}
				}
			}

			if (timer != null) timer.Stop();
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendGump(new PropertiesGump(from, this));
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write(Leader);
			writer.WriteMobileList(Killers, true);
			writer.Write((int)State);
			writer.Write(Target);
			writer.Write(Waypoint);
			writer.WriteDeltaTime(NextAction);
			writer.WriteDeltaTime(nextRefreshTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			Leader = reader.ReadMobile();
			Killers = reader.ReadMobileList();
			State = (HKState)reader.ReadInt();
			Target = reader.ReadMobile();
			Waypoint = reader.ReadItem() as WayPoint;
			NextAction = reader.ReadDeltaTime();
			nextRefreshTime = reader.ReadDeltaTime();

			timer = new SliceTimer(this);

			timer.Start();
		}
	}
}
