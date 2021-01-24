using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
public class Grab
{
#region Typy

    private static Type[] m_Arrows = new Type[]
    {
        typeof( Arrow ) , typeof( Bolt )
    };

    private static Type[] m_JewelStone = new Type[]
    {
        typeof( Amber ), typeof( Amethyst ) , typeof( Citrine ) ,
        typeof( Diamond ) , typeof( Emerald ) , typeof( Ruby ) , typeof( Sapphire ),
        typeof( StarSapphire ), typeof( Tourmaline )
    };

    private static Type[] m_Reagents = new Type[]
    {
        typeof( SpidersSilk ), typeof( SulfurousAsh ) , typeof( Nightshade ) ,
        typeof( MandrakeRoot ) , typeof( Ginseng ) , typeof( Garlic ) , typeof( Bloodmoss ),
        typeof( BlackPearl )
    };

    private static Type[] m_NecroReagents = new Type[]
    {
        typeof( BatWing ), typeof( GraveDust ), typeof( DaemonBlood ),
        typeof( NoxCrystal ), typeof( PigIron )
    };

    private static Type[] m_RareIngredients = new Type[]
    {
        typeof( Brimstone ), typeof( DaemonBone ), typeof( Bloodspawn ),
        typeof( DragonsBlood ), typeof( WyrmsHeart ), typeof( WyrmsHeart ), typeof( WyrmsHeart ),
        typeof( Blackmoor ), typeof( EyeOfNewt ), typeof( Pumice ), typeof( VolcanicAsh ),
        typeof( BlueDragonsHeart ), typeof( RedDragonsHeart ), typeof( DragonsHeart )
    };

    private static Type[] m_meat = new Type[]
    {
        typeof( RawBird ), typeof( RawChickenLeg ), typeof( RawFishSteak ),
        typeof( RawLambLeg ), typeof( RawRibs )
    };

    private static Type[] m_wool = new Type[]
    {
        typeof( Wool ), typeof( TaintedWool )
    };

    #endregion

    public static void Initialize()
    {
        CommandSystem.Register( "Grab", AccessLevel.Player, new CommandEventHandler( Grab_OnCommand ) );
        CommandSystem.Register("GrabPojemnik", AccessLevel.Player, new CommandEventHandler(GrabPojemnik_OnCommand));
    }

    [Usage("GrabPojemnik")]
    [Description("Pozwala wskazac pojemnik, do ktorego beda zbierane przedmioty po uzyciu komendy [grab")]
    private static void GrabPojemnik_OnCommand(CommandEventArgs e)
    {
        e.Mobile.SendMessage("Wskaz pojemnik, do ktorego beza zbierane przedmioty.");
        e.Mobile.Target = new GrabBagTarget();
    }

    public class GrabBagTarget : Target
    {
        public GrabBagTarget() : base(15, false, TargetFlags.None)
        {
        }

        protected override void OnTarget(Mobile from, object obj)
        {
            PlayerMobile pm = from as PlayerMobile;
            if (pm == null) // sanity
                return;

            if (obj == from || obj == from.Backpack)
            {
                // resetuj pojemnik (chcemy grabic do plecaka postaci)
                from.SendMessage("Ustawiono grabienie bezposrednio do plecaka postaci.");
                pm.GrabContainer = null;
                return;
            }

            Container bag = obj as Container;
            if (bag == null)
            {
                from.SendMessage("To nie jest pojemnik.");
                return;
            }
            if ( !bag.IsChildOf(from.Backpack) )
            {
                from.SendMessage("Pojemnik musi sie znajdowac w plecaku Twojej postaci.");
                return;
            }

            pm.GrabContainer = bag;
        }
    }

    [Usage( "Grab" )]
    [Description( "Grabi okolice skladujac przedmioty w plecaku. Dostepne opcje: gem, reg, necro, arrow, meat, wool, gold, bone, weapon, armor, jewel, scroll, feather, leather, spined, horned, barbed, leatherall, aids, cc, unsafe, gut, zoogie, egg" )]
    private static void Grab_OnCommand( CommandEventArgs e )
    {
        bool m_CheckCriminal = false;    
        bool m_Unsafe = false;

        try
        {
            ArrayList grabtype = new ArrayList();

            if( e.Length >= 1 )
            {
                string polecenie = e.ArgString;

                if (polecenie.IndexOf("gem") > -1 )
                    grabtype.AddRange( m_JewelStone );

                if (polecenie.IndexOf("reg") > -1 )
                    grabtype.AddRange( m_Reagents );

                if (polecenie.IndexOf("necro") > -1 )
                    grabtype.AddRange( m_NecroReagents );

                if (polecenie.IndexOf("arrow") > -1 )
                    grabtype.AddRange( m_Arrows );

                if (polecenie.IndexOf("meat") > -1 )
                    grabtype.AddRange( m_meat );

                if (polecenie.IndexOf("wool") > -1)
                    grabtype.AddRange(m_wool);

                if (polecenie.IndexOf("rare") > -1)
                    grabtype.AddRange(m_RareIngredients);

                if (polecenie.IndexOf("gold") > -1)
                    grabtype.Add(typeof( Gold ));

                if (polecenie.IndexOf("aids") > -1)
                    grabtype.Add(typeof(Bandage));

                if (polecenie.IndexOf("bone") > -1)
                {
                    grabtype.Add(typeof(Bone));
                    grabtype.Add(typeof(BonePile));
                }

                if (polecenie.IndexOf("weapon") > -1 )
                    grabtype.Add( typeof ( BaseWeapon ) );

                if (polecenie.IndexOf("armor") > -1 )
                    grabtype.Add( typeof ( BaseArmor ) );

                if (polecenie.IndexOf("jewel") > -1 )
                    grabtype.Add( typeof ( BaseJewel ) );

                if (polecenie.IndexOf("scroll") > -1 )
                    grabtype.Add( typeof ( SpellScroll ) );

                if (polecenie.IndexOf("feather") > -1 )
                    grabtype.Add(typeof(Feather));

				if (polecenie.IndexOf("leatherall") > -1 )
				{
                    grabtype.Add(typeof(BaseLeather));
					grabtype.Add(typeof(BaseHides));
				}
				else
				{
					if (polecenie.IndexOf("leather") > -1 )
					{
						grabtype.Add(typeof(Leather));
						grabtype.Add(typeof(Hides));
					}

					if (polecenie.IndexOf("spined") > -1 )
					{
						grabtype.Add(typeof(SpinedLeather));
						grabtype.Add(typeof(SpinedHides));
					}

					if (polecenie.IndexOf("horned") > -1 )
					{
						grabtype.Add(typeof(HornedLeather));
						grabtype.Add(typeof(HornedHides));
					}

					if (polecenie.IndexOf("barbed") > -1 )
					{
						grabtype.Add(typeof(BarbedLeather));
						grabtype.Add(typeof(BarbedHides));
					}
				}

				if (polecenie.IndexOf("gut") > -1 )
                    grabtype.Add(typeof(Gut));

				if (polecenie.IndexOf("zoogie") > -1 )
                    grabtype.Add(typeof(ZoogiFungus));

                if (polecenie.IndexOf("egg") > -1 )
                    grabtype.Add(typeof(Eggs));

                if( polecenie.IndexOf("cc") > -1 )
                    m_CheckCriminal = true;

                if( polecenie.IndexOf("unsafe") > -1 )
                    m_Unsafe = true;
            }

            if ( e.Mobile.Backpack == null )
                return;

            if ( !e.Mobile.CanBeginAction( "Grab" ) )
            {
                e.Mobile.SendLocalizedMessage( 505569 ); // Jestes juz w trakcie grabienia.
                return;
            }

            e.Mobile.RevealingAction();

            ArrayList items = new ArrayList();

            foreach ( Item item in e.Mobile.GetItemsInRange( 2 ) )
            {
                if ( item.Movable && item.Visible )
                {
                    if( grabtype.Count == 0 )
                        items.Add( item );
                    else if( CheckLoot( item, grabtype ) )
                        items.Add( item );
                }
                else if ( item is Corpse )
                {
                    Corpse corpse = item as Corpse;

                    if ( m_CheckCriminal && corpse.IsCriminalAction( e.Mobile ) )
                    {
                        if ( corpse.Owner == null || !corpse.Owner.Player )
                            e.Mobile.SendLocalizedMessage( 505571, corpse.Owner == null ? "" : corpse.Owner.Name ); // Spladrowanie ciala stworzenia ~1_NAME~bedzie przestepstwem! Pomijam.
                        else
                            e.Mobile.SendLocalizedMessage( 505570, corpse.Owner.Name ); // Spladrowanie zwlok gracz ~1_NAME~ bedzie przestepstwem! Pomijam.

                        continue;
                    }

                    foreach ( Item it in corpse.Items )
                    {
                        if ( it is Hair || it is Beard ) continue;

                        if ( it.Movable && item.Visible )
                        {
                            if( grabtype.Count == 0 )
                                items.Add( it );
                            else if( CheckLoot( it, grabtype ) )
                                items.Add( it );
                        }
                    }
                }
            }

            if ( items.Count == 0 )
            {
                e.Mobile.SendLocalizedMessage( 505575 );
            }
            else
            {        
                e.Mobile.BeginAction( "Grab" );
                GrabTimer gt = new GrabTimer( e.Mobile as PlayerMobile, items, m_Unsafe );
                e.Mobile.SendLocalizedMessage( 505572 );
                gt.Start();
            }
        }
        catch ( Exception exc )
        {
            Console.WriteLine( exc.ToString() );
            e.Mobile.SendLocalizedMessage( 505054 );
        }
    }

    private class GrabTimer : Timer
    {
        private ArrayList m_Items;
        private PlayerMobile m_From;
        private bool m_Unsafe;
        private Point3D m_location;

        public GrabTimer( PlayerMobile from, ArrayList items, bool us ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 1 ))
        {
            m_Items = items;
            m_From = from;
            m_Unsafe = us;
            m_location = m_From.Location;
        }

        protected override void OnTick()
        {
            if ( m_From == null || m_From.NetState == null )
            {
                Stop();
				m_From.EndAction( "Grab" );
                return;
            }

            if ( !m_From.Alive )
            {
                m_From.SendLocalizedMessage( 505576 ); // Grabienie zostalo przerwane!
                Stop();
				m_From.EndAction( "Grab" );
                return;
            }

            if ( m_From.Holding != null  )
            {
                if ( !m_Unsafe )
                {
                    m_From.SendLocalizedMessage( 505889 ); // Juz cos przenosisz!
                    m_From.SendLocalizedMessage( 505573 ); // Zakonczono grabienie.
                    Stop();
                    m_From.EndAction( "Grab" );
                    return;
                }
                else
                {
                    // Console.WriteLine( ( string ) CommandLogging.Format( m_From.Holding ) );
                    m_From.Holding = null;
                }
            }

            if ( m_From.CanBeginAction( "Grab" ) )
            {
                if ( m_Unsafe )
                    m_From.BeginAction( "Grab" );
                else
                {
                    m_From.SendLocalizedMessage( 505576 ); // Grabienie zostalo przerwane!
                    Stop();
					m_From.EndAction( "Grab" );
                    return;
                }
            }

            if ( m_Items.Count == 0 )
            {
                m_From.SendLocalizedMessage( 505573 ); // Zakonczono grabienie.
                Stop();
                m_From.EndAction( "Grab" );
                return;
            }

            if (m_From.GetDistanceToSqrt(m_location) > 0)
            {
                m_From.SendLocalizedMessage(1061407); // Poruszyles sie w trakcie grabienia...
                m_From.SendLocalizedMessage(505576); // Grabienie zostalo przerwane!
                Stop();
                m_From.EndAction("Grab");
                return;
            }

            Item item = m_Items[0] as Item;
            m_Items.RemoveAt( 0 );

            // Console.WriteLine( ( string ) CommandLogging.Format( item ) );

            Container bag = null;
            if (m_From.GrabContainer != null )
            {
                if (!m_From.GrabContainer.Deleted && (m_From.GrabContainer.IsChildOf(m_From.Backpack) || m_From.GrabContainer == m_From.Backpack))
                {
                    bag = m_From.GrabContainer;
                }
                else
                {
                    m_From.SendMessage("W plecaku twojej postaci nie ma pojemnika wybranego do przechowywania grabionych przedmiotow. Uzyj komendy [GrabPojemnik aby wskazac nowy pojemnik.");
                    Stop();
                    m_From.EndAction("Grab");
                    return;
                }
            }

            if (bag != null)
            {
                if (!bag.CheckHold(m_From, item, true, true))
                {
                    Stop();
                    m_From.EndAction("Grab");
                    return;
                }
            }

            if (!m_From.Backpack.CheckHold(m_From, item, true, true))
            {
                Stop();
                m_From.EndAction("Grab");
                return;
            }            

            bool rejected;
            LRReason reject;

            m_From.Lift( item, item.Amount, out rejected, out reject );

            if ( !rejected )
            {
                m_From.Emote( 505574 );
                m_From.Drop( m_From, Point3D.Zero );

                if (bag != null)
                {
                    m_From.Backpack.RemoveItem(item);
                    if (!bag.TryDropItem(m_From, item, false))
                    {
                        Console.WriteLine("ERROR: grab: bag.TryDropItem");
                    }
                }
            }
            else if ( !m_Unsafe )
            {
                m_From.EndAction("Grab");
                m_From.SendLocalizedMessage( 505576 ); // Grabienie zostalo przerwane!
                Stop();
                return;
            }

            m_From.BeginAction( "Grab" );
        }
    }

    private static bool CheckLoot( Item item, ArrayList types )
    {
        Type itemType = item.GetType();

        foreach( Type type in types )
        {
            if( type == itemType || itemType.IsSubclassOf( type ) )
                return true;
        }

        return false;
    }
}
}