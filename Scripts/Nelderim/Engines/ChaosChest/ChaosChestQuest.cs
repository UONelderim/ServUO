using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Targeting;

namespace Nelderim.Engines.ChaosChest
{
	public class ChaosChestQuest : Item
	{
		private static readonly double DEFAULT_DROP_CHANCE = 0.01;
		private static double DROP_CHANCE_OVERRIDE;
		private static ChaosSigilType CURRENT_STAGE_OVERRIDE;
		private static ChaosChestQuest instance;

		private static Dictionary<string, ChaosSigilType> REGION_MAP = new Dictionary<string, ChaosSigilType>
		{
			{ "TylReviaren", ChaosSigilType.Natury },
			{ "Melisande", ChaosSigilType.Natury },
			{ "Piramida", ChaosSigilType.Natury },
			{ "Elbrind", ChaosSigilType.Morlokow },
			{ "CzerwoneOrki", ChaosSigilType.Morlokow },
			{ "Hurengrav_LVL1", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1", ChaosSigilType.Smierci },
			{ "KryptyNaurow", ChaosSigilType.Smierci },
			{ "KrysztalowaJaskinia", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki", ChaosSigilType.Krysztalow },
			{ "Wulkan_LVL3", ChaosSigilType.Ognia },
			{ "Wulkan_LVL4", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL3", ChaosSigilType.Ognia },
			{ "Demonowo", ChaosSigilType.Ognia },
			{ "Shimmering", ChaosSigilType.Swiatla },
			{ "BialyWilk", ChaosSigilType.Swiatla },
			{ "UlnhyrOrben", ChaosSigilType.Swiatla },
			{ "HallTorech_LVL1", ChaosSigilType.Licza },
			{ "HallTorech_LVL2", ChaosSigilType.Licza },
			{ "HallTorech_LVL3", ChaosSigilType.Licza },
			{ "Garth_LVL1", ChaosSigilType.Licza },
			{ "Garth_LVL2", ChaosSigilType.Licza }
		};

		public static void Initialize()
		{
			CommandSystem.Register("CreateChaosChestQuest", AccessLevel.Counselor, CreateChaosChestQuest);
			EventSink.CreatureDeath += OnDeath;
		}

		private static void CreateChaosChestQuest(CommandEventArgs e)
		{
			if (instance == null)
				e.Mobile.Target = new CreateTarget();
			else
			{
				e.Mobile.Location = instance.Location;
				e.Mobile.Map = instance.Map;
			}
		}

		internal static ChaosSigilType CURRENT_STAGE
		{
			get
			{
				return CURRENT_STAGE_OVERRIDE != ChaosSigilType.NONE
					? CURRENT_STAGE_OVERRIDE
					: (ChaosSigilType)Math.Pow(2, (int)DateTime.Now.DayOfWeek);
			}
		}

		private static double DROP_CHANCE
		{
			get
			{
				return DROP_CHANCE_OVERRIDE > 0.0d
					? DROP_CHANCE_OVERRIDE
					: DEFAULT_DROP_CHANCE;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public double DropChanceOverride
		{
			get { return DROP_CHANCE_OVERRIDE; }
			set
			{
				DROP_CHANCE_OVERRIDE = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public double DropChance
		{
			get { return DROP_CHANCE; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigilType CurrentStageOverride
		{
			get { return CURRENT_STAGE_OVERRIDE; }
			set
			{
				CURRENT_STAGE_OVERRIDE = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigilType CurrentStage
		{
			get { return CURRENT_STAGE; }
		}

		private ChaosChestQuest() : base(0x1BC3)
		{
			Movable = false;
		}

		public ChaosChestQuest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write((int)CurrentStageOverride);
			writer.Write(DROP_CHANCE_OVERRIDE);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			CurrentStageOverride = (ChaosSigilType)reader.ReadInt();
			DropChanceOverride = reader.ReadDouble();
		}

		public static void OnDeath(CreatureDeathEventArgs e)
		{
			Mobile m = e.Creature;
			if (m != null && m.Region != null &&
			    m.Region.Name != null &&
			    REGION_MAP.ContainsKey(m.Region.Name) &&
			    REGION_MAP[m.Region.Name].Equals(CURRENT_STAGE))
			{
				if (Utility.RandomDouble() < DROP_CHANCE)
				{
					e.Corpse.DropItem(new ChaosKey(CURRENT_STAGE));
				}
			}
		}

		private class CreateTarget : Target
		{
			public CreateTarget() : base(-1, true, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				IPoint3D location = targeted as IPoint3D;

				if (location != null)
				{
					ChaosChestQuest quest = new ChaosChestQuest();

					quest.MoveToWorld(new Point3D(location), from.Map);
					instance = quest;
				}
				else
				{
					from.SendMessage("Invalid location");
				}
			}
		}
	}
}
