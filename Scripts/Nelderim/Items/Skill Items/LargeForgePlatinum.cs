using System;
using Nelderim.Towns;
using Server;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Items
{
	interface ITownItem
	{
		public Towns CitizienshipReq();
	}

	public class PlatinumSmeltHelper
	{
		public static double GetSmeltDifficulty(Mobile from, BaseOre ore, ISpecialForge forge)
		{
			if (ore.Resource != CraftResource.Platinum)
				return ore.GetSmeltDifficulty();	// standard behavior for non-platinum ore

			if (forge is ITownItem)
			{
				var forgeTown = ((ITownItem)forge).CitizienshipReq();

				if (forgeTown == Towns.None || TownDatabase.IsCitizenOfGivenTown(from, forgeTown))
					return 50.0; // same as Iron
				else
					from.SendMessage("Tylko obywatele miasta " + forgeTown.ToString() + " moga uzywac tego paleniska.");
			}
			return 800.0; // impossible to smelt
		}
	}

	public class BasePlatinumForge : Item, ISpecialForge, ITownItem
	{
		protected Towns m_CitizienshipRequirement;

		[CommandProperty(AccessLevel.GameMaster)]
		public Towns CitizienshipRequirement
		{
			get => m_CitizienshipRequirement;
			set { m_CitizienshipRequirement = value; }
		}

		protected BasePlatinumForge(int itemId) : base(itemId)
		{
			Movable = false;
		}

		public BasePlatinumForge(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_CitizienshipRequirement);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_CitizienshipRequirement = (Towns)reader.ReadInt();
		}

		public double GetSmeltDifficulty(Mobile from, BaseOre ore)
		{
			return PlatinumSmeltHelper.GetSmeltDifficulty(from, ore, this);
		}

		public Towns CitizienshipReq()
		{
			return m_CitizienshipRequirement;
		}
	}

	[Server.Engines.Craft.Forge]
	public class LargeForgePlatinumWest : BasePlatinumForge
	{
		private InternalItem m_Item;
		private InternalItem2 m_Item2;

		[Constructable]
		public LargeForgePlatinumWest() : base(0x199A)
		{
			Name = "Starozytny miech";

			m_Item = new InternalItem(this);
			m_Item2 = new InternalItem2(this);
		}

		public LargeForgePlatinumWest(Serial serial) : base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X, Y + 1, Z);
			if (m_Item2 != null)
				m_Item2.Location = new Point3D(X, Y + 2, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
			if (m_Item2 != null)
				m_Item2.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
			if (m_Item2 != null)
				m_Item2.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
			writer.Write(m_Item2);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
			m_Item2 = reader.ReadItem() as InternalItem2;
		}

		[Server.Engines.Craft.Forge]
		private class InternalItem : Item, ISpecialForge, ITownItem
		{
			[CommandProperty(AccessLevel.GameMaster)]
			public Towns CitizienshipRequirement
			{
				get { return m_Item != null ? m_Item.m_CitizienshipRequirement : Towns.None; }
				set { if (m_Item != null) m_Item.m_CitizienshipRequirement = value; }
			}

			private LargeForgePlatinumWest m_Item;

			public InternalItem(LargeForgePlatinumWest item) : base(0x1996)
			{
				Name = "Starozytne palenisko";
				Movable = false;

				m_Item = item;
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X, Y - 1, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as LargeForgePlatinumWest;
			}

			public double GetSmeltDifficulty(Mobile from, BaseOre ore)
			{
				return m_Item != null ? m_Item.GetSmeltDifficulty(from, ore) : 800;
			}

			public Towns CitizienshipReq()
			{
				return m_Item != null ? m_Item.CitizienshipReq() : Towns.None;
			}
		}

		[Server.Engines.Craft.Forge]
		private class InternalItem2 : Item, ISpecialForge, ITownItem
		{
			private LargeForgePlatinumWest m_Item;

			[CommandProperty(AccessLevel.GameMaster)]
			public Towns CitizienshipRequirement
			{
				get { return m_Item != null ? m_Item.m_CitizienshipRequirement : Towns.None; }
				set { if (m_Item != null) m_Item.m_CitizienshipRequirement = value; }
			}

			public InternalItem2(LargeForgePlatinumWest item) : base(0x1992)
			{
				Name = "Starozytne palenisko";
				Movable = false;

				m_Item = item;
			}

			public InternalItem2(Serial serial) : base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X, Y - 2, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as LargeForgePlatinumWest;
			}

			public double GetSmeltDifficulty(Mobile from, BaseOre ore)
			{
				return m_Item != null ? m_Item.GetSmeltDifficulty(from, ore) : 800;
			}

			public Towns CitizienshipReq()
			{
				return m_Item != null ? m_Item.CitizienshipReq() : Towns.None;
			}
		}
	}

	[Server.Engines.Craft.Forge]
	public class LargeForgePlatinumEast : BasePlatinumForge
	{
		private InternalItem m_Item;
		private InternalItem2 m_Item2;

		[Constructable]
		public LargeForgePlatinumEast() : base(0x197A)
		{
			Name = "Starozytny miech";

			m_Item = new InternalItem(this);
			m_Item2 = new InternalItem2(this);
		}

		public LargeForgePlatinumEast(Serial serial) : base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X + 1, Y, Z);
			if (m_Item2 != null)
				m_Item2.Location = new Point3D(X + 2, Y, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
			if (m_Item2 != null)
				m_Item2.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
			if (m_Item2 != null)
				m_Item2.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
			writer.Write(m_Item2);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
			m_Item2 = reader.ReadItem() as InternalItem2;
		}

		[Server.Engines.Craft.Forge]
		private class InternalItem : Item, ISpecialForge, ITownItem
		{
			private LargeForgePlatinumEast m_Item;

			[CommandProperty(AccessLevel.GameMaster)]
			public Towns CitizienshipRequirement
			{
				get { return m_Item != null ? m_Item.m_CitizienshipRequirement : Towns.None; }
				set { if (m_Item != null) m_Item.m_CitizienshipRequirement = value; }
			}

			public InternalItem(LargeForgePlatinumEast item) : base(0x197E)
			{
				Name = "Starozytne palenisko";
				Movable = false;

				m_Item = item;
			}

			public InternalItem(Serial serial) : base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X - 1, Y, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as LargeForgePlatinumEast;
			}

			public double GetSmeltDifficulty(Mobile from, BaseOre ore)
			{
				return m_Item != null ? m_Item.GetSmeltDifficulty(from, ore) : 800;
			}

			public Towns CitizienshipReq()
			{
				return m_Item != null ? m_Item.CitizienshipReq() : Towns.None;
			}
		}

		[Server.Engines.Craft.Forge]
		private class InternalItem2 : Item, ISpecialForge, ITownItem
		{
			private LargeForgePlatinumEast m_Item;

			[CommandProperty(AccessLevel.GameMaster)]
			public Towns CitizienshipRequirement
			{
				get { return m_Item != null ? m_Item.m_CitizienshipRequirement : Towns.None; }
				set { if (m_Item != null) m_Item.m_CitizienshipRequirement = value; }
			}

			public InternalItem2(LargeForgePlatinumEast item) : base(0x1982)
			{
				Name = "Starozytne palenisko";
				Movable = false;

				m_Item = item;
			}

			public InternalItem2(Serial serial) : base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X - 2, Y, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as LargeForgePlatinumEast;
			}

			public double GetSmeltDifficulty(Mobile from, BaseOre ore)
			{
				return m_Item != null ? m_Item.GetSmeltDifficulty(from, ore) : 800;
			}

			public Towns CitizienshipReq()
			{
				return m_Item != null ? m_Item.CitizienshipReq() : Towns.None;
			}
		}
	}
}