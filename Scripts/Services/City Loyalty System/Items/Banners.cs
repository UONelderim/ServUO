using Server.Items;
using System.Collections.Generic;

namespace Server.Engines.CityLoyalty
{
    public class CityBanner : InterchangeableAddon
    {
        public override BaseAddonDeed Deed => new CityBannerDeed(City);

        public override bool ForceShowProperties => true;

        [CommandProperty(AccessLevel.GameMaster)]
        public City City { get; private set; }

        public override int EastID => SouthID + 9;
        public override int SouthID => BannerInfo[City][0];

        [Constructable]
        public CityBanner(City city)
            : base(true, BannerInfo[city][1])
        {
            City = city;
        }

        public static Dictionary<City, int[]> BannerInfo { get; set; }

        public static void Initialize()
        {
            BannerInfo = new Dictionary<City, int[]>();
            //ID     Cliloc
            BannerInfo[City.Tasandora] = new int[] { 0x4B63, 1098171 };
            BannerInfo[City.Twierdza] = new int[] { 0x4B64, 1098172 };
            BannerInfo[City.Tirassa] = new int[] { 0x4B65, 1098173 };
            BannerInfo[City.LDelmah] = new int[] { 0x4B66, 1098174 };
            BannerInfo[City.Orod] = new int[] { 0x4B67, 1098175 };
            BannerInfo[City.Lotharn] = new int[] { 0x4B62, 1098170 };
            BannerInfo[City.Garlan] = new int[] { 0x4B6A, 1098178 };
            BannerInfo[City.ArtTrader] = new int[] { 0x4B69, 1098177 };
            BannerInfo[City.Celendir] = new int[] { 0x4B68, 1098176 };
        }

        public CityBanner(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write((int)City);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            City = (City)reader.ReadInt();
        }
    }

    public class CityBannerDeed : InterchangeableAddonDeed
    {
        public override BaseAddon Addon => new CityBanner(City);

        public override int LabelNumber => CityBanner.BannerInfo[City][1];

        public override int EastID => SouthID + 9;
        public override int SouthID => CityBanner.BannerInfo[City][0];

        [CommandProperty(AccessLevel.GameMaster)]
        public City City { get; private set; }

        [Constructable]
        public CityBannerDeed(City city)
        {
            City = city;

            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                CityLoyaltySystem sys = CityLoyaltySystem.GetCityInstance(City);

                if (CityLoyaltySystem.HasCitizenship(from, City) && sys.GetLoyaltyRating(from) >= LoyaltyRating.Commended)
                {
                    base.OnDoubleClick(from);
                }
                else
                    from.SendLocalizedMessage(1152361, string.Format("#{0}", CityLoyaltySystem.GetCityLocalization(City))); // You are not sufficiently loyal to ~1_CITY~ to use this.
            }
        }

        public CityBannerDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write((int)City);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            City = (City)reader.ReadInt();
        }
    }
}
