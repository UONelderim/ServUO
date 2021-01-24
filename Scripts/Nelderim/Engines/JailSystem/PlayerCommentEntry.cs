using System;
using System.Collections;
using System.Xml;
using System.IO;

using Server;
using Server.Accounting;
using Server.Targeting;
using Server.Regions;

namespace Arya.Jail
{
	/// <summary>
	///  Stores data about a quick jailing
	/// </summary>
	public class PlayerCommentEntry
	{
		
		private String m_Comment;
		private Mobile m_Player;
		private DateTime m_CreatedTime;
		
		
		public string Comment
		{
			get{ return m_Comment; }
			set{ m_Comment = value; }
		}
		
		public Mobile Player
		{
			get{ return m_Player; }
			set{ m_Player = value;  }
		}
		
		public DateTime CreatedTime
		{
			get{ return m_CreatedTime; }
			set{ m_CreatedTime = value ; }
		}	
		
		public PlayerCommentEntry( Mobile player , string comment , DateTime created )
		{
			m_Comment = comment;
			m_Player = player;
			m_CreatedTime =  created;
		}
		
		public PlayerCommentEntry( Mobile player , string comment )
		{
			m_Comment = comment;
			m_Player = player;
			m_CreatedTime = DateTime.Now;
		}
		
		public PlayerCommentEntry()
		{
		}
		
		
		public XmlNode GetXmlNode( XmlDocument dom )
		{
		
			XmlNode xNode = dom.CreateElement( "Comment" );
			
			XmlAttribute player = dom.CreateAttribute( "Player" );

			if ( m_Player != null )
			{
				player.Value = m_Player.Serial.ToString();
			}
			else
			{
				player.Value = null;
			}
			xNode.Attributes.Append( player );
			
			
			XmlAttribute createdtime = dom.CreateAttribute( "CreatedTime" );
			createdtime.Value = m_CreatedTime.ToString();
			xNode.Attributes.Append( createdtime );
			
			XmlAttribute comment = dom.CreateAttribute( "Comment" );
			comment.Value = m_Comment;
			xNode.Attributes.Append( comment );
			
			return xNode;
			
		}
		
		public static PlayerCommentEntry Load( XmlNode xNode )
		{
			PlayerCommentEntry comment = new PlayerCommentEntry();

			// Mobile
			int serial = -1;
			try
			{
				serial = Convert.ToInt32( xNode.Attributes[ "Player" ].Value, 16 );
			}
			catch {};
			if ( serial != -1 )
			{
				comment.m_Player = World.FindMobile( (Serial) serial );
			}

			// Jail Time
			DateTime createdtime = DateTime.MinValue;
			try
			{
				createdtime = DateTime.Parse( xNode.Attributes[ "CreatedTime" ].Value );
			}
			catch {}
			comment.CreatedTime = createdtime;


			// Reason
			comment.m_Comment = xNode.Attributes[ "Comment" ].Value;

			return comment;
		}
		
	}
}