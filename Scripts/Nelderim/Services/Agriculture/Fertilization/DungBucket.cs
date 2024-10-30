using System;
using System.Xml.Linq;
using Server.Items.Crops;
using Server.Targeting;

namespace Server.Items
{
	public class DungBucket : Item
	{
		public static int GraphicsEmpty => 0x0E83;
		private static int GraphicsFull => 0x0E7B;
		private static int GraphicsFertilizer => GraphicsFull;
		public static int HueEmpty => 2966;
		private static int HueFull => HueEmpty;
		private static int HueFertilizer => 2964;

		private static double WeightEmpty => 2;
		private static double WeightOfHay => 1;
		private double WeightWithContent => WeightEmpty + (m_IsFertilizer ? WeightOfHay : 0) + DungQuantity * 0.1;

		public int DungQuantityMax => 200;

		public override bool DisplayWeight => false;

		private int m_DungQuantity;
		[CommandProperty(AccessLevel.GameMaster)]
		public int DungQuantity
		{
			get { return m_DungQuantity; }
			set
			{
				m_DungQuantity = Math.Max(0, Math.Min(DungQuantityMax, value));
				if (m_DungQuantity == 0)
					m_IsFertilizer = false;

				Weight = WeightWithContent;

				LookUpdate();
			}
		}

		private bool m_IsFertilizer;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsFertilizer
		{
			get { return m_IsFertilizer; }
			set
			{
				m_IsFertilizer = value;
				Weight = WeightWithContent;
				LookUpdate();
			}
		}

		private void LookUpdate()
		{
			ItemID = m_DungQuantity <= 0 ? GraphicsEmpty : (m_IsFertilizer ? GraphicsFertilizer : GraphicsFull);
			Hue = m_DungQuantity <= 0 ? HueEmpty : (m_IsFertilizer ? HueFertilizer : HueFull);
			InvalidateProperties();
		}

		private void UseFertilizer(Mobile from, Plant plant)
		{
			if (plant == null || plant.Deleted || !IsFertilizer || DungQuantity < 1)
				return;

			if (plant.Fertilize(from))
				DungQuantity--;
		}

		[Constructable]
		public DungBucket() : base(GraphicsEmpty)
		{
			Name = "Wiadro na nawoz";
			Hue = HueEmpty;
			Weight = WeightWithContent;
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return false;
			}

			if (dropped is DungPile)
			{
				if (DungQuantity >= DungQuantityMax)
				{
					from.SendMessage("Wiadro nie pomiesci juz wiecej lajna.");
					return false;
				}
				int freeSpace = Math.Max(0, DungQuantityMax - DungQuantity);
				int receive = Math.Min(freeSpace, dropped.Amount);
				if (receive > 0)
				{
					DungQuantity += receive;

					dropped.Consume(receive);

					from.SendMessage("Wrzuciles lajno do wiadra.");
					return true;
				}
				return false;
			}

			if (dropped is SheafOfHay)
			{
				if (DungQuantity < 1)
				{
					from.SendMessage("Wrzuc wpierw troche lajna.");
					return false;
				}

				IsFertilizer = true;

				// Nie usuwamy siana.

				from.SendMessage("Zmieszales nieco siana z lajnem uzyskujac nawoz.");
				return true;
			}

			from.SendMessage("Aby uzyskac nawoz, zbierz do wiadra lajno, nastepnie dodaj troche siana.");
			return false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			if (DungQuantity <= 0)
			{
				from.SendMessage("Wiadro jest puste.");
				return;
			}

			if (IsFertilizer)
				from.SendMessage("Na czym uzyc nawozu?");
			else
				from.SendMessage("Wskaz siano, aby utworzyc nawoz, lub inne wiadro by przelac zawartosc.");

			from.Target = new InternalTarget(this, from);
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			AddNameProperty(list);

			if (m_DungQuantity <= 0)
				list.Add("Puste");
			else if (m_IsFertilizer)
				list.Add("Zawiera nawoz");
			else
				list.Add("Zawiera lajno");

			list.Add("Zawartosc: " + m_DungQuantity + "/" + DungQuantityMax);

			AddWeightProperty(list);
		}

		public DungBucket(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version

			writer.Write((int)DungQuantity);
			writer.Write((bool)IsFertilizer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			DungQuantity = reader.ReadInt();
			IsFertilizer = reader.ReadBool();
		}

		class InternalTarget : Target
		{
			DungBucket m_Bucket;
			Mobile m_From;

			public InternalTarget(DungBucket bucket, Mobile from) : base(2, true, TargetFlags.None)
			{
				m_Bucket = bucket;
				m_From = from;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Bucket.Deleted)
					return;

				if (targeted is DungBucket)
				{
					DungBucket target = (DungBucket)targeted;

					if (!m_Bucket.IsChildOf(from.Backpack) || !target.IsChildOf(from.Backpack))
					{
						from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
						return;
					}

					if (target.IsFertilizer == m_Bucket.IsFertilizer)
					{
						int targetCapacity = target.DungQuantityMax - target.DungQuantity;
						int toGive = Math.Min(targetCapacity, m_Bucket.DungQuantity);

						target.DungQuantity += toGive;
						m_Bucket.DungQuantity -= toGive;

						from.SendMessage("Przelewasz zawartosc do drugiego wiadra.");
					}
					else
					{
						from.SendMessage("Decydujesz sie nie mieszac zawartosci tych wiader, gdyz zawieraja rozne substancje.");
					}
				}
				else if (targeted is SheafOfHay)
				{
					if (m_Bucket.IsFertilizer)
					{
						from.SendMessage("Nie potrzeba wiecej siana w tym nawozie.");
					}
					else
					{
						m_Bucket.IsFertilizer = true;

						from.SendMessage("Zmieszales nieco siana z lajnem uzyskujac nawoz.");
					}
				}
				else if (targeted is Plant)
				{
					m_Bucket.UseFertilizer(m_From, (Plant)targeted);
				}
				else if (targeted is IPoint3D)
				{
					m_Bucket.UseFertilizer(m_From, FindPlant((IPoint3D)targeted, from.Map));
				}
				else
				{
					from.SendMessage("Nie mozesz uzyc na tym zawartosci wiadra.");
				}
			}

			private Plant FindPlant(IPoint3D p, Map map)
			{
				Plant plant = null;

				IPooledEnumerable eable = map.GetItemsInRange(new Point3D(p), 0);
				foreach (Item item in eable)
				{
					if (item != null && item is Plant)
					{
						plant = (Plant) item;
						break;
					}
				}
				eable.Free();

				return plant;
			}
		}
	}
}