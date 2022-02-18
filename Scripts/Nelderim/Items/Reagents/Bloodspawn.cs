#region References

using System;

#endregion

namespace Server.Items
{
	public class Bloodspawn : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} serce demona", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public Bloodspawn() : this(1)
		{
		}

		[Constructable]
		public Bloodspawn(int amount) : base(3964, amount)
		{
			Name = "Serce demona";
		}

		public Bloodspawn(Serial serial) : base(serial)
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
