//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
using System;
using Server.Regions;
using Server.Mobiles;

namespace Server.Items
{
  	public abstract class BaseTinkerTrap : Item
    {
        #region Internal Definitions

        // Config Variables
        private bool tr_KarmaLossOnArming = Trapcrafting.Config.KarmaLossOnArming;
        private bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
        private int m_TrapLimitNumber = Trapcrafting.Config.TrapLimitNumber;

        // Individual Trap Parameters
        private string tr_UnarmedName;
        private string tr_ArmedName;
        private double tr_ExpiresIn;
        private int tr_ArmingSkillReq;
        private int tr_DisarmingSkillReq;
        private int tr_KarmaLoss;
        private bool tr_AllowedInTown;

        // Internal Variables
        private DateTime tr_TimeTrapArmed;
        private Boolean tr_PlayerSafe = true;
        private Boolean tr_TrapArmed = false;
        private Mobile tr_Owner;
        #endregion

        [Constructable]
        public BaseTinkerTrap() : this(1)
		{
		}

        [Constructable]
        public BaseTinkerTrap( string ArmedName, string UnarmedName, double ExpiresIn, int ArmingSkill, int DisarmingSkill, int KarmaLoss, bool AllowedInTown) : base(0x2AAA)
        {
            this.Name = UnarmedName;
            this.Light = LightType.Empty;

            this.ArmedName = ArmedName;
            this.UnarmedName = UnarmedName;
            this.ExpiresIn = ExpiresIn;
            this.ArmingSkillReq = ArmingSkill;
            this.DisarmingSkillReq = DisarmingSkill;
            this.KarmaLoss = KarmaLoss;
            this.AllowedInTown = AllowedInTown;
        }

        #region Trap Properties
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return tr_Owner; }
            set { tr_Owner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool TrapArmed
        {
            get { return tr_TrapArmed; }
            set { tr_TrapArmed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime TimeTrapArmed
        {
            get { return tr_TimeTrapArmed; }
            set { tr_TimeTrapArmed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double ExpiresIn
        {
            get { return tr_ExpiresIn; }
            set { tr_ExpiresIn = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowedInTown
        {
            get { return tr_AllowedInTown; }
            set { tr_AllowedInTown = value; }
        }

        public string ArmedName
        {
            get { return tr_ArmedName; }
            set { tr_ArmedName = value; }
        }

        public string UnarmedName
        {
            get { return tr_UnarmedName; }
            set { tr_UnarmedName = value; }
        }

        public int ArmingSkillReq
        {
            get { return tr_ArmingSkillReq; }
            set { tr_ArmingSkillReq = value; }
        }

        public int DisarmingSkillReq
        {
            get { return tr_DisarmingSkillReq; }
            set { tr_DisarmingSkillReq = value; }
        }

        public int KarmaLoss
        {
            get { return tr_KarmaLoss; }
            set { tr_KarmaLoss = value; }
        }

        public bool PlayerSafe
        {
            get { return tr_PlayerSafe; }
            set { tr_PlayerSafe = value; }
        }
        #endregion

        public override void OnDoubleClick( Mobile from )
		{
            if (!this.TrapArmed)  //  Code for arming the Trap
            {
                if (this.IsChildOf(from.Backpack))
                    from.SendMessage("Pułapka musi zostać najpierw ustawiona zanim ją uzbroisz.");

                else if (from.Skills.Tinkering.Value < this.ArmingSkillReq)
                    from.SendMessage("Niestety nie masz na tyle umiejętności Majsterkowania, by uzbroić tę pułapkę");

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
                    if ( this.Map.Rules == MapRules.FeluccaRules )
                        PlayerSafe = false;

                    this.TimeTrapArmed = DateTime.Now;
                    Timer.DelayCall( TimeSpan.FromSeconds ( this.ExpiresIn ), new TimerCallback( TrapExpiry ));

                    ArmTrap(from); // Any specialised trap arming code?
                    from.SendMessage("Ta pułapka jest uzbrojona.");
                    from.PlaySound(0x4A);
                }
            }
            else  //  Code for disarming the Trap
            {
                if (this.Visible)
                {
                    double m_RemoveTrap = (from.Skills.RemoveTrap.Value / 100);                // Get disarmers Remove Traps skill.
                    if (from.Skills.RemoveTrap.Value + 1 < this.DisarmingSkillReq)
                        from.SendMessage("Ta pułapka wygląda na zbyt skomplikowaną byś mógł ją rozbroić.");

                    else if (!from.InRange(this.GetWorldLocation(), 2))
                        from.SendMessage("Nie możesz rozbrajać z tego miejsca.");

                    else if (this.DisarmingSkillReq > 0 && (m_RemoveTrap < Utility.RandomDouble()))  // Failed to disarm the Trap.
                        TrapEffect(from);

                    else
                    {
                        if ((tr_KarmaLossOnArming) && (this.Owner == from))
                            from.Karma += (this.KarmaLoss / 2);  // Recover half Karma Loss for disarming your own trap. 

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
            Mobile from = this.tr_Owner;
            if (age < TimeSpan.FromSeconds(tr_ExpiresIn))
                return;

            if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
                ((PlayerMobile)this.Owner).TrapsActive -= 1;

            this.Delete();
        }

        public override bool OnMoveOver(Mobile from)
        {
            if (TrapArmed)
            {
                if (!(from.Player && this.PlayerSafe))  // Make sure the rules are Felucca
                    TrapCheckTrigger(from);             // for players triggering the trap.

                return true;
            }
            else
                return false;
        }

        public virtual void ArmTrap(Mobile from)  // Default for Trap Specific Arming Effects.
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

            writer.Write((int)1); // version

            // Version 1
            writer.Write((Mobile)this.Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                this.Owner = reader.ReadMobile();
            }

        }
    }
}
