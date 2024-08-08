#region References

using Server.Gumps;

#endregion

namespace Server.Items
{
	public abstract class RaceChoiceItem : Item
	{
		protected Race m_Race;

		public RaceChoiceItem(Serial serial) : base(serial)
		{
		}

		public RaceChoiceItem(int itemID) : base(itemID)
		{
			Weight = 1.0;
			Movable = false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_Race == null)
			{
				from.SendMessage("Artefakt nie jest skonfigurowany poprawnie.");
			}
			if (!from.InRange(this.GetWorldLocation(), 2))
			{
				from.SendLocalizedMessage(500295); // Jestes za daleko, zeby to zrobic.
			}
			if (from.IsPlayer() && (from.Faction == null || !from.Faction.IsFactionRace(m_Race)))
			{
				from.SendMessage("Nie mozesz wybrac tej rasy.");
			}
			else
			{
				from.SendGump(new RaceChoiceGump(from, m_Race));
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);

			writer.Write(m_Race);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			m_Race = reader.ReadRace();
		}
	}
}
