#region References

using System.Collections.Generic;

#endregion

namespace Server.Mobiles
{
	public class Konsorcjum : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		public override NpcGuild NpcGuild { get { return NpcGuild.MagesGuild; } }

		[Constructable]
		public Konsorcjum() : base("- Członek Kompanii Handlowej")
		{
			SetSkill(SkillName.Meditation, 80.0, 100.0);
			SetSkill(SkillName.Begging, 80.0, 100.0);
			SetSkill(SkillName.TasteID, 80.0, 100.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBKonsorcjum());
		}

		public override VendorShoeType ShoeType
		{
			get { return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
		}

		public Konsorcjum(Serial serial) : base(serial)
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
