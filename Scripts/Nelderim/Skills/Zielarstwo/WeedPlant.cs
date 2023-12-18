#region References

using System;

#endregion

namespace Server.Items.Crops
{
	public class WeedPlant : Item
	{
		public virtual string MsgCantBeMounted => "Nie mozesz zabrac pzedmiotu bedac konno.";
		public virtual string MsgMustGetCloser => "Musisz podejsc blizej, aby to zebrac.";
		public virtual string MsgPlantTooYoung => "Przedmiot jest jeszcze gotowy do zabrania.";
		public virtual string MsgNoChanceToGet => "Twoja wiedza o tym przedmiocie jest za mala, aby go zabrac.";
		public virtual string MsgSuccesfull => "Udalo ci sie zebrac przedmiot.";
		public virtual string MsgGotSeed => "Udalo ci sie zebrac szczepke rosliny!";
		public virtual string MsgFailToGet => "Nie udalo ci sie zebrac przedmiotu.";
		public virtual string MsgPlantDestroyed => "Zniszczyles przedmiot.";

		private DateTime m_PlantedTime;

		//private bool m_DisableSeed;			// pozwala zablokowac uzyskiwanie sadzonek z danego egzemplarza krzaczka

		public virtual int SeedAmount => 3; // ilosc uzyskiwanych nasion  (było 1)

		public virtual bool GivesSeed => false; // czy daje sadzonki? (FALSE dla zbieractwa [regi nekro])

		public virtual int CropAmount(Mobile from) // ilosc uzyskiwanego plon
		{
			double skill = WeedHelper.GetHighestSkillValue(from, SkillsRequired);
			return (int)Math.Round(skill / 100 * 12);
		}

		protected static SkillName[] defaultSkillsRequired = { WeedHelper.MainWeedSkill };
		public virtual SkillName[] SkillsRequired => defaultSkillsRequired;
		public override bool ForceShowProperties => true;

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

			// 10% przy 40 skilla,  30% przy 100 skilla
			return WeedHelper.CheckSkills(from, SkillsRequired, 40, 10, 100, 30);
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

			if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
			{
				from.SendMessage(MsgMustGetCloser); // Musisz podejsc blizej, aby to zebrac.
				return;
			}

			if (m_PlantedTime.AddSeconds(GrowingTime) > DateTime.Now)
			{
				from.SendMessage(MsgPlantTooYoung); // Roslina jest jeszcze niedojrzala.
				return;
			}

			double skill = WeedHelper.GetHighestSkillValue(from, SkillsRequired);

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
			new WeedTimer(from, this, Animate, PullWeed, Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart),
				TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
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
			if (!from.InRange(GetWorldLocation(), 2))
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

			from.CheckSkill(WeedHelper.MainWeedSkill, SkillMin, SkillMax); // koks zielarstwa na krzaczku

			if (WeedHelper.CheckSkills(from, SkillsRequired, SkillMin, SkillMax))
			{
				from.SendMessage(MsgSuccesfull); // Udalo ci sie zebrac surowiec.
				CreateCrop(from, CropAmount(from));

				if (CheckSeedGain(from))
				{
					from.SendMessage(MsgGotSeed); // Udalo ci sie zebrac szczepke rosliny!
					CreateSeed(from, SeedAmount);
				}

				Delete();
			}
			else
			{
				from.SendMessage(MsgFailToGet); // Nie udalo ci sie zebrac surowica.
				if (from.Skills[WeedHelper.MainWeedSkill].Value >= SkillDestroy)
				{
					// Usuwanie surowca z mapy w przypadku niepowodzenia:
					Delete();
					from.SendMessage(MsgPlantDestroyed); // Zniszczyles surowiec.
				}
			}

			Unlock(from);
		}
	}
}
