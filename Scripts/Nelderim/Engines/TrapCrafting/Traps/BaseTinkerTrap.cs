using System;
using System.Collections.Generic;
using Server.Regions;

namespace Server.Items
{
	public abstract class BaseTinkerTrap : Item
	{
		private const bool _KarmaLossOnArming = false;
		private const int _MaxTraps = 10;

		private static Dictionary<Mobile, int> _Table = [];
		private DateTime _ExpireTime = DateTime.MinValue;
		public static int ActiveTraps(Mobile pm) => _Table.GetValueOrDefault(pm, 0);

		public abstract int DisarmingSkillReq { get; }
		protected virtual TimeSpan ExpirationDuration => TimeSpan.FromMinutes(15);
		protected abstract int KarmaLoss { get; }
		protected abstract bool AllowedInTown { get; }
		
		[Constructable]
		public BaseTinkerTrap() : base(0x2AAA)
		{
			Light = LightType.Empty;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime ExpireTime
		{
			get => _ExpireTime;
			set
			{
				_ExpireTime = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool TrapArmed => ExpireTime > DateTime.MinValue;

		public override void AddNameProperty(ObjectPropertyList list)
		{
			var prefix = TrapArmed ? "Uzbrojona" : "Nieuzbrojona";
			list.Add($"{prefix} {Name}"); 
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == null || from.Deleted)
				return;
			
			if (!TrapArmed) //  Code for arming the Trap
			{
				if (IsChildOf(from.Backpack))
					from.SendMessage("Pułapka musi zostać najpierw ustawiona zanim ją uzbroisz.");
				else if (!from.InRange(GetWorldLocation(), 2))
					from.SendMessage("Z tego miejsca nie możesz uzbroić pułapki.");
				else if ((from.Region is CityRegion) && (!AllowedInTown))
					from.SendMessage("Nie możesz tego zrobić w mieście.");
				else if (ActiveTraps(from) > _MaxTraps)
					from.SendMessage("Niestety osiągnąłeś limit pułapek, które możesz ustawić.");
				else
				{
					ArmTrap(from);
					from.SendMessage("Ta pułapka jest uzbrojona.");
					from.PlaySound(0x4A);
				}
			}
			else //  Code for disarming the Trap
			{
				if (Visible)
				{
					if (from.Skills.RemoveTrap.Value + 1 < DisarmingSkillReq)
						from.SendMessage("Ta pułapka wygląda na zbyt skomplikowaną byś mógł ją rozbroić.");
					else if (!from.InRange(GetWorldLocation(), 2))
						from.SendMessage("Nie możesz rozbrajać z tego miejsca.");
					else if (DisarmingSkillReq > 0 && !from.CheckSkill(SkillName.RemoveTrap, DisarmingSkillReq - 25, DisarmingSkillReq + 25)) // Failed to disarm the Trap.
						TrapEffect(from);
					else
					{
						DisarmTrap(from);
						from.SendMessage("The trap is disarmed.");
						from.PlaySound(0x4A);
					}
				}
			}
		}

		private void ArmTrap(Mobile from)
		{
			Movable = false;
			Visible = false;
			Owner = from;
			_Table[from] = ActiveTraps(from) + 1;
					
			if (_KarmaLossOnArming)
				from.Karma -= KarmaLoss;
			
			Timer.DelayCall(ExpirationDuration, Expire);
			ExpireTime = DateTime.UtcNow + ExpirationDuration;
			OnTrapArmed(from);
		}

		private void DisarmTrap(Mobile from)
		{
			Movable = true;
			Visible = true;
			if (Owner != null)
			{
				_Table[Owner] = Math.Min(0, ActiveTraps(Owner) - 1);
				Owner = null;
			}

			if (_KarmaLossOnArming && from == Owner)
				from.Karma += (KarmaLoss / 2); // Recover half Karma Loss for disarming your own trap. 
			
			ExpireTime = DateTime.MinValue;
			OnTrapDisarmed(from);
		}

		private void Expire()
		{
			if (Deleted || !TrapArmed)
				return;
			
			DisarmTrap(Owner);
			Delete();
		}

		public override bool OnMoveOver(Mobile from)
		{
			if (TrapArmed && CheckTrigger(from))
			{
				Timer.DelayCall(() =>
				{
					from.PlaySound(0x4A);  // click sound
					TrapEffect(from);
					Expire();
				});
			}
			return true;
		}

		public virtual bool CheckTrigger(Mobile from)
		{
			return true;
		}

		public virtual void OnTrapArmed(Mobile from)
		{
		}

		public virtual void OnTrapDisarmed(Mobile from)
		{
		}

		public abstract void TrapEffect(Mobile from);

		public BaseTinkerTrap(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2); // version

			writer.Write(ExpireTime - DateTime.UtcNow);
			writer.Write(Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version >= 2)
			{
				ExpireTime = DateTime.UtcNow + reader.ReadTimeSpan();
			}
			Owner = reader.ReadMobile();
			
			if (ExpireTime >= DateTime.UtcNow)
				Timer.DelayCall(DateTime.UtcNow - ExpireTime, Expire);
			else
				Timer.DelayCall(TimeSpan.Zero, Expire);
		}
	}
}
