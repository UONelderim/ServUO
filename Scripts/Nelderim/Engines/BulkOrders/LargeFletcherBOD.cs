#region References

using System;
using System.Collections.Generic;

#endregion

namespace Server.Engines.BulkOrders
{
	[TypeAlias("Scripts.Engines.BulkOrders.LargeFletcherBOD")]
	public class LargeFletcherBOD : LargeBOD //TO BE REMOVED AFTER MIGRATION
	{
		private BulkMaterialType m_Material2;
		public override BODType BODType => BODType.Fletching;

		//public override int ComputeFame()
		//{
		//	return FletcherRewardCalculator.Instance.ComputeFame( this );
		//}

		//public override int ComputeGold()
		//{
		//	return FletcherRewardCalculator.Instance.ComputeGold( this );
		//}

		[Constructable]
		public LargeFletcherBOD()
		{
			LargeBulkEntry[] entries = new LargeBulkEntry[0];
			bool useMaterials = Utility.RandomBool();

			int amountMax = Utility.RandomList(10, 15, 20, 20);
			bool reqExceptional = (0.825 > Utility.RandomDouble());

			BulkMaterialType material;

			if (useMaterials)
				material = SmallBOD.GetRandomMaterial(BulkMaterialType.OakWood,
					SmallFletcherBOD.m_BowFletchingMaterialChances);
			else
				material = BulkMaterialType.None;


			Hue = 1425;
			AmountMax = amountMax;
			Entries = entries;
			RequireExceptional = reqExceptional;
			Material = material;
		}

		public LargeFletcherBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
		{
			Hue = 1425;
			AmountMax = amountMax;
			Entries = entries;
			RequireExceptional = reqExceptional;
			Material = mat;
		}

		public override List<Item> ComputeRewards(bool full)
		{
			List<Item> list = new List<Item>();

			//RewardGroup rewardGroup = FletcherRewardCalculator.Instance.LookupRewards(FletcherRewardCalculator.Instance.ComputePoints( this ) );

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

		public LargeFletcherBOD(Serial serial) : base(serial)
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
			ReplaceWith(new LargeFletchingBOD(AmountMax, RequireExceptional, Material, Entries));
		}

		public override int ComputeGold()
		{
			throw new NotImplementedException();
		}

		public override int ComputeFame()
		{
			throw new NotImplementedException();
		}
	}
}
