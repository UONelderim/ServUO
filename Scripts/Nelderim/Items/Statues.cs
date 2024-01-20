namespace Server.Items
{
	public class GargulecStatua : Item
	{
		[Constructable]
		public GargulecStatua() : base(0x0499)
		{
			Weight = 10;
			Name = "statuetka gargulca";
		}

		public GargulecStatua(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

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
	
	public class Piedestal : Item
	{
		[Constructable]
		public Piedestal() : base(0x1223)
		{
			Name = "Granitowy piedestal";
			Weight = 100;
		}

//Nastepna
		public Piedestal(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class WojownikStatua : Item
	{
		[Constructable]
		public WojownikStatua() : base(0x05EE)
		{
			Weight = 10;
			Name = "statuetka wojownika";
		}

		public WojownikStatua(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

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

	public class KamiennaWaza : Item
	{
		[Constructable]
		public KamiennaWaza() : base(0x08AB)
		{
			Weight = 10;
			Name = "Kamienna Waza";
		}

		public KamiennaWaza(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

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
	
	// NOWE!!!




public class StatuetkaDemonaS : Item
	{
		[Constructable]
		public StatuetkaDemonaS() : base( 0x499 )
		{
		Name="Statuetka Demona";
			Weight = 30;
		}

		public StatuetkaDemonaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

public class StatuetkaDemonaE : Item
	{
		[Constructable]
		public StatuetkaDemonaE() : base( 0x49A )
		{
			Name="Statuetka Demona";
			Weight = 30;
		}

		public StatuetkaDemonaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaDuchaS : Item
	{
		[Constructable]
		public StatuetkaDuchaS() : base( 0x52D )
		{
			Name="Statuetka Ducha";
			Weight = 30;
		}

		public StatuetkaDuchaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna


public class StatuetkaDuchaE : Item
	{
		[Constructable]
		public StatuetkaDuchaE() : base( 0x52E )
		{
			Name="Statuetka Ducha";
			Weight = 30;
		}

		public StatuetkaDuchaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaOrdyjskaS : Item
	{
		[Constructable]
		public StatuetkaOrdyjskaS() : base( 0x5D5 )
		{
			Name="Statuetka Ordyjska";
			Weight = 30;
		}

		public StatuetkaOrdyjskaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaOrdyjskaE : Item
	{
		[Constructable]
		public StatuetkaOrdyjskaE() : base( 0x5ED )
		{
			Name="Statuetka Ordyjska";
			Weight = 30;
		}

		public StatuetkaOrdyjskaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaLahlithS : Item
	{
		[Constructable]
		public StatuetkaLahlithS() : base( 0x5EE )
		{
			Name="Statuetka Lahlith";
			Weight = 30;
		}

		public StatuetkaLahlithS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaLahlithE : Item
	{
		[Constructable]
		public StatuetkaLahlithE() : base( 0x63F )
		{
			Name="Statuetka Lahlith";
			Weight = 30;
		}

		public StatuetkaLahlithE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaLowcyS : Item
	{
		[Constructable]
		public StatuetkaLowcyS() : base( 0x640 )
		{
			Name="Statuetka Lowcy";
			Weight = 30;
		}

		public StatuetkaLowcyS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaLowcyE : Item
	{
		[Constructable]
		public StatuetkaLowcyE() : base( 0x641 )
		{
			Name="Statuetka Lowcy";
			Weight = 30;
		}

		public StatuetkaLowcyE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaElfaS : Item
	{
		[Constructable]
		public StatuetkaElfaS() : base( 0x642 )
		{
			Name="Statuetka Elfa";
			Weight = 30;
		}

		public StatuetkaElfaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaElfaE : Item
	{
		[Constructable]
		public StatuetkaElfaE() : base( 0x643 )
		{
			Name="Statuetka Elfa";
			Weight = 30;
		}

		public StatuetkaElfaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaGargulcaF : Item
	{
		[Constructable]
		public StatuetkaGargulcaF() : base( 0x644 )
		{
			Name="Statuetka Gargulca";
			Weight = 30;
		}

		public StatuetkaGargulcaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}




//Nastepna

public class StatuetkaSmokaS : Item
	{
		[Constructable]
		public StatuetkaSmokaS() : base( 0x645 )
		{
			Name="Statuetka Smoka";
			Weight = 30;
		}

		public StatuetkaSmokaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaSmokaE : Item
	{
		[Constructable]
		public StatuetkaSmokaE() : base( 0x646 )
		{
			Name="Statuetka Smoka";
			Weight = 30;
		}

		public StatuetkaSmokaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaGryfaS : Item
	{
		[Constructable]
		public StatuetkaGryfaS() : base( 0x647 )
		{
			Name="Statuetka Gryfa";
			Weight = 30;
		}

		public StatuetkaGryfaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaGryfaE : Item
	{
		[Constructable]
		public StatuetkaGryfaE() : base( 0x6F7 )
		{
			Name="Statuetka Gryfa";
			Weight = 30;
		}

		public StatuetkaGryfaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaKaplankiF : Item
	{
		[Constructable]
		public StatuetkaKaplankiF() : base( 0x6F8 )
		{
			Name="Statuetka Kaplanki";
			Weight = 30;
		}

		public StatuetkaKaplankiF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaJezdzcaS : Item
	{
		[Constructable]
		public StatuetkaJezdzcaS() : base( 0x6F9 )
		{
			Name="Statuetka Jezdzca";
			Weight = 30;
		}

		public StatuetkaJezdzcaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaJezdzcaE : Item
	{
		[Constructable]
		public StatuetkaJezdzcaE() : base( 0x6FA ) // 07.04.13 :: emfor
		{
			Name="Statuetka Jezdzca";
			Weight = 30;
		}

		public StatuetkaJezdzcaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaKrasnoludaS : Item
	{
		[Constructable]
		public StatuetkaKrasnoludaS() : base( 0x701 )
		{
			Name="Statuetka Krasnoluda";
			Weight = 30;
		}

		public StatuetkaKrasnoludaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaKrasnoludaE : Item
	{
		[Constructable]
		public StatuetkaKrasnoludaE() : base( 0x702 )
		{
			Name="Statuetka Krasnoluda";
			Weight = 30;
		}

		public StatuetkaKrasnoludaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

//Nastepna

public class StatuetkaBerserkeraS : Item
	{
		[Constructable]
		public StatuetkaBerserkeraS() : base( 0x703 )
		{
			Name="Statuetka Berserkera";
			Weight = 30;
		}

		public StatuetkaBerserkeraS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaBerserkeraE : Item
	{
		[Constructable]
		public StatuetkaBerserkeraE() : base( 0x704 )
		{
			Name="Statuetka Berserkera";
			Weight = 30;
		}

		public StatuetkaBerserkeraE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaLotheS : Item
	{
		[Constructable]
		public StatuetkaLotheS() : base( 0x705 )
		{
			Name="Statuetka Lothe";
			Weight = 30;
		}

		public StatuetkaLotheS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaLotheE : Item
	{
		[Constructable]
		public StatuetkaLotheE() : base( 0x706 )
		{
			Name="Statuetka Lothe";
			Weight = 30;
		}

		public StatuetkaLotheE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaPanaF : Item
	{
		[Constructable]
		public StatuetkaPanaF() : base( 0x707 )
		{
			Name="Statuetka Pana";
			Weight = 30;
		}

		public StatuetkaPanaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaStraznikaF : Item
	{
		[Constructable]
		public StatuetkaStraznikaF() : base( 0x708 )
		{
			Name="Statuetka Straznika";
			Weight = 30;
		}

		public StatuetkaStraznikaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaSmierciS : Item
	{
		[Constructable]
		public StatuetkaSmierciS() : base( 0x7D1 )
		{
			Name="Statuetka Smierci";
			Weight = 30;
		}

		public StatuetkaSmierciS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaSmierciE : Item
	{
		[Constructable]
		public StatuetkaSmierciE() : base( 0x7D2 )
		{
			Name="Statuetka Smierci";
			Weight = 30;
		}

		public StatuetkaSmierciE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaDuszka : Item
	{
		[Constructable]
		public StatuetkaDuszka() : base( 0xC1 )
		{
			Name="Statuetka Duszka";
			Weight = 40;
		}

		public StatuetkaDuszka( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaDuszkaF : Item
	{
		[Constructable]
		public StatuetkaDuszkaF() : base( 0xC2 )
		{
			Name="Statuetka Duszka";
			Weight = 40;
		}

		public StatuetkaDuszkaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaDuszkaaF : Item
	{
		[Constructable]
		public StatuetkaDuszkaaF() : base( 0xC3 )
		{
			Name="Statuetka Duszka";
			Weight = 40;
		}

		public StatuetkaDuszkaaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class StatuetkaDuszkafF : Item
	{
		[Constructable]
		public StatuetkaDuszkafF() : base( 0x1FB )
		{
			Name="Statuetka Duszka";
			Weight = 40;
		}

		public StatuetkaDuszkafF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class OltarzKoncaF : Item
	{
		[Constructable]
		public OltarzKoncaF() : base( 0x1FC )
		{
			Name="Oltarz Konca";
			Weight = 150;
		}

		public OltarzKoncaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class OltarzMatkiF : Item
	{
		[Constructable]
		public OltarzMatkiF() : base( 0x1FE )
		{
			Name="Oltarz Matki";
			Weight = 150;
		}

		public OltarzMatkiF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class OltarzPoczatkuF : Item
	{
		[Constructable]
		public OltarzPoczatkuF() : base( 0x1FD )
		{
			Name="Oltarz Poczatku";
			Weight = 150;
		}

		public OltarzPoczatkuF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikOrlaS : Item
	{
		[Constructable]
		public PomnikOrlaS() : base( 0x1446 )
		{
			Name="Pomnik Orla";
			Weight = 250;
		}

		public PomnikOrlaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikOrlaE : Item
	{
		[Constructable]
		public PomnikOrlaE() : base( 0x1447 )
		{
			Name="Pomnik Orla";
			Weight = 250;
		}

		public PomnikOrlaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikFeniksaS : Item
	{
		[Constructable]
		public PomnikFeniksaS() : base( 0x224 )
		{
			Name="Pomnik Feniksa";
			Weight = 300;
		}

		public PomnikFeniksaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikFeniksaE : Item
	{
		[Constructable]
		public PomnikFeniksaE() : base( 0x225 )
		{
			Name="Pomnik Feniksa";
			Weight = 300;
		}

		public PomnikFeniksaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMalaF : Item
	{
		[Constructable]
		public WazaMalaF() : base( 0x8AD )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMalaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMala1F : Item
	{
		[Constructable]
		public WazaMala1F() : base( 0x8FF )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMala1F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMala2F : Item
	{
		[Constructable]
		public WazaMala2F() : base( 0x902 )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMala2F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMala3F : Item
	{
		[Constructable]
		public WazaMala3F() : base( 0x905 )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMala3F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMala4F : Item
	{
		[Constructable]
		public WazaMala4F() : base( 0x908 )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMala4F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class WazaMala5F : Item
	{
		[Constructable]
		public WazaMala5F() : base( 0x90B )
		{
			Name="Mala Waza";
			Weight = 30;
		}

		public WazaMala5F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class Waza1F : Item
	{
		[Constructable]
		public Waza1F() : base( 0x8FE )
		{
			Name="Waza";
			Weight = 40;
		}

		public Waza1F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class Waza2F : Item
	{
		[Constructable]
		public Waza2F() : base( 0x901 )
		{
			Name="Waza";
			Weight = 40;
		}

		public Waza2F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class Waza3F : Item
	{
		[Constructable]
		public Waza3F() : base( 0x904 )
		{
			Name="Waza";
			Weight = 40;
		}

		public Waza3F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class Waza4F : Item
	{
		[Constructable]
		public Waza4F() : base( 0x907 )
		{
			Name="Waza";
			Weight = 40;
		}

		public Waza4F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class Waza5F : Item
	{
		[Constructable]
		public Waza5F() : base( 0x90A )
		{
			Name="Waza";
			Weight = 40;
		}

		public Waza5F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza1F : Item
	{
		[Constructable]
		public WazaDuza1F() : base( 0x8AB )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza1F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza2F : Item
	{
		[Constructable]
		public WazaDuza2F() : base( 0x8FD )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza2F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza3F : Item
	{
		[Constructable]
		public WazaDuza3F() : base( 0x900 )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza3F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza4F : Item
	{
		[Constructable]
		public WazaDuza4F() : base( 0x903 )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza4F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza5F : Item
	{
		[Constructable]
		public WazaDuza5F() : base( 0x906 )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza5F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaDuza6F : Item
	{
		[Constructable]
		public WazaDuza6F() : base( 0x909 )
		{
			Name="Duza Waza";
			Weight = 60;
		}

		public WazaDuza6F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna1F : Item
	{
		[Constructable]
		public WazaOgromna1F() : base( 0x287 )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna1F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna2F : Item
	{
		[Constructable]
		public WazaOgromna2F() : base( 0x288 )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna2F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna3F : Item
	{
		[Constructable]
		public WazaOgromna3F() : base( 0x289 )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna3F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna4F : Item
	{
		[Constructable]
		public WazaOgromna4F() : base( 0x28A )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna4F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna5F : Item
	{
		[Constructable]
		public WazaOgromna5F() : base( 0x28B )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna5F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class WazaOgromna6F : Item
	{
		[Constructable]
		public WazaOgromna6F() : base( 0x28C )
		{
			Name="Ogromna Waza";
			Weight = 100;
		}

		public WazaOgromna6F( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikDuchaS : Item
	{
		[Constructable]
		public PomnikDuchaS() : base( 0x7D3 )
		{
			Name="Pomnik Ducha";
			Weight = 300;
		}

		public PomnikDuchaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikDuchaE : Item
	{
		[Constructable]
		public PomnikDuchaE() : base( 0x7D4 )
		{
			Name="Pomnik Ducha";
			Weight = 300;
		}

		public PomnikDuchaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikOrdyjskaS : Item
	{
		[Constructable]
		public PomnikOrdyjskaS() : base( 0x7D5 )
		{
			Name="Pomnik Ordyjska";
			Weight = 300;
		}

		public PomnikOrdyjskaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikOrdyjskaE : Item
	{
		[Constructable]
		public PomnikOrdyjskaE() : base( 0x7DB )
		{
			Name="Pomnik Ordyjska";
			Weight = 300;
		}

		public PomnikOrdyjskaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikLahlithS : Item
	{
		[Constructable]
		public PomnikLahlithS() : base( 0x7DC )
		{
			Name="Pomnik Lahlith";
			Weight = 300;
		}

		public PomnikLahlithS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikLahlithE : Item
	{
		[Constructable]
		public PomnikLahlithE() : base( 0x7DD )
		{
			Name="Pomnik Lahlith";
			Weight = 300;
		}

		public PomnikLahlithE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikLowcyS : Item
	{
		[Constructable]
		public PomnikLowcyS() : base( 0x7DE )
		{
			Name="Pomnik Lowcy";
			Weight = 300;
		}

		public PomnikLowcyS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikLowcyE : Item
	{
		[Constructable]
		public PomnikLowcyE() : base( 0x819 )
		{
			Name="Pomnik Lowcy";
			Weight = 300;
		}

		public PomnikLowcyE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikElfaS : Item
	{
		[Constructable]
		public PomnikElfaS() : base( 0x81A )
		{
			Name="Pomnik Elfa";
			Weight = 300;
		}

		public PomnikElfaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikElfaE : Item
	{
		[Constructable]
		public PomnikElfaE() : base( 0x81B )
		{
			Name="Pomnik Elfa";
			Weight = 300;
		}

		public PomnikElfaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikGargulcaF : Item
	{
		[Constructable]
		public PomnikGargulcaF() : base( 0x81C )
		{
			Name="Pomnik Gargulca";
			Weight = 300;
		}

		public PomnikGargulcaF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikKaplankiF : Item
	{
		[Constructable]
		public PomnikKaplankiF() : base( 0x16DC )
		{
			Name="Pomnik Kaplanki";
			Weight = 300;
		}

		public PomnikKaplankiF( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikSmokaS : Item
	{
		[Constructable]
		public PomnikSmokaS() : base( 0x81E )
		{
			Name="Pomnik Smoka";
			Weight = 300;
		}

		public PomnikSmokaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikSmokaE : Item
	{
		[Constructable]
		public PomnikSmokaE() : base( 0x81F )
		{
			Name="Pomnik Smoka";
			Weight = 300;
		}

		public PomnikSmokaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikGryfaS : Item
	{
		[Constructable]
		public PomnikGryfaS() : base( 0x820 )
		{
			Name="Pomnik Gryfa";
			Weight = 300;
		}

		public PomnikGryfaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikGryfaE : Item
	{
		[Constructable]
		public PomnikGryfaE() : base( 0x879 )
		{
			Name="Pomnik Gryfa";
			Weight = 300;
		}

		public PomnikGryfaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikJezdzcaS : Item
	{
		[Constructable]
		public PomnikJezdzcaS() : base( 0x88D )
		{
			Name="Pomnik Jezdzca";
			Weight = 300;
		}

		public PomnikJezdzcaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikJezdzcaE : Item
	{
		[Constructable]
		public PomnikJezdzcaE() : base( 0x88E )
		{
			Name="Pomnik Jezdzca";
			Weight = 300;
		}

		public PomnikJezdzcaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikPanaKoncaS : Item
	{
		[Constructable]
		public PomnikPanaKoncaS() : base( 0x88F )
		{
			Name="Pomnik Pana Konca";
			Weight = 300;
		}

		public PomnikPanaKoncaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikPanaKoncaE : Item
	{
		[Constructable]
		public PomnikPanaKoncaE() : base( 0x890 )
		{
			Name="Pomnik Pana Konca";
			Weight = 300;
		}

		public PomnikPanaKoncaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikLotheS : Item
	{
		[Constructable]
		public PomnikLotheS() : base( 0x891 )
		{
			Name="Pomnik Loethe";
			Weight = 300;
		}

		public PomnikLotheS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikLotheE : Item
	{
		[Constructable]
		public PomnikLotheE() : base( 0x892 )
		{
			Name="Pomnik Loethe";
			Weight = 300;
		}

		public PomnikLotheE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikMagaS : Item
	{
		[Constructable]
		public PomnikMagaS() : base( 0x893 )
		{
			Name="Pomnik Maga";
			Weight = 300;
		}

		public PomnikMagaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikMagaE : Item
	{
		[Constructable]
		public PomnikMagaE() : base( 0x894 )
		{
			Name="Pomnik Maga";
			Weight = 300;
		}

		public PomnikMagaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikOrbaS : Item
	{
		[Constructable]
		public PomnikOrbaS() : base( 0x895 )
		{
			Name="Pomnik Orba";
			Weight = 300;
		}

		public PomnikOrbaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikOrbaE : Item
	{
		[Constructable]
		public PomnikOrbaE() : base( 0x896 )
		{
			Name="Pomnik Orba";
			Weight = 300;
		}

		public PomnikOrbaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikSfinksaS : Item
	{
		[Constructable]
		public PomnikSfinksaS() : base( 0x8A7 )
		{
			Name="Pomnik Sfinksa";
			Weight = 300;
		}

		public PomnikSfinksaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikSfinksaE : Item
	{
		[Constructable]
		public PomnikSfinksaE() : base( 0x8A8 )
		{
			Name="Pomnik Sfinksa";
			Weight = 300;
		}

		public PomnikSfinksaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}



//Nastepna

public class PomnikStraznikaS : Item
	{
		[Constructable]
		public PomnikStraznikaS() : base( 0x8A9 )
		{
			Name="Pomnik Straznika";
			Weight = 300;
		}

		public PomnikStraznikaS( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}


//Nastepna

public class PomnikStraznikaE : Item
	{
		[Constructable]
		public PomnikStraznikaE() : base( 0x8AA )
		{
			Name="Pomnik Straznika";
			Weight = 300;
		}

		public PomnikStraznikaE( Serial serial ) : base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class Nagrobek : Item
	{
		[Constructable]
		public Nagrobek() : base(0x0ED8)
		{
			Weight = 10;
			Name = "Nagrobek";
		}

		public Nagrobek(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

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
