#region References

using System;

#endregion

namespace Server.Items.Crops
{
	// WeedSeed: Szczepka ziola - do sadzenia.
	public class WeedSeed : Item
	{
		public virtual string MsgCantBeMounted => "Musisz stac na ziemi, aby moc to zrobic.";
		public virtual string MsgBadTerrain => "Nie mozesz tego zrobic na tym terenie.";
		public virtual string MsgPlantAlreadyHere => "W tym miejscu cos juz cos jest.";
		public virtual string MsgTooLowSkillToPlant => "Nie masz wystarczajacej wiedzy, aby wykorzystac przetmiot.";
		public virtual string MsgPlantSuccess => "Udalo ci sie zostawic przedmiot.";
		public virtual string MsgPlantFail => "Nie udalo ci sie zostawic przedmiot, zmarnowales okazje.";

		// Typ terenu umozliwiajacy sadzenie:
		public virtual bool CanGrowFurrows => true;
		public virtual bool CanGrowGrass => false;
		public virtual bool CanGrowForest => false;
		public virtual bool CanGrowJungle => false;
		public virtual bool CanGrowCave => false;
		public virtual bool CanGrowSand => false;
		public virtual bool CanGrowSnow => false;
		public virtual bool CanGrowSwamp => false;

		protected static SkillName[] defaultSkillsRequired = { WeedHelper.MainWeedSkill };
		public virtual SkillName[] SkillsRequired => defaultSkillsRequired;

		public virtual double MinSkillReq => 60.0; //bylo 90

		public WeedSeed(int itemID) : base(itemID)
		{
			Stackable = true;
			Weight = 0.2;
		}

		public WeedSeed(Serial serial) : base(serial)
		{
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

		protected virtual bool CheckPlantChance(Mobile from)
		{
			from.CheckSkill(WeedHelper.MainWeedSkill, 80, 100); // zawsze mozliwy koksa zielarstwa (ale tylko ten skill)

			return WeedHelper.CheckSkills(from, SkillsRequired, 80,
				100); // pozwol sadzic ziolo uzywajac innych umiejetnosci
		}

		public virtual Item CreateWeed() { return null; }

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
			if (WeedHelper.GetHighestSkillValue(from, SkillsRequired) < MinSkillReq)
			{
				from.SendMessage(MsgTooLowSkillToPlant); // Nie wiesz zbyt wiele o sadzeniu ziol.
				return;
			}

			//if ( this.BumpZ )
			//	++m_pnt.Z;

			// Sadzimy ziolo:
			from.BeginAction(LockKind());
			from.RevealingAction();
			double AnimationDelayBeforeStart = 0.5;
			double AnimationIntervalBetween = 1.75;
			int AnimationNumberOfRepeat = 2;
			// Wpierw delay i animacja wewnatrz timera, a po ostatniej animacji timer uruchamia funkcje wyrywajaca ziolo (trzeci parametr):
			new WeedTimer(from, this, Animate, PlantWeed, Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart),
				TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
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
				from.SendMessage(MsgCantBeMounted); // Musisz stac na ziemi, aby moc sadzic rosliny!
				return false;
			}

			Point3D m_pnt = from.Location;
			Map m_map = from.Map;

			if (!WeedHelper.CheckCanGrow(this, m_map, m_pnt.X, m_pnt.Y))
			{
				from.SendMessage(MsgBadTerrain); // Roslina na pewno nie urosnie na tym terenie.
				return false;
			}

			if (!WeedHelper.CheckSpace(m_pnt, m_map))
			{
				from.SendMessage(MsgPlantAlreadyHere); // W tym miejscu cos juz rosnie.
				return false;
			}

			return true;
		}

		// Jakiego typu czynnosci nie mozna wykonywac jednoczesnie ze zrywaniem ziol:
		public object LockKind()
		{
			return typeof(Herbalism);
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
					Map m_map = from.Map;
					item.Location = m_pnt;
					item.Map = m_map;
					from.SendMessage(MsgPlantSuccess); // Udalo ci sie zasadzic rosline.
				}

				WeedPlant plant = item as WeedPlant;
				if (plant != null)
					plant.GrowingTime = 60 * 15;
			}
			else
			{
				from.SendMessage(MsgPlantFail); // Nie udalo ci sie zasadzic rosliny, zmarnowales szczepke.
			}

			Consume();
			Unlock(from);
		}
	}
}
