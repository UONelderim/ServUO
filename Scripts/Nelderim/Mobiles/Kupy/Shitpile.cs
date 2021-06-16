using System; 
using System.Collections; 
using Server.Network; 
using Server.Items; 
using Server.Targeting;
using Server.Mobiles; 

namespace Server.Items 
{ 

   public class Shitpile : Item 
   { 


      [Constructable] 
      public Shitpile() : base( 0x913 ) 
      { 
      	Hue = Utility.RandomList( 0x0, 1161, 0x44 );
        	Name = "Smierdzaca Kupa";
		m_NextAbilityTime = DateTime.Now;
	} 

      public Shitpile( Serial serial ) : base( serial ) 
      { 
      } 

	private Shoes m_Shoes;
      private DateTime m_NextAbilityTime;
      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); // version 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
      	base.Deserialize( reader ); 
      	int version = reader.ReadInt(); 
      	m_NextAbilityTime = DateTime.Now;
      } 

	public override bool OnMoveOver( Mobile from )
	{
		if(from is PlayerMobile && from.FindItemOnLayer(Layer.Shoes) !=null){
		Item feet = from.FindItemOnLayer( Layer.Shoes );
		feet.Hue = 1161;
		feet.Name = "Oblepione lajnem buty";
		Effects.PlaySound( from.Location, from.Map, 1064);
		from.SendMessage( "Wlazles w Kupe! Znajdz czyscidlo do butow!" );}
		return true;
	}
       
      public override void OnSingleClick( Mobile from ) 
      { 
      	this.LabelTo( from, 1005578 ); 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
      	if (!IsChildOf(from.Backpack)) 
         { 
            from.SendMessage("Musisz miec w plecaku aby uzyc"); //Musisz miec to w plecaku by tego urzyc. 
            return; 
         } 
      	else 
         { 
            
            if ( DateTime.Now >= m_NextAbilityTime )
		{
               from.Target = new ShitTarget( from, this ); 
               from.SendMessage( "Ostroznie pakujesz kupe w kule..." );   // Ostroznie pakujesz kupe w kule... 
               Delete();
            } 
            else 
            { 
               from.SendLocalizedMessage( 1005574 ); 
            } 

         } 
	    
      } 
       
      private class ShitTarget : Target 
      { 
         private Timer timer;
         private Mobile mobile;
         private Mobile m_Thrower; 
		private Shitpile m_Shit;

         public ShitTarget( Mobile thrower, Shitpile shit ) : base ( 10, false, TargetFlags.None ) 
         { 
            m_Thrower = thrower; 
		m_Shit = shit;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendLocalizedMessage( 1005576 ); 
             
            else if( target is Mobile) 
            { 
               Mobile m = (Mobile)target; 
               from.PlaySound( 0x145 ); 
               from.Animate( 9, 1, 1, true, false, 0 ); 
               from.SendMessage( "Rzucasz smierdzaca kupa!" );   // Rzucasz smierdzaca kupa! 
               from.Criminal = true;
               
               if( target is Mobile && m.AccessLevel >= AccessLevel.GameMaster )
               {
               from.PublicOverheadMessage(MessageType.Regular, 1161, true, "*Nie trafiles w cel*");
               return;
               }
               
               m.SendMessage( "Wlasnie zostales trafiony smierdzaca kupa!"  ); // Wlasnie zostales trafiony smierdzaca kupa! 
               timer = new ShitTimer(m);
               timer.Start();
               Effects.SendMovingEffect( from, m, 0x36E4, 7, 0, false, true, 0x44, 0 ); 
               m_Shit.m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( 10.0 ) ;
        } 

            else 
            { 
               from.SendLocalizedMessage( 1005577 ); 
            } 
         } 
          
      } 
      
      public class ShitTimer : Timer
      
      {
      
      private Mobile m_Mobile;
		private DateTime m_ATime;
      public ShitTimer(Mobile mobile) : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)){
		m_Mobile = mobile;
		m_ATime = DateTime.Now;
		
      
      }//end ctor
      
      protected override void OnTick(){
      try{
			m_Mobile.PublicOverheadMessage(MessageType.Regular, 1161, true, "*smierdzi niemilosiernie*");
      }catch{} 
      
      if (DateTime.Now > m_ATime + TimeSpan.FromMinutes(1.0))
		this.Stop();
      }
      }//end: ShitTimer
   } 
    

       
       
} 



