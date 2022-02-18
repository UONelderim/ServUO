#region References

using System;

#endregion

namespace Server.Items
{
	public class WyrmsHeart : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} serce wyrma", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public WyrmsHeart() : this(1)
		{
		}

		[Constructable]
		public WyrmsHeart(int amount) : base(3985, amount)
		{
		}

		public WyrmsHeart(Serial serial) : base(serial)
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
