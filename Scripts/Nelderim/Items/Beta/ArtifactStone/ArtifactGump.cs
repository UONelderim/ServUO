/////////////////////
//Crafted By Broze///
/////////////////////
using System; 
using System.Net; 
using Server; 
using Server.Accounting; 
using Server.Gumps; 
using Server.Items; 
using Server.Mobiles; 
using Server.Network; 

namespace Server.Gumps 
{ 
   public class ArtifactGump : Gump 
   { 
      private Mobile m_Mobile;
      private Item m_Deed;
 

      public ArtifactGump( Mobile from, Item deed ) : base( 30, 20 ) 
      { 
         m_Mobile = from;
	 m_Deed = deed; 
	
	 AddPage( 1 ); 

         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 ); 

         AddLabel( 40, 12, 0, "Artifact List" ); 

         Account a = from.Account as Account; 


         AddLabel( 52, 40, 0, "Weapons Menu" ); 
         AddButton( 12, 40, 4005, 4007, 0, GumpButtonType.Page, 2 ); 
         AddLabel( 52, 60, 0, "Armor Menu" ); 
         AddButton( 12, 60, 4005, 4007, 0, GumpButtonType.Page, 3 ); 
         AddLabel( 52, 80, 0, "Jewelery Menu" ); 
         AddButton( 12, 80, 4005, 4007, 10, GumpButtonType.Page, 4 ); 
         AddLabel( 52, 100, 0, "Shields Menu" ); 
         AddButton( 12, 100, 4005, 4007, 0, GumpButtonType.Page, 5 );
         AddLabel( 52, 120, 0, "Hats & Masks Menu" ); 
         AddButton( 12, 120, 4005, 4007, 0, GumpButtonType.Page, 6 );  
         AddLabel( 52, 340, 0, "Close" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
	

	 AddPage( 2 ); 

         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 ); 

	 AddLabel( 40, 12, 0, "Weapons List" );
        	
          

         AddLabel( 52, 40, 0, "Axe of the Heavens" ); 
         AddButton( 12, 40, 4005, 4007, 1, GumpButtonType.Reply, 1 ); 
         AddLabel( 52, 60, 0, "Blade of Insanity" ); 
         AddButton( 12, 60, 4005, 4007, 2, GumpButtonType.Reply, 2 ); 
         AddLabel( 52, 80, 0, "Blade of the Righteous" ); 
         AddButton( 12, 80, 4005, 4007, 3, GumpButtonType.Reply, 3 ); 
         AddLabel( 52, 100, 0, "Bone Crusher" ); 
         AddButton( 12, 100, 4005, 4007, 4, GumpButtonType.Reply, 4 ); 
         AddLabel( 52, 120, 0, "Breath of the Dead" ); 
         AddButton( 12, 120, 4005, 4007, 5, GumpButtonType.Reply, 5 ); 
         AddLabel( 52, 140, 0, "Frostbringer" ); 
         AddButton( 12, 140, 4005, 4007, 6, GumpButtonType.Reply, 6 ); 
         AddLabel( 52, 160, 0, "Legacy of the Dread Lord" ); 
         AddButton( 12, 160, 4005, 4007, 7, GumpButtonType.Reply, 7 ); 
         AddLabel( 52, 180, 0, "Serpent's Fang" ); 
         AddButton( 12, 180, 4005, 4007, 8, GumpButtonType.Reply, 8 ); 
         AddLabel( 52, 200, 0, "Staff of the Magi" ); 
         AddButton( 12, 200, 4005, 4007, 9, GumpButtonType.Reply, 9 ); 
         AddLabel( 52, 220, 0, "The Beserker's Maul" ); 
         AddButton( 12, 220, 4005, 4007, 10, GumpButtonType.Reply, 10 ); 
         AddLabel( 52, 240, 0, "The Dragon Slayer" ); 
         AddButton( 12, 240, 4005, 4007, 11, GumpButtonType.Reply, 11 );  
         AddLabel( 52, 260, 0, "Titans Hammer" ); 
         AddButton( 12, 260, 4005, 4007, 12, GumpButtonType.Reply, 12 );
         AddLabel( 52, 280, 0, "The Taskmaster" ); 
         AddButton( 12, 280, 4005, 4007, 13, GumpButtonType.Reply, 13 );
         AddLabel( 52, 300, 0, "Zyronic Claw" ); 
         AddButton( 12, 300, 4005, 4007, 14, GumpButtonType.Reply, 14 );


         AddLabel( 52, 340, 0, "Main Menu" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Page, 1 ); 
	

         AddPage( 3 ); 

         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 ); 

         AddLabel( 40, 12, 0, "Armor List" ); 

         
         AddLabel( 52, 40, 0, "Armor of Fortune" ); 
         AddButton( 12, 40, 4005, 4007, 16, GumpButtonType.Reply, 1 ); 
         AddLabel( 52, 60, 0, "Gauntlets of Nobility" ); 
         AddButton( 12, 60, 4005, 4007, 17, GumpButtonType.Reply, 2 ); 
         AddLabel( 52, 80, 0, "Helm of Insight" ); 
         AddButton( 12, 80, 4005, 4007, 18, GumpButtonType.Reply, 3 ); 
         AddLabel( 52, 100, 0, "Holy Knight's Breastplate" ); 
         AddButton( 12, 100, 4005, 4007, 19, GumpButtonType.Reply, 4 ); 
         AddLabel( 52, 120, 0, "Jackal's Collar" ); 
         AddButton( 12, 120, 4005, 4007, 20, GumpButtonType.Reply, 5 ); 
         AddLabel( 52, 140, 0, "Leggings of Bane" ); 
         AddButton( 12, 140, 4005, 4007, 21, GumpButtonType.Reply, 6 ); 
         AddLabel( 52, 160, 0, "Midnight Bracers" ); 
         AddButton( 12, 160, 4005, 4007, 22, GumpButtonType.Reply, 7 ); 
         AddLabel( 52, 180, 0, "Ornate Crown of the Harrower" ); 
         AddButton( 12, 180, 4005, 4007, 23, GumpButtonType.Reply, 8 ); 
         AddLabel( 52, 200, 0, "Shadow Dancer Leggings" ); 
         AddButton( 12, 200, 4005, 4007, 24, GumpButtonType.Reply, 9 ); 
         AddLabel( 52, 220, 0, "The Inquisitor's Resolution" ); 
         AddButton( 12, 220, 4005, 4007, 25, GumpButtonType.Reply, 10 ); 
         AddLabel( 52, 240, 0, "Tunic of Fire" ); 
         AddButton( 12, 240, 4005, 4007, 26, GumpButtonType.Reply, 11 ); 
         AddLabel( 52, 260, 0, "Voice of the Fallen King" ); 
         AddButton( 12, 260, 4005, 4007, 27, GumpButtonType.Reply, 12 ); 


         AddLabel( 52, 340, 0, "Main Menu" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Page, 1 );
	 

	 AddPage( 4 ); 

         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 );  

         AddLabel( 40, 12, 0, "Jewelery List" ); 

         

         AddLabel( 52, 40, 0, "Bracelet of Health" ); 
         AddButton( 12, 40, 4005, 4007, 29, GumpButtonType.Reply, 1 ); 
         AddLabel( 52, 60, 0, "Ornament of the Magician" ); 
         AddButton( 12, 60, 4005, 4007, 30, GumpButtonType.Reply, 2 ); 
         AddLabel( 52, 80, 0, "Ring of the Elements" ); 
         AddButton( 12, 80, 4005, 4007, 31, GumpButtonType.Reply, 3 ); 
         AddLabel( 52, 100, 0, "Ring of the Vile" );
	 AddButton( 12, 100, 4005, 4007, 32, GumpButtonType.Reply, 4 ); 

         AddLabel( 52, 340, 0, "Main Menu" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Page, 1 );
	  

	 AddPage( 5 ); 
  
         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 );

         AddLabel( 40, 12, 0, "Shields List" ); 

         

         AddLabel( 52, 40, 0, "Ægis" ); 
         AddButton( 12, 40, 4005, 4007, 34, GumpButtonType.Reply, 1 ); 
         AddLabel( 52, 60, 0, "Arcane Shield" ); 
         AddButton( 12, 60, 4005, 4007, 35, GumpButtonType.Reply, 2 );  

         AddLabel( 52, 340, 0, "Main Menu" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Page, 1 );


	 AddPage( 6 ); 

         AddBackground( 0, 0, 300, 400, 5054 ); 
         AddBackground( 8, 8, 284, 384, 3000 );  

         AddLabel( 40, 12, 0, "Hats & Masks List" ); 

         

         AddLabel( 52, 40, 0, "Divine Countenance" ); 
         AddButton( 12, 40, 4005, 4007, 36, GumpButtonType.Reply, 1 ); 
         AddLabel( 52, 60, 0, "Hat of the Magi" ); 
         AddButton( 12, 60, 4005, 4007, 37, GumpButtonType.Reply, 2 ); 
         AddLabel( 52, 80, 0, "Hunters Headdress" ); 
         AddButton( 12, 80, 4005, 4007, 38, GumpButtonType.Reply, 3 ); 
         AddLabel( 52, 100, 0, "Spirit of the Totem" );
	 AddButton( 12, 100, 4005, 4007, 39, GumpButtonType.Reply, 4 ); 

         AddLabel( 52, 340, 0, "Main Menu" ); 
         AddButton( 12, 340, 4005, 4007, 0, GumpButtonType.Page, 1 );


      } 


      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Close Gump 
            { 
               from.CloseGump( typeof( ArtifactGump ) );	 
               break; 
            } 
            case 1: // Axe of the Heavens 
            { 
		Item item = new AxeOfTheHeavens();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 2: // Blade of insanity 
            { 
		Item item = new BladeOfInsanity();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 3: //Blade of the Righteous
            { 
		Item item = new BladeOfTheRighteous();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 4: //Bone Crusher 
            { 
		Item item = new BoneCrusher();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 5: //Breath of the Dead 
            { 
		Item item = new BreathOfTheDead();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 6: //Frostbringer 
            { 
		Item item = new Frostbringer();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 7: //Legacy of the Dread Lord
            { 
		Item item = new LegacyOfTheDreadLord();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
            case 8: //Serpent's Fang 
            { 
		Item item = new SerpentsFang();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
	    case 9: //Staff of the Magi
	    { 
		Item item = new StaffOfTheMagi();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 10: //The Beserker's Maul 
            { 
		Item item = new TheBeserkersMaul();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 11: //The Dragon Slayer 
            { 
		Item item = new TheDragonSlayer();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 12: //Titans Hammer 
            { 
		Item item = new TitansHammer();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break;
            }
	    case 13: //The Taskmaster 
            { 
		Item item = new TheTaskmaster();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
	    case 14: //Zyronic Claw
            { 
		Item item = new ZyronicClaw();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break;
            
            } 
	    case 16: //Armor of Fortune 
            { 
		Item item = new ArmorOfFortune();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
	    case 17: //Gauntlets of Nobility 
            { 
		Item item = new GauntletsOfNobility();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
 	    case 18: //Helm of Insight 
            { 
		Item item = new HelmOfInsight();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 19: //Holy Knights Breastplate 
            { 
		Item item = new HolyKnightsBreastplate();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 20: //Jackal's Collar 
            { 
		Item item = new JackalsCollar();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 21: //Leggings of Bane 
            { 
		Item item = new LeggingsOfBane();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 22: //Midnight Bracers 
            { 
		Item item = new MidnightBracers();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 23: //Ornate Crown of the Harrower 
            { 
		Item item = new OrnateCrownOfTheHarrower();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 24: //Shadow Dancer Leggings 
            { 
		Item item = new ShadowDancerLeggings();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 25: //Inquisitor's Resolution 
            { 
		Item item = new InquisitorsResolution();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 26: //Tunic of Fire 
            { 
		Item item = new TunicOfFire();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 27: //Voice of the Fallen King 
            { 
		Item item = new VoiceOfTheFallenKing();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            
            } 
	    case 29: //Bracelet of Health 
            { 
		Item item = new BraceletOfHealth();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 30: //Ornament of the Magician 
            { 
		Item item = new OrnamentOfTheMagician();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 31: //Ring of the Elements 
            { 
		Item item = new RingOfTheElements();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
	    case 32: //Ring of the Vile 
            { 
		Item item = new RingOfTheVile();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break;
            }
	    case 34: //Aegis 
            { 
		Item item = new Aegis();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 35: //Arcane Shield 
            { 
		Item item = new ArcaneShield();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 36: //Divine Countenance 
            { 
		Item item = new DivineCountenance();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 37: //Hat of the Magi
            { 
		Item item = new HatOfTheMagi();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 38: //Hunters Headdress 
            { 
		Item item = new HuntersHeaddress();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            }
	    case 39: //Spirit of the Totem
            { 
		Item item = new SpiritOfTheTotem();
		item.LootType = LootType.Blessed;
		from.AddToBackpack( item ); 
		from.CloseGump( typeof( ArtifactGump ) );
		m_Deed.Delete(); 
		break; 
            } 
	         
         }    
      } 
   } 
} 
