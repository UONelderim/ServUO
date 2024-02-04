using System;
using Server.Engines.Craft;

namespace Server.Items.Crops
{


	// BasePlant: Rosnacy krzaczek lub surowiec - do zbierania.
	public abstract class BasePlant : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public virtual TimeSpan GrowMatureTime => TimeSpan.FromHours(3); // czas od posadzenia rosliny do momentu az wyda pierwszy owoc)

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual TimeSpan CropRespawnTime => m_IsFertilized ? TimeSpan.FromSeconds(3600/14) : TimeSpan.FromSeconds(3600/12); // co jaki czas jeden owoc odrodza sie na roslinie

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual int CropCountMax => m_IsFertilized ? 14 : 12;  // ile owocow maksymalnie urosnie na roslinie


		private double GrandMasterSkillCropBonus => 0.15;


		public virtual PlantMsgs msg => new PlantMsgs();

		public Mobile m_Farmer;
		
		private MaturationTimer maturationTimer;
		private CropRespawnTimer cropRespawnTimer;

		protected abstract int YoungPlantGraphics { get; }
		protected abstract int MaturePlantGraphics { get; }

		private DateTime m_PlantedTime;

		private bool m_IsMatureInternal;
		private bool m_IsMature
		{
			get { return m_IsMatureInternal; }
			set
			{
				m_IsMatureInternal = value;
				ItemID = m_IsMature ? MaturePlantGraphics : YoungPlantGraphics;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsMature => m_IsMature;

		private int m_CropCount;

		[CommandProperty(AccessLevel.GameMaster)]
		public int CropCount
		{
			get { return m_CropCount; }
			set { m_CropCount = Math.Max(0, Math.Min(value, CropCountMax)); InvalidateProperties(); }
		}

		private bool m_CropRespawn = false;

		private bool m_IsFertilized = false;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsFertilized
		{
			get { return m_IsFertilized; }
			set
			{
				if (m_IsFertilized != value)
				{
					if (cropRespawnTimer != null)
						cropRespawnTimer.Delay = CropRespawnTime;
					m_IsFertilized = value;
					InvalidateProperties();
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime PlantedTime => m_PlantedTime;


		[CommandProperty(AccessLevel.GameMaster)]
		public bool CropRespawn => m_CropRespawn;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool CropRespawnTick => cropRespawnTimer != null && cropRespawnTimer.Running; // for debug purposes

		[CommandProperty(AccessLevel.GameMaster)]
		public bool MaturationTick => maturationTimer != null && maturationTimer.Running; // for debug purposes

		public virtual Type SeedType { get { return null; } }
        public virtual Type CropType { get { return null; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool GivesSeed => false; // czy dany typ zielska daje sadzonki? (FALSE dla zbieractwa [regi nekro])

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Farmer => m_Farmer;

		// Ponizej cztery parametry decydujace o szansie na zebrani plonu z krzaka
		// Przykladowo: 0% przy 0 skilla,  100% przy 100 skilla
		[CommandProperty(AccessLevel.GameMaster)]
        public virtual double HarvestMinSkill => 0.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double HarvestChanceAtMinSkill => 0.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double HarvestMaxSkill => 100.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double HarvestChanceAtMaxSkill => 100.0;


		[CommandProperty(AccessLevel.GameMaster)]
		public virtual double DestroyChance => 0.15;


        // Ponizej cztery parametry decydujace o szansie na pozyskanie szczepki.
        // Przykladowo: 10% przy 40 skilla,  30% przy 100 skilla
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SeedAcquireMinSkill => 0.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SeedAcquireChanceAtMinSkill => 1.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SeedAcquireMaxSkill => 100.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual double SeedAcquireChanceAtMaxSkill => DestroyChance;

        protected static SkillName[] defaultSkillsRequired = new SkillName[] { WeedHelper.MainWeedSkill };
        public virtual SkillName[] SkillsRequired { get { return defaultSkillsRequired; } }
        public override bool ForceShowProperties { get { return true; } }


		public override void GetProperties(ObjectPropertyList list)
		{
			AddNameProperty(list);

			if (!m_IsMature)
				list.Add("niedojrzale");
			else if (m_CropCount < CropCountMax)
				list.Add("dojrzale");
			else
				list.Add("obfite");

			if (m_IsFertilized)
				list.Add("wzmocnione nawozem");
		}

		public BasePlant(int itemID) : base(itemID)
        {
			MakePlentiful();

			Movable = false;
		}

		private void MakePlentiful()
		{
            m_PlantedTime = DateTime.MinValue;
			m_IsMature = true;
			m_CropCount = CropCountMax;
			m_CropRespawn = false;

			if (maturationTimer != null)
			{
				maturationTimer.Stop();
				maturationTimer = null;
			}
			if (cropRespawnTimer != null)
			{
				cropRespawnTimer.Stop();
				cropRespawnTimer = null;
			}
			InvalidateProperties();
		}

		public void StartSeedlingGrowth(Mobile farmer)
		{
			m_Farmer = farmer;
			m_PlantedTime = DateTime.Now;
			m_IsMature = false;
			m_CropCount = 0;
			m_CropRespawn = true;

			if (maturationTimer != null)
			{
				maturationTimer.Stop();
				maturationTimer = null;
			}
			StopCropRespawnTimer();

			maturationTimer = new MaturationTimer(this, GrowMatureTime);
			maturationTimer.Start();
			InvalidateProperties();
		}

		private void StartCropRespawnTimer()
		{
			StopCropRespawnTimer();

			if (m_IsMature && m_CropRespawn && m_CropCount < CropCountMax)
			{
				cropRespawnTimer = new CropRespawnTimer(this, CropRespawnTime, CropRespawnTime);
				cropRespawnTimer.Start();
			}
		}

		private void StopCropRespawnTimer()
		{
			if (cropRespawnTimer != null)
			{
				cropRespawnTimer.Stop();
				cropRespawnTimer = null;
			}
		}

		public bool Fertilize(Mobile from)
		{
			if (IsFertilized)
			{
				from.SendMessage(msg.AlreadyFertilized);
				return false;
			}
			else
			{
				IsFertilized = true;

				from.SendMessage(msg.FertilizeSuccess);
				return true;
			}
		}

		public BasePlant(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 2); // version
			writer.Write((long)m_PlantedTime.ToBinary());
			writer.Write((bool)m_IsMature);
			writer.Write((int)m_CropCount);
			writer.Write((bool)m_CropRespawn);
			writer.Write((Mobile)m_Farmer);
			writer.Write((bool)m_IsFertilized);
        }

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			if (version == 0)
			{
				reader.ReadInt();    // deprecated: m_TimeGrowInSeconds
				reader.ReadDouble(); // deprecated: m_SkillMin
				reader.ReadDouble(); // deprecated: m_SkillMax
				reader.ReadDouble(); // deprecated: m_SkillDestroy

				m_PlantedTime = DateTime.MinValue;
				m_IsMature = true;
				m_CropCount = CropCountMax;
				m_CropRespawn = false;
			}
			else if (version >= 1)
			{
				m_PlantedTime = DateTime.FromBinary(reader.ReadLong());
				m_IsMature = reader.ReadBool();
				m_CropCount = reader.ReadInt();
				m_CropRespawn = reader.ReadBool();
				m_Farmer = reader.ReadMobile();

				if (version == 2)
					m_IsFertilized = reader.ReadBool();
			}
			
			if (!m_IsMature)
			{
				TimeSpan toMature = GrowMatureTime - (DateTime.Now - m_PlantedTime);
				if (toMature.TotalMilliseconds > 0)
				{
					maturationTimer = new MaturationTimer(this, toMature);
					maturationTimer.Start();
				}
				else
					m_IsMature = true;
			}
			
			if (m_IsMature && m_CropRespawn && m_CropCount < CropCountMax)
				StartCropRespawnTimer();
		}

		// Funkcja determinujaca sukces w uzyskaniu szczepki podczas zbioru:
		private bool CheckSeedGain(Mobile from)
		{
			if (!GivesSeed)
				return false;

			return WeedHelper.CheckSkills(from, SkillsRequired, SeedAcquireMinSkill, SeedAcquireChanceAtMinSkill, SeedAcquireMaxSkill, SeedAcquireChanceAtMaxSkill);
		}

		public virtual int DefaultSeedCount => 1;

		//public virtual int DefaultCropCount(Mobile from)
		//{
		//	double skill = WeedHelper.GetHighestSkillValue(from, SkillsRequired);
		//	return (int)Math.Round(skill / 100 * 12);
		//}
		public bool CreateSeed(Mobile from)
		{
			return CreateItem(SeedType, DefaultSeedCount, from);
        }
		private static bool CreateItem(Type type, int amount, Mobile m)
        {
			if (amount < 1)
			{
				return false;
            }
			if (type == null || !typeof(Item).IsAssignableFrom(type))
			{
				return false;
			}

            Item seed = Activator.CreateInstance(type) as Item;
            if (seed != null)
            {
                seed.Amount = amount;
                m.AddToBackpack(seed);
                return true;
            }

            return false;
        }

        public override void OnDoubleClick(Mobile from)
		{
			if (from == null || !from.Alive)
				return;

			if (!from.CanBeginAction(LockKind()))
			{
				from.SendLocalizedMessage(1070062); // Jestes zajety czyms innym
				return;
			}

			if (from.Mounted)
			{
				from.SendMessage(msg.CantBeMounted); // Nie mozesz zbierac surowcow bedac konno.
				return;
			}

			if (!from.InRange(this.GetWorldLocation(), 2) || !from.InLOS(this))
			{
				from.SendMessage(msg.MustGetCloser); // Musisz podejsc blizej, aby to zebrac.
				return;
			}

			if (!m_IsMature)
			{
				from.SendMessage(msg.PlantTooYoung); // Roslina jest jeszcze niedojrzala.
				return;
			}

			if (m_CropCount < 1)
			{
				from.SendMessage(msg.EmptyCrop);    // Roslina nie zrodzila jeszcze plonu.
				return;
			}

			double skill = WeedHelper.GetHighestSkillValue(from, SkillsRequired);

			if (skill < HarvestMinSkill)
			{
				from.SendMessage(msg.NoChanceToGet); // Twoja wiedza o tym surowcu jest za mala, aby go zebrac.
				return;
			}

			// Zbieranie surowca:
			from.BeginAction(LockKind());
			if (m_Farmer != null && from.IsHarmfulCriminal(m_Farmer))
				from.Criminal = true;
			double AnimationDelayBeforeStart = 0.5;
			double AnimationIntervalBetween = 1.75;
			int AnimationNumberOfRepeat = 2;
			// Wpierw delay i animacja wewnatrz timera, a po ostatniej animacji timer uruchamia funkcje wyrywajaca ziolo (trzeci parametr):
			new WeedTimer(from, this, this.Animate, this.PullWeed, this.Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
		}

		// Jakiego typu czynnosci nie mozna wykonywac jednoczesnie ze zrywaniem ziol:
		public object LockKind()
		{
			return typeof(CraftSystem);
		}

		public void Unlock(Mobile from)
		{
			if (from != null)
				from.EndAction(LockKind());
		}

		public bool Animate(Mobile from)
		{
			if (!from.InRange(this.GetWorldLocation(), 2))
			{
				from.SendMessage("Oddaliles sie.");
				return false;
			}
			from.Direction = from.GetDirectionTo(this);
			from.Animate(32, 5, 1, true, false, 0);
			return true;
		}

		public void PullWeed(Mobile from)
		{
			Unlock(from);

			if (from == null || !from.Alive)
			{
				return;
			}

			if (m_CropCount < 1)
			{
				from.SendMessage(msg.EmptyCrop);    // Roslina nie zrodzila jeszcze plonu.
				return;
			}

			from.CheckSkill(WeedHelper.MainWeedSkill, HarvestMinSkill, HarvestMaxSkill); // koks zielarstwa na krzaczku

			if (WeedHelper.CheckSkills(from, SkillsRequired, HarvestMinSkill, HarvestChanceAtMinSkill, HarvestMaxSkill, HarvestChanceAtMaxSkill))
			{
				int bonus = (WeedHelper.GetMainSkillValue(from) >= 100) ? WeedHelper.Bonus(m_CropCount, GrandMasterSkillCropBonus) : 0;

				CreateItem(CropType, m_CropCount + bonus, from);
				from.SendMessage(msg.Succesfull);    // Udalo ci sie zebrac surowiec.

				m_CropCount = 0;
				InvalidateProperties();
				StartCropRespawnTimer();

				if (CheckSeedGain(from))
				{
					CreateSeed(from);
					from.SendMessage(msg.GotSeed);   // Udalo ci sie zebrac szczepke rosliny!
				}

				if (!m_CropRespawn || WeedHelper.Check(DestroyChance))
				{
					StopCropRespawnTimer();
					Delete();
					from.SendMessage(msg.PlantDestroyed);    // Zniszczyles surowiec.
				}
			}
			else
			{
				from.SendMessage(msg.FailToGet); // Nie udalo ci sie zebrac surowica.
			}
		}

		public class CropRespawnTimer : Timer
		{
			private BasePlant m_Plant;

			public CropRespawnTimer(BasePlant plant, TimeSpan delay, TimeSpan interval) : base(delay, interval)
			{
				m_Plant = plant;
			}

			protected override void OnTick()
			{
				m_Plant.CropCount++;

				if (m_Plant.CropCount >= m_Plant.CropCountMax)
				{
					Stop();
					m_Plant.cropRespawnTimer = null;
				}
			}
		}

		public class MaturationTimer : Timer
		{
			private BasePlant m_Plant;

			public MaturationTimer(BasePlant plant, TimeSpan delay) : base(delay)
			{
				m_Plant = plant;
			}

			protected override void OnTick()
			{
				m_Plant.m_IsMature = true;
				m_Plant.StartCropRespawnTimer();

				m_Plant.InvalidateProperties();
			}
		}
	}



}
