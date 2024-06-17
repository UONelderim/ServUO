using Server.Engines.Craft;

namespace Server.Items
{
    [Flipable(0x13c6, 0x13ce)]
    public class LeatherGlovesOfCooking : BaseGlovesOfCooking
    {
        public override bool IsArtifact => true;
        [Constructable]
        public LeatherGlovesOfCooking(int bonus)
            : base(bonus, 0x13C6)
        {
            Weight = 1;
        }

        public LeatherGlovesOfCooking(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 2;
        public override int BaseFireResistance => 4;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 30;
        public override int InitMaxHits => 40;
        public override int StrReq => 20;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;
        public override int LabelNumber => 3060054;// leather blacksmith gloves of Cooking
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Flipable(0x13d5, 0x13dd)]
    public class StuddedGlovesOfCooking : BaseGlovesOfCooking
    {
        [Constructable]
        public StuddedGlovesOfCooking(int bonus)
            : base(bonus, 0x13D5)
        {
            Weight = 2;
        }

        public StuddedGlovesOfCooking(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 2;
        public override int BaseFireResistance => 4;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 4;
        public override int InitMinHits => 35;
        public override int InitMaxHits => 45;
        public override int StrReq => 25;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Studded;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override int LabelNumber => 3060056;// studded leather blacksmith gloves of Cooking
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Alterable(typeof(DefBlacksmithy), typeof(GargishKiltOfCooking))]
    [Flipable(0x13eb, 0x13f2)]
    public class RingmailGlovesOfCooking : BaseGlovesOfCooking
    {
        [Constructable]
        public RingmailGlovesOfCooking(int bonus)
            : base(bonus, 0x13EB)
        {
            Weight = 1;
        }

        public RingmailGlovesOfCooking(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 3;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 1;
        public override int BasePoisonResistance => 5;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 40;
        public override int InitMaxHits => 50;
        public override int StrReq => 40;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Ringmail;
        public override int LabelNumber => 3060059;// ringmail blacksmith gloves of Cooking
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Flipable(0x13eb, 0x13f2)]
    public class GargishKiltOfCooking : BaseGlovesOfCooking
    {
        [Constructable]
        public GargishKiltOfCooking() : this(5)
        {
        }

        [Constructable]
        public GargishKiltOfCooking(int bonus)
            : base(bonus, 0x030C)
        {
            Weight = 1;
        }

        public GargishKiltOfCooking(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 3;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 1;
        public override int BasePoisonResistance => 5;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 40;
        public override int InitMaxHits => 50;
        public override int StrReq => 40;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Ringmail;
        public override int LabelNumber => 1045124;// ringmail blacksmith gloves of Cooking
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public abstract class BaseGlovesOfCooking : BaseArmor
    {
        private int m_Bonus;
        private SkillMod m_SkillMod;
        public BaseGlovesOfCooking(int bonus, int itemID)
            : base(itemID)
        {
            m_Bonus = bonus;

            Hue = CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.DullCopper, (int)CraftResource.Valorite));
        }

        public BaseGlovesOfCooking(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bonus
        {
            get
            {
                return m_Bonus;
            }
            set
            {
                m_Bonus = value;
                InvalidateProperties();

                if (m_Bonus == 0)
                {
                    if (m_SkillMod != null)
                        m_SkillMod.Remove();

                    m_SkillMod = null;
                }
                else if (m_SkillMod == null && Parent is Mobile mp)
                {
                    m_SkillMod = new EquipedSkillMod(SkillName.Cooking, true, m_Bonus, this, mp);
                    mp.AddSkillMod(m_SkillMod);
                }
                else if (m_SkillMod != null)
                {
                    m_SkillMod.Value = m_Bonus;
                }
            }
        }
        public override void OnAdded(IEntity parent)
		{
            base.OnAdded(parent);

            if (m_Bonus != 0 && parent is Mobile mp)
            {
                if (m_SkillMod != null)
                    m_SkillMod.Remove();

                m_SkillMod = new EquipedSkillMod(SkillName.Cooking, true, m_Bonus, this, mp);
                mp.AddSkillMod(m_SkillMod);
            }
        }

        public override void OnRemoved(IEntity parent)
		{
            base.OnRemoved(parent);

            if (m_SkillMod != null)
                m_SkillMod.Remove();

            m_SkillMod = null;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Bonus != 0)
                list.Add(3060060, m_Bonus.ToString()); // Cooking bonus +~1_val~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Bonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Bonus = reader.ReadInt();
                        break;
                    }
            }

			Timer.DelayCall(() =>
			{
				if (m_Bonus != 0 && Parent is Mobile mp)
				{
					if (m_SkillMod != null)
						m_SkillMod.Remove();

					m_SkillMod = new EquipedSkillMod(SkillName.Cooking, true, m_Bonus, this, mp);
					mp.AddSkillMod(m_SkillMod);
				}
			});
        }
    }
}
