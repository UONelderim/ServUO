#region References

using System;

#endregion

namespace Server.Items
{
	public class Brimstone : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} brimstone", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public Brimstone() : this(1)
		{
		}

		[Constructable]
		public Brimstone(int amount) : base(3967, amount)
		{
		}

		public Brimstone(Serial serial) : base(serial)
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
