using System;
using Server.Engines.Craft;
using Server.Targeting;

namespace Server.Items
{
	public class DungShovel : BaseTool, ICraftable
	{
		public static int DefaultHue => 0x8000 + 0x21E;

		[Constructable]
		public DungShovel() : this(50)
		{
		}

		[Constructable]
		public DungShovel(int uses) : base(uses, 0xF39)
		{
			Weight = 5.0;
			Name = "szufla do lajna";
			Hue = DefaultHue;
		}

		public DungShovel(Serial serial)
			: base(serial)
		{
		}

		public new int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
		{
			int r = base.OnCraft(quality, makersMark, from, craftSystem, typeRes, tool, craftItem, resHue);

			Hue = DefaultHue;

			return r;
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("Do czego chcesz uzyc szufli?");
			from.Target = new InternalDungTarget(this);
		}

		private class InternalDungTarget : Target
		{
			DungShovel m_Shovel;

			public InternalDungTarget(DungShovel shovel) : base(2, true, TargetFlags.None)
			{
				m_Shovel = shovel;
			}
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is DungPile)
				{
					DungPile dung = (DungPile)targeted;
					DungBucket bucket = FindBucket(from);
					if (bucket != null)
					{
						if (bucket.DungQuantity < bucket.DungQuantityMax)
						{
							if (bucket.OnDragDrop(from, dung))
							{
								m_Shovel.UsesRemaining -= 1;

								if (m_Shovel.UsesRemaining <= 0)
								{
									m_Shovel.Delete();
									from.SendMessage("Szufla zlamala sie!");
								}
							}
						}
						else
						{
							from.SendMessage("Wiadro nie pomiesci wiecej nawozu.");
						}
					}
					else
					{
						from.SendMessage("Musisz posiadac wiadro na nawoz w plecaku.");
					}
				}
				else
				{
					from.SendMessage("Szufla nie nadaje sie do tego.");
				}
			}

			private static DungBucket FindBucket(Mobile from)
			{
				if (from == null || from.Backpack == null)
					return null;

				DungBucket bucketToUse = null;

				Item[] found = from.Backpack.FindItemsByType(typeof(DungBucket));
				foreach (Item it in found)
				{
					DungBucket bucket = it as DungBucket;
					if (bucketToUse == null)
					{
						bucketToUse = bucket;
					}
					else if (bucket.DungQuantity < bucket.DungQuantityMax && bucket.DungQuantity > bucketToUse.DungQuantity) // prefer the fullest bucket
					{
						bucketToUse = bucket;
					}
				}

				return bucketToUse;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override Engines.Craft.CraftSystem CraftSystem
		{
			get { return null; }
		}
	}
}
