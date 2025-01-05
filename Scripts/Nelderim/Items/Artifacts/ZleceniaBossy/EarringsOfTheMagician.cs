using System;

namespace Server.Items
{
    public class EarringsOfTheMagician : GoldEarrings
    {
	    private Timer _StaminaLossTimer;

        public override int InitMinHits => 50;
        public override int InitMaxHits => 50;

        [Constructable]
        public EarringsOfTheMagician()
        {
            Name = "Kolczyki Krasnoludzkiego Maga";
            Hue = 0x554;
            Attributes.CastRecovery = 1;
            Attributes.LowerRegCost = 10;
            Attributes.Luck = -200;
            Resistances.Energy = 5;
            Resistances.Fire = 5;
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

        public EarringsOfTheMagician(Serial serial) : base(serial)
        {
        }

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
}
