#region References

using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;

#endregion

namespace Server.Mobiles
{
	public class Hunter : BaseVendor
	{
		public override bool CanTeach { get { return false; } }
		protected override List<SBInfo> SBInfos { get; } = new List<SBInfo>();

		public override NpcGuild NpcGuild { get { return NpcGuild.RangersGuild; } }
		public override bool IsActiveBuyer { get { return false; } }
		public override bool IsActiveSeller { get { return false; } }

		[Constructable]
		public Hunter() : base("- mysliwy")
		{
			SetSkill(SkillName.Camping, 64.0, 100.0);
			SetSkill(SkillName.DetectHidden, 64.0, 100.0);
			SetSkill(SkillName.Tracking, 64.0, 100.0);
		}

		public override void InitSBInfo()
		{
		}

		public override VendorShoeType ShoeType
		{
			get { return Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes; }
		}

		#region Bulk Orders

		public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
		{
			if (!IsAssignedBuildingWorking())
			{
				return null;
			}

			PlayerMobile pm = from as PlayerMobile;

			if (pm != null && pm.NextHunterBulkOrder == TimeSpan.Zero &&
			    (fromContextMenu || 0.2 > Utility.RandomDouble()))
			{
				double theirSkill = pm.Skills[SkillName.Magery].Base;
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.Necromancy].Base);
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.Archery].Base);
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.Fencing].Base);
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.Macing].Base);
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.Swords].Base);
				theirSkill = Math.Max(theirSkill, pm.Skills[SkillName.AnimalTaming].Base);

				double largeprop = 0.0;

				if (theirSkill >= 105.1)
				{
					pm.NextHunterBulkOrder = TimeSpan.FromMinutes(60);
					largeprop = 0.18; // (2+8+8)
				}
				else if (theirSkill >= 90.1)
				{
					pm.NextHunterBulkOrder = TimeSpan.FromMinutes(30);
					largeprop = 0.15; // (5+10+0)
				}
				else if (theirSkill >= 70.1)
				{
					pm.NextHunterBulkOrder = TimeSpan.FromMinutes(20);
					largeprop = 0.1; // (10+0+0)
				}
				else
				{
					pm.NextHunterBulkOrder = TimeSpan.FromMinutes(10);
					largeprop = 0.0;
				}

				if (theirSkill >= 70.1 && largeprop > Utility.RandomDouble())
					return new LargeHunterBOD(theirSkill);

				return SmallHunterBOD.CreateRandomFor(from, theirSkill);
			}

			return null;
		}

		public override bool IsValidBulkOrder(Item item)
		{
			if (!IsAssignedBuildingWorking())
			{
				return false;
			}

			return (item is SmallHunterBOD || item is LargeHunterBOD);
		}

		public override bool SupportsBulkOrders(Mobile from)
		{
			if (from is PlayerMobile)
			{
				PlayerMobile pm = from as PlayerMobile;
				if (pm.Skills[SkillName.Magery].Base > 0 ||
				    pm.Skills[SkillName.Necromancy].Base > 0 ||
				    pm.Skills[SkillName.Fencing].Base > 0 ||
				    pm.Skills[SkillName.Swords].Base > 0 ||
				    pm.Skills[SkillName.Archery].Base > 0 ||
				    pm.Skills[SkillName.Macing].Base > 0 ||
				    pm.Skills[SkillName.AnimalTaming].Base > 0
				   )
				{
					return true;
				}
			}

			return false;
		}

		public override TimeSpan GetNextBulkOrder(Mobile from)
		{
			if (from is PlayerMobile)
				return ((PlayerMobile)from).NextHunterBulkOrder;

			return TimeSpan.Zero;
		}

		public override void OnSuccessfulBulkOrderReceive(Mobile from)
		{
			if (from is PlayerMobile)
				((PlayerMobile)from).NextHunterBulkOrder = TimeSpan.Zero;
		}

		#endregion

		public Hunter(Serial serial) : base(serial)
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
