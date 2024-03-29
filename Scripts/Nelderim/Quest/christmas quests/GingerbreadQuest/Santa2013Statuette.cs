#region References

using Server.Network;

#endregion

namespace Server.Items
{
	public class Santa2013Statuette : BaseStatuette
	{
		[Constructable]
		public Santa2013Statuette(int itemID) : base(itemID)
		{
			ItemID = 0x194A;
			{
				LootType = LootType.Blessed;
				Weight = 10.0;
				Name = "Swiateczna statuetka";
			}
		}

		public Santa2013Statuette(Serial serial) : base(serial)
		{
		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (TurnedOn && IsLockedDown && (!m.Hidden || m.AccessLevel == AccessLevel.Player) &&
			    Utility.InRange(m.Location, this.Location, 2) && !Utility.InRange(oldLocation, this.Location, 2))
			{
				int cliloc = Utility.RandomMinMax(1007149, 1007165);

				PublicOverheadMessage(MessageType.Regular, 0x3B2, cliloc);
				Effects.PlaySound(Location, Map, 0x669); //HO HO HO!
			}

			base.OnMovement(m, oldLocation);
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

	[FlipableAttribute(0x194A, 0x1949)]
	public class SantaStatuette : Santa2013Statuette
	{
		//public override int LabelNumber{ get{ return 1097968; } } // santa statue

		[Constructable]
		public SantaStatuette() : base(0x194A)
		{
		}

		public SantaStatuette(Serial serial)
			: base(serial)
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
