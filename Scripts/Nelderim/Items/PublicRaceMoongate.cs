using System;
using System.Collections.Generic;
using Nelderim.Races;
using Server;
using Server.Commands;
using Server.Engines.CityLoyalty;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class PublicRaceMoongate : Item
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }

		[Constructable]
		public PublicRaceMoongate() : base( 0xF6C )
		{
			Movable = false;
			Light = LightType.Circle300;
		}

		public PublicRaceMoongate( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Player )
				return;

			if ( from.InRange( GetWorldLocation(), 1 ) )
				UseGate( from );
			else
				from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public override bool OnMoveOver( Mobile m )
		{
			// Changed so criminals are not blocked by it.
			if ( m.Player )
				UseGate( m );

			return true;
		}

		public override bool HandlesOnMovement{ get{ return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m is PlayerMobile )
			{
				if ( !Utility.InRange( m.Location, this.Location, 1 ) && Utility.InRange( oldLocation, this.Location, 1 ) )
					m.CloseGump( typeof( RaceMoongateGump ) );
			}
		}

		public bool UseGate( Mobile m )
		{
			if ( m.Criminal )
			{
				m.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}
			else if ( SpellHelper.CheckCombat( m ) )
			{
				m.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}
			else if ( m.Spell != null )
			{
				m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
				return false;
			}
			else
			{
				m.CloseGump( typeof( RaceMoongateGump ) );
				m.SendGump( new RaceMoongateGump( m, this ) );

				if ( !m.Hidden || m.AccessLevel == AccessLevel.Player )
					Effects.PlaySound( m.Location, m.Map, 0x20E );

				return true;
			}
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

		public static void Initialize()
		{
			CommandSystem.Register("MoonGenNelderim", AccessLevel.Administrator, new CommandEventHandler(MoonGenNelderim_OnCommand) );
		}

		[Usage("MoonGenNelderim")]
		[Description( "Generuje wszystkie publiczne gejty, usuwajac wpierw wszystkie istniejace" )]
		public static void MoonGenNelderim_OnCommand( CommandEventArgs e )
		{
			DeleteAll();

			int count = 0;

			count += MoonGen(PRMLocation.AllGates);

			World.Broadcast( 0x35, true, "Wygenerowano publicznych gejtow: {0}", count );
		}

		private static void DeleteAll()
		{
			List<Item> list = new List<Item>();

			foreach ( Item item in World.Items.Values )
			{
				if ( item is PublicRaceMoongate )
					list.Add( item );
			}

			foreach ( Item item in list )
				item.Delete();

			if ( list.Count > 0 )
				World.Broadcast( 0x35, true, "Usunieto publicznych gejtow: {0}", list.Count );
		}

		private static int MoonGen(PRMLocation[] list)
		{
			foreach ( PRMLocation entry in list )
			{
				Item item = new PublicRaceMoongate();
				item.Hue = entry.GateHue;

				item.MoveToWorld( entry.Location, entry.Map );
			}

			return list.Length;
		}
	}

	public class PRMLocation
	{
		private Map m_Map;
		private Point3D m_Location;
		private string m_Name;
		private int m_GateHue;

		public Map Map
		{
			get
			{
				return m_Map;
			}
		}

		public Point3D Location
		{
			get
			{
				return m_Location;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}

		public int GateHue => m_GateHue;

		public PRMLocation(Map map, Point3D loc, string name, int gateHue)
		{
			m_Map = map;
			m_Location = loc;
			m_Name = name;
			m_GateHue = gateHue;
		}

		public static readonly PRMLocation Garlan = new PRMLocation(Map.Felucca, new Point3D(851, 739, 20), "Garlan", 2571);
		public static readonly PRMLocation Orod = new PRMLocation(Map.Felucca, new Point3D(543, 1840, 0), "Orod", 2571);
		public static readonly PRMLocation Tasandora = new PRMLocation(Map.Felucca, new Point3D(1515, 2183, 0), "Tasandora", 2571);
		public static readonly PRMLocation Tirassa = new PRMLocation(Map.Felucca, new Point3D(2004, 2875, 0), "Tirassa", 2571);
		public static readonly PRMLocation Twierdza = new PRMLocation(Map.Felucca, new Point3D(2462, 1859, 0), "Twierdza", 2571);
		public static readonly PRMLocation Imloth = new PRMLocation(Map.Felucca, new Point3D(2696, 784, 0), "Imloth", 2571);
		public static readonly PRMLocation Lotharn = new PRMLocation(Map.Felucca, new Point3D(1966, 554, 0), "Lotharn", 2571);
		
		public static readonly PRMLocation[] TravelDestinationssNonElf = new PRMLocation[] { Garlan, Orod, Tasandora, Tirassa, Twierdza, Imloth, Lotharn };
		public static readonly PRMLocation[] TravelDestinationsElf = new PRMLocation[] { Garlan, Orod, Tasandora, Tirassa, Twierdza, Imloth, Lotharn };

		public static readonly PRMLocation[] AllGates = new PRMLocation[] { Garlan, Orod, Tasandora, Tirassa, Twierdza, Imloth, Lotharn }; // used by gate generation command
	}

	public class RaceMoongateGump : Gump
	{
		private Mobile m_Mobile;
		private Item m_Moongate;
		private PRMLocation[] m_Lists;

		public RaceMoongateGump( Mobile mobile, Item moongate ) : base( 100, 100 )
		{
			m_Mobile = mobile;
			m_Moongate = moongate;

			m_Lists = m_Mobile.Race is NElf ? PRMLocation.TravelDestinationsElf : PRMLocation.TravelDestinationssNonElf;

			AddPage( 0 );

			AddBackground( 0, 0, 165, 270, 5054 );

			AddHtmlLocalized(5, 5, 200, 20, 1012011, false, false); // Pick your destination:

			for (int i = 0; i < m_Lists.Length; ++i)
			{
				AddButton(10, 35 + (i * 25), 4005, 4007, 100 + i, GumpButtonType.Reply, 0); // Location 
				AddHtml(45, 35 + (i * 25), 150, 20, m_Lists[i].Name, false, false);
			}

			AddButton( 10, 235, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 236, 140, 25, 1011012, false, false ); // CANCEL
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 0 ) // Cancel
				return;
			else if (m_Mobile.Deleted || m_Moongate.Deleted || m_Mobile.Map == null)
				return;


			if (info.ButtonID < 100) // wrong data
				return;

			int index = info.ButtonID % 100;

			if (index < 0 || index >= m_Lists.Length)
				return;

			PRMLocation location = m_Lists[index];


			if (m_Mobile.Map == location.Map && m_Mobile.InRange(location.Location, 1))
			{
				m_Mobile.SendLocalizedMessage(1019003); // You are already there.
				return;
			}
			if (m_Mobile.IsStaff())
			{
				//Staff can always use a gate!
			}
			else if (!m_Mobile.InRange(m_Moongate.GetWorldLocation(), 1) || m_Mobile.Map != m_Moongate.Map)
			{
				m_Mobile.SendLocalizedMessage(1019002); // You are too far away to use the gate.
				return;
			}
			else if (Engines.VvV.VvVSigil.ExistsOn(m_Mobile) && location.Map != Engines.VvV.ViceVsVirtueSystem.Facet)
			{
				m_Mobile.SendLocalizedMessage(1019004); // You are not allowed to travel there.
				return;
			}
			else if (m_Mobile.Criminal)
			{
				m_Mobile.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
				return;
			}
			else if (SpellHelper.CheckCombat(m_Mobile))
			{
				m_Mobile.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
				return;
			}
			else if (m_Mobile.Spell != null)
			{
				m_Mobile.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
				return;
			}

			BaseCreature.TeleportPets(m_Mobile, location.Location, location.Map);

			m_Mobile.Combatant = null;
			m_Mobile.Warmode = false;
			m_Mobile.Hidden = true;

			m_Mobile.MoveToWorld(location.Location, location.Map);

			Effects.PlaySound(location.Location, location.Map, 0x1FE);

			CityTradeSystem.OnQuickTravelUsed(m_Mobile);
		}
	}
}
