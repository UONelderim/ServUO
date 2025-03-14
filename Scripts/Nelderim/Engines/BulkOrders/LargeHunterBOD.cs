#region References

using System;
using System.Collections.Generic;

#endregion

namespace Server.Engines.BulkOrders
{
	[TypeAlias("Scripts.Engines.BulkOrders.LargeHunterBOD")]
	public class LargeHunterBOD : LargeBOD
	{
		public override BODType BODType => BODType.Hunter;

		public override int ComputeFame()
		{
			return HunterRewardCalculator.Instance.ComputeFame(this);
		}

		public override int ComputeGold()
		{
			return HunterRewardCalculator.Instance.ComputeGold(this);
		}


		[Constructable]
		public LargeHunterBOD() : this(Utility.RandomMinMax(71, 120))
		{
		}

		[Constructable]
		public LargeHunterBOD(double theirSkill)
		{
            double[] chances = { 0, 0, 0, 0 };
            if (theirSkill >= 105.1)
            {
                chances[0] = 0.20;
                chances[1] = 0.25;
                chances[2] = 0.30;
                chances[3] = 0.25;
            }
            else if (theirSkill >= 90.1)
            {
                chances[0] = 0.33;
                chances[1] = 0.67;
            }
            else
            {
                chances[0] = 1.00;
            }

            int level = Utility.RandomIndex(chances) + 1;
			Console.WriteLine("level=" + level + ", theirSkill=" + theirSkill + ", chances[3]=" + chances[3]);

			Hue = 1556;
			AmountMax = Utility.RandomList(10, 15, 20, 20);
			Entries = LargeBulkEntry.ConvertEntries(this, level switch
			{
				4 => Utility.RandomList(LargeBulkEntry.HunterLargeBoss),
				3 => Utility.RandomList(LargeBulkEntry.HunterLargeHard),
				2 => Utility.RandomList(LargeBulkEntry.HunterLargeMedium),
				_ => Utility.RandomList(LargeBulkEntry.HunterLargeEasy),
			});;
		}

		public LargeHunterBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
		{
			Hue = 1556;
			AmountMax = amountMax;
			Entries = entries;
			RequireExceptional = reqExceptional;
		}

		public override List<Item> ComputeRewards(bool full)
		{
			var list = new List<Item>();

			var rewardGroup =
				HunterRewardCalculator.Instance.LookupRewards(HunterRewardCalculator.Instance.ComputePoints(this));

			var item = rewardGroup?.AcquireItem()?.Construct();

			if (item != null)
				list.Add(item);

			return list;
		}

		public LargeHunterBOD(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(2); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if(version < 2)
				reader.ReadDouble();//CollectedPoints
		}
	}
}
