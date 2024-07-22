#region References

using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Nelderim.Towns
{
	public enum TownResourcesGumpPage
	{
		Information,
		Resources,
		Citizens,
		TownDevelopment,
		Building,
		Maintance,
		BuildingOnHold,
		BuildingOngoing,
		BuildingWorking
	}

	public enum TownResourcesGumpSubpages
	{
		None,
		Citizens,
		ToplistCitizens,
		OldestCitizens,
		ConsellourCitizens,
		RemoveCitizen,
		CitizenDetails,
		BuildingList
	}

	public enum TownMaintananceGumpSubpages
	{
		None,
		Budowanie,
		Zawieszone,
		Dziala,
		Podatki,
		Armia,
		Ekonomia,
		Dyplomacja,
		WydawaniePoswiecenia
	}

	public enum TownResourceType
	{
		Zloto = 0,
		Deski = 1,
		Sztaby = 2,
		Skora = 3,
		Material = 4,
		Kosci = 5,
		Kamienie = 6,
		Piasek = 7,
		Klejnoty = 8,
		Ziola = 9,
		Zbroje = 10,
		Bronie = 11,
		Invalid = 99
	}

	// Priority top-down
	public enum TownStatus
	{
		Leader,
		Counsellor,
		Citizen,
		NPC,
		Vendor,
		Guard,
		None
	}

	public enum TownCounsellor
	{
		Prime,
		Army,
		Diplomacy,
		Economy,
		Architecture,
		None
	}

	// Nowe miasto moze byc dodane na koncu listy, musi jednak zawierac poprawny kolejny numer, 
	public enum Towns
	{
		None = 0,
		Orod = 1,
		Garlan = 2,
		Twierdza = 3,
		LDelmah = 4,
		Lotharn = 5
	}

	public class TownResource
	{
		public TownResourceType ResourceType { get; }

		public TownResource(TownResourceType nType, int nAmount, int nMaxAmount, int nDailyChange)
		{
			ResourceType = nType;
			Amount = nAmount;
			MaxAmount = nMaxAmount;
			DailyChange = nDailyChange;
		}

		public int Amount { get; set; }

		public int MaxAmount { get; set; }

		public int DailyChange { get; set; }
	}

	public class TownResources
	{
		#region Typy

		private static readonly List<Type> m_klejnoty = new List<Type>
		{
			typeof(Amber),
			typeof(Amethyst),
			typeof(Citrine),
			typeof(Diamond),
			typeof(Emerald),
			typeof(Ruby),
			typeof(Sapphire),
			typeof(StarSapphire),
			typeof(Tourmaline),
			typeof(BaseJewel),
			typeof(ShimmeringCrystals),
			typeof(CrystallineFragments),
			typeof(ObsidianStone)
		};

		private static readonly List<Type> m_ziola = new List<Type>
		{
			typeof(SpidersSilk),
			typeof(SulfurousAsh),
			typeof(Nightshade),
			typeof(MandrakeRoot),
			typeof(Ginseng),
			typeof(Garlic),
			typeof(Bloodmoss),
			typeof(BlackPearl),
			typeof(BatWing),
			typeof(GraveDust),
			typeof(DaemonBlood),
			typeof(NoxCrystal),
			typeof(PigIron),
			typeof(SpringWater),
			typeof(DestroyingAngel),
			typeof(PetrafiedWood),
			typeof(TaintedMushroom),
			typeof(HornOfTheDreadhorn)
		};

		#endregion

		public List<TownResource> Resources { get; set; }

		public TownResources()
		{
			Resources = new List<TownResource>();
		}

		public TownResourceType CheckResourceType(Item res, out int amount)
		{
			TownResourceType resourceType = TownResourceType.Invalid;
			Type resType = res.GetType();
			amount = 1;

			/* Dla poszczegolnych typow mozna zamienic w amount = res.Amount * 1; 
			 * na amount = res.Amount * wartosc_dla_danego_typu;
			 * dla dodatkowej dywersyfikacji nalezy dodac wiecej else ifow */

			if (resType == typeof(Gold))
			{
				resourceType = TownResourceType.Zloto;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseWoodBoard)) || resType.IsSubclassOf(typeof(BaseLog)))
			{
				resourceType = TownResourceType.Deski;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseIngot)))
			{
				amount = res.Amount * 1;
				resourceType = TownResourceType.Sztaby;
			}
			else if (resType.IsSubclassOf(typeof(BaseLeather)) || resType.IsSubclassOf(typeof(BaseHides)))
			{
				resourceType = TownResourceType.Skora;
				amount = res.Amount * 1;
			}
			else if (resType == typeof(Cloth))
			{
				resourceType = TownResourceType.Material;
				amount = res.Amount * 1;
			}
			else if (resType == typeof(UncutCloth))
			{
				resourceType = TownResourceType.Material;
				amount = res.Amount * 1;
			}
			else if (resType == typeof(BoltOfCloth))
			{
				resourceType = TownResourceType.Material;
				amount = res.Amount * 50; // BoltOfCloth zawiera 50 Cloth
			}
			else if (resType == typeof(Bone))
			{
				resourceType = TownResourceType.Kosci;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseGranite)))
			{
				resourceType = TownResourceType.Kamienie;
				amount = res.Amount * 1;
			}
			else if (resType == typeof(Sand))
			{
				resourceType = TownResourceType.Piasek;
				amount = res.Amount * 1;
			}
			else if (m_klejnoty.Contains(resType))
			{
				resourceType = TownResourceType.Klejnoty;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseJewel)))
			{
				resourceType = TownResourceType.Klejnoty;
				amount = res.Amount * 1;
			}
			else if (m_ziola.Contains(resType))
			{
				resourceType = TownResourceType.Ziola;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseArmor)))
			{
				resourceType = TownResourceType.Zbroje;
				amount = res.Amount * 1;
			}
			else if (resType.IsSubclassOf(typeof(BaseWeapon)))
			{
				resourceType = TownResourceType.Bronie;
				amount = res.Amount * 1;
			}
			else if (resType == typeof(CommodityDeed))
			{
				Item resT = ((CommodityDeed)(res)).Commodity;
				resourceType = CheckResourceType(resT, out amount);
			}

			return resourceType;
		}

		#region Resource Amount

		public int ResourceAmount(TownResourceType nType)
		{
			return Resources.Find(obj => obj.ResourceType == nType).Amount;
		}

		public bool HasResource(TownResourceType nType)
		{
			return (Resources.Find(obj => obj.ResourceType == nType) != null);
		}

		public bool HasResourceAmount(TownResourceType nType, int amount)
		{
			if (HasResource(nType))
			{
				return ResourceAmount(nType) >= amount;
			}

			return false;
		}

		public void ResourceIncreaseAmount(TownResourceType nType, int amount)
		{
			ResourceChangeAmount(nType, amount);
		}

		public void ResourceDecreaseAmount(TownResourceType nType, int amount)
		{
			ResourceChangeAmount(nType, -amount);
		}

		private void ResourceChangeAmount(TownResourceType nType, int amount)
		{
			if (nType != TownResourceType.Invalid)
			{
				Resources.Find(obj => obj.ResourceType == nType).Amount += amount;
				if (Resources.Find(obj => obj.ResourceType == nType).Amount < 0)
					Resources.Find(obj => obj.ResourceType == nType).Amount = 0;
			}
		}

		#endregion

		#region Max amount

		public int ResourceMaxAmount(TownResourceType nType)
		{
			return Resources.Find(obj => obj.ResourceType == nType).MaxAmount;
		}

		public void ResourceMaxAmountSet(TownResourceType nType, int max)
		{
			Resources.Find(obj => obj.ResourceType == nType).MaxAmount = max;
		}

		public void ResourceMaxAmountIncrease(TownResourceType nType, int max)
		{
			Resources.Find(obj => obj.ResourceType == nType).MaxAmount += max;
		}

		#endregion

		#region Daily change

		public int ResourceDailyChange(TownResourceType nType)
		{
			return Resources.Find(obj => obj.ResourceType == nType).DailyChange;
		}

		public void ResourceDailyChangeSet(TownResourceType nType, int daily)
		{
			Resources.Find(obj => obj.ResourceType == nType).DailyChange = daily;
		}

		public void ResourceDailyChangeIncrease(TownResourceType nType, int daily)
		{
			Resources.Find(obj => obj.ResourceType == nType).DailyChange += daily;
		}

		#endregion

		public bool IsResourceAcceptable(Item res, out int amount)
		{
			if (CheckResourceType(res, out amount) != TownResourceType.Invalid)
			{
				return true;
			}

			return false;
		}

		public bool IsResourceAmountProper(Item res)
		{
			int m_amount = 1;
			TownResourceType m_resourceType = CheckResourceType(res, out m_amount);
			if (m_resourceType == TownResourceType.Zloto)
			{
				return true;
			}

			if ((ResourceAmount(m_resourceType) + m_amount) <=
			    ResourceMaxAmount(
				    m_resourceType)) // Czy obecny stan surowca + surowiec do dodania zmiesci sie w skarbcu
			{
				return true;
			}

			return false;
		}

		public bool IsResourceAmountProper(TownResourceType m_resourceType, int amount)
		{
			amount = 1;
			if (m_resourceType == TownResourceType.Zloto)
			{
				return true;
			}

			if ((ResourceAmount(m_resourceType) + amount) <=
			    ResourceMaxAmount(
				    m_resourceType)) // Czy obecny stan surowca + surowiec do dodania zmiesci sie w skarbcu
			{
				return true;
			}

			return false;
		}
	}

	public enum TownBuildingStatus
	{
		Niedostepny = 0,
		Dostepny = 1,
		Budowanie = 2,
		Dziala = 3,
		Zawieszony = 4
	}

	public enum TownBuildingName
	{
		MiejsceSpotkan = 0,
		DomZdziercy = 1,
		Bank = 2,
		DomUzdrowiciela = 3,
		Karczma = 4,
		Stajnia = 5,
		WarsztatKrawiecki = 6,
		WarsztatKowalski = 7,
		WarsztatStolarski = 8,
		WarsztatMajstra = 9,
		WarsztatAlchemika = 10,
		WarsztatLukmistrza = 11,
		Torturownia = 12,
		Piekarnia = 13,
		Farma = 14,
		Port = 15,
		DomGornika = 16,
		Arena = 17,
		Palisada = 18,
		Fosa = 19,
		MurObronny = 20,
		BramaDrewniana = 21,
		BramaStalowa = 22,
		WiezaStraznica = 23,
		PlacTreningowy = 24,
		Koszary = 25,
		Twierdza = 26,
		MostDrewniany = 27,
		MostKamienny = 28,
		Targowisko = 29,
		RozbudowaTargowiska = 30,
		Kapliczka = 31,
		Swiatynia = 32,
		Biblioteka = 33,
		DuszaMiasta = 34,
		Ratusz = 35,
		Ambasada = 36,
		Spichlerz = 37,
		SkladZapasow = 38,
		Skarbiec = 39,
		WarsztatMaga = 40,
		ZagrodaNaOwce = 41,
	}

	public enum TownGuards
	{
		Straznik,
		CiezkiStraznik,
		Strzelec,
		StraznikKonny,
		StraznikMag,
		StraznikElitarny
	}

	public class TownPost
	{
		BaseNelderimGuard m_guard;
		public int m_x, m_y, m_z;
		readonly Map m_map;

		public Towns HomeTown { get; set; } = Towns.None;

		public Serial GuardSerial { get; set; }

		DateTime m_activatedDate;

		public DateTime ActivatedDate
		{
			get { return m_activatedDate; }
			set { m_activatedDate = value; }
		}

		public string PostName { get; set; }

		TownGuards m_townGuard; // Poziom straznika

		public TownGuards TownGuard
		{
			get { return m_townGuard; }
			set
			{
				PostStatus = TownBuildingStatus.Dostepny;
				m_townGuard = value;
			}
		}

		TownBuildingStatus m_postStatus; // Status posterunku

		public TownBuildingStatus PostStatus
		{
			get { return m_postStatus; }
			set
			{
				if (value == TownBuildingStatus.Dziala)
				{
					SetGuard();
					ActivatedDate = DateTime.Now;
					RessurectAmount = 0;
				}
				else
				{
					RemoveGuard();
				}

				m_postStatus = value;
			}
		}

		public void SetGuard()
		{
			RemoveGuard();
			switch (m_townGuard)
			{
				case TownGuards.Straznik:
					m_guard = new StandardNelderimGuard();
					break;
				case TownGuards.CiezkiStraznik:
					m_guard = new HeavyNelderimGuard();
					break;
				case TownGuards.Strzelec:
					m_guard = new ArcherNelderimGuard();
					break;
				case TownGuards.StraznikKonny:
					m_guard = new MountedNelderimGuard();
					break;
				case TownGuards.StraznikMag:
					m_guard = new MageNelderimGuard();
					break;
				case TownGuards.StraznikElitarny:
					m_guard = new EliteNelderimGuard();
					break;
			}

			m_guard.Home = new Point3D(m_x, m_y, m_z);
			m_guard.RangeHome = 3;
			m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
			m_guard.HomeRegionName = HomeTown.ToString();
			m_guard.UpdateRegion();
			GuardSerial = m_guard.Serial;
		}

		public bool IsGuardAlive()
		{
			if (m_guard != null && m_guard.Alive)
			{
				return true;
			}

			return false;
		}

		public void RessurectGuard()
		{
			RessurectAmount++;
			SetGuard();
		}

		void RemoveGuard()
		{
			if (m_guard != null && m_guard.Alive)
			{
				m_guard.Kill();
				m_guard.Corpse.Delete();
			}

			m_guard = null;
		}

		public int RessurectAmount { get; set; }

		public int ActiveDaysAmount
		{
			get
			{
				DateTime m_now = DateTime.Now;
				if (m_postStatus == TownBuildingStatus.Dziala &&
				    DateTime.Now.CompareTo(m_activatedDate.AddDays(1)) == 1)
					return (int)(m_now.Date - m_activatedDate.Date).TotalDays;
				return 0;
			}
		}

		public int RessurectPerDay
		{
			get
			{
				if (ActiveDaysAmount <= 0)
				{
					return 0;
				}

				return RessurectAmount / ActiveDaysAmount;
			}
		}

		public void UpdatePostLocation(Mobile from)
		{
			m_x = from.Location.X;
			m_y = from.Location.Y;
			m_z = from.Location.Z;
			m_guard.Home = new Point3D(m_x, m_y, m_z);
			m_guard.RangeHome = 3;
			m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
			m_guard.HomeRegionName = HomeTown.ToString();
			m_guard.UpdateRegion();
		}

		public void SpawnGuard()
		{
			switch (TownGuard)
			{
				case TownGuards.Straznik:
					m_guard = new StandardNelderimGuard();
					break;
				case TownGuards.CiezkiStraznik:
					m_guard = new HeavyNelderimGuard();
					break;
				case TownGuards.Strzelec:
					m_guard = new ArcherNelderimGuard();
					break;
				case TownGuards.StraznikKonny:
					m_guard = new MountedNelderimGuard();
					break;
				case TownGuards.StraznikMag:
					m_guard = new MageNelderimGuard();
					break;
				case TownGuards.StraznikElitarny:
					m_guard = new EliteNelderimGuard();
					break;
			}

			m_guard.MoveToWorld(new Point3D(m_x, m_y, m_z), m_map);
			m_guard.UpdateRegion();
		}

		// Zwykly konstruktor
		public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg = TownGuards.Straznik,
			TownBuildingStatus status = TownBuildingStatus.Dostepny)
		{
			PostName = nazwa;
			TownGuard = tg;
			ActivatedDate = DateTime.Now;
			m_x = x;
			m_y = y;
			m_z = z;
			m_map = map;
			HomeTown = homeTown;
			PostStatus = status;
		}

		// Konstruktor dla posterunku, ktorego straznik zyje, a posterunek jest aktywny
		public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg, DateTime dt,
			Serial guardSerial)
		{
			PostName = nazwa;
			TownGuard = tg;
			ActivatedDate = dt;
			m_x = x;
			m_y = y;
			m_z = z;
			m_map = map;
			HomeTown = homeTown;
			m_postStatus = TownBuildingStatus.Dziala;
			GuardSerial = guardSerial;
			switch (m_townGuard)
			{
				case TownGuards.Straznik:
					m_guard = (StandardNelderimGuard)World.FindMobile(GuardSerial);
					break;
				case TownGuards.CiezkiStraznik:
					m_guard = (HeavyNelderimGuard)World.FindMobile(GuardSerial);
					break;
				case TownGuards.Strzelec:
					m_guard = (ArcherNelderimGuard)World.FindMobile(GuardSerial);
					break;
				case TownGuards.StraznikKonny:
					m_guard = (MountedNelderimGuard)World.FindMobile(GuardSerial);
					break;
				case TownGuards.StraznikMag:
					m_guard = (MageNelderimGuard)World.FindMobile(GuardSerial);
					break;
				case TownGuards.StraznikElitarny:
					m_guard = (EliteNelderimGuard)World.FindMobile(GuardSerial);
					break;
			}
		}

		// Konstruktor dla posterunku, ktorego straznik nie zyje, ale posterunek jest aktywny
		public TownPost(string nazwa, int x, int y, int z, Towns homeTown, Map map, TownGuards tg, DateTime dt)
		{
			PostName = nazwa;
			TownGuard = tg;
			ActivatedDate = dt;
			m_x = x;
			m_y = y;
			m_z = z;
			m_map = map;
			HomeTown = homeTown;
			m_postStatus = TownBuildingStatus.Dziala;
			m_guard = null;
			GuardSerial = Serial.Zero;
		}
	}

	public class TownBuilding
	{
		public TownResources Resources { get; set; }

		public bool Charge { get; set; } = true;

		public List<TownBuildingName> Dependecies { get; set; }

		public TownBuilding(TownBuildingName typBudynku, int poziom, TownBuildingStatus status, int opisID)
		{
			Resources = new TownResources();
			Dependecies = new List<TownBuildingName>();
			BuildingType = typBudynku;
			Poziom = poziom;
			Status = status;
			OpisID = opisID;
		}

		public TownBuildingName BuildingType { get; set; }

		public int Poziom { get; set; }

		public int OpisID { get; set; }

		public TownBuildingStatus Status { get; set; } = 0;
	}


	public class TownRelation
	{
		public Towns TownOfRelation;
		public int AmountOfRelation;

		public TownRelation(Towns t, int a)
		{
			this.TownOfRelation = t;
			this.AmountOfRelation = a;
		}

		// Override the ToString method:
		public override string ToString()
		{
			return (String.Format("({0} : {1})", TownOfRelation.ToString(), AmountOfRelation));
		}
	}

	public class TownLog
	{
		public DateTime LogDate { get; set; }

		public TownLogTypes LogType { get; set; }

		public int A { get; set; }

		public int B { get; set; }

		public int C { get; set; }

		public string Text { get; set; }


		public TownLog(DateTime dt, TownLogTypes tlp, string txt = "", int a = 0, int b = 0, int c = 0)
		{
			this.LogDate = dt;
			this.LogType = tlp;
			this.Text = txt;
			this.A = a;
			this.B = b;
			this.C = c;
		}

		// Override the ToString method:
		public override string ToString()
		{
			string toReturn = String.Format("{0} : ", LogDate.ToString());

			switch (LogType)
			{
				case TownLogTypes.OBYWATELSTWO_NADANIE:
					toReturn = String.Format("{0}{1} zostal obywatelem", toReturn, Text);
					break;
				case TownLogTypes.OBYWATELSTWO_ZAKONCZONO:
					toReturn = String.Format("{0}{1} przestal byc obywatelem", toReturn, Text);
					break;
				case TownLogTypes.OBYWATELSTWO_STATUS_NADANO:
					TownStatus st = (TownStatus)A;
					string stString = "";
					switch (st)
					{
						case TownStatus.Leader:
							stString = "przedstawicielem, niech zyje dlugo!";
							break;
						case TownStatus.Counsellor:
							stString = "kanclerzem";
							break;
						case TownStatus.Citizen:
							stString = "obywatelem";
							break;
					}

					toReturn = String.Format("{0}{1} zostal {2}", toReturn, Text, stString);
					break;
				case TownLogTypes.KANCLERZ_NADANO_GLOWNY:
					toReturn = String.Format("{0}{1} zostal kanclerzem glownym", toReturn, Text);
					break;
				case TownLogTypes.KANCLERZ_NADANO_DYPLOMACJI:
					toReturn = String.Format("{0}{1} zostal kanclerzem dyplomacji", toReturn, Text);
					break;
				case TownLogTypes.KANCLERZ_NADANO_ARMII:
					toReturn = String.Format("{0}{1} zostal kanclerzem armii", toReturn, Text);
					break;
				case TownLogTypes.KANCLERZ_NADANO_EKONOMII:
					toReturn = String.Format("{0}{1} zostal kanclerzem ekonomii", toReturn, Text);
					break;
				case TownLogTypes.KANCLERZ_NADANO_BUDOWNICTWA:
					toReturn = String.Format("{0}{1} zostal kanclerzem budownictwa", toReturn, Text);
					break;
				case TownLogTypes.BUDYNEK_ZLECONO_BUDOWE:
					toReturn = String.Format("{0}zlecono budowe budynku - {1}", toReturn, (TownBuildingName)A);
					break;
				case TownLogTypes.BUDYNEK_ZAKONCZONO_BUDOWE:
					toReturn = String.Format("{0}zakonczono budowe budynku - {1}", toReturn, (TownBuildingName)A);
					break;
				case TownLogTypes.BUDYNEK_ZAWIESZONO_DZIALANIE:
					toReturn = String.Format("{0}zawieszono dzialanie budynku - {1}", toReturn, (TownBuildingName)A);
					break;
				case TownLogTypes.BUDYNEK_WZNOWIONO_DZIALANIE:
					toReturn = String.Format("{0}wznowiono dzialanie budynku - {1}", toReturn, (TownBuildingName)A);
					break;
				case TownLogTypes.BUDYNEK_ZNISZCZONO:
					toReturn = String.Format("{0}budynek {1} zostal zniszczony!", toReturn, (TownBuildingName)A);
					break;
				case TownLogTypes.PODATKI_ZMIENIONO:
					string forWhom = "";
					if (A == 0)
						forWhom = "obywateli tego miasta";
					else if (A == 1)
						forWhom = "obywateli innych miasta";
					else if (A == 2)
						forWhom = "nie bedacych obywatelami zadnego miasta";
					toReturn = String.Format("{0}podatki dla {1} zostaly zmienione na {2}%", toReturn, forWhom, B);
					break;
				case TownLogTypes.POSTERUNEK_WYBUDOWANO:
					toReturn = String.Format("{0}postawiono nowy posterunek", toReturn);
					break;
				case TownLogTypes.POSTERUNEK_ZAWIESZONO:
					toReturn = String.Format("{0}posterunek {1} zostal zawieszony w dzialaniu", toReturn, Text);
					break;
				case TownLogTypes.POSTERUNEK_WZNOWIONO:
					toReturn = String.Format("{0}posterunek {1} zostal wznowiony w dzialaniu", toReturn, Text);
					break;
				case TownLogTypes.SUROWCE_WYSLANO:
					toReturn = String.Format("{0}wyslano {1} surowca {2} do miasta {3}", toReturn, A,
						((TownResourceType)B).ToString(), ((Towns)C).ToString());
					break;
				case TownLogTypes.SUROWCE_OTRZYMANO:
					toReturn = String.Format("{0}otrzymano {1} surowca {2} od miasta {3}", toReturn, A,
						((TownResourceType)B).ToString(), ((Towns)C).ToString());
					break;
				case TownLogTypes.RELACJE_ZMIENIONO:
					toReturn = String.Format("{0}relacje z miastem {1} zostaly zmienione o {2}", toReturn,
						((Towns)A).ToString(), B);
					break;
			}

			return toReturn;
		}
	}

	public enum TownLogTypes
	{
		OBYWATELSTWO_NADANIE = 0,
		OBYWATELSTWO_ZAKONCZONO = 1,
		OBYWATELSTWO_STATUS_NADANO = 2,
		KANCLERZ_NADANO_GLOWNY = 3,
		KANCLERZ_NADANO_DYPLOMACJI = 4,
		KANCLERZ_NADANO_ARMII = 5,
		KANCLERZ_NADANO_EKONOMII = 6,
		KANCLERZ_NADANO_BUDOWNICTWA = 7,
		BUDYNEK_ZLECONO_BUDOWE = 8,
		BUDYNEK_ZAKONCZONO_BUDOWE = 9,
		BUDYNEK_ZAWIESZONO_DZIALANIE = 10,
		BUDYNEK_WZNOWIONO_DZIALANIE = 11,
		BUDYNEK_ZNISZCZONO = 12,
		PODATKI_ZMIENIONO = 13,
		POSTERUNEK_WYBUDOWANO = 14,
		POSTERUNEK_ZAWIESZONO = 15,
		POSTERUNEK_WZNOWIONO = 16,
		SUROWCE_WYSLANO = 17,
		SUROWCE_OTRZYMANO = 18,
		RELACJE_ZMIENIONO = 19
	}
}
