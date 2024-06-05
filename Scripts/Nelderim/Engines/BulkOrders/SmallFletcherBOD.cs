#region References

using System;
using System.Collections.Generic;
using Server.Engines.Craft;

#endregion

namespace Server.Engines.BulkOrders
{
	[TypeAlias("Scripts.Engines.BulkOrders.SmallFletcherBOD")]
	public class SmallFletcherBOD : SmallBOD //TO BE REMOVED AFTER MIGRATION
	{
		private BulkMaterialType m_Material2;

		public static double[] m_BowFletchingMaterialChances =
		{
			0.501812500, // None
			0.251000000, // Zywiczne / Oak
			0.126000000, // Puste / Ash
			0.063500000, // Skamieniale / Yew
			0.032250000, // Opalone / Bloodwood
			0.016625000, // Gietkie / Heartwood
			0.008812500 // Zmarzniete / Frostwood
		};

		public static double[] m_BowFletchingMaterial2Chances =
		{
			0.25, // Leather
			0.25, // Gut
			0.25, // Cannabis
			0.25, // Silk
		};

		public override BODType BODType => BODType.Fletching;

		public override int ComputeFame()
		{
			return 0;
		}

		public override int ComputeGold()
		{
			return 0;
		}

		public override List<Item> ComputeRewards(bool full)
		{
			List<Item> list = new List<Item>();

			//RewardGroup rewardGroup = FletcherRewardCalculator.Instance.LookupRewards( FletcherRewardCalculator.Instance.ComputePoints( this ) );

			//if ( rewardGroup != null )
			//{
			//	if ( full )
			//	{
			//		for ( int i = 0; i < rewardGroup.Items.Length; ++i )
			//		{
			//			Item item = rewardGroup.Items[i].Construct();

			//			if ( item != null )
			//				list.Add( item );
			//		}
			//	}
			//	else
			//	{
			//		RewardItem rewardItem = rewardGroup.AcquireItem();

			//		if ( rewardItem != null )
			//		{
			//			Item item = rewardItem.Construct();

			//			if ( item != null )
			//				list.Add( item );
			//		}
			//	}
			//}

			return list;
		}

		public static SmallFletcherBOD CreateRandomFor(Mobile m)
		{
			SmallBulkEntry[] entries = new SmallBulkEntry[0];
			bool useMaterials = Utility.RandomBool();

			if (entries.Length > 0)
			{
				double theirSkill = m.Skills[SkillName.Fletching].Base;
				int amountMax;

				if (theirSkill >= 70.1)
					amountMax = Utility.RandomList(10, 15, 20, 20);
				else if (theirSkill >= 50.1)
					amountMax = Utility.RandomList(10, 15, 15, 20);
				else
					amountMax = Utility.RandomList(10, 10, 15, 20);

				BulkMaterialType material = BulkMaterialType.None;

				if (useMaterials && theirSkill >= 70.1)
				{
					for (int i = 0; i < 20; ++i)
					{
						BulkMaterialType check =
							GetRandomMaterial(BulkMaterialType.OakWood, m_BowFletchingMaterialChances);
						double skillReq = 0.0;

						switch (check)
						{
							case BulkMaterialType.OakWood:
								skillReq = 65.0;
								break;
							case BulkMaterialType.AshWood:
								skillReq = 75.0;
								break;
							case BulkMaterialType.YewWood:
								skillReq = 85.0;
								break;
							case BulkMaterialType.Heartwood:
								skillReq = 95.0;
								break;
							case BulkMaterialType.Bloodwood:
								skillReq = 99.0;
								break;
							case BulkMaterialType.Frostwood:
								skillReq = 99.0;
								break;
						}

						if (theirSkill >= skillReq)
						{
							material = check;
							break;
						}
					}
				}

				//BulkMaterialType material2 = GetRandomMaterial(BulkMaterialType.BowstringLeather, BulkMaterialType.BowstringGut, m_BowFletchingMaterial2Chances);

				double excChance = 0.0;

				if (theirSkill >= 70.1)
					excChance = (theirSkill + 80.0) / 200.0;

				bool reqExceptional = (excChance > Utility.RandomDouble());

				CraftSystem system = DefBowFletching.CraftSystem;

				List<SmallBulkEntry> validEntries = new List<SmallBulkEntry>();

				for (int i = 0; i < entries.Length; ++i)
				{
					CraftItem item = system.CraftItems.SearchFor(entries[i].Type);

					if (item != null)
					{
						bool allRequiredSkills = true;
						double chance = item.GetSuccessChance(m, null, system, false, ref allRequiredSkills);

						if (allRequiredSkills && chance >= 0.0)
						{
							if (reqExceptional)
								chance = item.GetExceptionalChance(system, chance, m);

							if (chance > 0.0)
								validEntries.Add(entries[i]);
						}
					}
				}

				if (validEntries.Count > 0)
				{
					SmallBulkEntry entry = validEntries[Utility.Random(validEntries.Count)];
					return new SmallFletcherBOD(entry, material, amountMax, reqExceptional);
				}
			}

			return null;
		}

		private SmallFletcherBOD(SmallBulkEntry entry, BulkMaterialType material, int amountMax, bool reqExceptional)
		{
			this.Hue = 1425;
			this.AmountMax = amountMax;
			this.Type = entry.Type;
			this.Number = entry.Number;
			this.Graphic = entry.Graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = material;
		}

		[Constructable]
		public SmallFletcherBOD()
		{
			SmallBulkEntry[] entries = new SmallBulkEntry[0];
			bool useMaterials = Utility.RandomBool();

			if (entries.Length > 0)
			{
				int hue = 1425;
				int amountMax = Utility.RandomList(10, 15, 20);

				BulkMaterialType material;

				if (useMaterials)
					material = GetRandomMaterial(BulkMaterialType.OakWood, m_BowFletchingMaterialChances);
				else
					material = BulkMaterialType.None;

				//BulkMaterialType material2 = GetRandomMaterial(BulkMaterialType.BowstringLeather, BulkMaterialType.BowstringGut, m_BowFletchingMaterial2Chances);

				bool reqExceptional = Utility.RandomBool() || (material == BulkMaterialType.None);

				SmallBulkEntry entry = entries[Utility.Random(entries.Length)];

				this.Hue = hue;
				this.AmountMax = amountMax;
				this.Type = entry.Type;
				this.Number = entry.Number;
				this.Graphic = entry.Graphic;
				this.RequireExceptional = reqExceptional;
				this.Material = material;
				//this.Material2 = material2;
			}
		}

		public SmallFletcherBOD(int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional,
			BulkMaterialType mat)
		{
			this.Hue = 1425;
			this.AmountMax = amountMax;
			this.AmountCur = amountCur;
			this.Type = type;
			this.Number = number;
			this.Graphic = graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = mat;
			//if (mat2 != BulkMaterialType.None)
			//	this.Material2 = mat2;
			//else
			//	this.Material2 = BulkMaterialType.BowstringLeather;
		}

		public SmallFletcherBOD(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
			writer.Write((int)m_Material2);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
					m_Material2 = (BulkMaterialType)reader.ReadInt();
					break;
			}
			ReplaceWith(new SmallFletchingBOD(AmountCur, AmountMax, Type, Number, Graphic, RequireExceptional, Material, GraphicHue));
		}
	}
}
