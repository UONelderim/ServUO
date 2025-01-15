#region References

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mat = Server.Engines.BulkOrders.BulkMaterialType;

#endregion

namespace Server.Engines.BulkOrders
{
	[TypeAlias("Scripts.Engines.BulkOrders.LargeHunterBOD")]
	public class LargeHunterBOD : LargeBOD
	{
		public override BODType BODType => BODType.Hunter;
		
		public override BulkMaterialType Material => (BulkMaterialType)CollectedPoints;

		private double _CollectedPoints;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public double CollectedPoints
		{
			get => _CollectedPoints;
			set
			{
				_CollectedPoints = value;
				InvalidateProperties();
			}
		}

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
				4 => Utility.RandomList(LargeBulkEntry.LargeBoss),
				3 => Utility.RandomList(LargeBulkEntry.LargeHard),
				2 => Utility.RandomList(LargeBulkEntry.LargeMedium),
				_ => Utility.RandomList(LargeBulkEntry.LargeEasy),
			});;
		}

		public LargeHunterBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
		{
			Hue = 1556;
			AmountMax = amountMax;
			Entries = entries;
			RequireExceptional = reqExceptional;
			CollectedPoints = (double)mat;
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

		public override void OnEndCombine(SmallBOD small)
		{
			if (small is SmallHunterBOD hunterBod)
				_CollectedPoints += hunterBod.CollectedPoints;
		}
		
		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			
			list.Add(1060658, "{0}\t{1}", "Zebrane punkty", $"{_CollectedPoints:F2}"); // ~1_val~: ~2_val~
		}

		public LargeHunterBOD(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
			writer.Write(_CollectedPoints);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if(version > 0)
				_CollectedPoints = reader.ReadDouble();
		}
	}
}
