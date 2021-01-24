//// 05.08.17 :: troyan :: naprawa bledu z niepotrzebnym komunikatme, ze jest sie z dala od slupka
//// 06.01.22 :: troyan :: zmiana zasad dzialania systemu
//// 06.02.02 :: troyan :: zasiegi menu kontekstowego
//// 06.04.29 :: troyan :: naprawa "zbondowanego przekazania" + kasacja uszkodzonych przy save
//// 07.04.07 :: emfor :: pet po zminiejszeniu nie zajmuje slot'a

//using System;
//using System.Collections;
//using Server.Items;
//using Server.Mobiles;
//using Server.Commands;
//using Server.ContextMenus;
//using Server.Gumps;
//using Server.Network;
//using System.Collections.Generic;
//using Server;
//using Server.Multis;
//using Server.Prompts;

//namespace Server.Items
//{
//    public class ShrinkItem : Item
//    {
//        private BaseCreature m_Creature;
//        private Mobile m_FirstOwner; // 07.04.07 :: emfor
//        private bool m_toDeletePet;
//        private DateTime m_StabledFrom;
//        private bool m_Stabled;

//        [CommandProperty( AccessLevel.Counselor )]
//        public bool Damaged { get { return ( m_Creature == null ) || m_Creature.Deleted; } }
//        [CommandProperty( AccessLevel.Counselor )]
//        public DateTime StabledAt { get { return m_StabledFrom; } }
//         // 07.04.07 :: emfor
//        [CommandProperty( AccessLevel.Counselor )]
//        public Mobile FirstOwner { get { return m_FirstOwner; }
//                               set { m_FirstOwner = value; } }
//        [CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
//        public bool IsFeeded 
//        { 
//            get { return m_Stabled; } 
//            set { m_Stabled = value; } 
//        }
		
//        public int Hours
//        {
//            get
//            {
//                if ( m_StabledFrom == DateTime.MaxValue )
//                    return -1;
				
//                TimeSpan span = DateTime.Now - m_StabledFrom;
						
//                return span > TimeSpan.FromHours( 24 ) ? 25 : span.Hours;
//            }
//        }
		
//        public bool Ageless
//        {
//            get
//            {
//                return Hours >= 0;
//            }
//            set 
//            {
//                if ( value )
//                {
//                    m_StabledFrom = DateTime.MaxValue;
//                    m_Stabled = false;
//                }
//                else
//                {
//                    m_StabledFrom = DateTime.Now;
//                }
//            }
//        }
		
//        public ShrinkItem( Serial serial ) : base( serial )
//        {
//        }

//        public ShrinkItem( BaseCreature bc )
//        {
//            m_toDeletePet = true;
//            m_Creature = bc;
			
//            if ( !( bc.Body==400 || bc.Body==401 ) )
//            {			
//                Hue = bc.Hue;
//            }
			
//            Name = bc.Name;
//            Weight = 1;
//            ItemID = ShrinkTable.Lookup( bc );
			
//            m_Creature.Loyalty--;
//            m_StabledFrom = DateTime.Now;
//        }

//        public override void OnDoubleClick( Mobile from )
//        {
//            if ( !Movable )
//                return;

//            if ( !IsChildOf( from.Backpack ) )
//            {
//                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
//            }
//            else if ( m_Creature == null || m_Creature.Deleted )
//            {
//                from.SendLocalizedMessage( 505659 ); // Wystapil blad krytyczny. Zwierze przepadlo.
//                Delete();
//                return;
//                //Basically just an "In case something happened"  Thing so Server don't crash.
//            }
//            else if ( !m_Creature.Tamable && from.AccessLevel < AccessLevel.GameMaster )
//            {
//                from.SendLocalizedMessage( 505660 ); // Powiekszyc zwierze mozesz tylko zwierze tamowalne.
//                return;
//            }
//            else if ( !m_Creature.CanBeControlledBy( from ) && from.AccessLevel < AccessLevel.GameMaster )
//            {
//                from.SendLocalizedMessage( 505661 ); // "Posiadasz za malo wiedzy o oswajaniu by przywolac tego zwierzaka."
//                return;
//            }
//            else if( !from.CheckAlive())
//            {
//                from.SendLocalizedMessage( 1060190 ); //You cannot do that while dead!
//            }
//            else if ( m_Creature.ControlMaster != from && from.Followers + m_Creature.ControlSlots > from.FollowersMax )
//            {
//                from.SendLocalizedMessage( 505662 ); // "Masz za duzo zwierzat pod swoja opieka by przywolac kolejne zwierze."
//                return;
//            }
//            else
//            {
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
				
//                if ( thereis && m_Creature.OnUnshrink( from ) )
//                {
//                    if ( m_Stabled && Hours <= 24 )
//                    {
//                        from.SendLocalizedMessage( 505681 ); // Nie masz dosc zlota, by oplacic chlopca pilnujacego istote.
//                        return;
//                    }
//                    /*
//                    else if ( ( m_Stabled && Hours > 24 ) || ( !m_Stabled && Hours > 1 ) )
//                    {
//                        from.SendLocalizedMessage( 505685 ); // Zbyt dlugo trzymales istote uwiazana u palika! Zwierze przepadlo!
//                        Delete();
//                        return;
//                    }
//                    */
//                    bool alreadyOwned = m_Creature.Owners.Contains( from );
				    	
//                    if ( !alreadyOwned )
//                        m_Creature.Owners.Add( from );
					
//                    m_Creature.SetControlMaster( from );
						
//                    if ( m_FirstOwner != null && m_FirstOwner != from )
//                    {
//                        m_Creature.IsBonded = false;
//                    }
					
//                    m_toDeletePet = false;
	
//                    m_Creature.Location = from.Location;
//                    m_Creature.Map=from.Map;
//                    m_Creature.ControlTarget = from;
//                    m_Creature.ControlOrder = OrderType.Follow;
					
//                    if ( m_Creature.Summoned )
//                        m_Creature.SummonMaster = from;
	
//                    if ( m_StabledFrom == DateTime.MaxValue )
//                        m_Creature.Loyalty--;
//                    else if ( !m_Stabled && Hours > 0 )
//                    {
//                        int hours = Hours;
						
//                        // Console.WriteLine( hours );
//                        // Console.WriteLine( m_Creature.Loyalty );
						
//                        /*while ( hours-- > 0 && m_Creature.Loyalty-- != Loyalty.None )
//                        {
//                            // Console.WriteLine( hours );
//                            // Console.WriteLine( m_Creature.Loyalty );
//                        }*/
//                    }
					
//                    if ( m_Stabled )
//                    {
//                        from.SendLocalizedMessage( 505689, ( 5 + Hours * 5 ).ToString() ); // Wloczega pobral od Ciebie ~1_GOLD~ centarow za pilnowanie zwierzecia.
//                        from.PlaySound( 0x32 );
//                    }	
					
//                    if ( from.AccessLevelMixed > AccessLevel.Player )
//                        m_Creature.Cheater_Name = from.Name;
					
//                    this.Delete();
//                }
//                else
//                    from.SendLocalizedMessage( 505664 ); // Powiekszyc zwierze mozesz tylko w poblizu palika.
//            }
//        }
		
//        public override void OnDelete()
//        {
//            try 
//            {
//                if ( m_toDeletePet )
//                {
//                    m_Creature.Delete();
//                }
//            } 
//            catch
//            {
//                Console.WriteLine( "[ShrinkItem Error]: {0}", CommandLogging.Format( this ) );
//            }

//            base.OnDelete();
//        }
		
//        #region Serialization
		
//        public override void Serialize( GenericWriter writer )
//        {
//            base.Serialize( writer );

//            writer.Write( (int) 2 ); // version
			
//            writer.Write( m_FirstOwner );

//            writer.Write( ( DateTime ) m_StabledFrom );
//            writer.Write( ( bool ) m_Stabled );
		
//            writer.Write( m_Creature );
//            writer.Write( m_toDeletePet );
//        }

//        public override void Deserialize( GenericReader reader )
//        {
//            base.Deserialize( reader );

//            int version = reader.ReadInt();

//            switch ( version )
//            {
//              case 2:
//              {
//          m_FirstOwner = reader.ReadMobile();
          
//          goto case 1;
//        }
//                case 1:
//                {
//                    m_StabledFrom = reader.ReadDateTime();
//                    m_Stabled = reader.ReadBool();

//                    goto case 0;
//                }
//                case 0:
//                {
//                    m_Creature = (BaseCreature)reader.ReadMobile();
//                    m_toDeletePet = reader.ReadBool();

//                    break;
//                }
//            }
			
//            if ( version < 1 )
//            {
//                m_StabledFrom = DateTime.MaxValue;
//                m_Stabled = false;
//            }
						
//            if ( m_Creature != null )
//            {
//                if ( ( m_StabledFrom != DateTime.MaxValue && DateTime.Now > m_StabledFrom + TimeSpan.FromDays( 28 ) ) || Damaged )
//                {
//                    Console.WriteLine( "[ShrinkItem Delete]: {0}", CommandLogging.Format( this ) );
//                    this.Delete();
//                }
//                else
//                {
//                    m_Creature.IsStabled = true;
//                }
//            }
//        }
		
//        #endregion
	
//        public override void OnSingleClick( Mobile from )
//        {
//            if ( Damaged )
//                base.LabelTo( from, 505675 ); // [ uszkodzony ]
//            else
//            {
//                if ( m_StabledFrom == DateTime.MaxValue )
//                    base.LabelTo( from, 505679 ); // figurka starego typu
//                else
//                {
//                    if ( Hours < 1 )
//                    {
//                        base.LabelTo( from, 505676 ); // [ uwiazany do palika ]
//                        base.LabelTo( from, m_Stabled ? 505690 : 505680 ); // istota dopiero co zostala uwiazana {i oddana pod opieke}
//                    }
//                    else if ( m_Stabled && Hours <= 24 )
//                    {
//                        base.LabelTo( from, 505676 ); // [ uwiazany do palika ]
//                        base.LabelTo( from, 505678, Hours.ToString() ); // istota pod opieka dni - ~1_COUNT~
//                    }
//                    /*
//                    else if ( Hours <= 24 )
//                    {
//                        base.LabelTo( from, 505676 ); // [ uwiazany do palika ]
//                        base.LabelTo( from, 505677, Hours.ToString() ); // istota jest uwiazana dni - ~1_COUNT~
//                    }
//                    */
//                    else
//                    {
//                        base.LabelTo( from, 505684 ); // [ istota porzucona ]
//                    }
//                }
//            }
//        }

//        public override void AddNameProperties( ObjectPropertyList list )
//        {
//            base.AddNameProperties( list );
			
//            if ( Damaged )
//                list.Add( 505675 ); // [ uszkodzony ]
//            else
//            {
//                if ( m_StabledFrom == DateTime.MaxValue )
//                    list.Add( 505679 ); // figurka starego typu
//                else
//                {
//                    if ( Hours < 1 )
//                    {
//                        list.Add( 505676 ); // [ uwiazany do palika ]
//                        list.Add( m_Stabled ? 505690 : 505680 ); // istota dopiero co zostala uwiazana
//                    }
//                    else if ( m_Stabled && Hours <= 24 )
//                    {
//                        list.Add( 505676 ); // [ uwiazany do palika ]
//                        list.Add( 505678, Hours.ToString() ); // istota pod opieka dni - ~1_COUNT~
//                    }
//                    /*
//                    else if ( Hours <= 24 )
//                    {
//                        list.Add( 505676 ); // [ uwiazany do palika ]
//                        list.Add( 505677, Hours.ToString() ); // istota jest uwiazana dni - ~1_COUNT~
//                    }
//                    */
//                    else
//                    {
//                        list.Add( 505684 ); // [ istota porzucona ]
//                    }
//                }
//            }
//        }			
	
//        public override void GetContextMenuEntries( Mobile from,List<ContextMenuEntry> list )
//        {
//            base.GetContextMenuEntries( from, list );

//            if ( IsChildOf( from.Backpack ) && m_StabledFrom != DateTime.MaxValue && !m_Stabled && Hours < 1 )
//                list.Add( new StableEntry( this, from ) );
			
//            list.Add( new HelpEntry( from ) );
//        }

//        private class StableEntry : ContextMenuEntry
//        {
//            private ShrinkItem m_Item;
//            private Mobile m_From;
				
//            public StableEntry( ShrinkItem item, Mobile from ) : base( 50020, 1 )
//            {
//                m_Item = item;
//                m_From = from;
//            }

//            public override void OnClick()
//            {
//                m_Item.IsFeeded = true;
//                m_From.SendLocalizedMessage( 505682 ); // Wluczega wezmie 5 centarow za kazda rozpoczeta godzine.
//                m_Item.InvalidateProperties();
//            }
//        }
	
//        private class HelpEntry : ContextMenuEntry
//        {
//            private Mobile m_From;
				
//            public HelpEntry( Mobile from ) : base( 50021 )
//            {
//                m_From = from;
//            }

//            public override void OnClick()
//            {
//                m_From.CloseGump( typeof( ShrinkItem.HelpEntry.InternalGump ) );
//                m_From.SendGump( new InternalGump( m_From ) );
//            }
			
//            public class InternalGump : Gump
//            {
//                private Mobile m_Mobile;
				
//                public InternalGump( Mobile mobile ) : base( 25, 50 )
//                {
//                    m_Mobile = mobile;
					
//                    AddPage( 0 );
	
//                    AddBackground( 25, 10, 420, 200, 5054 );
	
//                    AddImageTiled( 33, 20, 401, 181, 2624 );
//                    AddAlphaRegion( 33, 20, 401, 181 );
	
//                    AddHtmlLocalized( 40, 48, 387, 100, 505687, true, true ); /* <B>System Palikow Nelderim<B><BR><BR>
//                                                                               * Do konca stycznia 2006 roku na serwerze dzialal system palikow do pomniejszania zwierzat zaczerpniety z forum RunUO. Pozwalal on na pomniejszanie, <I>de facto</I> stable'owanie, dowolnej ilosci zwierzat za darmo. W oczywisty sposob stalo to w sprzecznosci z ograniczona liczba platnych miejsc w stajni i systemem bondowania.<BR>
//                                                                               * Z tego powodu z poczatkiem lutego na serwer wszedl zupelnie nowy, jedynie pozornie podobny, system palikow do pomnijszania zwierzat. Nizej opisane zostana jego szczegoly.<BR><BR>
//                                                                               * <B>Idea<B><BR><BR>
//                                                                               * W nowym systemie chcemy zachowac zalete palikow polegajaca na wygodzie szybkiego, doraznego i darmowego przechowania zwierzecia w miescie, np. gdy trzeba na chwile wejsc do banku. Z drugiej strony chcemy zlikwidowac glowna wade starego systemu - darmowy i nieograniczony stabling. Te cele zlozyly sie na ponizsze zaady...<BR><BR>
//                                                                               * <B>ZASADY:<B><BR><BR>
//                                                                               * * zmniejszeniu ulegla waga figurki do 1 kamienia;<BR>
//                                                                               * * ograniczeniu do 24 godzin ulegl czas trzymania zwierzecia w pomniejszeniu;<BR>
//                                                                               * * z kazda rozpoczeta godzina zmniejszenia zwierze traci jedno oczko lojalnosci;<BR>
//                                                                               * * jesli chce sie tego uniknac, nalezy z menu kontekstowego oplacic wloczege (5 centarow za kazda godzine pilnowania); oplata pobierana jest przy odbieraniu zwierzecia;<BR><BR>
//                                                                               * <B>Migracja<B><BR><BR>
//                                                                               * Do odwolania na serwerze moga rownolegle istniec figurki starego typu - oznaczone jako <I>[ figurka starego typu ]</I>. Te mozna na starych zasadach trzymac dowolnie dlugo (do odwolania).<BR><BR>
//                                                                               * <B>ZASTRZE¿ENIE<B><BR><BR>
//                                                                               * <I>Zastrzegamy sobie mozliwosc calkowitego wycofania sie z systemu figurek w imie zgodnosci mechaniki z OSI.</I>
//                                                                               * */
	
//                    AddButton( 190, 172, 4005, 4007, 0, GumpButtonType.Reply, 0 );
//                    AddHtmlLocalized( 230, 172, 120, 20, 505686, 0xFFFFFF, false, false ); // Zamknij
	
//                    AddHtmlLocalized( 40, 20, 380, 20, 505688, 0xFFFFFF, false, false ); // Informacje o systemie zmniejszania zwierzat
//                }
	
//                public override void OnResponse( NetState state, RelayInfo info )
//                {
//                }
//            }
	
//        }
//    }
//}
