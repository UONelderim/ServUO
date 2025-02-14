using Server;
using Server.Mobiles;
using Xanthos.Interfaces;
using Xanthos.Utilities;
using static Arya.Auction.AuctionMessages;

namespace Arya.Auction
{
	public class MobileStatuette : Item, IShrinkItem
	{
		private BaseCreature m_Creature;

		public BaseCreature ShrunkenPet
		{
			get
			{
				if (m_Creature == null)
					return null;
				return m_Creature.Deleted ? null : m_Creature;
			}
		}

		private MobileStatuette(BaseCreature creature)
		{
			m_Creature = creature;
			ItemID = ShrinkTable.Lookup(m_Creature);
			Hue = m_Creature.Hue;

			m_Creature.ControlTarget = null;
			m_Creature.ControlOrder = OrderType.Stay;
			m_Creature.Internalize();
			m_Creature.SetControlMaster(null);
			m_Creature.SummonMaster = null;
			m_Creature.IsStabled = true;

			// Set the type of the creature as the name for this item
			Name = Misc.GetFriendlyClassName(creature.GetType().Name);
		}

		public MobileStatuette(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version

			writer.Write(m_Creature);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Creature = reader.ReadMobile() as BaseCreature;
		}

		public static MobileStatuette Create(Mobile from, BaseCreature creature)
		{
			if (!creature.Controlled || creature.ControlMaster != from)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_OWNER);
				return null;
			}

			if (creature.IsAnimatedDead || creature.IsDeadPet)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_DEAD);
				return null;
			}

			if (creature.Summoned)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_SUMMONED);
				return null;
			}

			if (creature is BaseFamiliar)
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_FAMILIAR);
				return null;
			}

			if ((creature is PackLlama || creature is PackHorse || creature is Beetle) &&
			    (creature.Backpack != null && creature.Backpack.Items.Count > 0))
			{
				from.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_BACKPACK);
				return null;
			}

			return new MobileStatuette(creature);
		}

		public void GiveCreatureTo(Mobile m)
		{
			m_Creature.SetControlMaster(m);
			m_Creature.MoveToWorld(m.Location, m.Map);

			m_Creature = null;
			Delete();
		}

		public bool Stable(Mobile m)
		{
			if (m_Creature == null)
			{
				m.SendMessage(AuctionConfig.MessageHue, ERR_CREATURE_REMOVED);
				Delete();
				return true;
			}

			if (m.Stabled.Count > AnimalTrainer.GetMaxStabled(m))
			{
				m.SendMessage(AuctionConfig.MessageHue, ERR_STABLE_FULL);
				return false;
			}

			m_Creature.ControlMaster = m;

			m_Creature.ControlTarget = null;
			m_Creature.ControlOrder = OrderType.Stay;
			m_Creature.Internalize();

			m_Creature.SetControlMaster(null);
			m_Creature.SummonMaster = null;
			m_Creature.IsBonded = false;

			m_Creature.IsStabled = true;
			m.Stabled.Add(m_Creature);

			m_Creature = null;
			Delete();

			return true;
		}

		public void ForceDelete()
		{
			if (null != m_Creature)
				m_Creature.Delete();

			Delete();
		}
	}
}
