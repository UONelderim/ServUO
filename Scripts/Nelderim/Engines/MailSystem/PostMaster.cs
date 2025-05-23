using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class PostMaster : BaseVendor
	{
		private List<SBInfo> m_SBInfos = [];
		protected override List<SBInfo> SBInfos => m_SBInfos;

		public override NpcGuild NpcGuild => NpcGuild.MagesGuild;

		[Constructable]
		public PostMaster() : base("- posłaniec")
		{
			SetSkill(SkillName.EvalInt, 60.0, 83.0);
			SetSkill(SkillName.Inscribe, 90.0, 100.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBPostMaster());
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is IMailItem)
			{
				//Show send gump
			}
			return base.OnDragDrop(from, dropped);
		}

		public PostMaster(Serial serial)
			: base(serial)
		{
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
	}
}
