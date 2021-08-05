using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class NPCElghin : BaseCreature
    {
        [Constructable]
        public NPCElghin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            InitStats(100, 125, 25);

            Body = 400;
            CantWalk = true;
            Hue = 1641;
            Blessed = true;
            Name = "NPCElghin";

            Skills[SkillName.Anatomy].Base = 120.0;
            Skills[SkillName.Tactics].Base = 120.0;
            Skills[SkillName.Magery].Base = 120.0;
            Skills[SkillName.MagicResist].Base = 120.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

        }

        public NPCElghin(Serial serial)
            : base(serial)
        {
        }

        public override void OnSpeech( SpeechEventArgs e )
{
base.OnSpeech( e );

Mobile from = e.Mobile;

if ( from.InRange( this, 2 ))
{
if (e.Speech.ToLower().IndexOf( "zadan" ) >= 0 || e.Speech.ToLower().IndexOf( "witaj" ) >= 0 ) {
 string message1 = "Hrrr... Czego tu szukasz? Nie powinno Cie tu byc... Lepiej zawroc... Chociaz, skoro tak bardzo chcesz zajrzec do krainy, ktorej nawet drowy sie obiawiaja... smialo. Przepuszcze Cie... ale najpierw przynies mi serca smoka, starozytnego ognistego i starozytnego lodowego smoka. Potrzebuje ich do mojego wywaru... Ruszaj wiec...";
                    this.Say(message1);
}
}
}








public int serce1;
public int serce2;
public int serce3;
public int serce4;
public override bool OnDragDrop( Mobile from, Item dropped )
		{

			if ( dropped is DragonsHeart )
			{
				dropped.Delete();
			
	
						
								Say( true, " Pierwsza pieczec zostala otwarta, do otwarcia nastepnej potrzebuje Serce Lodowego Smoka! " );

			serce1=1;
}
	




		if ( dropped is BlueDragonsHeart && serce1<1 )
			{
		
			
	

						
Say( true, "Glupcze najpierw potrzebuje serce zwyklego smoka!" );

			
}
	








		if ( dropped is BlueDragonsHeart && serce1>0 )
			{
				dropped.Delete();
					
								Say( true, " Druga pieczec zostala otwarta, do otwarcia przedostateniej potrzebuje serce Ognistego Smoka " );

			serce2=1;
}








	if ( dropped is RedDragonsHeart && serce1<1 )
			{
		
			
	

						
								Say( true, "Glupcze najpierw potrzebuje serce Starozytnego Lodowego Smoka!" );

			
}


	if ( dropped is RedDragonsHeart && serce2<1 && serce1>0 )
			{
		
			
	

						
								Say( true, "Glupcze, teraz potrzebuje serce Starozytnego Lodowego Smoka!" );

			
}
	

	



if ( dropped is RedDragonsHeart && serce2>0 )
			{
				dropped.Delete();

			serce3=1;
if ( serce1>0 && serce2>0 && serce3>0)
{
	Point3D loc = new Point3D(5914, 3227, 0); 
			Map map = Map.Felucca;
WrotaElghin portal = new WrotaElghin();
portal.MoveToWorld( loc, map );



						
								Say( true, " Ooo tak! Wspaniale... Prosze oto portal, ktory zaprowadzi cie w szpony smierci... hahaha, ruszaj smialo, tylko spiesz sie! Za chwile go zamkne! " );

serce1=0;
serce2=0;
serce3=0;

}
            }

            return base.OnDragDrop(from, dropped);
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