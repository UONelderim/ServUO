using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class PanSmierc : BaseCreature
	{

        private static TimeSpan m_DDT = TimeSpan.FromSeconds( 15.0 ); // czas rozpadu w sekundach
        private static TimeSpan m_DD2 = TimeSpan.FromSeconds( 3.0 ); // czas rozpadu w sekundach
		[Constructable]
		public PanSmierc() : base( AIType.AI_Archer, FightMode.Aggressor, 12, 1, 0.2, 0.4 )
		{


			InitStats( 100, 125, 25 );
            Timer.DelayCall( m_DD2, new TimerCallback( lol ) ); 
			Timer.DelayCall( m_DDT, new TimerCallback( Delete) ); 

			Body = 0x190;

			Hue = 0x0;
			Blessed=true;
			Name="Smierc";
			Title="- Sluga Wiecznosci";

			HoodedShroudOfShadows chest = new HoodedShroudOfShadows();
			chest.Hue = 0x1;
			AddItem( chest );

		

			Scythe gloves = new Scythe();
			gloves.Hue = 0x482;
			AddItem( gloves );


			



			//PlateLegs legs = new PlateLegs();
			//legs.Hue = 0x966;
			//AddItem( legs );
			//PlateHelm helm = new PlateHelm();
			//helm.Hue = 0x966;
			//AddItem( helm );

	



		}

		public PanSmierc( Serial serial ) : base( serial )
		{

		}






public virtual void lol()
{
int s1 = Utility.Random( 100 );
int s2 = Utility.Random( 14 );
switch ( s2 )
{
case 1: Say( true, " A zyc mozesz tylko dzieki temu, za co moglbys umrzec. " );
break;
case 2: Say( true, " Cierpienie wymaga wiecej odwagi niz smierc. " );
break;
case 3: Say( true, " Bac sie smierci jest tym samym, co miec sie za madrego nim nie bedac. " );
break;
case 4: Say( true, " Dlaczego ludzie potrafia lepiej umierac niz zyc? Dlatego, ze zyc trzeba dlugo, a umrzec mozna predko. " );
break;
case 5: Say( true, " Ulubiency bogow umieraja mlodo, lecz pozniej zyja wiecznie w ich towarzystwie... " );
break;
case 6: Say( true, " Czy nauczyc sie umierac znaczy to samo co nauczyc sie przestac zyc? " );
break;
case 7: Say( true, " Nielatwo jest zyc po smierci. Czasem trzeba na to stracic cale zycie. " );
break;
case 8: Say( true, " Dopoki zyjemy, nie ma smierci: gdy jest smierc, nas juz nie ma. " );
break;
case 9: Say( true, "  Smierc, ktorej ludzie sie boja, to jest odlaczenie duszy od ciala, natomiast smierc, ktorej ludzie sie nie boja, a bac powinni, to jest odlaczenie od Pana " );
break;
case 10: Say( true, " Piekno, ktore pomaga zyc, pomaga rowniez umierac. " );
break;
case 11: Say( true, " Majestat smierci jest taki, ze ten, kto dobrowolnie umiera za cos, ma zawsze racje. " );
break;
case 12: Say( true, " Jak ci mam powiedziec, czym jest smierc, jesli jeszcze nie wiem, czym jest zycie? " );
break;
case 13: Say( true, " Pytasz, przyjacielu, co jest lepsze: byc dreczonym przez nieczyste sumienie czy tez zupelnie spokojnie lezec trupem. " );
break;
default: Say( true, " Tylko smierc moze wyrwac prawego czlowieka z rak Losu. " );
break;

}
}

//public override void OnSpeech( SpeechEventArgs e )
//{
//base.OnSpeech( e );
//
//Mobile from = e.Mobile;

//if ( from.InRange( this, 2 ))
//{
//if (e.Speech.ToLower().IndexOf( "zadan" ) >= 0 || e.Speech.ToLower().IndexOf( "praca" ) >= 0 ) {
//string message1 = "Strzege przejscia do tego, ktory jest najpotezniejszy. Jesli twa dusza jest gotowa na spotkanie z przeznaczniem przynies mi serca trzech ksiazat demonow!";
//this.Say( message1 );
//string message2 = "Wpierw przynies mi serce Sirchade Ksiecia Cierpienia!";
//this.Say( message2 );
//string message3 = "Nastepnie zas serce Aamona Pana Mordu.. . .. by dopelnic przeznaczenia jako ostatnie przynies mi serce Agaresa Ksiecia Ciemnosci";
//this.Say( message3 );
//}
//}
//}






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
}
