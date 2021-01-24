using System;
using Nelderim.Towns;

namespace Server.Items
{
	[Flipable]
	public class MiastowaSzata : BaseOuterTorso
	{
    	public override void OnAdded( object parent )
		{
			base.OnAdded( parent );
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved( parent );
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public MiastowaSzata() : this(0)
		{
		}

		public MiastowaSzata( int hue) : base( 0x1F03, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;
		}

        public MiastowaSzata(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    [Flipable]
    public class MiastowaSzataTasandora : MiastowaSzata
    {
        public override int BaseColdResistance { get { return 1; } }
        public override int LabelNumber { get { return 1063965; } } // Szata miasta Bedwyrgard

        [Constructable]
        public MiastowaSzataTasandora()
            : base(1570)
        {
            Hue = 327;
        }

        public MiastowaSzataTasandora(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Tasandora))
            {
                return true;
            }
            else
	        {
                m.SendLocalizedMessage(1063970);
                return false;
	        }
        }
    }

    [Flipable]
    public class MiastowaSzataTwierdza : MiastowaSzata
    {
        public override int BaseEnergyResistance { get { return 1; } }
        public override int LabelNumber { get { return 1063966; } } // Szata miasta Malluan

        [Constructable]
        public MiastowaSzataTwierdza()
            : base(2942)
        {
            Hue = 1333;
        }

        public MiastowaSzataTwierdza(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Twierdza))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

    [Flipable]
    public class MiastowaSzataWioskaDrowow : MiastowaSzata
    {
        public override int BaseFireResistance { get { return 1; } }
        //public override int LabelNumber { get { return 1063967; } } // Szata miasta Nehkrumorgh

        [Constructable]
        public MiastowaSzataWioskaDrowow()
            : base(1908)
        {
            Name = "Szata wioski Drowow";
        }

        public MiastowaSzataWioskaDrowow(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Wioska_Drowow))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

    [Flipable]
    public class MiastowaSzataGarlan : MiastowaSzata
    {
        public override int BasePoisonResistance { get { return 1; } }
        public override int LabelNumber { get { return 1063968; } } // Szata miasta Magizhaar

        [Constructable]
        public MiastowaSzataGarlan()
            : base(2315)
        {
            Hue = 1253;
        }

        public MiastowaSzataGarlan(Serial serial)
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
        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Garlan))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

	[Flipable( 0x2684, 0x2683 )]
	public class MiastowaSzataZKapturem : BaseOuterTorso
	{
		public MiastowaSzataZKapturem( int hue ) : base( 0x2684, hue )
		{
			LootType = LootType.Blessed;
			Weight = 4.0;
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

        public MiastowaSzataZKapturem(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    [Flipable(0x2684, 0x2683)]
    public class MiastowaSzataZKapturemTasandora : MiastowaSzataZKapturem
    {
        public override int BaseColdResistance { get { return 2; } }
        public override int LabelNumber { get { return 1063965; } } // Szata miasta Bedwyrgard

        [Constructable]
        public MiastowaSzataZKapturemTasandora()
            : base(1570)
        {
            Hue = 327;
        }

        public MiastowaSzataZKapturemTasandora(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Tasandora))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

    [Flipable(0x2684, 0x2683)]
    public class MiastowaSzataZKapturemTwierdza : MiastowaSzataZKapturem
    {
        public override int BaseEnergyResistance { get { return 2; } }
        public override int LabelNumber { get { return 1063966; } } // Szata miasta Malluan

        [Constructable]
        public MiastowaSzataZKapturemTwierdza()
            : base(2942)
        {
            Hue = 1333;
        }

        public MiastowaSzataZKapturemTwierdza(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Twierdza))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

    [Flipable(0x2684, 0x2683)]
    public class MiastowaSzataZKapturemWioskaDrowow : MiastowaSzataZKapturem
    {
        public override int BaseFireResistance { get { return 2; } }
        public override int LabelNumber { get { return 1063967; } } // Szata miasta Nehkrumorgh

        [Constructable]
        public MiastowaSzataZKapturemWioskaDrowow()
            : base(1908)
        {
            Name = "Szata wioski Drowow";
        }

        public MiastowaSzataZKapturemWioskaDrowow(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Wioska_Drowow))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

    [Flipable(0x2684, 0x2683)]
    public class MiastowaSzataZKapturemGarlan : MiastowaSzataZKapturem
    {
        public override int BasePoisonResistance { get { return 2; } }
        public override int LabelNumber { get { return 1063968; } } // Szata miasta Magizhaar

        [Constructable]
        public MiastowaSzataZKapturemGarlan()
            : base(2315)
        {
            Hue = 1253;
        }

        public MiastowaSzataZKapturemGarlan(Serial serial)
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

        public override bool CanEquip(Mobile m)
        {
            if (TownDatabase.IsCitizenOfGivenTown(m, Towns.Garlan))
            {
                return true;
            }
            else
            {
                m.SendLocalizedMessage(1063970);
                return false;
            }
        }
    }

}