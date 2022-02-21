#region References

using System;

#endregion

namespace Server.Items
{
	public class PetrafiedWood : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} spetryfikowane drzewo", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public PetrafiedWood() : this(1)
		{
		}

		[Constructable]
		public PetrafiedWood(int amount) : base(0x97A, amount)
		{
			Hue = 0x46C;
			Name = "spetryfikowane drzewo";
		}

		public PetrafiedWood(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
