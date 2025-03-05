using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Mobiles
{
	public abstract class NBaseTalkingNPC : BaseVendor
	{
		private DateTime _lastAction;
		public override bool IsInvulnerable => false;

		protected virtual Dictionary<Race, List<Action>> NpcActions { get; }

		protected virtual TimeSpan ActionDelay => TimeSpan.FromSeconds(10);

		protected override List<SBInfo> SBInfos { get; }

		public NBaseTalkingNPC(string title) : base(title)
		{
		}

		public NBaseTalkingNPC(Serial serial) : base(serial)
		{
		}

		public override void OnThink()
		{
			if (DateTime.Now - _lastAction < ActionDelay) return;
			var players = GetClientsInRange(Core.GlobalUpdateRange);
			var playersCount = players.Count();
			players.Free();

			if (NpcActions == null ||
			    Muted ||
			    !(Utility.RandomDouble() < 0.01 ) ||
			    playersCount < 1)
			{
				return;
			}

			try
			{
				var actions = NpcActions.ContainsKey(Race)
					? NpcActions[Race]
					: NpcActions[Race.DefaultRace];

				if (actions.Count > 0)
				{
					Action action = Utility.RandomList(actions);
					action.Invoke(this);
				}
				else
				{
					Console.WriteLine("No action for npc " + Serial + " " + Name);
				}
				_lastAction = DateTime.Now;
			}
			catch (Exception ex)
			{
				Console.WriteLine("NBaseTalkingNPC error");
				Console.WriteLine(ex);
			}
		}

		public override void InitSBInfo()
		{
		}

		protected void MakeAggressive()
		{
			AI = AIType.AI_Melee;
			FightMode = FightMode.Closest;
			RangePerception = 12;
			RangeFight = 1;
			ActiveSpeed = 0.2;
			PassiveSpeed = 0.4;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		protected delegate void Action(Mobile from);
	}
}
