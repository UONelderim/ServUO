#region References

using Server.Targeting;

#endregion

namespace Server.Items
{
	public class TrapRemovalKit : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int Charges { get; set; }

		[Constructable]
		public TrapRemovalKit() : base(7867)
		{
			Charges = 15;
		}

		public override int LabelNumber { get { return 1041508; } }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060584, Charges.ToString()); // uses remaining: ~1_val~
		}

		public TrapRemovalKit(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.InRange(GetWorldLocation(), 2))
			{
				from.RevealingAction();

				from.SendMessage("Wskaz pulapke, ktora chcesz rozbroic.");

				from.Target = new InternalTarget(this);
			}
			else
			{
				from.SendLocalizedMessage(500295); // You are too far away to do that.
			}
		}

		public void ConsumeCharge(Mobile consumer)
		{
			--Charges;

			if (Charges <= 0)
			{
				Delete();

				if (consumer != null)
					consumer.SendLocalizedMessage(1042531); // You have used all of the parts in your trap removal kit.
			}
			else
				InvalidateProperties();
		}

		private class InternalTarget : Target
		{
			private readonly TrapRemovalKit m_Kit;

			public InternalTarget(TrapRemovalKit kit) : base(7, false, TargetFlags.None)
			{
				m_Kit = kit;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Kit.Deleted)
					return;

				if (targeted is BaseTrap)
				{
					if (from.InRange(m_Kit.GetWorldLocation(), 7))
					{
						BaseTrap trap = (BaseTrap)targeted;
						trap.Untrap(from, m_Kit, false);
					}
					else
					{
						from.SendLocalizedMessage(500295); // You are too far away to do that.
					}
				}
				else
				{
					from.SendMessage("To nie jest pulapka!");
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.WriteEncodedInt(Charges);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					Charges = reader.ReadEncodedInt();
					break;
				}
			}
		}
	}
}
