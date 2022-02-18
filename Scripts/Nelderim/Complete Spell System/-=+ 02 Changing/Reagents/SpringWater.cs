#region References

using System;

#endregion

namespace Server.Items
{
	public class SpringWater : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} wiosenna woda", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public SpringWater() : this(1)
		{
		}

		[Constructable]
		public SpringWater(int amount) : base(0xE24, amount)
		{
			Hue = 0x47F;
			Name = "wiosenna woda";
		}

		public SpringWater(Serial serial) : base(serial)
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
