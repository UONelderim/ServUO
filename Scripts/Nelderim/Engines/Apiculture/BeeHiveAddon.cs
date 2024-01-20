#region References

using System;
using Server.Engines.Plants;
using Server.Items;
using Server.Multis;

#endregion

namespace Server.Engines.Apiculture
{
	public class apiBeeHive : BaseAddon
	{
		public static readonly int MaxHoney = 255; //maximum amount of honey

		public static readonly int MaxWax = 255; //maximum amount of wax

		public static readonly bool LessWax = true; //wax production is slower then honey (realistic)

		public override BaseAddonDeed Deed { get { return new apiBeeHiveDeed(); } }

		apiBeesComponent m_Bees; //for displaying bee swarm

		int m_Health = 10; //current health

		int m_Population = 1; //bee population (*10k)

		int m_Parasite; //parasite level(0, 1, 2)
		int m_Disease; //disease level (0, 1, 2)

		int m_Wax; //amount of Wax
		int m_Honey; //amount of Honey

		int m_PotAgility; //number of agility potions
		int m_PotHeal; //number of heal potions
		int m_PotCure; //number of cure potions
		int m_PotStr; //number of stength potions
		int m_PotPoison; //number of poison potions

		public int HiveAge { get; set; }

		public DateTime NextCheck { get; set; }

		public HiveStatus HiveStage { get; set; } = HiveStatus.Stage1;

		public HiveGrowthIndicator LastGrowth { get; set; } = 0;

		public int Wax
		{
			get { return m_Wax; }
			set
			{
				if (value < 0)
					m_Wax = 0;
				else if (value > MaxWax)
					m_Wax = MaxWax;
				else
					m_Wax = value;
			}
		}

		public int Honey
		{
			get { return m_Honey; }
			set
			{
				if (value < 0)
					m_Honey = 0;
				else if (value > MaxHoney)
					m_Honey = MaxHoney;
				else
					m_Honey = value;
			}
		}

		public int Health
		{
			get { return m_Health; }
			set
			{
				if (value < 0)
					m_Health = 0;
				else if (value > MaxHealth)
					m_Health = MaxHealth;
				else
					m_Health = value;

				if (m_Health == 0)
					Die();

				BeeHiveComponent.InvalidateProperties();
			}
		}

		public int MaxHealth
		{
			get { return 10 + ((int)HiveStage * 2); }
		}

		public HiveHealth OverallHealth
		{
			get
			{
				int perc = m_Health * 100 / MaxHealth;

				if (perc < 33)
					return HiveHealth.Dying;
				if (perc < 66)
					return HiveHealth.Sickly;
				if (perc < 100)
					return HiveHealth.Healthy;
				return HiveHealth.Thriving;
			}
		}

		public int Population
		{
			get { return m_Population; }
			set
			{
				if (value < 0)
					m_Population = 0;
				else if (value > 10)
					m_Population = 10;
				else
					m_Population = value;
			}
		}

		public int ParasiteLevel
		{
			get { return m_Parasite; }
			set
			{
				if (value < 0)
					m_Parasite = 0;
				else if (value > 2)
					m_Parasite = 2;
				else
					m_Parasite = value;
			}
		}

		public int DiseaseLevel
		{
			get { return m_Disease; }
			set
			{
				if (value < 0)
					m_Disease = 0;
				else if (value > 2)
					m_Disease = 2;
				else
					m_Disease = value;
			}
		}

		public bool IsFullAgilityPotion { get { return m_PotAgility >= 2; } }

		public int potAgility
		{
			get { return m_PotAgility; }
			set
			{
				if (value < 0)
					m_PotAgility = 0;
				else if (value > 2)
					m_PotAgility = 2;
				else
					m_PotAgility = value;
			}
		}

		public bool IsFullHealPotion { get { return m_PotHeal >= 2; } }

		public int potHeal
		{
			get { return m_PotHeal; }
			set
			{
				if (value < 0)
					m_PotHeal = 0;
				else if (value > 2)
					m_PotHeal = 2;
				else
					m_PotHeal = value;
			}
		}

		public bool IsFullCurePotion { get { return m_PotCure >= 2; } }

		public int potCure
		{
			get { return m_PotCure; }
			set
			{
				if (value < 0)
					m_PotCure = 0;
				else if (value > 2)
					m_PotCure = 2;
				else
					m_PotCure = value;
			}
		}

		public bool IsFullStrengthPotion { get { return m_PotStr >= 2; } }

		public int potStrength
		{
			get { return m_PotStr; }
			set
			{
				if (value < 0)
					m_PotStr = 0;
				else if (value > 2)
					m_PotStr = 2;
				else
					m_PotStr = value;
			}
		}

		public bool IsFullPoisonPotion { get { return m_PotPoison >= 2; } }

		public int potPoison
		{
			get { return m_PotPoison; }
			set
			{
				if (value < 0)
					m_PotPoison = 0;
				else if (value > 2)
					m_PotPoison = 2;
				else
					m_PotPoison = value;
			}
		}

		public int FlowersInRange { get; set; }

		public int FlowersX { get; set; }

		public int FlowersY { get; set; }

		public HoneyType FlowersQuarter
		{
			// Quartes of the XY axis
			// X>=0Y>=0 -> 1
			// X<0Y>=0  -> 2
			// X<0Y<0   -> 3
			// X>=0Y<0  -> 4
			get
			{
				if (FlowersX >= 0)
					if (FlowersY >= 0)
						return HoneyType.Gryczany;
					else
						return HoneyType.Spadziowy;
				if (FlowersY >= 0)
					return HoneyType.Lesny;
				return HoneyType.Nostrzykowy;
			}
		}

		public int FlowersPower
		{
			get
			{
				int m_power = (FlowersX * FlowersX + FlowersY * FlowersY) / 100;
				return m_power < 3 ? m_power : 2;
			}
		}

		public int WaterInRange { get; set; }

		public int Range
		{
			get { return m_Population + 2 + potAgility; } //bees work harder
		}

		public bool IsCheckable
		{
			get { return HiveStage != HiveStatus.Empty; }
		}

		public bool IsGrowable
		{
			get { return HiveStage != HiveStatus.Empty; }
		}

		public bool HasMaladies
		{
			get { return DiseaseLevel > 0 || ParasiteLevel > 0; }
		}

		public apiBeeHiveComponent BeeHiveComponent { get; private set; }

		[Constructable]
		public apiBeeHive()
		{
			AddComponent(new AddonComponent(2868), 0, 0, 0); //table
			//AddComponent( new apiBeeHiveComponent(this),0,0,+6 ); //beehive
			BeeHiveComponent = new apiBeeHiveComponent(this);
			AddComponent(BeeHiveComponent, 0, 0, +6);
			m_Bees = new apiBeesComponent(this);
			m_Bees.Visible = false;
			AddComponent(m_Bees, 0, 0, +6);
		}

		public apiBeeHive(Serial serial) : base(serial)
		{
		}

		public override void Delete()
		{
			//make sure we delete any swarms
			if (m_Bees != null)
				m_Bees.Delete();

			base.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(m_Bees); //swarm item
			writer.Write(m_Health); //hive health
			writer.Write(NextCheck); //next update check
			writer.Write((int)HiveStage); //growth stage
			writer.Write((int)LastGrowth); //last growth
			writer.Write(HiveAge); //age of hive
			writer.Write(m_Population); //bee population (*10k)
			writer.Write(m_Parasite); //parasite level(0, 1, 2)
			writer.Write(m_Disease); //disease level (0, 1, 2)
			writer.Write(FlowersInRange); //amount of water tiles in range (during last check)
			writer.Write(WaterInRange); //number of flowers in range (during last check)
			writer.Write(m_Wax); //amount of Wax
			writer.Write(m_Honey); //amount of Hone
			writer.Write(m_PotAgility); //number of agility potions
			writer.Write(m_PotHeal); //number of heal potions
			writer.Write(m_PotCure); //number of cure potions
			writer.Write(m_PotStr); //number of stength potions
			writer.Write(m_PotPoison); //number of poison potions
			writer.Write(BeeHiveComponent); //for storing the top of the hive
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Bees = (apiBeesComponent)reader.ReadItem(); //for displaying bee swarm
			m_Health = reader.ReadInt(); //current health
			NextCheck = reader.ReadDateTime(); //next update check
			HiveStage = (HiveStatus)reader.ReadInt(); //growth stage
			LastGrowth = (HiveGrowthIndicator)reader.ReadInt(); //last growth
			HiveAge = reader.ReadInt(); //age of hive
			m_Population = reader.ReadInt(); //bee population (*10k)
			m_Parasite = reader.ReadInt(); //parasite level(0, 1, 2)
			m_Disease = reader.ReadInt(); //disease level (0, 1, 2)
			FlowersInRange = reader.ReadInt(); //amount of water tiles in range (during last check)
			WaterInRange = reader.ReadInt(); //number of flowers in range (during last check)
			m_Wax = reader.ReadInt(); //amount of Wax
			m_Honey = reader.ReadInt(); //amount of Honey
			m_PotAgility = reader.ReadInt(); //number of agility potions
			m_PotHeal = reader.ReadInt(); //number of heal potions
			m_PotCure = reader.ReadInt(); //number of cure potions
			m_PotStr = reader.ReadInt(); //number of stength potions
			m_PotPoison = reader.ReadInt(); //number of poison potions
			BeeHiveComponent = (apiBeeHiveComponent)reader.ReadItem(); //for storing the top of the hive
		}

		public ResourceStatus ScaleWater()
		{
			//scale amount of water for bee population
			if (WaterInRange == 0)
				return ResourceStatus.None;

			int perc = WaterInRange * 250 / Population;

			if (perc < 33)
				return ResourceStatus.VeryLow;
			if (perc < 66)
				return ResourceStatus.Low;
			if (perc < 101)
				return ResourceStatus.Normal;
			if (perc < 133)
				return ResourceStatus.High;
			return ResourceStatus.VeryHigh;
		}

		public ResourceStatus ScaleFlower()
		{
			//scale amount of flowers for bee population
			if (FlowersInRange == 0)
				return ResourceStatus.None;

			int perc = FlowersInRange * 100 / Population;

			if (perc < 33)
				return ResourceStatus.VeryLow;
			if (perc < 66)
				return ResourceStatus.Low;
			if (perc < 101)
				return ResourceStatus.Normal;
			if (perc < 133)
				return ResourceStatus.High;
			return ResourceStatus.VeryHigh;
		}

		public bool IsUsableBy(Mobile from)
		{
			Item root = RootParent as Item;
			return IsChildOf(from.Backpack) || IsChildOf(from.BankBox) || IsLockedDown && IsAccessibleTo(from) ||
			       root != null && root.IsSecure && root.IsAccessibleTo(from);
		}

		public void Pour(Mobile from, Item item)
		{
			if (!IsAccessibleTo(from))
			{
				LabelTo(from, "Nie mozesz wylac tego na ul.");
				return;
			}

			if (item is BasePotion)
			{
				BasePotion potion = (BasePotion)item;

				string message;
				if (ApplyPotion(potion.PotionEffect, false, out message))
				{
					if (potion.Amount == 1)
						potion.Delete();
					else
						potion.Amount--;
					from.PlaySound(0x240);
					from.AddToBackpack(new Bottle());
				}

				LabelTo(from, message);
			}
			else if (item is PotionKeg)
			{
				PotionKeg keg = (PotionKeg)item;

				if (keg.Held <= 0)
				{
					LabelTo(from, "Nie mozesz tego uzyc na ulu!");
					return;
				}

				string message;
				if (ApplyPotion(keg.Type, false, out message))
				{
					keg.Held--;
					from.PlaySound(0x240);
				}

				LabelTo(from, message);
			}
			else
			{
				LabelTo(from, "Nie mozesz tego uzyc na ulu!");
			}
		}

		public bool ApplyPotion(PotionEffect effect, bool testOnly, out string message)
		{
			bool full = false;

			if (effect == PotionEffect.PoisonGreater || effect == PotionEffect.PoisonDeadly)
			{
				if (IsFullPoisonPotion)
					full = true;
				else if (!testOnly)
					potPoison++;
			}
			else if (effect == PotionEffect.CureGreater)
			{
				if (IsFullCurePotion)
					full = true;
				else if (!testOnly)
					potCure++;
			}
			else if (effect == PotionEffect.HealGreater)
			{
				if (IsFullHealPotion)
					full = true;
				else if (!testOnly)
					potHeal++;
			}
			else if (effect == PotionEffect.StrengthGreater)
			{
				if (IsFullStrengthPotion)
					full = true;
				else if (!testOnly)
					potStrength++;
			}
			else if (effect == PotionEffect.AgilityGreater)
			{
				if (IsFullAgilityPotion)
					full = true;
				else if (!testOnly)
					potAgility++;
			}
			else if (effect == PotionEffect.PoisonLesser || effect == PotionEffect.Poison ||
			         effect == PotionEffect.CureLesser || effect == PotionEffect.Cure ||
			         effect == PotionEffect.HealLesser || effect == PotionEffect.Heal ||
			         effect == PotionEffect.Strength)
			{
				message = "Ta mikstura jest za slaba, by uzyc jej na ulu!";
				return false;
			}
			else
			{
				message = "Nie mozesz tego uzyc na ulu!";
				return false;
			}

			if (full)
			{
				message = "Ul jest juz nasiakniety ta mikstura!";
				return false;
			}

			message = "Wylewasz miksture na ul.";
			return true;
		}

		public void FindWaterInRange()
		{
			//check area around hive for water (WATER)

			WaterInRange = 0;

			Map map = Map;

			if (map == null)
				return;

			IPooledEnumerable eable = map.GetItemsInRange(Location, Range);

			foreach (Item item in eable)
			{
				string iName = item.ItemData.Name.ToUpper();

				if (iName.IndexOf("WOD") != -1)
					WaterInRange++;
			}

			eable.Free();
		}

		public void FindFlowersInRange()
		{
			//check area around hive for flowers (flower, snowdrop, poppie)

			FlowersInRange = 0;
			int m_x = 0, m_y = 0;

			Map map = Map;

			if (map == null)
				return;

			IPooledEnumerable eable = map.GetItemsInRange(Location, Range);

			foreach (Item item in eable)
			{
				if (item.GetType() == typeof(PlantItem))
				{
					if (IsPlantStatusValid(((PlantItem)item).PlantStatus))
					{
						if (IsPlantTypeValid(((PlantItem)item).PlantType, out m_x, out m_y))
						{
							FlowersInRange++;
							FlowersX += m_x;
							FlowersY += m_y;
						}
					}
				}
			}

			eable.Free();
		}

		private bool IsPlantStatusValid(PlantStatus statusOfPlant)
		{
			switch (statusOfPlant)
			{
				case PlantStatus.Plant:
					return true;
				case PlantStatus.FullGrownPlant:
					return true;
				case PlantStatus.Stage5:
					return true;
				case PlantStatus.Stage6:
					return true;
				case PlantStatus.Stage8:
					return true;
				case PlantStatus.Stage9:
					return true;
				default:
					return false;
			}
		}

		private bool IsPlantTypeValid(PlantType typeOfPlant, out int x, out int y)
		{
			x = 0;
			y = 0;
			bool m_isPlantTypeValid = false;
			switch (typeOfPlant)
			{
				case PlantType.CampionFlowers:
					m_isPlantTypeValid = true;
					x = 0;
					y = 1;
					break;
				case PlantType.Poppies:
					m_isPlantTypeValid = true;
					x = 3;
					y = 0;
					break;
				case PlantType.Snowdrops:
					m_isPlantTypeValid = true;
					x = 2;
					y = 0;
					break;
				case PlantType.Bulrushes:
					m_isPlantTypeValid = true;
					x = 0;
					y = 3;
					break;
				case PlantType.Lilies:
					m_isPlantTypeValid = true;
					x = 1;
					y = 1;
					break;
				case PlantType.PampasGrass:
					m_isPlantTypeValid = true;
					x = 3;
					y = 1;
					break;
				case PlantType.Rushes:
					m_isPlantTypeValid = true;
					x = 0;
					y = 2;
					break;
				case PlantType.ElephantEarPlant:
					m_isPlantTypeValid = true;
					x = 1;
					y = 3;
					break;
				case PlantType.Fern:
					m_isPlantTypeValid = true;
					x = -1;
					y = 0;
					break;
				case PlantType.PonytailPalm:
					m_isPlantTypeValid = true;
					x = 3;
					y = 2;
					break;
				case PlantType.SmallPalm:
					m_isPlantTypeValid = true;
					x = 2;
					y = -1;
					break;
				case PlantType.CenturyPlant:
					m_isPlantTypeValid = true;
					x = 2;
					y = 3;
					break;
				case PlantType.WaterPlant:
					m_isPlantTypeValid = true;
					x = -1;
					y = -1;
					break;
				case PlantType.SnakePlant:
					m_isPlantTypeValid = true;
					x = 3;
					y = -3;
					break;
				case PlantType.PricklyPearCactus:
					m_isPlantTypeValid = true;
					x = -3;
					y = 0;
					break;
				case PlantType.BarrelCactus:
					m_isPlantTypeValid = true;
					x = -2;
					y = 2;
					break;
				case PlantType.TribarrelCactus:
					m_isPlantTypeValid = true;
					x = 1;
					y = 0;
					break;
				default:
					x = 0;
					y = 0;
					m_isPlantTypeValid = false;
					break;
			}

			return m_isPlantTypeValid;
		}

		public void Grow()
		{
			if (OverallHealth < HiveHealth.Healthy)
			{
				//not healthy enough to grow or produce
				if (LastGrowth != HiveGrowthIndicator.PopulationDown) //population down takes precedence
					LastGrowth = HiveGrowthIndicator.NotHealthy;
			}
			else if (ScaleFlower() < ResourceStatus.Low || ScaleWater() < ResourceStatus.Low)
			{
				//resources too low to grow or produce
				if (LastGrowth != HiveGrowthIndicator.PopulationDown) //population down takes precedence
					LastGrowth = HiveGrowthIndicator.LowResources;
			}
			else if (HiveStage < HiveStatus.Stage5)
			{
				//not producing yet, so just grow
				int curStage = (int)HiveStage;
				HiveStage = (HiveStatus)(curStage + 1);

				LastGrowth = HiveGrowthIndicator.Grown;
			}
			else
			{
				//production
				if (Wax < MaxWax)
				{
					int baseWax = 1;

					if (this.OverallHealth == HiveHealth.Thriving)
						baseWax++;

					baseWax += potAgility; //bees work harder

					baseWax *= Population;

					if (LessWax)
						baseWax = Math.Max(1, (baseWax / 3)); //wax production is slower then honey

					Wax += baseWax;
					LastGrowth = HiveGrowthIndicator.Grown;
				}

				if (Honey < MaxHoney)
				{
					int baseHoney = 1;

					if (this.OverallHealth == HiveHealth.Thriving)
						baseHoney++;

					baseHoney += potAgility; //bees work harder

					baseHoney += FlowersPower;

					baseHoney *= Population;

					Honey += baseHoney;
					LastGrowth = HiveGrowthIndicator.Grown;
				}

				potAgility = 0;

				if (Population < 10 && !(ScaleFlower() < ResourceStatus.Normal) &&
				    !(ScaleWater() < ResourceStatus.Normal))
				{
					LastGrowth = HiveGrowthIndicator.PopulationUp;
					Population++;
				}
			}

			if (HiveStage >= HiveStatus.Producing && !m_Bees.Visible)
				m_Bees.Visible = true;
		}

		public void ApplyBenefitEffects()
		{
			if (potPoison >= ParasiteLevel)
			{
				potPoison -= ParasiteLevel;
				ParasiteLevel = 0;
			}
			else
			{
				ParasiteLevel -= potPoison;
				potPoison = 0;
			}

			if (potCure >= DiseaseLevel)
			{
				potCure -= DiseaseLevel;
				DiseaseLevel = 0;
			}
			else
			{
				DiseaseLevel -= potCure;
				potCure = 0;
			}

			if (!HasMaladies)
			{
				if (potHeal > 0)
					Health += potHeal * 7;
				else
					Health += 2;
			}

			potHeal = 0;
		}

		public bool ApplyMaladiesEffects()
		{
			int damage = 0;

			if (ParasiteLevel > 0)
				damage += ParasiteLevel * Utility.RandomMinMax(3, 6);

			if (DiseaseLevel > 0)
				damage += DiseaseLevel * Utility.RandomMinMax(3, 6);

			if (ScaleWater() < ResourceStatus.Low)
				damage += (2 - (int)ScaleWater()) * Utility.RandomMinMax(3, 6);

			if (ScaleFlower() < ResourceStatus.Low)
				damage += (2 - (int)ScaleFlower()) * Utility.RandomMinMax(3, 6);

			Health -= damage;

			return IsGrowable;
		}

		public void UpdateMaladies()
		{
			//more water = more chance to come into contact with parasites?
			double parasiteChance =
				0.30 - (potStrength * 0.075) + (((int)ScaleWater() - 3) * 0.10) +
				(HiveAge * 0.01); //Older hives are more susceptible to infestation 

			if (Utility.RandomDouble() < parasiteChance)
				ParasiteLevel++;

			//more flowers = more chance to come into conctact with disease carriers
			double diseaseChance =
				0.30 - (potStrength * 0.075) + (((int)ScaleFlower() - 3) * 0.10) +
				(HiveAge * 0.01); //Older hives are more susceptible to disease 

			if (Utility.RandomDouble() < diseaseChance)
				DiseaseLevel++;

			if (potPoison > 0) //there are still poisons to apply
			{
				if (potCure > 0) //cures can negate poisons
				{
					potPoison -= potCure;
					potCure = 0;
				}

				if (potPoison > 0) //if there are still poisons, hurt the hive
				{
					Health -= potPoison * Utility.RandomMinMax(3, 6);
				}

				potPoison = 0;
			}

			potStrength = 0; //reset strength potions
		}

		public void Die()
		{
			//handle death
			if (HiveStage >= HiveStatus.Producing)
			{
				Population--;
				LastGrowth = HiveGrowthIndicator.PopulationDown;

				if (Population == 0)
				{
					//hive is dead
					HiveStage = HiveStatus.Empty;
				}
			}
			else
			{
				HiveStage = HiveStatus.Empty;
			}

			m_Bees.Visible = false;
		}
	}

	public class apiBeesComponent : AddonComponent
	{
		apiBeeHive m_Hive;

		public apiBeesComponent(apiBeeHive hive) : base(0x91b)
		{
			m_Hive = hive;
			Movable = false;
		}

		public apiBeesComponent(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
			writer.Write(m_Hive);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Hive = (apiBeeHive)reader.ReadItem();
		}
	}

	public class apiBeeHiveComponent : AddonComponent
	{
		apiBeeHive m_Hive;

		public override bool ForceShowProperties { get { return true; } }

		public apiBeeHiveComponent(apiBeeHive hive) : base(2330)
		{
			m_Hive = hive;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_Hive == null)
			{
				LabelTo(from, "Ten ul jest niedzialajacy.  Uzyj topora, by zebrac go na zlecenie.");
				return;
			}
			if (m_Hive.HiveStage == HiveStatus.Empty)
			{
				LabelTo(from, "Ten ul jest pusty.  Uzyj topora, zeby go zebrac na zlecenie.");
				return;
			}
			if (!from.InLOS(this))
			{
				LabelTo( from, "Nie widzisz tego." );
				return;
			}
			var house = BaseHouse.FindHouseAt(this);
			if (house != null && !house.HasAccess(from))
			{
				LabelTo(from, "Nie masz dostepu do tego ula");
				return;
			}

			from.SendGump(new apiBeeHiveMainGump(from, m_Hive));
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (m_Hive == null)
			{
				//just in case
				list.Add("Nieprawidlowy ul");
				return;
			}

			if (m_Hive.HiveStage == HiveStatus.Empty)
				list.Add("Ul");
			else
			{
				switch (m_Hive.OverallHealth)
				{
					case HiveHealth.Dying:
						list.Add("Umierajacy ul");
						break;
					case HiveHealth.Sickly:
						list.Add("Chory ul");
						break;
					case HiveHealth.Healthy:
						list.Add("Zdrowy ul");
						break;
					case HiveHealth.Thriving:
						list.Add("Prosperujacy ul");
						break;
					default:
						list.Add("Ul");
						break;
				}
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Hive == null) //just in case
				return;

			if (m_Hive.HiveStage >= HiveStatus.Producing)
				list.Add(1049644, "Produkcja");
			else if (m_Hive.HiveStage >= HiveStatus.Brooding)
				list.Add(1049644, "Rozmnazanie");
			else if (m_Hive.HiveStage >= HiveStatus.Colonizing)
				list.Add(1049644, "Kolonizacja");
			else
				list.Add(1049644, "Pusty");

			if (m_Hive.HiveStage != HiveStatus.Empty)
				list.Add(1060663, "{0}\t{1}", "Wiek", m_Hive.HiveAge + (m_Hive.HiveAge == 1 ? " dzien" : " dni"));
			if (m_Hive.HiveStage >= HiveStatus.Producing)
				list.Add(1060662, "{0}\t{1}", "Kolonia", m_Hive.Population + "0k pszczol");
		}

		public apiBeeHiveComponent(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
			writer.Write(m_Hive);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Hive = (apiBeeHive)reader.ReadItem();
		}
	}

	public class apiBeeHiveDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new apiBeeHive(); } }

		[Constructable]
		public apiBeeHiveDeed()
		{
			Name = "zlecenie na ul";
		}

		public apiBeeHiveDeed(Serial serial) : base(serial)
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
	}
}
