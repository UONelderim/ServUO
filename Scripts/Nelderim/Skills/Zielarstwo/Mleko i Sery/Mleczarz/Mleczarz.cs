#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Mleczarz : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public Mleczarz() : base("- Mleczarz")
		{
			SetSkill(SkillName.Cooking, 90.0, 100.0);
			SetSkill(SkillName.TasteID, 75.0, 98.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBMleczarz());
		}

		public override VendorShoeType ShoeType
		{
			get { return Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes; }
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem(new HalfApron());
		}

		public Mleczarz(Serial serial) : base(serial)
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
