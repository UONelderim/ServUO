#region References

using System;
using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	abstract public class ResouceCraftableBaseContainer : BaseContainer, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get
			{
				return m_Resource;
			}
			set
			{
				if (m_Resource != value)
				{
					m_Resource = value;
					Hue = CraftResources.GetHue(m_Resource);

					InvalidateProperties();
				}
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (Name == null)
				list.Add(LabelNumber);
			else
				list.Add(Name);

			//int woodType = BaseArmor.ResourceNameNumber(m_Resource);
			//if (woodType != 0)
			//    list.Add(woodType);
		}

		public int OnCraft(int quality,
			bool makersMark,
			Mobile from,
			CraftSystem craftSystem,
			Type typeRes,
			Type typeRes2,
			ITool tool,
			CraftItem craftItem,
			int resHue)
		{
			Resource = CraftResources.GetFromType(typeRes);
			return quality;
		}

		[Constructable]
		public ResouceCraftableBaseContainer(int itemID)
			: base(itemID)
		{
		}

		public ResouceCraftableBaseContainer(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write((int)m_Resource);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Resource = (CraftResource)reader.ReadInt();
		}
	}

	abstract public class ResouceCraftableBaseLight : BaseLight, ICraftable
	{
		private CraftResource m_Resource;

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (Name == null)
				list.Add(LabelNumber);
			else
				list.Add(Name);

			//int woodType = BaseArmor.ResourceNameNumber(Resource);
			//if (woodType != 0)
			//    list.Add(woodType);
		}

		public override int OnCraft(int quality,
			bool makersMark,
			Mobile from,
			CraftSystem craftSystem,
			Type typeRes,
			Type typeRes2,
			ITool tool,
			CraftItem craftItem,
			int resHue)
		{
			Resource = CraftResources.GetFromType(typeRes);
			return quality;
		}

		[Constructable]
		public ResouceCraftableBaseLight(int itemID)
			: base(itemID)
		{
		}

		public ResouceCraftableBaseLight(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
			writer.Write((int)m_Resource);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_Resource = (CraftResource)reader.ReadInt();
		}
	}

	abstract public class ResouceCraftable : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get
			{
				return m_Resource;
			}
			set
			{
				if (m_Resource != value)
				{
					m_Resource = value;
					Hue = CraftResources.GetHue(m_Resource);

					InvalidateProperties();
				}
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (Name == null)
				list.Add(LabelNumber);
			else
				list.Add(Name);

			//int woodType = BaseArmor.ResourceNameNumber(m_Resource);
			//if (woodType != 0)
			//    list.Add(woodType);
		}

		public int OnCraft(int quality,
			bool makersMark,
			Mobile from,
			CraftSystem craftSystem,
			Type typeRes,
			Type typeRes2,
			ITool tool,
			CraftItem craftItem,
			int resHue)
		{
			Resource = CraftResources.GetFromType(typeRes);
			return quality;
		}

		[Constructable]
		public ResouceCraftable(int itemID)
			: base(itemID)
		{
		}

		public ResouceCraftable(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write((int)m_Resource);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Resource = (CraftResource)reader.ReadInt();
		}
	}
}
