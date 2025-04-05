#region References

using System;
using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	class Deck : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045025; } } // Poklad lodzi

		[Constructable]
		public Deck()
			: base(15969)
		{
			Weight = 3;
		}

		public Deck(Serial serial)
			: base(serial)
		{
		}
	}

	class Rudder : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045026; } } // Ster lodzi

		[Constructable]
		public Rudder()
			: base(15983)
		{
			Weight = 3;
		}

		public Rudder(Serial serial)
			: base(serial)
		{
		}
	}

	class Mast : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045027; } } // Maszt

		[Constructable]
		public Mast()
			: base(0x1E7D)
		{
			Weight = 3;
		}

		public Mast(Serial serial)
			: base(serial)
		{
		}
	}

	class Oars : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045028; } } // Wiosla

		[Constructable]
		public Oars()
			: base(0x1E27) //7722
		{
			Weight = 2;
		}

		public Oars(Serial serial)
			: base(serial)
		{
		}
	}

	class Prow : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045029; } } // Dziob lodzi

		[Constructable]
		public Prow()
			: base(15976)
		{
			Weight = 3;
		}

		public Prow(Serial serial)
			: base(serial)
		{
		}
	}

	class BoatHatch : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045030; } } // Wlaz

		[Constructable]
		public BoatHatch()
			: base(15973)
		{
			Name = "Wlaz";
			Weight = 3;
		}

		public BoatHatch(Serial serial)
			: base(serial)
		{
		}
	}

	class Sail : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045031; } } // Zagiel

		[Constructable]
		public Sail()
			: base(0x3DAC)
		{
			Weight = 2;
		}

		public Sail(Serial serial)
			: base(serial)
		{
		}
	}

	class Side : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045032; } } // Burta

		[Constructable]
		public Side()
			: base(16041)
		{
			Weight = 3;
		}

		public Side(Serial serial)
			: base(serial)
		{
		}
	}

	class BoatFront : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			Resource = CraftResource.None; // for now
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045033; } } // Przod lodzi

		[Constructable]
		public BoatFront()
			: base(5361)
		{
			Weight = 11;
		}

		public BoatFront(Serial serial)
			: base(serial)
		{
		}
	}

	class BoatBack : Item, ICraftable
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set
			{
				m_Resource = value;
				Hue = CraftResources.GetHue(m_Resource);
				InvalidateProperties();
			}
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
			Resource = CraftResource.None; // for now
			return 0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045034; } } // Tyl lodzi

		[Constructable]
		public BoatBack()
			: base(5361)
		{
			Weight = 11;
		}

		public BoatBack(Serial serial)
			: base(serial)
		{
		}
	}

	class BoatBuildProject : Item
	{
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override int LabelNumber { get { return 1045037; } }

		[Constructable]
		public BoatBuildProject()
			: base(0x14ED)
		{
			Hue = 6;
			Weight = 1;
		}

		public BoatBuildProject(Serial serial)
			: base(serial)
		{
		}
	}
}
