using System;

namespace Server.Items
{
    public class ArcaneTunic : LeatherChest
    {
        public override int InitMinHits => 60;
        public override int InitMaxHits => 60;

        private int originalDefenseChance;
        private DateTime nextUseTime; // Stores the next allowed use time
        private Timer resetTimer;

        [Constructable]
        public ArcaneTunic()
        {
            Name = "Tunika Arkanisty z Thila";
            Hue = 0x556;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 5;
            Attributes.SpellDamage = 4;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "Dotkniecie symbolu wyrytego na piersi tuniki powoduje zwiekszenie umiejetnosci unikania ciosow");
        }

        public override void OnDoubleClick(Mobile from)
        {
	        if (Parent != from)
	        {
		        from.SendLocalizedMessage(502641); // You must equip this item to use it.
		        return;
	        }

	        if (!from.BeginAction(typeof(ArcaneTunic)))
	        {
		        from.SendMessage("Musisz odczekac jeszcze troche czasu przed uzyciem tego ponownie.");
		        return;
	        }
	        BeginEffect(from);
        }

        private void BeginEffect(Mobile from)
        {
	        Attributes.DefendChance = 10;
	        Movable = false;
	        Timer.DelayCall(TimeSpan.FromMinutes(5), () => EndEffect(from));
	        from.SendMessage("Twoja umiejetnosc unikania ciosow wzrasta.");
        }

        private void EndEffect(Mobile from)
        {
	        Movable = true;
	        Attributes.DefendChance = 0;
	        from.EndAction(typeof(ArcaneTunic));
	        from.SendMessage("Efekt tuniki przestaje dzialac");
        }

        public ArcaneTunic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
	        Attributes.DefendChance = 0;
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
