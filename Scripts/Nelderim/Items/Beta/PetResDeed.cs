
using System; 
using System.Collections;
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Mobiles;
using Server.Targeting; 
using Server.Gumps;
using Server;

namespace Server.Items 
{ 
   public class ResPetTarget : Target 
   { 
      private PetResDeed mb_Deed; 

      public ResPetTarget( PetResDeed deed ) : base( 1, false, TargetFlags.None ) 
      { 
         mb_Deed = deed; 
      } 

      protected override void OnTarget( Mobile from, object target ) 
      { 
BaseCreature pet = target as BaseCreature;
        	if ( pet != null && pet.IsDeadPet )
         { 


		Mobile master = pet.ControlMaster;

           
                if ( master != null && master.InRange( pet, 3 ) )
							{
								
								master.CloseGump( typeof( PetResurrectGump ) );
								master.SendGump( new PetResurrectGump( master, pet ) );
								mb_Deed.Delete();
							}
            
         } 


               } 
   } 

   public class PetResDeed : Item // Create the item class which is derived from the base item class 
   { 
      [Constructable] 
      public PetResDeed() : base( 0x14F0 ) 
      { 
         Weight = 1.0; 
         Name = "zwoj wskrzeszenia zwierzecia"; 
         LootType = LootType.Blessed; 
	   Hue = 1153;
      } 

      public PetResDeed( Serial serial ) : base( serial ) 
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
         LootType = LootType.Blessed; 

         int version = reader.ReadInt(); 
      } 

      public override bool DisplayLootType{ get{ return false; } } 

      public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target 
      { 
         if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack 
         { 
             from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
         } 
         else 
         { 
            from.SendMessage( "Choose the pet you wish to bond with." );  
            from.Target = new ResPetTarget( this ); // Call our target 
          } 
      }    
   } 
}
