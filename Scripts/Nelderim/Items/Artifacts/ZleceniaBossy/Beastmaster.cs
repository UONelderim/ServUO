using System;

namespace Server.Items
{
    public class Beastmaster : GoldEarrings
    {
        private Timer _StaminaLossTimer;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        [Constructable]
        public Beastmaster()
        {
            Name = "Kolczyki Wladcy Bestii";
            Hue = 1153;

            Attributes.BonusStr = -20;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 5.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 5.0);
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
	        base.AddNameProperties(list);
	        list.Add("*grawer w języku krasnoludow rzecze, iz owe kolczyki wysysaja wytrzymalosc noszacego*");
        }
        
        public override bool OnEquip(Mobile from)
        {
	        if (!base.OnEquip(from)) return false;
	        
	        from.SendMessage("Zakładając te kolczyki czujesz jak krasnoludzkie runy powoduja spadek Twojej wytrzymałości.");
	        _StaminaLossTimer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => DoStaminaLoss(from));
	        return true;
        }

        private void DoStaminaLoss(Mobile from)
        {
	        if (from == null || from.Deleted || !from.Alive || from.FindItemOnLayer(Layer.Earrings) != this)
	        {
		        _StaminaLossTimer.Stop();
		        return;
	        }
	        
		    from.Stam--;
        }

        public Beastmaster(Serial serial) : base(serial)
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
