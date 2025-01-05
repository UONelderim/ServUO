namespace Server.Items
{
    public class AzysBracelet : BaseBracelet
    {
        private bool isSecondClick = false; 

        public override int InitMinHits => 50;
        public override int InitMaxHits => 50;

        [Constructable]
        public AzysBracelet()
            : base(0x1F06)
        {
            this.Weight = 0.1;
            this.Name = "Branzoleta Rzemieslnika z Ferion";
            this.Hue = 0x78A;
            this.Attributes.Luck = -100;
            this.Attributes.BonusStr = 15;
            this.SkillBonuses.Skill_2_Name = SkillName.Mining;
            this.SkillBonuses.Skill_2_Value = 5;
            this.SkillBonuses.Skill_1_Name = SkillName.Inscribe;
            this.SkillBonuses.Skill_1_Value = 0;
            Label1 = "*branzoleta ma miejsce na 3 palce - czy odwazysz sie jej dotknac?*";
        }

        public AzysBracelet(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!isSecondClick)
            {
                this.Attributes.BonusInt = 10;
                this.Attributes.BonusStr = -30;
                this.Weight = 5.0;
                this.SkillBonuses.Skill_1_Name = SkillName.Inscribe;
                this.SkillBonuses.Skill_1_Value = 5;

                isSecondClick = true;
            }
            else
            {
                // On the second double-click, revert attributes and weight to original values
                this.Attributes.BonusInt = 0; // Reset BonusDex
                this.Attributes.BonusStr = 15; // Reset BonusInt to the original value
                this.Weight = 0.1; // Reset weight to the original value
                this.SkillBonuses.Skill_1_Name = SkillName.Inscribe;
                this.SkillBonuses.Skill_1_Value = 0;

                isSecondClick = false;
            }

            base.OnDoubleClick(from);
        }
    }
}
