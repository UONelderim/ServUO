#region References
using Server.Items;
using System.Collections.Generic;
#endregion

namespace Server.Mobiles
{
    public class Barkeeper : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool RequiresCustomerIdentity => false; // kazdy karczmarz obsluzy zawsze kapturnika

        [Constructable]
        public Barkeeper()
            : base("- karczmarz")
        { }

        public Barkeeper(Serial serial)
            : base(serial)
        { }

        public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.ThighBoots : VendorShoeType.Boots;
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBarkeeper());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            SetWearable(new HalfApron(), Utility.RandomBrightHue(), 1);
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
