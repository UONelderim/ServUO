using System;
using Server.Engines.Craft;

namespace Server.Items.Crops
{


    // BaseSeedling: Szczepka/nasiono ziola - do sadzenia.
    public abstract class BaseSeedling : Item
    {
        public virtual SeedlingMsgs msg => new SeedlingMsgs();

        // Typ terenu umozliwiajacy sadzenie:
        public virtual bool CanGrowFurrows { get { return true; } }
        public virtual bool CanGrowGrass { get { return true; } }
        public virtual bool CanGrowForest { get { return true; } }
        public virtual bool CanGrowJungle { get { return true; } }
        public virtual bool CanGrowCave { get { return false; } }
        public virtual bool CanGrowSand { get { return false; } }
        public virtual bool CanGrowSnow { get { return false; } }
        public virtual bool CanGrowSwamp { get { return false; } }
		public virtual bool CanGrowDirt { get { return false; } }
		public virtual bool CanGrowGarden { get { return true; } } // ogrod w domku

        public virtual Type PlantType { get { return null; } }

        // Ponizej cztery parametry decydujace o szansie na uzycie szczepki w celu posadzenia rosliny.
        // Przykladowo: 50% przy 90 skilla,  50% przy 100 skilla
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SowMinSkill => 0.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SowChanceAtMinSkill => 20.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SowMaxSkill => 100.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SowChanceAtMaxSkill => 100.0;

        public bool BumpZ { get; set; }

        protected static SkillName[] defaultSkillsRequired = new SkillName[] { WeedHelper.MainWeedSkill };
        public virtual SkillName[] SkillsRequired { get { return defaultSkillsRequired; } }

		public BaseSeedling(int itemID) : this(1, itemID)
        {
        }

		public BaseSeedling(int amount, int itemID) : base(itemID)
        {
            Stackable = true;
            Amount = amount;
            Weight = 0.2;
        }

        public BaseSeedling(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)0); // deprecated (from removed WeedSeedZiolaUprawne child class)
		}

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
			reader.ReadInt(); // deprecated (from removed WeedSeedZiolaUprawne child class)
		}

        private bool CheckPlantChance(Mobile from)
        {
            from.CheckSkill(WeedHelper.MainWeedSkill, SowMinSkill, SowMaxSkill); // zawsze mozliwy koksa zielarstwa (ale tylko ten skill)

            return WeedHelper.CheckSkills(from, SkillsRequired, SowMinSkill, SowChanceAtMinSkill, SowMaxSkill, SowChanceAtMaxSkill); // pozwol sadzic ziolo uzywajac innych umiejetnosci
        }

        public virtual Item CreateWeed()
        {
            Item it = Activator.CreateInstance(PlantType) as Item;
            return it;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.CanBeginAction(LockKind()))
            {
                from.SendLocalizedMessage(1070062);
                return;
            }

            if (!CheckConditions(from))
                return;

            // Prog skilla umozliwiajacy sadzenie ziol:
            if (WeedHelper.GetHighestSkillValue(from, SkillsRequired) < SowMinSkill)
            {
                from.SendMessage(msg.TooLowSkillToPlant);    // Nie wiesz zbyt wiele o sadzeniu ziol.
                return;
            }

            // Sadzimy ziolo:
            from.BeginAction(LockKind());
            from.RevealingAction();
            double AnimationDelayBeforeStart = 0.5;
            double AnimationIntervalBetween = 1.75;
            int AnimationNumberOfRepeat = 2;
            // Wpierw delay i animacja wewnatrz timera, a po ostatniej animacji timer uruchamia funkcje wyrywajaca ziolo (trzeci parametr):
            new WeedTimer(from, this, this.Animate, this.PlantWeed, this.Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
        }

        public bool CheckConditions(Mobile from)
        {
            if (from == null || !from.Alive)
            {
                return false;
            }
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return false;
            }

            if (from.Mounted)
            {
                from.SendMessage(msg.CantBeMounted); // Musisz stac na ziemi, aby moc sadzic rosliny!
                return false;
            }

            Point3D m_pnt = from.Location;
            Map m_map = from.Map;

            if (!WeedHelper.CheckCanGrow(this, m_map, m_pnt))
            {
                from.SendMessage(msg.BadTerrain);    // Roslina na pewno nie urosnie na tym terenie.
                return false;
            }

            if (!WeedHelper.CheckSpaceForPlants(m_map, m_pnt))
            {
                from.SendMessage(msg.PlantAlreadyHere);  // W tym miejscu cos juz rosnie.
                return false;
            }

			if (!WeedHelper.CheckSpaceForObstacles(m_map, m_pnt))
			{
				from.SendMessage(msg.Obstacle);  // Cos blokuje to miejsce
				return false;
			}

			return true;
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

        public bool Animate(Mobile from)
        {
            if (!CheckConditions(from))
                return false;

            if (!from.Mounted)
                from.Animate(32, 5, 1, true, false, 0);

            return true;
        }

        public void PlantWeed(Mobile from)
        {
            if (!CheckConditions(from))
            {
                Unlock(from);
                return;
            }

            if (CheckPlantChance(from))
            {
                // Sadzenie szczepki
                Item item = CreateWeed();
                if (item != null)
                {
                    Point3D m_pnt = from.Location;
                    if (BumpZ)
                        ++m_pnt.Z;
                    Map m_map = from.Map;
                    item.Location = m_pnt;
                    item.Map = m_map;
                    from.SendMessage(msg.PlantSuccess);  // Udalo ci sie zasadzic rosline.
                }
                BasePlant plant = item as BasePlant;
                if (plant != null)
                    plant.StartSeedlingGrowth(from);

				this.Consume();
			}
            else
            {
                if (WeedHelper.Check(0.8))
                    from.SendMessage(msg.PlantFail); // Nie udalo ci sie zasadzic rosliny. Sprobuj ponownie.
                else
                {
                    from.SendMessage(msg.PlantFailWithLoss); // Nie udalo ci sie zasadzic rosliny, zmarnowales szczepke.

                    this.Consume();
                }
			}

            Unlock(from);
        }

    }

}
