namespace Server.Items
{
    public class SpecialDyeTub : DyeTub, Engines.VeteranRewards.IRewardItem
    {
        private bool m_IsRewardItem;
        [Constructable]
        public SpecialDyeTub()
        {
            LootType = LootType.Blessed;
            Name = "sepcjalna farba";
        }

        public SpecialDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override CustomHuePicker CustomHuePicker => CustomHuePicker.SpecialDyeTub;
        public override int LabelNumber => 1041285;// Special Dye Tub
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get
            {
                return m_IsRewardItem;
            }
            set
            {
                m_IsRewardItem = value;
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (m_IsRewardItem && !Engines.VeteranRewards.RewardSystem.CheckIsUsableBy(from, this, null))
                return;

            base.OnDoubleClick(from);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_IsRewardItem)
                list.Add(1076217); // 1st Year Veteran Reward
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_IsRewardItem = reader.ReadBool();
                        break;
                    }
            }
        }
    }
}
