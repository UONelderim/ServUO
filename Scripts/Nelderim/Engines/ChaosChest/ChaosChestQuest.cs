using System;
using Server;
using Server.Mobiles;
using System.Collections.Generic;
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

		private static readonly Dictionary<string, ChaosSigilType> REGION_MAP = new()
		{
			{ "TylReviaren_VeryEasy", ChaosSigilType.Natury },
			{ "TylReviaren_Easy", ChaosSigilType.Natury },
			{ "TylReviaren_Medium", ChaosSigilType.Natury },
			{ "TylReviaren_Difficult", ChaosSigilType.Natury },
			{ "TylReviaren_VeryDifficult", ChaosSigilType.Natury },
			{ "Melisande_VeryEasy", ChaosSigilType.Natury },
			{ "Melisande_Easy", ChaosSigilType.Natury },
			{ "Melisande_Medium", ChaosSigilType.Natury },
			{ "Melisande_Difficult", ChaosSigilType.Natury },
			{ "Melisande_VeryDifficult", ChaosSigilType.Natury },
			{ "Piramida_VeryEasy", ChaosSigilType.Natury },
			{ "Piramida_Easy", ChaosSigilType.Natury },
			{ "Piramida_Medium", ChaosSigilType.Natury },
			{ "Piramida_Difficult", ChaosSigilType.Natury },
			{ "Piramida_VeryDifficult", ChaosSigilType.Natury },
			{ "Elbrind", ChaosSigilType.Morlokow },
			{ "CzerwoneOrki", ChaosSigilType.Morlokow },
			{ "Hurengrav_VeryEasy", ChaosSigilType.Smierci },
			{ "Hurengrav_Easy", ChaosSigilType.Smierci },
			{ "Hurengrav_Medium", ChaosSigilType.Smierci },
			{ "Hurengrav_Difficult", ChaosSigilType.Smierci },
			{ "Hurengrav_VeryDifficult", ChaosSigilType.Smierci },
			{ "KrolewskieKrypty", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2_VeryEasy", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2_Easy", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2_Medium", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2_Difficult", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL2_VeryDifficult", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1_VeryEasy", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1_Easy", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1_Medium", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1_Difficult", ChaosSigilType.Smierci },
			{ "WielkaPokracznaBestia_LVL1_VeryDifficult", ChaosSigilType.Smierci },
			{ "KryptyNaurow", ChaosSigilType.Smierci },
			{ "Gorogon_VeryEasy", ChaosSigilType.Krysztalow },
			{ "Gorogon_Easy", ChaosSigilType.Krysztalow },
			{ "Gorogon_Medium", ChaosSigilType.Krysztalow },
			{ "Gorogon_Difficult", ChaosSigilType.Krysztalow },
			{ "Gorogon_VeryDifficult", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki_Easy", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki_VeryEasy", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki_Medium", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki_Difficult", ChaosSigilType.Krysztalow },
			{ "KrysztaloweSmoki_VeryDifficult", ChaosSigilType.Krysztalow },
			{ "Wulkan_LVL3_Difficult", ChaosSigilType.Ognia },
			{ "Wulkan_LVL3_Difficult_2", ChaosSigilType.Ognia },
			{ "Wulkan_LVL3_VeryDifficult", ChaosSigilType.Ognia },
			{ "Wulkan_LVL4_VeryDifficult", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL1_Medium", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL1_Difficult", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL1_VeryDifficult", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2_VeryEasy", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2_Easy", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2_Medium", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2_Difficult", ChaosSigilType.Ognia },
			{ "LezeOgnistychSmokow_LVL2_VeryDifficult", ChaosSigilType.Ognia },
			{ "Demonowo", ChaosSigilType.Ognia },
			{ "Shimmering_LV1_VeryEasy", ChaosSigilType.Swiatla },
			{ "Shimmering_LV1_Easy", ChaosSigilType.Swiatla },
			{ "Shimmering_LV1_Medium", ChaosSigilType.Swiatla },
			{ "Shimmering_LV1_Difficult", ChaosSigilType.Swiatla },
			{ "Shimmering_LV1_VeryDifficult", ChaosSigilType.Swiatla },
			{ "Shimmering_LV2_VeryEasy", ChaosSigilType.Swiatla },
			{ "Shimmering_LV2_Easy", ChaosSigilType.Swiatla },
			{ "Shimmering_LV2_Medium", ChaosSigilType.Swiatla },
			{ "Shimmering_LV2_Difficult", ChaosSigilType.Swiatla },
			{ "Shimmering_LV2_VeryDifficult", ChaosSigilType.Swiatla },
			{ "BialyWilk_VeryEasy", ChaosSigilType.Swiatla },
			{ "BialyWilk_Easy", ChaosSigilType.Swiatla },
			{ "BialyWilk_Medium", ChaosSigilType.Swiatla },
			{ "BialyWilk_Difficult", ChaosSigilType.Swiatla },
			{ "BialyWilk_VeryDifficult", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV1_VeryEasy", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV1_Easy", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV1_Medium", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV1_Difficult", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV1_VeryDifficult", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV2_VeryEasy", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV2_Easy", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV2_Medium", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV2_Difficult", ChaosSigilType.Swiatla },
			{ "UlnhyrOrbben_LV2_VeryDifficult", ChaosSigilType.Swiatla },
			{ "HallTorech_LVL1_VeryEasy", ChaosSigilType.Licza },
			{ "HallTorech_LVL1_Easy", ChaosSigilType.Licza },
			{ "HallTorech_LVL1_Medium", ChaosSigilType.Licza },
			{ "HallTorech_LVL1_Difficult", ChaosSigilType.Licza },
			{ "HallTorech_LVL1_VeryDifficult", ChaosSigilType.Licza },
			{ "HallTorech_LVL2_VeryEasy", ChaosSigilType.Licza },
			{ "HallTorech_LVL2_Easy", ChaosSigilType.Licza },
			{ "HallTorech_LVL2_Medium", ChaosSigilType.Licza },
			{ "HallTorech_LVL2_Difficult", ChaosSigilType.Licza },
			{ "HallTorech_LVL2_VeryDifficult", ChaosSigilType.Licza },
			{ "Garth_LVL1_VeryEasy", ChaosSigilType.Licza },
			{ "Garth_LVL1_Easy", ChaosSigilType.Licza },
			{ "Garth_LVL1_Medium", ChaosSigilType.Licza },
			{ "Garth_LVL1_Difficult", ChaosSigilType.Licza },
			{ "Garth_LVL1_VeryDifficult", ChaosSigilType.Licza },
			{ "Garth_LVL2_VeryEasy", ChaosSigilType.Licza },
			{ "Garth_LVL2_Easy", ChaosSigilType.Licza },
			{ "Garth_LVL2_Medium", ChaosSigilType.Licza },
			{ "Garth_LVL2_Difficult", ChaosSigilType.Licza },
			{ "Garth_LVL2_VeryDifficult", ChaosSigilType.Licza }
		};

		public static void Initialize()
		{
			CommandSystem.Register("CreateChaosChestQuest", AccessLevel.Counselor, CreateChaosChestQuest);
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

		internal static ChaosSigilType CURRENT_STAGE =>
			CURRENT_STAGE_OVERRIDE != ChaosSigilType.NONE
				? CURRENT_STAGE_OVERRIDE
				: (ChaosSigilType)Math.Pow(2, (int)DateTime.Now.DayOfWeek);

		private static double DROP_CHANCE =>
			DROP_CHANCE_OVERRIDE > 0.0d
				? DROP_CHANCE_OVERRIDE
				: DEFAULT_DROP_CHANCE;

		[CommandProperty(AccessLevel.GameMaster)]
		public double DropChanceOverride
		{
			get => DROP_CHANCE_OVERRIDE;
			set => DROP_CHANCE_OVERRIDE = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public double DropChance => DROP_CHANCE;

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigilType CurrentStageOverride
		{
			get => CURRENT_STAGE_OVERRIDE;
			set => CURRENT_STAGE_OVERRIDE = value;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigilType CurrentStage => CURRENT_STAGE;

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

		public static void AddLoot(BaseCreature baseCreature)
		{
			if (baseCreature != null && baseCreature.Region != null &&
			    baseCreature.Region.Name != null &&
			    REGION_MAP.ContainsKey(baseCreature.Region.Name) &&
			    REGION_MAP[baseCreature.Region.Name].Equals(CURRENT_STAGE))
			{
				if (Utility.RandomDouble() < DROP_CHANCE)
				{
					baseCreature.AddToBackpack(new ChaosKey(CURRENT_STAGE));
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
