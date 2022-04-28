#region References

using System;

#endregion

namespace Server.Items
{
	public class RedDragonsHeart : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} serce ognistego smoka", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public RedDragonsHeart() : this(1)
		{
		}

		[Constructable]
		public RedDragonsHeart(int amount) : base(3985, amount)
		{
			Name = "Serce ognistego smoka";
			Hue = 2143;
		}

		public RedDragonsHeart(Serial serial) : base(serial)
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

	public class BlueDragonsHeart : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} serce lodowego smoka", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public BlueDragonsHeart() : this(1)
		{
		}

		[Constructable]
		public BlueDragonsHeart(int amount) : base(3985, amount)
		{
			Name = "Serce lodowego smoka";
			Hue = 2150;
		}

		public BlueDragonsHeart(Serial serial) : base(serial)
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

	public class DragonsHeart : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return new TextDefinition(LabelNumber, String.Format("{0} serce smoka", Amount));
			}
		}

		bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public DragonsHeart() : this(1)
		{
		}

		[Constructable]
		public DragonsHeart(int amount) : base(3985, amount)
		{
			Name = "Serce smoka";
			Hue = 2666;
		}

		public DragonsHeart(Serial serial) : base(serial)
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
