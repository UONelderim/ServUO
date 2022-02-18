#region References

using System;

#endregion

namespace Server.Items
{
	public class DragonsBlood : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} krew smoka", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public DragonsBlood() : this(1)
		{
		}

		[Constructable]
		public DragonsBlood(int amount) : base(3970, amount)
		{
			Name = "Krew smoka";
		}

		public DragonsBlood(Serial serial) : base(serial)
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
