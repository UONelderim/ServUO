#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public abstract class SpecializedPowderOfTemperament : PowderOfTemperament
	{
		protected SpecializedPowderOfTemperament() : this(1)
		{
		}
		
		protected SpecializedPowderOfTemperament(int uses) : base(uses)
		{
		}

		public virtual CraftSystem CraftSystem { get; }

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
				from.Target = new InternalTarget(this);
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public SpecializedPowderOfTemperament(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		new protected class InternalTarget : PowderOfTemperament.InternalTarget
		{
			public InternalTarget(PowderOfTemperament powder) : base(powder) { }

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Powder is SpecializedPowderOfTemperament powder &&
				    powder.CraftSystem.CraftItems.SearchForSubclass(targeted.GetType()) != null)
					base.OnTarget(from, targeted);
				else
					from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
			}
		}
	}
}
