using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using System.Xml;

namespace Server.Regions
{
	public enum RaceRoomType
	{
		None,
        HumanRoom,
		ElfRoom,
		KrasnoludRoom,
		DrowRoom,
		TeleportRoom
	}
	
	public class RaceRoomRegion : BaseRegion
	{
		private RaceRoomType m_Room;

		public RaceRoomRegion( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
            try
            {
                int roomType = XmlConvert.ToInt32( xml.GetAttribute( "roomtype" ) );

                m_Room = (RaceRoomType)roomType;
            }
            catch ( Exception e )
            {
                Console.WriteLine( "RaceRoom roomType error: ", e.Message );
            }
		}

		public override bool OnCombatantChange( Mobile from, IDamageable Old, IDamageable New )
		{
			return ( from.AccessLevel > AccessLevel.Player );
		}
				
		public override void OnEnter( Mobile m )
		{
            if( m_Room != RaceRoomType.None && m_Room != RaceRoomType.TeleportRoom )
            {
                m.SendGump( new RaceRoomGump( m , m_Room ) );
            }
		}
	
		public override void OnExit( Mobile m )
      	{
		}
		
		//public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		//{
        //    global = 30;
		//}
	}
}
