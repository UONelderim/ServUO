#region References

using System;
using System.Collections;
using Server.Mobiles;

#endregion

namespace Server.Items.Crops
{
	// WeedCrop: Zebrany plon - do obrobki.
	public class WeedCrop : Item, ICarvable
	{
		public virtual string MsgCreatedZeroReagent { get { return "Nie uzyskales wystarczajacej ilosci produktu."; } }
		public virtual string MsgFailedToCreateReagents { get { return "Nie udalo ci sie uzyskac produktu."; } }
		public virtual string MsgCreatedReagent { get { return "Uzyskales pewna ilosc produktu."; } }
		public virtual string MsgStartedToCut { get { return "Zaczynasz obrabiac przedmiot..."; } }

		protected static SkillName[] defaultSkillsRequired = { WeedHelper.MainWeedSkill };
		public virtual SkillName[] SkillsRequired { get { return defaultSkillsRequired; } }

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
			return 5; //bylo 2
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
			new WeedTimer(from, this, Animate, CutWeed, Unlock, TimeSpan.FromSeconds(AnimationDelayBeforeStart),
				TimeSpan.FromSeconds(AnimationIntervalBetween), AnimationNumberOfRepeat).Start();
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

			from.CheckSkill(WeedHelper.MainWeedSkill, 0,
				90); // granice skilla umozliwiajace przyrost podczas krojenia ziol

			//double chance = skill / 100.0;
			if (true /*chance > Utility.RandomDouble()*/)
			{
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
			}
			else
			{
				from.SendMessage(MsgFailedToCreateReagents); // Nie udalo ci sie uzyskac reagentow.
			}

			Consume();
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

		public object LockKind()
		{
			return typeof(Herbalism);
		}

		public void Unlock(Mobile from)
		{
			from.EndAction(LockKind());
		}
	}
}
