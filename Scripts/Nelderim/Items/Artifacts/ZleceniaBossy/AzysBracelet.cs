namespace Server.Items
{
    public class AzysBracelet : BaseBracelet
    {
        public override int InitMinHits => 50;
        public override int InitMaxHits => 50;

        [Constructable]
        public AzysBracelet()
            : base(0x1F06)
        {
            Weight = 0.1;
            Name = "Branzoleta Rzemieslnika z Ferion";
            Hue = 0x78A;
            
            Attributes.Luck = -100;
            Attributes.BonusStr = 15;
            SkillBonuses.Skill_1_Name = SkillName.Mining;
            SkillBonuses.Skill_1_Value = 5;
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
	        if (SkillBonuses.Skill_1_Name == SkillName.Mining)
	        {
		        Weight = 5.0;
		        Attributes.BonusInt = 10;
		        Attributes.BonusStr = -30;
		        SkillBonuses.Skill_1_Name = SkillName.Inscribe;
	        }
            else
            {
	            Weight = 0.1;
	            Attributes.BonusInt = 0;
	            Attributes.BonusStr = 15;
	            SkillBonuses.Skill_1_Name = SkillName.Mining;
            }

            base.OnDoubleClick(from);
        }
    }
}
