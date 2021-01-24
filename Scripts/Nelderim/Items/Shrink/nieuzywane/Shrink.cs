//// 05.08.17 :: troyan :: naprawa bledu ze zdalnym zmniejszaniem (po teleportacji od slupka)
//// 05.09.02 :: troyan :: zmiana zasad dzialania systemu
//// 06.01.19 :: troyan :: przebudowa + lokalizacja + logowanie
//// 06.04.17 :: troyan :: naprawa "zbondowanego przekazania"
//// 06.11.21 :: emfor :: figurka zwierzaka jest blessed
//// 07.04.07 :: emfor :: pet po zminiejszeniu nie zajmuje slot'a

//using System;
//using Server;
//using Server.Items;
//using Server.Mobiles;
//using Server.Commands;
//using System.Linq;

//namespace Server
//{
//    public class ShrinkFunctions 
//    {
//        public static bool Shrink( Mobile from, object targ, bool restricted )
//        {
//            var Test = new int[] { 1, 2, 3, }.Select( i => 3 * i ).ToArray();
//            foreach ( var t in Test ) 
//            from.SendMessage( t.ToString() ); 

//            int error = 505673;
			
//            if ( from == targ )
//                error = 505665; // Nie mozesz uwiazac sie u palika!
//            else if ( !( targ is BaseCreature ) )
//                error = 505666; // Nie mozesz tego pomniejszyc!
//            else
//            {
//                BaseCreature bc = ( BaseCreature ) targ;
				
//                IPooledEnumerable eable = from.GetItemsInRange( 2 );
//                bool thereis = false;  
				
//                foreach( Item item in eable )
//                {
//                    if ( item is HitchingPost )
//                    {
//                        thereis = true;
//                        break;
//                    }
//                }
				
//                #region Shrink Restrictions
				
//                if ( !restricted || from.AccessLevel > AccessLevel.Counselor )
//                {
//                    //Don't check anything if not a restricted Shrink
//                }
//                else if ( !thereis )
//                {
//                    error = 505667; // "Zmniejszyc zwierze mozesz tylko w poblizu palika.";
//                }
//                else if ( !bc.Tamable )
//                {
//                    error = 505668; // "Zmniejszyc zwierze mozesz tylko zwierze tamowalne.";
//                }
//                else if ( !bc.CanBeControlledBy( from ) )
//                {
//                    error = 505669; // "Potrzebujesz wiekszej wiedzy o oswajaniu, by zmniejszyc zwierze..";
//                }
//                else if( bc.Summoned )
//                { 
//                    error = 505670; // "Nie mozesz pomniejszac przyzwanych zwierzat i potworow.";
//                }
//                else if ( bc.Combatant != null && bc.InRange( bc.Combatant, 12 ) && bc.Map == bc.Combatant.Map )
//                {
//                    error = 505671; // "Nie mozesz tego pomniejszyc podczas walki.";
//                }
//                else if (!(bc.Controlled && bc.ControlMaster==from))
//                {
//                    error = 1042562;	//That is not your pet!
//                }
//                else if ( ( bc is PackLlama || bc is PackHorse || bc is Beetle ) && (bc.Backpack != null && bc.Backpack.Items.Count > 0) )
//                {
//                    error = 1042563; //Unload the pet first	
//                }
//                else if (bc.Hits < bc.HitsMax )
//                {
//                    error = 505672; // "Nie mozesz zmniejszyc rannego stworzenia.";
//                }
				
//                #endregion

//                ShrinkItem shrunkenPet = new ShrinkItem( bc );
//                shrunkenPet.Name = "Uwiazane zwierze";     // 06.11.21 :: emfor
//                shrunkenPet.LootType = LootType.Blessed;   // 06.11.21 :: emfor
//                shrunkenPet.FirstOwner = from;  // 07.04.07 :: emfor

				
//                if ( !restricted )
//                    shrunkenPet.Ageless = true;

//                if ( from.AccessLevelMixed > AccessLevel.Player && Config.ValidateLabeling( shrunkenPet ) )
//                {
//                    shrunkenPet.Label1 = String.Format( "Ten przedmiot zostal utworzony przez GMa {0}", from.Name );
//                }
				
//                shrunkenPet.LabelOfCreator = ( string ) CommandLogging.Format( from );
				
//                if ( error == 505673 )
//                {
//                    if ( from != null )
//                    {
//                        from.SendLocalizedMessage( error );
						
//                        if ( !from.AddToBackpack ( shrunkenPet ) )
//                        {
//                            shrunkenPet.MoveToWorld( new Point3D( from.X, from.Y, from.Z ), from.Map );
//                            from.SendLocalizedMessage( 505674 ); // Twoj plecak jest pelny. Figurka lezy u Twych stop.
//                        }
//                    }
//                    else
//                    {
//                        shrunkenPet.MoveToWorld( new Point3D( bc.X, bc.Y, bc.Z ), bc.Map );  // place shrunken pet at current location
//                    }

//                    bc.Controlled = true;	//To make it so It won't still be a part of a spawner. 
					
//                    SendAway( bc );

//                    return true;
//                }
//            }

//            from.SendLocalizedMessage( error );
			
//            return false;
//        }

//        public static bool Shrink( Mobile from, object targ )
//        {	
//            return Shrink( from, targ, true );
//        }

//        public static bool Shrink( object targ )
//        {
//            return Shrink( null, targ, false );
//        }

//        private static void SendAway( BaseCreature pet )
//        {
//            pet.ControlTarget = null;
//            pet.ControlOrder = OrderType.Stay;
//            pet.Internalize();

//            pet.SetControlMaster( null );
//            pet.SummonMaster = null;

//            pet.IsStabled = true;
//        }
//    }
//}
