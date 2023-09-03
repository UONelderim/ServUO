#region References

using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;

#endregion

namespace Server.Mobiles
{
	public class Hunter : BaseVendor
	{
		public override bool CanTeach => false;
		protected override List<SBInfo> SBInfos { get; } = new List<SBInfo>();

		public override NpcGuild NpcGuild => NpcGuild.RangersGuild;
		public override bool IsActiveBuyer => false;
		public override bool IsActiveSeller => false;
		public override bool SupportsBribes => false;

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

		public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes;

		public override BODType BODType => BODType.Hunter;

		public override bool IsValidBulkOrder(Item item)
		{
			return (item is SmallHunterBOD || item is LargeHunterBOD);
		}

		public override bool SupportsBulkOrders(Mobile from)
		{
			return BulkOrderSystem.NewSystemEnabled && 
			       from is PlayerMobile pm && (
				       pm.Skills[SkillName.Magery].Base > 0 ||
                       pm.Skills[SkillName.Necromancy].Base > 0 ||
                       pm.Skills[SkillName.Fencing].Base > 0 ||
                       pm.Skills[SkillName.Swords].Base > 0 ||
                       pm.Skills[SkillName.Archery].Base > 0 ||
                       pm.Skills[SkillName.Macing].Base > 0 ||
                       pm.Skills[SkillName.AnimalTaming].Base > 0 ||
                       pm.Skills[SkillName.Throwing].Base > 0
				       );
		}

		public override void OnSuccessfulBulkOrderReceive(Mobile from)
		{
			if (from is PlayerMobile)
				((PlayerMobile)from).NextHunterBulkOrder = TimeSpan.Zero;
		}

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
