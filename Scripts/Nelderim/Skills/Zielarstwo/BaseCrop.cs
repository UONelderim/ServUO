using System;
using System.Collections;
using Server.Engines.Craft;
using Server.Mobiles;



namespace Server.Items.Crops
{

    // BaseCrop: Zebrany plon - do obrobki w plecaku.
    public abstract class BaseCrop : Item, ICarvable
    {
        public virtual int DefaulReagentCount => 2;


        private double GrandMasterSkillCropBonus => 0.05;


        public virtual CropMsgs msg => new CropMsgs();

        protected static SkillName[] defaultSkillsRequired = new SkillName[] { WeedHelper.MainWeedSkill };
        public virtual SkillName[] SkillsRequired { get { return defaultSkillsRequired; } }

        public BaseCrop(int itemID) : this(1, itemID)
        {
        }

        public BaseCrop(int amount, int itemID) : base(itemID)
        {
            Stackable = true;
            Weight = 0.2;
            Amount = amount;
        }

        public BaseCrop(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public virtual Type ReagentType => null;

        private bool CreateReagent(Mobile m)
        {
            int amount = DefaulReagentCount;
            if (amount < 1)
            {
                m.SendMessage(msg.CreatedZeroReagent);    // Nie uzyskales wystarczajacej ilosci reagentu.
                return false;
            }

            Type type = ReagentType;
            if (type == null || !typeof(Item).IsAssignableFrom(type))
            {
                return false;
            }

            Item reagent = Activator.CreateInstance(type) as Item;
            if (reagent != null)
            {
                MutateReagent(reagent);
                int bonus = (WeedHelper.GetMainSkillValue(m) >= 100) ? WeedHelper.Bonus(amount, GrandMasterSkillCropBonus) : 0;
                reagent.Amount = amount + bonus;
                m.AddToBackpack(reagent);
                return true;
            }

            return false;
        }

        public virtual void MutateReagent(Item reagent)
        {
        }

        public bool Carve(Mobile from, Item item)
        {
            if (!CheckForBlade(from, item))
            {
                from.SendMessage("Do tej czynnosci potrzebny ci bedzie jakis maly noz.");
                return false;
            }

            OnDoubleClick(from);
            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
                return;
            }
            if (!from.CanBeginAction(LockKind()))
            {
                from.SendLocalizedMessage(1070062); // Jestes zajety czyms innym
                return;
            }
            if (!CheckForBlade(from))
            {
                return;
            }

            from.BeginAction(LockKind());
            from.RevealingAction();
            from.SendMessage(msg.StartedToCut);
            double AnimationDelayBeforeStart = 1.5;
            double AnimationIntervalBetween = 0.0;
            int AnimationNumberOfRepeat = 1;
            new WeedTimer(from, this, this.Animate, this.CutWeed, this.Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
        }

        public bool Animate(Mobile from)
        {
            return true;
        }

        public void CutWeed(Mobile from)
        {
            if (from == null || !from.Alive)
            {
                Unlock(from);
                return;
            }
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
                Unlock(from);
                return;
            }

            double skill = WeedHelper.GetHighestSkillValue(from, SkillsRequired);

            from.CheckSkill(WeedHelper.MainWeedSkill, 0, 100);  // granice skilla umozliwiajace przyrost podczas krojenia ziol

            //double chance = skill / 100.0;
            if (true /*chance > Utility.RandomDouble()*/ )
            {
                if (DefaulReagentCount == 0)
                {
                    from.SendMessage(msg.CreatedZeroReagent);    // Nie uzyskales wystarczajacej ilosci reagentu.
                }
                else
                {
                    if (CreateReagent(from))
                        from.SendMessage(msg.CreatedReagent);    // Uzyskales reagenty.
                }
            }
            else
            {
                from.SendMessage(msg.FailedToCreateReagents);    // Nie udalo ci sie uzyskac reagentow.
            }

            this.Consume();
            Unlock(from);
        }

        private bool CheckForBlade(Mobile from, Item item)
        {
            if (item is ButcherKnife || item is SkinningKnife || item is Dagger)
                return true;
            return false;
        }
        private bool CheckForBlade(Mobile from)
        {
            Item check = from.FindItemOnLayer(Layer.OneHanded);

            if (CheckForBlade(from, check))
                return true;

            PlayerMobile player = from as PlayerMobile;
            Container backpack = player.Backpack;
            ArrayList backpackItems = new ArrayList(backpack.Items);

            foreach (Item item in backpackItems)
            {
                if (CheckForBlade(from, item))
                    return true;
            }

            from.SendMessage("Do tej czynnosci potrzebny ci bedzie jakis maly noz.");
            return false;
        }

        // Jakiego typu czynnosci nie mozna wykonywac jednoczesnie ze zrywaniem ziol:
        public object LockKind()
        {
            return typeof(CraftSystem);
        }

        public void Unlock(Mobile from)
        {
            from.EndAction(LockKind());
        }

    }

}
