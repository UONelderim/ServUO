#region References

using System;
using System.Collections.Generic;
using Nelderim.Configuration;
using Nelderim.Engines.ChaosChest;
using Server.ContextMenus;
using Server.Gumps;
using Server.Nelderim;

#endregion

namespace Server.Mobiles
{
	public partial class BaseCreature
	{
		public void AnnounceRandomRumor(PriorityLevel level)
		{
			try
			{
				List<RumorRecord> RumorsList = RumorsSystem.GetRumors(this, level);

				if (RumorsList == null || RumorsList.Count == 0)
					return;

				int sum = 0;

				foreach (RumorRecord r in RumorsList)
					sum += (int)r.Priority;

				int index = Utility.Random(sum);
				double chance = sum / (4.0 * (int)level);

				sum = 0;
				RumorRecord rumor = null;

				foreach (RumorRecord r in RumorsList)
				{
					sum += (int)r.Priority;

					if (sum > index)
					{
						rumor = r;
						break;
					}
				}

				if (Utility.RandomDouble() < chance)
					Say(rumor.Coppice);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public double GetRumorsActionPropability()
		{
			try
			{
				List<RumorRecord> RumorsList = RumorsSystem.GetRumors(this, PriorityLevel.Low);

				if (RumorsList == null || RumorsList.Count == 0)
					return 0;

				int sum = 0;

				foreach (RumorRecord r in RumorsList)
					sum += (int)r.Priority;

				double chance = sum / 320.0;

				return Math.Max(1.0, chance);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			return 0.00;
		}

		public bool Activation(Mobile target)
		{
			return (Utility.RandomDouble() < Math.Pow(this.GetDistanceToSqrt(target), -2));
		}

		[CommandProperty(AccessLevel.Counselor)]
		public virtual double AttackMasterChance => 0.05;

		[CommandProperty(AccessLevel.Counselor)]
		public virtual double SwitchTargetChance => 0.05;

		public virtual bool IgnoreHonor => false;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Muted
		{
			get => DateTime.Now < MutedUntil;
			set => MutedUntil = DateTime.Now.AddHours(value ? 3 : 0);
		}

		[CommandProperty(AccessLevel.GameMaster, true)]
		public DateTime MutedUntil { get; private set; }
		
		public virtual List<OnSpeechEntry> OnSpeechActions => new List<OnSpeechEntry>();

		public override bool UseRealName(Mobile m)
		{
			return m != null && m.IsStaff() || ControlMaster == null || ControlMaster == m;
		}

		public override string FallbackName => "zwierze";

		public void NGenerateExtraLoot()
		{
			ChaosChestQuest.AddLoot(this);
		}

		public override void NAddProperties(ObjectPropertyList list)
		{
			if (NConfig.Loot.DebugDifficulty)
			{
				list.Add("Difficulty: " + Difficulty);
				list.Add("DPS:" + DPS);
				list.Add("Melee DPS: " + MeleeDPS);
				list.Add("Magic DPS: " + MagicDPS);
				list.Add("Life: " + Life);
			}
		}

		public override void NContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if (NConfig.Loot.DebugDifficulty && from is PlayerMobile pm && from.IsStaff())
			{
				list.Add(new DebugDifficultyEntry(this, pm));
			}
		}
		
		private class DebugDifficultyEntry : ContextMenuEntry
		{
			private readonly PlayerMobile _Player;
			private readonly BaseCreature _Creature;

			public DebugDifficultyEntry(BaseCreature creature, PlayerMobile player)
				: base(10008, 12)
			{
				_Creature = creature;
				_Player = player;
				Color = 0b1_11111_00000_00000;
			}

			public override void OnClick()
			{
				BaseGump.SendGump(new CreatureDifficultyGump(_Player, _Creature));
			}
		}
	}
}
