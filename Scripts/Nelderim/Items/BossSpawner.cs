#define BSdbg_RemoveThisSuffixToEnableDebugConsolePrintouts

using Server.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Server.ContextMenus;
using static Server.Mobiles.BossSpawner;

namespace Server.Mobiles
{

	public class BossSpawner : Item, ISpawner
	{
		private static TimeSpan TriggerCheckDelay = TimeSpan.FromSeconds(3); // jak czesto sprawdzac licznosc mobkow w xmlSpawnerach?
		private static int TriggerTresholdPerSpawner = 1; // wystarczy ubic spawn do tej ilosci, aby pojawil sie boss (nie musi byc zero)

		public class CooldownLoop : Timer
		{
			private BossSpawner bs;

			public CooldownLoop(BossSpawner bs, TimeSpan delay, TimeSpan interval) : base(delay, interval)
			{
				this.bs = bs;
			}

			protected override void OnTick()
			{
				if (bs == null)
					return;

				if (bs.Deleted || !bs.Running)
				{
					bs.DebugPrint("CooldownLoop.OnTick() --> null / not running / cool-down / already spawned");
					return;
				}

				bs.DebugPrint("CooldownLoop.Tick()");

				bs.m_CoolDownPhase = false;
				bs.m_LastCooldownPeriodReset = DateTime.Now;

				bs.HideSeal();
				if (bs.m_SpawnedBoss == null)
				{
					bs.DebugPrint("CooldownReset() m_SpawnedBoss == null");
					bs.TriggerCheckStart();
				}

				bs.InvalidateProperties();
			}
		}

		public class TriggerCheck : Timer
		{
			private BossSpawner bs;

			public TriggerCheck(BossSpawner bs) : base(TriggerCheckDelay, TriggerCheckDelay)
			{
				this.bs = bs;
			}

			protected override void OnTick()
			{
				if (bs == null)
					return;

				if (bs.Deleted || !bs.Running || bs.CoolDownPhase || bs.BossAlreadySpawned)
				{
					bs.DebugPrint("TriggerCheck.OnTick() --> null / not running / cool-down / already spawned");
					return;
				}

				foreach (XmlSpawner spawner in bs.m_TriggerSpawner)
					if (spawner != null && !spawner.Deleted && spawner.Running && spawner.CurrentCount > TriggerTresholdPerSpawner)
					{
						bs.DebugPrint("TriggerCheck.TriggerCheck.OnTick() --> conditions not met");
						return;
					}

				bs.DebugPrint("TriggerCheck.OnTick() --> triggeded!");
				bs.Spawn();
			}
		}

		#region ISpawner interface properties
		public bool UnlinkOnTaming => true;
		public Point3D HomeLocation => this.Location;
		public int HomeRange => m_RangeHome;
		public void GetSpawnProperties(ISpawnable spawn, ObjectPropertyList list) { }
		public void GetSpawnContextEntries(ISpawnable spawn, Mobile m, List<ContextMenuEntry> list) { }
		#endregion

		private Mobile m_SpawnedBoss;

		[CommandProperty(AccessLevel.GameMaster)]
		public int SpawnedBossSerial => (m_SpawnedBoss == null || !m_SpawnedBoss.Alive || m_SpawnedBoss.Deleted) ? -1 : m_SpawnedBoss.Serial;

		private bool m_AllowParagon = false;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool AllowParagon
		{
			get { return m_AllowParagon; }
			set { m_AllowParagon = value; }
		}

		private Point3D m_SealTargetLocation;
		[CommandProperty(AccessLevel.GameMaster)]

		public Point3D SealTargetLocation
		{
			get { return m_SealTargetLocation; }
			set { m_SealTargetLocation = value; }
		}

		private Map m_SealTargetMap;
		[CommandProperty(AccessLevel.GameMaster)]
		public Map SealTargetMap
		{
			get { return m_SealTargetMap; }
			set { m_SealTargetMap = value; }
		}

		private string m_SealTargetName;
		[CommandProperty(AccessLevel.GameMaster)]
		public string SealTargetName
		{
			get { return m_SealTargetName; }
			set
			{
				m_SealTargetName = value;

				if (m_SealItem != null && !m_SealItem.Deleted)
					m_SealItem.Name = DefaultSealName;
			}
		}

		bool m_Running = false;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Running
		{
			get => m_Running;
			set
			{
				if (m_Running != value)
				{
					if (m_Running)
					{
						Stop();
						InvalidateProperties();
					}
					else
					{
						Start();
						InvalidateProperties();
					}
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool BossAlreadySpawned { get { return m_SpawnedBoss != null; } }

		// Seal item (visible for players) indicates that there is no point to go to boss location.
		// It appears upon boss death.
		// It disappears when the cooldown-phgase elapses.
		private Item m_SealItem;
		[CommandProperty(AccessLevel.GameMaster)]
		public Item SealItem => m_SealItem;

		// The purpose of cooldown-phase is to limit the rate of boss spawn to a particular period.
		// It activates upon boss death.
		// It deactivates once every RespawnPeriod (e.g. every 12 hours).
		bool m_CoolDownPhase = false;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool CoolDownPhase => m_CoolDownPhase;

		private TimeSpan m_RespawnPeriod = TimeSpan.FromHours(24);

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan RespawnPeriod
		{
			get { return m_RespawnPeriod; }
			set { m_RespawnPeriod = value; InvalidateProperties(); }
		}

		private DateTime m_LastCooldownPeriodReset;

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime LastCooldownPeriodReset => m_LastCooldownPeriodReset;

		private string m_BossType;
		[CommandProperty(AccessLevel.GameMaster)]
		public string BossType
		{
			get { return m_BossType; }
			set { m_BossType = value; InvalidateProperties(); }
		}

		private string m_BossXmlProps;
		[CommandProperty(AccessLevel.GameMaster)]
		public string BossXmlProps
		{
			get { return m_BossXmlProps; }
			set { m_BossXmlProps = value; InvalidateProperties(); }
		}

		private int m_RangeHome = 8;
		[CommandProperty(AccessLevel.GameMaster)]
		public int RangeHome
		{
			get { return m_RangeHome; }
			set { m_RangeHome = value; }
		}

		XmlSpawner[] m_TriggerSpawner = new XmlSpawner[6];

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner0
		{
			get { return m_TriggerSpawner[0]; }
			set { m_TriggerSpawner[0] = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner1
		{
			get { return m_TriggerSpawner[1]; }
			set { m_TriggerSpawner[1] = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner2
		{
			get { return m_TriggerSpawner[2]; }
			set { m_TriggerSpawner[2] = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner3
		{
			get { return m_TriggerSpawner[3]; }
			set { m_TriggerSpawner[3] = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner4
		{
			get { return m_TriggerSpawner[4]; }
			set { m_TriggerSpawner[4] = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public XmlSpawner TriggerSpawner5
		{
			get { return m_TriggerSpawner[5]; }
			set { m_TriggerSpawner[5] = value; InvalidateProperties(); }
		}

		CooldownLoop m_CooldownLoop;

		private void CooldownLoopStart(TimeSpan delay)
		{
			if (m_CooldownLoop != null)
				m_CooldownLoop.Stop();
			m_CooldownLoop = new CooldownLoop(this, delay, m_RespawnPeriod);
			m_CooldownLoop.Start();
		}

		private void CooldownLoopStop()
		{
			if (m_CooldownLoop != null)
				m_CooldownLoop.Stop();
		}

		TriggerCheck m_TriggerCheck;

		private void TriggerCheckStart()
		{
			if (m_TriggerCheck != null)
				m_TriggerCheck.Stop();
			m_TriggerCheck = new TriggerCheck(this);
			m_TriggerCheck.Start();
		}

		private void TriggerCheckStop()
		{
			if (m_TriggerCheck != null)
				m_TriggerCheck.Stop();
		}

		public void Start()
		{
			m_Running = true;
			m_CoolDownPhase = false;
			m_LastCooldownPeriodReset = DateTime.Now;

			TriggerCheckStart();
			CooldownLoopStart(m_RespawnPeriod);

			InvalidateProperties();
		}

		public void Stop()
		{
			m_Running = false;

			TriggerCheckStop();
			CooldownLoopStop();

			if (m_SpawnedBoss != null)
			{
				m_SpawnedBoss.Delete();
				m_SpawnedBoss = null;
			}

			HideSeal();

			InvalidateProperties();
		}

		private void HideSeal()
		{
			if (m_SealItem != null)
				m_SealItem.Delete();
		}

		private void ShowSeal()
		{
			if (m_SealItem != null) // sanity
				m_SealItem.Delete();

			if (m_SealTargetMap != null && m_SealTargetMap != Map.Internal && m_SealTargetLocation != Point3D.Zero)
			{
				m_SealItem = new Static(0x1184);
				m_SealItem.Name = DefaultSealName;
				m_SealItem.MoveToWorld(m_SealTargetLocation, m_SealTargetMap);
			}
		}

		private string DefaultSealName { get { return m_SealTargetName != null ? m_SealTargetName : "Pieczec"; } }

		public void Remove(ISpawnable spawn) // on boss removed/killed
		{
			m_SpawnedBoss = null;

			DebugPrint("Remove() boss killed");

			m_CoolDownPhase = true;

			if (m_Running)
			{
				if (m_CoolDownPhase)
					ShowSeal();
				else
					TriggerCheckStart();
			}

			InvalidateProperties();
		}

		public void Spawn() // spawning the boss
		{
			if (m_SpawnedBoss != null)
				return;

			Type type = SpawnerType.GetType(m_BossType);
			if (type != null && type.IsSubclassOf(typeof(BaseCreature)))
			{
				DebugPrint("Spawn() type recognized");

				// the order of things done to spawned mobile is based on how this is done in Spawner and XmlSpawner classes

				BaseCreature boss = Activator.CreateInstance(type) as BaseCreature;
				if (boss != null)
				{
					m_SpawnedBoss = boss;
					boss.Spawner = this;

					int hue = boss.Hue;
					boss.OnBeforeSpawn(Location, Map);

					boss.MoveToWorld(Location, Map);

					boss.RangeHome = m_RangeHome;
					boss.Home = this.Location;

					if (!m_AllowParagon)
					{
						boss.IsParagon = false;
						boss.Hue = hue;
					}

					boss.OnAfterSpawn();

					try
					{
						string statusStr = "?";
						if (m_BossXmlProps != null)
						{
							BaseXmlSpawner.ApplyObjectStringProperties(null, m_BossXmlProps, boss, null, this, out statusStr);
							DebugPrint(statusStr);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					DebugPrint("Spawn() spawned successfully");
				}
				else
					DebugPrint("Spawn() fail to instantiate");
			}
			else
				DebugPrint("Spawn() type unrecognized");

			//m_CoolDownPhase = true;

			TriggerCheckStop();

			InvalidateProperties();
		}

		[Constructable]
		public BossSpawner() : base(0x1F1C)
		{
			Visible = false;
			Movable = false;
			Name = "BossSpawner";
		}

		public BossSpawner(Serial serial) : base(serial)
		{
		}

		public override void OnDelete()
		{
			Stop();

			base.OnDelete();
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			AddNameProperty(list);

			list.Add("Wlaczony: " + (m_Running ? "Tak" : "Nie"));
			list.Add("Czas respawnu: " + m_RespawnPeriod.ToString());
			list.Add("Rodzaj spawnu: " + m_BossType);
			list.Add("Podlegle spawnery:");
			foreach (var sp in m_TriggerSpawner)
				if (sp != null && !sp.Deleted)
					list.Add("- \"" + sp.Name + "\"" + (sp.Running ? "" : " (wylaczony)"));
			list.Add("Stan: " + (m_Running ? (m_SpawnedBoss!=null ? "mobek zyje" : (m_CoolDownPhase ? "czekam na cooldown respawnu (docelowo " +(m_LastCooldownPeriodReset+m_RespawnPeriod).ToString()+")" : "oczekiwanie na wybicie podlegajacych spawnow") ):"wylaczony"));
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)2); // version

			writer.Write(m_BossType);
			foreach (XmlSpawner sp in m_TriggerSpawner)
				writer.Write(sp);

			writer.Write(m_Running);
			writer.Write(m_CoolDownPhase);

			writer.Write(m_RespawnPeriod);
			writer.Write(m_LastCooldownPeriodReset);
			writer.Write(m_RangeHome);

			writer.Write(m_SealTargetLocation);
			writer.Write(m_SealTargetMap);
			writer.Write(m_SealItem);
			writer.Write(m_SealTargetName);

			writer.Write(m_SpawnedBoss);
			writer.Write(m_AllowParagon);
			writer.Write(m_BossXmlProps);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_BossType = reader.ReadString();
			for (int i = 0; i < m_TriggerSpawner.Length; i++)
				m_TriggerSpawner[i] = (XmlSpawner)reader.ReadItem();

			m_Running = reader.ReadBool();
			m_CoolDownPhase = reader.ReadBool();

			m_RespawnPeriod = reader.ReadTimeSpan();
			m_LastCooldownPeriodReset = reader.ReadDateTime();
			m_RangeHome = reader.ReadInt();

			m_SealTargetLocation = reader.ReadPoint3D();
			m_SealTargetMap = reader.ReadMap();
			m_SealItem = reader.ReadItem();
			m_SealTargetName = reader.ReadString();

			m_SpawnedBoss = reader.ReadMobile();

			if (version >= 1)
				m_AllowParagon = reader.ReadBool();

			if (version >= 2)
				m_BossXmlProps = reader.ReadString();

			if (m_Running)
			{
				TimeSpan alreadyPassed = DateTime.Now - m_LastCooldownPeriodReset;
				TimeSpan cooldownLeft = m_RespawnPeriod - alreadyPassed;
				DebugPrint("Deserialize() --> m_LastCooldownPeriodReset: " + m_LastCooldownPeriodReset);
				DebugPrint("Deserialize() --> cooldownLeft 1: " + cooldownLeft);
				while (cooldownLeft < TimeSpan.Zero)
					cooldownLeft += m_RespawnPeriod; // maintain the previous start time of cooldown-phace cycle
				if (m_Running)
					m_LastCooldownPeriodReset = DateTime.Now + cooldownLeft - m_RespawnPeriod; // update to lastest possible cycle, to be able to display the date of next respawn for GM

				DebugPrint("Deserialize() --> cooldownLeft 2: " + cooldownLeft);
				DebugPrint("Deserialize() --> m_SealItem " + m_SealItem);

				CooldownLoopStart(cooldownLeft);

				if (m_SpawnedBoss == null && !m_CoolDownPhase)
				{
					DebugPrint("Deserialize() --> TriggerCheckStart()");
					TriggerCheckStart();
				}
			}
		}

		public static void Initialize()
		{
			foreach (Item item in World.Items.Values)
			{
				if (item is BossSpawner)
				{
					BossSpawner bs = (BossSpawner)item;
					if (bs.m_SpawnedBoss != null)
					{
						bs.m_SpawnedBoss.Spawner = bs;  // renew the ownership
						bs.InvalidateProperties();
					}
				}
			}


		}

		[Conditional("BSdbg")]
		private void DebugPrint(string text)
		{
			Console.WriteLine("BossSpawner(" + Name + ")." + text);
		}
	}
}
