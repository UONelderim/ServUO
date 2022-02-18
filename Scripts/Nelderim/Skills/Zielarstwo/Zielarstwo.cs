#region References

using System;
using System.Collections;
using Server.Engines.Harvest;
using Server.Mobiles;

#endregion

/*
TODO:
  * - Dokonczyc klasy CustomPlant (dodac duzo zmiennych, konstruktorow, pola typu Type? Przerobic wtedy m_SkillMin w zwyklym WeedPlant na statyczne.)
*/


namespace Server.Items.Crops
{
	// WeedPlant: Rosnacy krzaczek lub surowiec - do zbierania.
	public class WeedPlant : Item
	{
		public virtual string MsgCantBeMounted { get { return "Nie mozesz zabrac pzedmiotu bedac konno."; } }
		public virtual string MsgMustGetCloser { get { return "Musisz podejsc blizej, aby to zebrac."; } }
		public virtual string MsgPlantTooYoung { get { return "Przedmiot jest jeszcze gotowy do zabrania."; } }

		public virtual string MsgNoChanceToGet
		{
			get { return "Twoja wiedza o tym przedmiocie jest za mala, aby go zabrac."; }
		}

		public virtual string MsgSuccesfull { get { return "Udalo ci sie zebrac przedmiot."; } }
		public virtual string MsgGotSeed { get { return "Udalo ci sie zebrac szczepke rosliny!"; } }
		public virtual string MsgFailToGet { get { return "Nie udalo ci sie zebrac przedmiotu."; } }
		public virtual string MsgPlantDestroyed { get { return "Zniszczyles przedmiot."; } }

		private DateTime m_PlantedTime;

		//private bool m_DisableSeed;			// pozwala zablokowac uzyskiwanie sadzonek z danego egzemplarza krzaczka

		public virtual int SeedAmount { get { return 1; } } // ilosc uzyskiwanych nasion

		public virtual bool
			GivesSeed
		{
			get { return false; }
		} // czy dany typ zielska daje sadzonki? (FALSE dla zbieractwa [regi nekro])

		public virtual int CropAmount(Mobile from) // ilosc uzyskiwanego plon
		{
			double skill = from.Skills[SkillRequired].Value;
			return (int)Math.Round(skill / 100 * 12);
		}

		public virtual SkillName SkillRequired { get { return SkillName.Herbalism; } }

		public override bool ForceShowProperties { get { return true; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int GrowingTime { get; set; }

		/* DisableSeed
		[CommandProperty( AccessLevel.GameMaster )]
		public int DisableSeed
		{
			get{ return m_DisableSeed; }
			set{ m_DisableSeed = value; }
		}*/

		[CommandProperty(AccessLevel.GameMaster)]
		public double SkillMin { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public double SkillMax { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public double SkillDestroy { get; set; }

		public WeedPlant(int itemID) : base(itemID)
		{
			m_PlantedTime = DateTime.Now;

			Movable = false;
		}

		public WeedPlant(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
			writer.Write(GrowingTime);
			//writer.Write( (bool) m_DisableSeed );
			writer.Write(SkillMin);
			writer.Write(SkillMax);
			writer.Write(SkillDestroy);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			// version 0:
			GrowingTime = reader.ReadInt();
			//m_DisableSeed = reader.ReadBool();
			SkillMin = reader.ReadDouble();
			SkillMax = reader.ReadDouble();
			SkillDestroy = reader.ReadDouble();
		}

		// Funkcja determinujaca sukces w uzyskaniu szczepki podczas zbioru:
		public virtual bool CheckSeedGain(Mobile from)
		{
			if (!GivesSeed /* || m_DisableSeed */)
				return false;

			double skill = from.Skills[SkillRequired].Value;
			if (skill < 40)
				return false;

			double seedChance = 0.10 + (skill - 40) / (100 - 40) * 0.20;
			// 10% przy 40 skilla,  30% przy 100 skilla

			return seedChance > Utility.RandomDouble();
		}

		public virtual void CreateCrop(Mobile from, int count) { }
		public virtual void CreateSeed(Mobile from, int count) { }

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
				from.SendMessage(MsgCantBeMounted); // Nie mozesz zbierac surowcow bedac konno.
				return;
			}

			if (!from.InRange(this.GetWorldLocation(), 2) || !from.InLOS(this))
			{
				from.SendMessage(MsgMustGetCloser); // Musisz podejsc blizej, aby to zebrac.
				return;
			}

			if (m_PlantedTime.AddSeconds(GrowingTime) > DateTime.Now)
			{
				from.SendMessage(MsgPlantTooYoung); // Roslina jest jeszcze niedojrzala.
				return;
			}

			double skill = from.Skills[SkillRequired].Value;

			if (skill < SkillMin)
			{
				from.SendMessage(MsgNoChanceToGet); // Twoja wiedza o tym surowcu jest za mala, aby go zebrac.
				return;
			}

			// Zbieranie surowca:
			from.BeginAction(LockKind());
			from.RevealingAction();
			double AnimationDelayBeforeStart = 0.5;
			double AnimationIntervalBetween = 1.75;
			int AnimationNumberOfRepeat = 2;
			// Wpierw delay i animacja wewnatrz timera, a po ostatniej animacji timer uruchamia funkcje wyrywajaca ziolo (trzeci parametr):
			new WeedTimer(from, this, this.Animate, this.PullWeed, this.Unlock,
				TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween),
				AnimationNumberOfRepeat).Start();
		}

		// Jakiego typu czynnosci nie mozna wykonywac jednoczesnie ze zrywaniem ziol:
		public object LockKind()
		{
			return typeof(HarvestSystem);
		}

		public void Unlock(Mobile from)
		{
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
			if (from == null || !from.Alive)
			{
				return;
			}

			double skill = from.Skills[SkillRequired].Value;
			if (from.CheckSkill(SkillRequired, SkillMin, SkillMax))
			{
				from.SendMessage(MsgSuccesfull); // Udalo ci sie zebrac surowiec.
				CreateCrop(from, CropAmount(from));

				if (CheckSeedGain(from))
				{
					from.SendMessage(MsgGotSeed); // Udalo ci sie zebrac szczepke rosliny!
					CreateSeed(from, SeedAmount);
				}

				this.Delete();
			}
			else
			{
				from.SendMessage(MsgFailToGet); // Nie udalo ci sie zebrac surowica.
				if (skill >= SkillDestroy)
				{
					// Usuwanie surowca z mapy w przypadku niepowodzenia:
					this.Delete();
					from.SendMessage(MsgPlantDestroyed); // Zniszczyles surowiec.
				}
			}

			Unlock(from);
		}
	}


	// WeedSeed: Szczepka ziola - do sadzenia.
	public class WeedSeed : Item
	{
		public virtual string MsgCantBeMounted { get { return "Musisz stac na ziemi, aby moc to zrobic."; } }
		public virtual string MsgBadTerrain { get { return "Nie mozesz tego zrobic na tym terenie."; } }
		public virtual string MsgPlantAlreadyHere { get { return "W tym miejscu cos juz cos jest."; } }

		public virtual string MsgTooLowSkillToPlant
		{
			get { return "Nie masz wystarczajacej wiedzy, aby wykorzystac przetmiot."; }
		}

		public virtual string MsgPlantSuccess { get { return "Udalo ci sie zostawic przedmiot."; } }

		public virtual string MsgPlantFail
		{
			get { return "Nie udalo ci sie zostawic przedmiot, zmarnowales okazje."; }
		}

		// Typ terenu umozliwiajacy sadzenie:
		public virtual bool CanGrowFurrows { get { return true; } }
		public virtual bool CanGrowGrass { get { return false; } }
		public virtual bool CanGrowForest { get { return false; } }
		public virtual bool CanGrowJungle { get { return false; } }
		public virtual bool CanGrowCave { get { return false; } }
		public virtual bool CanGrowSand { get { return false; } }
		public virtual bool CanGrowSnow { get { return false; } }
		public virtual bool CanGrowSwamp { get { return false; } }

		public virtual SkillName SkillRequired { get { return SkillName.Herbalism; } }
		public virtual double MinSkillReq { get { return 90.0; } }

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

		public virtual bool CheckPlantChance(Mobile from)
		{
			return from.CheckSkill(SkillRequired, 80, 100);
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
			if (from.Skills[SkillRequired].Value < MinSkillReq)
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
			new WeedTimer(from, this, this.Animate, this.PlantWeed, this.Unlock,
				TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween),
				AnimationNumberOfRepeat).Start();
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
			return typeof(HarvestSystem);
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

			this.Consume();
			Unlock(from);
		}
	}


	// WeedCrop: Zebrany plon - do obrobki.
	public abstract class WeedCrop : Item, ICarvable
	{
		public virtual string MsgCreatedZeroReagent { get { return "Nie uzyskales wystarczajacej ilosci produktu."; } }
		public virtual string MsgFailedToCreateReagents { get { return "Nie udalo ci sie uzyskac produktu."; } }
		public virtual string MsgCreatedReagent { get { return "Uzyskales pewna ilosc produktu."; } }
		public virtual string MsgStartedToCut { get { return "Zaczynasz obrabiac przedmiot..."; } }

		public virtual SkillName SkillRequired { get { return SkillName.Herbalism; } }

		public WeedCrop(int itemID) : base(itemID)
		{
			Stackable = true;
			Weight = 0.2;
		}

		public WeedCrop(Serial serial) : base(serial)
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

		// Oblicza ilosc reagentow uzyskanych z jednego plonu:
		public virtual int AmountOfReagent(double skill)
		{
			return 2;
			//return (int) Math.Round( skill/100 * 25 );
		}

		public virtual void CreateReagent(Mobile from, int count) { }

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
			from.SendMessage(MsgStartedToCut);
			double AnimationDelayBeforeStart = 1.5;
			double AnimationIntervalBetween = 0.0;
			int AnimationNumberOfRepeat = 1;
			new WeedTimer(from, this, this.Animate, this.CutWeed, this.Unlock,
				TimeSpan.FromSeconds(AnimationDelayBeforeStart), TimeSpan.FromSeconds(AnimationIntervalBetween),
				AnimationNumberOfRepeat).Start();
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

			double skill = from.Skills[SkillRequired].Value;

			from.CheckSkill(SkillRequired, 0, 90); // granice skilla umozliwiajace przyrost podczas krojenia ziol

			int count = AmountOfReagent(skill);
			if (count == 0)
			{
				from.SendMessage(MsgCreatedZeroReagent); // Nie uzyskales wystarczajacej ilosci reagentu.
			}
			else
			{
				from.SendMessage(MsgCreatedReagent); // Uzyskales reagenty.
				CreateReagent(from, count);
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
			return typeof(HarvestSystem);
		}

		public void Unlock(Mobile from)
		{
			from.EndAction(LockKind());
		}
	}


	delegate bool WeedTimerAnimation(Mobile from);

	delegate void WeedTimerAction(Mobile from);

	delegate void WeedTimerFail(Mobile from);

	class WeedTimer : Timer
	{
		private readonly Mobile m_From;
		private readonly Item m_Plant;
		readonly WeedTimerAnimation m_Animation;
		readonly WeedTimerAction m_Action;
		readonly WeedTimerFail m_Fail;
		int m_Count;

		public WeedTimer(Mobile from, Item plant, WeedTimerAnimation animation, WeedTimerAction action,
			WeedTimerFail fail, TimeSpan delay, TimeSpan interval, int count) : base(delay, interval, count)
		{
			m_From = from;
			m_Plant = plant;
			m_Count = count;
			m_Action = action;
			m_Animation = animation;
			m_Fail = fail;
		}

		protected override void OnTick()
		{
			if (m_Plant == null || m_Plant.Deleted || m_From == null || !m_From.Alive)
			{
				Stop();
				m_Fail(m_From);
				return;
			}

			if (!m_Animation(m_From))
			{
				Stop();
				m_Fail(m_From);
				return;
			}

			--m_Count;
			if (m_Count < 1)
			{
				m_Action(m_From);
			}
		}
	}


	// Funkcje pomocne do sprawdzania terenu pod uprawe ziol.
	class WeedHelper
	{
		// 21.11.2012 mortuus: wygenerowalem grupy tiles'ow automatem bazujac na ich nazwach z pliku tiledata.mul. Nie wszystko sprawdzone.
		public static int[] TilesFurrows =
		{
			0x0009, 0x000A, 0x000B, 0x000C, 0x000D, 0x000E, 0x000F, 0x0010, 0x0011, 0x0012, 0x0013, 0x0014, 0x0015,
			0x0150, 0x0151, 0x0152, 0x0153, 0x0154, 0x0155, 0x0156, 0x0157, 0x0158, 0x0159, 0x015A, 0x015B, 0x015C
		};

		public static int[] TilesGrass =
		{
			0x0003, 0x0004, 0x0005, 0x0006, 0x003B, 0x003C, 0x003D, 0x003E, 0x007D, 0x007E, 0x007F, 0x00C0, 0x00C1,
			0x00C2, 0x00C3, 0x00D8, 0x00D9, 0x00DA, 0x00DB, 0x01A4, 0x01A5, 0x01A6, 0x01A7, 0x0242, 0x0243, 0x036F,
			0x0370, 0x0371, 0x0372, 0x0373, 0x0374, 0x0375, 0x0376, 0x037B, 0x037C, 0x037D, 0x037E, 0x03BF, 0x03C0,
			0x03C1, 0x03C2, 0x03C3, 0x03C4, 0x03C5, 0x03C6, 0x03CB, 0x03CC, 0x03CD, 0x03CE, 0x0579, 0x057A, 0x057B,
			0x057C, 0x057D, 0x057E, 0x057F, 0x0580, 0x058B, 0x058C, 0x05D7, 0x05D8, 0x05D9, 0x05DA, 0x05DB, 0x05DC,
			0x05DD, 0x05DE, 0x05E3, 0x05E4, 0x05E5, 0x05E6, 0x067D, 0x067E, 0x067F, 0x0680, 0x0681, 0x0682, 0x0683,
			0x0684, 0x0689, 0x068A, 0x068B, 0x068C, 0x0695, 0x0696, 0x0697, 0x0698, 0x0699, 0x069A, 0x069B, 0x069C,
			0x06A1, 0x06A2, 0x06A3, 0x06A4, 0x06B5, 0x06B6, 0x06B7, 0x06B8, 0x06B9, 0x06BA, 0x06BF, 0x06C0, 0x06C1,
			0x06C2, 0x06DE, 0x06DF, 0x06E0, 0x06E1
		};

		public static int[] TilesForest =
		{
			0x00C4, 0x00C5, 0x00C6, 0x00C7, 0x00C8, 0x00C9, 0x00CA, 0x00CB, 0x00CC, 0x00CD, 0x00CE, 0x00CF, 0x00D0,
			0x00D1, 0x00D2, 0x00D3, 0x00D4, 0x00D5, 0x00D6, 0x00D7, 0x00EF, 0x00F0, 0x00F1, 0x00F2, 0x00F3, 0x00F8,
			0x00F9, 0x00FA, 0x00FB, 0x015D, 0x015E, 0x015F, 0x0160, 0x0161, 0x0162, 0x0163, 0x0164, 0x0165, 0x0166,
			0x0167, 0x0168, 0x0324, 0x0325, 0x0326, 0x0327, 0x0328, 0x0329, 0x032A, 0x032B, 0x054F, 0x0550, 0x0551,
			0x0552, 0x05F1, 0x05F2, 0x05F3, 0x05F4, 0x05F9, 0x05FA, 0x05FB, 0x05FC, 0x05FD, 0x05FE, 0x05FF, 0x0600,
			0x0601, 0x0602, 0x0603, 0x0604, 0x0611, 0x0612, 0x0613, 0x0614, 0x0653, 0x0654, 0x0655, 0x0656, 0x065B,
			0x065C, 0x065D, 0x065E, 0x065F, 0x0660, 0x0661, 0x0662, 0x066B, 0x066C, 0x066D, 0x066E, 0x06AF, 0x06B0,
			0x06B1, 0x06B2, 0x06B3, 0x06B4, 0x06BB, 0x06BC, 0x06BD, 0x06BE, 0x0709, 0x070A, 0x070B, 0x070C, 0x0715,
			0x0716, 0x0717, 0x0718, 0x0719, 0x071A, 0x071B, 0x071C
		};

		public static int[] TilesJungle =
		{
			0x00AC, 0x00AD, 0x00AE, 0x00AF, 0x00B0, 0x00B3, 0x00B6, 0x00B9, 0x00BC, 0x00BD, 0x00BE, 0x00BF, 0x0100,
			0x0101, 0x0102, 0x0103, 0x0108, 0x0109, 0x010A, 0x010B, 0x01F0, 0x01F1, 0x01F2, 0x01F3, 0x026E, 0x026F,
			0x0270, 0x0271, 0x0276, 0x0277, 0x0278, 0x0279, 0x027A, 0x027B, 0x027C, 0x027D, 0x0286, 0x0287, 0x0288,
			0x0289, 0x0292, 0x0293, 0x0294, 0x0295, 0x0581, 0x0582, 0x0583, 0x0584, 0x0585, 0x0586, 0x0587, 0x0588,
			0x0589, 0x058A, 0x058D, 0x058E, 0x058F, 0x0590, 0x059F, 0x05A0, 0x05A1, 0x05A2, 0x05A3, 0x05A4, 0x05A5,
			0x05A6, 0x05B3, 0x05B4, 0x05B5, 0x05B6, 0x05B7, 0x05B8, 0x05B9, 0x05BA, 0x05F5, 0x05F6, 0x05F7, 0x05F8,
			0x0605, 0x0606, 0x0607, 0x0608, 0x0609, 0x060A, 0x060B, 0x060C, 0x060D, 0x060E, 0x060F, 0x0610, 0x0615,
			0x0616, 0x0617, 0x0618, 0x0727, 0x0728, 0x0729, 0x0733, 0x0734, 0x0735, 0x0736, 0x0737, 0x0738, 0x0739,
			0x073A
		};

		public static int[] TilesSwamp =
		{
			0x3DC0, 0x3DC1, 0x3DC2, 0x3DC3, 0x3DC4, 0x3DC5, 0x3DC6, 0x3DC7, 0x3DC8, 0x3DC9, 0x3DCA, 0x3DCB, 0x3DCC,
			0x3DCD, 0x3DCE, 0x3DCF, 0x3DD0, 0x3DD1, 0x3DD2, 0x3DD3, 0x3DD4, 0x3DD5, 0x3DD6, 0x3DD7, 0x3DD8, 0x3DD9,
			0x3DDA, 0x3DDB, 0x3DDC, 0x3DDD, 0x3DDE, 0x3DDF, 0x3DE0, 0x3DE1, 0x3DE2, 0x3DE3, 0x3DE4, 0x3DE5, 0x3DE6,
			0x3DE7, 0x3DE8, 0x3DE9, 0x3DEA, 0x3DEB, 0x3DEC, 0x3DED, 0x3DEE, 0x3DEF, 0x3DF0, 0x3DF1
		};

		public static int[] TilesCave = { 0x0245, 0x0246, 0x0247, 0x0248, 0x0249, 0x063B, 0x063C, 0x063D, 0x063E };

		public static int[] TilesSand =
		{
			0x0016, 0x0017, 0x0018, 0x0019, 0x0033, 0x0034, 0x0035, 0x0036, 0x0037, 0x0038, 0x0039, 0x003A, 0x011E,
			0x011F, 0x0120, 0x0121, 0x012A, 0x012B, 0x012C, 0x012D, 0x0192, 0x01A8, 0x01A9, 0x01AA, 0x01AB, 0x0282,
			0x0283, 0x0284, 0x0285, 0x028A, 0x028B, 0x028C, 0x028D, 0x028E, 0x028F, 0x0290, 0x0291, 0x0335, 0x0336,
			0x0337, 0x0338, 0x0339, 0x033A, 0x033B, 0x033C, 0x0341, 0x0342, 0x0343, 0x0344, 0x034D, 0x034E, 0x034F,
			0x0350, 0x0351, 0x0352, 0x0353, 0x0354, 0x0359, 0x035A, 0x035B, 0x035C, 0x03B7, 0x03B8, 0x03B9, 0x03BA,
			0x03BB, 0x03BC, 0x03BD, 0x03BE, 0x03C7, 0x03C8, 0x03C9, 0x03CA, 0x05A7, 0x05A8, 0x05A9, 0x05AA, 0x05AB,
			0x05AC, 0x05AD, 0x05AE, 0x05AF, 0x05B0, 0x05B1, 0x05B2, 0x064B, 0x064C, 0x064D, 0x064E, 0x064F, 0x0650,
			0x0651, 0x0652, 0x0657, 0x0658, 0x0659, 0x065A, 0x0663, 0x0664, 0x0665, 0x0666, 0x0667, 0x0668, 0x0669,
			0x066A, 0x066F, 0x0670, 0x0671, 0x0672
		};

		public static int[] TilesSnow =
		{
			0x011A, 0x011B, 0x011C, 0x011D, 0x0179, 0x017A, 0x017B, 0x0385, 0x0386, 0x0387, 0x0388, 0x0389, 0x038A,
			0x038B, 0x038C, 0x0391, 0x0392, 0x0393, 0x0394, 0x039D, 0x039E, 0x039F, 0x03A0, 0x03A1, 0x03A2, 0x03A3,
			0x03A4, 0x03A9, 0x03AA, 0x03AB, 0x03AC, 0x05BF, 0x05C0, 0x05C1, 0x05C2, 0x05C3, 0x05C4, 0x05C5, 0x05C6,
			0x05C7, 0x05C8, 0x05C9, 0x05CA, 0x05CB, 0x05CC, 0x05CD, 0x05CE, 0x05CF, 0x05D0, 0x05D1, 0x05D2, 0x05D3,
			0x05D4, 0x05D5, 0x05D6, 0x05DF, 0x05E0, 0x05E1, 0x05E2, 0x0745, 0x0746, 0x0747, 0x0748, 0x0751, 0x0752,
			0x0753, 0x0754, 0x075D, 0x075E, 0x075F, 0x0760
		};

		public static int[] TilesWater = { 0x00A8, 0x00A9, 0x00AA, 0x00AB, 0x0136, 0x0137 };
		public static int[] TilesLava = { 0x01F4, 0x01F5, 0x01F6, 0x01F7 };

		public static int[] TilesAcid =
		{
			0x2E02, 0x2E03, 0x2E04, 0x2E05, 0x2E06, 0x2E07, 0x2E08, 0x2E09, 0x2E0A, 0x2E0B, 0x2E0C, 0x2E0D, 0x2E0E,
			0x2E0F, 0x2E10, 0x2E11, 0x2E12, 0x2E13, 0x2E14, 0x2E15, 0x2E16, 0x2E17, 0x2E18, 0x2E19, 0x2E1A, 0x2E1B,
			0x2E1C, 0x2E1D, 0x2E1E, 0x2E1F, 0x2E20, 0x2E21, 0x2E22, 0x2E23, 0x2E24, 0x2E25, 0x2E26, 0x2E27, 0x2E28,
			0x2E29, 0x2E2A, 0x2E2B, 0x2E2C, 0x2E2D, 0x2E2E, 0x2E2F, 0x2E30, 0x2E31, 0x2E32, 0x2E33, 0x2E34, 0x2E35,
			0x2E36, 0x2E37, 0x2E38, 0x2E39, 0x2E3A, 0x2E3B
		};

		public static bool CheckCanGrow(WeedSeed crop, Map map, int x, int y)
		{
			if (crop.CanGrowFurrows && ValidateTiles(TilesFurrows, map, x, y))
				return true;
			if (crop.CanGrowGrass && ValidateTiles(TilesGrass, map, x, y))
				return true;
			if (crop.CanGrowForest && ValidateTiles(TilesForest, map, x, y))
				return true;
			if (crop.CanGrowJungle && ValidateTiles(TilesJungle, map, x, y))
				return true;
			if (crop.CanGrowCave && ValidateTiles(TilesCave, map, x, y))
				return true;
			if (crop.CanGrowSand && ValidateTiles(TilesSand, map, x, y))
				return true;
			if (crop.CanGrowSnow && ValidateTiles(TilesSnow, map, x, y))
				return true;
			if (crop.CanGrowSwamp && ValidateTiles(TilesSwamp, map, x, y))
				return true;
			return false;
		}

		/* ValidateGardenPlot
		public static bool ValidateGardenPlot( Map map, int x, int y )
		{
			bool ground = false;
			
			// Test for Dynamic Item
			IPooledEnumerable eable = map.GetItemsInBounds( new Rectangle2D( x, y, 1, 1 ) );
			foreach( Item item in eable )
			{
				if( item.ItemID == 0x32C9 ) // dirt; possibly also 0x32CA 
					ground = true;
			}
			eable.Free();

			// Test for Frozen into Map
			if ( !ground )
			{
			Tile[] tiles = map.Tiles.GetStaticTiles( x, y );
				for ( int i = 0; i < tiles.Length; ++i )
				{
					if ( ( tiles[i].ID & 0x3FFF ) == 0x32C9 )
						ground = true;
				}
			}

			return ground;
		}
		*/

		public static bool ValidateTiles(int[] tileTab, Map map, int x, int y)
		{
			int tileID = map.Tiles.GetLandTile(x, y).ID & 0x3FFF;
			for (int i = 0; i < tileTab.Length; ++i)
				if (tileID == tileTab[i])
					return true;
			return false;
		}

		public static bool CheckSpace(Point3D pnt, Map map)
		{
			bool freeSpace = true;
			IPooledEnumerable eable = map.GetItemsInRange(pnt, 0);
			foreach (Item crop in eable)
			{
				if ((crop != null) && (crop is WeedPlant))
				{
					freeSpace = false;
					break;
				}
			}

			eable.Free();
			return freeSpace;
		}
	}
}
