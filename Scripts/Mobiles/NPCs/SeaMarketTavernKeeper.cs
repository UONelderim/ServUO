using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SeaMarketTavernKeeper : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos => m_SBInfos;

        [Constructable]
        public SeaMarketTavernKeeper() : base("- karczmarz nadmorski")
        {
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSeaMarketTavernKeeper());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

			SetWearable(new HalfApron(), dropChance: 1);
        }

        public SeaMarketTavernKeeper(Serial serial) : base(serial)
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
