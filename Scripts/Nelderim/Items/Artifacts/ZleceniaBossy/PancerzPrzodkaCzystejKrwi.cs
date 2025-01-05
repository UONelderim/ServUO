using System;

namespace Server.Items
{
    public class PancerzPrzodkaCzystejKrwi : DragonChest
    {
	    private static readonly TimeSpan _Cooldown = TimeSpan.FromMinutes(15);

	    private DateTime _LastUsage = DateTime.MinValue;

	    [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastUsage
        {
            get => _LastUsage;
            set { _LastUsage = value; InvalidateProperties(); }
        }

	    [CommandProperty(AccessLevel.GameMaster)]
        public bool OnCooldown => DateTime.UtcNow - _LastUsage < _Cooldown;


	    public override int BasePhysicalResistance => 10;
	    public override int BaseFireResistance => 15;
	    public override int BaseColdResistance => 5;
	    public override int BasePoisonResistance => 14;
	    public override int BaseEnergyResistance => 8;
	    public override CraftResource DefaultResource => CraftResource.Iron;


	    [Constructable]
        public PancerzPrzodkaCzystejKrwi()
        {
            Name = "Pancerz Przodka Czystej Krwi";
            Hue = 1109;

            Attributes.BonusHits = 10;
            Attributes.BonusStam = 6;
            Attributes.RegenHits = 5;
        }
	    public override void AddNameProperties(ObjectPropertyList list)
	    {
		    base.AddNameProperties(list);
			list.Add("Grawerowana runa emanuje moca oczyszczenia krwii");
	    }

        public override void OnDoubleClick(Mobile from)
        {
            if (Parent != from)
            {
                from.SendLocalizedMessage(502641); // You must equip this item to use it.
            }
            else if (OnCooldown)
            {
                from.SendMessage("Moc oczyszczenia jest wyczerpana.");
            }
            else if (from.Poison == null)
            {
                from.SendMessage("Nie jestes zatruty.");
            }
            else
            {
	            from.CurePoison(from);
	            from.SendMessage("Uzyles mocy oczyszczenia.");
	            if (from.Hidden == false)
	            {
		            from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
	            }
	            from.PlaySound(0x1E0);
	            LastUsage = DateTime.UtcNow;
            }
        }

        public PancerzPrzodkaCzystejKrwi(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
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
