using System;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
	public abstract class BaseTinkerTrap : Item
	{
		#region Internal Definitions

		// Config Variables
		private readonly bool tr_KarmaLossOnArming = Trapcrafting.Config.KarmaLossOnArming;
		private readonly bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
		private readonly int m_TrapLimitNumber = Trapcrafting.Config.TrapLimitNumber;

		// Individual Trap Parameters

		// Internal Variables

		#endregion

		[Constructable]
		public BaseTinkerTrap(string ArmedName, string UnarmedName, double ExpiresIn,
			int DisarmingSkill, int KarmaLoss, bool AllowedInTown) : base(0x2AAA)
		{
			this.Name = UnarmedName;
			this.Light = LightType.Empty;

			this.ArmedName = ArmedName;
			this.UnarmedName = UnarmedName;
			this.ExpiresIn = ExpiresIn;
			this.DisarmingSkillReq = DisarmingSkill;
			this.KarmaLoss = KarmaLoss;
			this.AllowedInTown = AllowedInTown;
		}

		#region Trap Properties

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool TrapArmed { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime TimeTrapArmed { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public double ExpiresIn { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AllowedInTown { get; set; }

		public string ArmedName { get; set; }

		public string UnarmedName { get; set; }

		public int DisarmingSkillReq { get; set; }

		public int KarmaLoss { get; set; }

		public bool PlayerSafe { get; set; } = true;

		#endregion

		public override void OnDoubleClick(Mobile from)
		{
			if (!this.TrapArmed) //  Code for arming the Trap
			{
				if (this.IsChildOf(from.Backpack))
					from.SendMessage("Pułapka musi zostać najpierw ustawiona zanim ją uzbroisz.");

				else if (!from.InRange(this.GetWorldLocation(), 2))
					from.SendMessage("Z tego miejsca nie możesz uzbroić pułapki.");

				else if ((from.Region is CityRegion) && (!this.AllowedInTown))
					from.SendMessage("Nie możesz tego zrobić w mieście.");

				else if ((m_TrapsLimit) && ((((PlayerMobile)from).TrapsActive + 1) > m_TrapLimitNumber))
					from.SendMessage("Niestety osiągąłeś limit pułapek, które możesz ustawić.");

				else
				{
					if (tr_KarmaLossOnArming)
						from.Karma -= this.KarmaLoss;

					if (m_TrapsLimit)
						((PlayerMobile)from).TrapsActive += 1;

					this.Name = this.ArmedName;
					this.Movable = false;
					this.TrapArmed = true;
					this.Owner = from;

					this.PlayerSafe = true;
					if (this.Map.Rules == MapRules.FeluccaRules)
						PlayerSafe = false;

					this.TimeTrapArmed = DateTime.Now;
					Timer.DelayCall(TimeSpan.FromSeconds(this.ExpiresIn), TrapExpiry);

					ArmTrap(from); // Any specialised trap arming code?
					from.SendMessage("Ta pułapka jest uzbrojona.");
					from.PlaySound(0x4A);
				}
			}
			else //  Code for disarming the Trap
			{
				if (this.Visible)
				{
					double m_RemoveTrap = (from.Skills.RemoveTrap.Value / 100); // Get disarmers Remove Traps skill.
					if (from.Skills.RemoveTrap.Value + 1 < this.DisarmingSkillReq)
						from.SendMessage("Ta pułapka wygląda na zbyt skomplikowaną byś mógł ją rozbroić.");

					else if (!from.InRange(this.GetWorldLocation(), 2))
						from.SendMessage("Nie możesz rozbrajać z tego miejsca.");

					else if (this.DisarmingSkillReq > 0 &&
					         (m_RemoveTrap < Utility.RandomDouble())) // Failed to disarm the Trap.
						TrapEffect(from);

					else
					{
						if ((tr_KarmaLossOnArming) && (this.Owner == from))
							from.Karma += (this.KarmaLoss / 2); // Recover half Karma Loss for disarming your own trap. 

						if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
							((PlayerMobile)this.Owner).TrapsActive -= 1;

						this.Name = this.UnarmedName;
						this.Movable = true;
						this.TrapArmed = false;
						this.Owner = from;

						this.PlayerSafe = true;

						DisarmTrap(from); // Any specialised trap disarming code?
						from.SendMessage("The trap is disarmed.");
						from.PlaySound(0x4A);
					}
				}
			}
		}

		private void TrapExpiry()
		{
			// Trap may have been triggered or disarmed since the timer started. 
			if (this == null || this.Deleted || !this.TrapArmed)
				return;

			//  Double check the trap has expired since the LAST arming.
			DateTime now = DateTime.Now;
			TimeSpan age = now - this.TimeTrapArmed;
			Mobile from = this.Owner;
			if (age < TimeSpan.FromSeconds(ExpiresIn))
				return;

			if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
				((PlayerMobile)this.Owner).TrapsActive -= 1;

			this.Delete();
		}

		public override bool OnMoveOver(Mobile from)
		{
			if (TrapArmed)
			{
				if (!(from.Player && this.PlayerSafe)) // Make sure the rules are Felucca
					Timer.DelayCall(() =>TrapCheckTrigger(from)); // for players triggering the trap.

				return true;
			}

			return false;
		}

		public virtual void ArmTrap(Mobile from) // Default for Trap Specific Arming Effects.
		{
			this.Visible = false;
		}

		public virtual void DisarmTrap(Mobile from) // Default for Trap Specific Disarming Effects
		{
		}

		public virtual void TrapCheckTrigger(Mobile from)
		{
			TrapEffect(from); // Default behavior for trap triggering (trigger all the time).
		}

		public abstract void TrapEffect(Mobile from);

		public BaseTinkerTrap(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2); // version updated to 2

			// Version 2
			writer.Write(this.TrapArmed);
			writer.Write(this.TimeTrapArmed);
			writer.Write(this.ExpiresIn);
			writer.Write(this.AllowedInTown);
			writer.Write(this.ArmedName);
			writer.Write(this.UnarmedName);
			writer.Write(this.DisarmingSkillReq);
			writer.Write(this.KarmaLoss);
			writer.Write(this.PlayerSafe);

			// Version 1
			writer.Write(this.Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version >= 2)
			{
				this.TrapArmed = reader.ReadBool();
				this.TimeTrapArmed = reader.ReadDateTime();
				this.ExpiresIn = reader.ReadDouble();
				this.AllowedInTown = reader.ReadBool();
				this.ArmedName = reader.ReadString();
				this.UnarmedName = reader.ReadString();
				this.DisarmingSkillReq = reader.ReadInt();
				this.KarmaLoss = reader.ReadInt();
				this.PlayerSafe = reader.ReadBool();
			}

			if (version >= 1)
			{
				this.Owner = reader.ReadMobile();
			}
            
			// Restart the expiry timer if the trap is still armed after server restart
			if (this.TrapArmed)
			{
				// Check if the trap should have expired already
				DateTime now = DateTime.Now;
				TimeSpan age = now - this.TimeTrapArmed;
				
				if (age >= TimeSpan.FromSeconds(ExpiresIn))
				{
					// Trap should have expired already
					if ((m_TrapsLimit) && (this.Owner is PlayerMobile pm) && (pm.TrapsActive > 0))
						pm.TrapsActive -= 1;
						
					Timer.DelayCall(TimeSpan.Zero, () => this.Delete());
				}
				else
				{
					// Restart the timer with remaining time
					double remainingTime = ExpiresIn - age.TotalSeconds;
					Timer.DelayCall(TimeSpan.FromSeconds(remainingTime > 0 ? remainingTime : 0), TrapExpiry);
				}
			}
		}
	}
}
