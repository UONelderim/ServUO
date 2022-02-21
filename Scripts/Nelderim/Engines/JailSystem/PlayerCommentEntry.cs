#region References

using System;
using System.Xml;
using Server;

#endregion

namespace Arya.Jail
{
	/// <summary>
	///     Stores data about a quick jailing
	/// </summary>
	public class PlayerCommentEntry
	{
		private DateTime m_CreatedTime;


		public string Comment { get; set; }

		public Mobile Player { get; set; }

		public DateTime CreatedTime
		{
			get { return m_CreatedTime; }
			set { m_CreatedTime = value; }
		}

		public PlayerCommentEntry(Mobile player, string comment, DateTime created)
		{
			Comment = comment;
			Player = player;
			m_CreatedTime = created;
		}

		public PlayerCommentEntry(Mobile player, string comment)
		{
			Comment = comment;
			Player = player;
			m_CreatedTime = DateTime.Now;
		}

		public PlayerCommentEntry()
		{
		}


		public XmlNode GetXmlNode(XmlDocument dom)
		{
			XmlNode xNode = dom.CreateElement("Comment");

			XmlAttribute player = dom.CreateAttribute("Player");

			if (Player != null)
			{
				player.Value = Player.Serial.ToString();
			}
			else
			{
				player.Value = null;
			}

			xNode.Attributes.Append(player);


			XmlAttribute createdtime = dom.CreateAttribute("CreatedTime");
			createdtime.Value = m_CreatedTime.ToString();
			xNode.Attributes.Append(createdtime);

			XmlAttribute comment = dom.CreateAttribute("Comment");
			comment.Value = Comment;
			xNode.Attributes.Append(comment);

			return xNode;
		}

		public static PlayerCommentEntry Load(XmlNode xNode)
		{
			PlayerCommentEntry comment = new PlayerCommentEntry();

			// Mobile
			int serial = -1;
			try
			{
				serial = Convert.ToInt32(xNode.Attributes["Player"].Value, 16);
			}
			catch { }

			;
			if (serial != -1)
			{
				comment.Player = World.FindMobile(new Serial(serial));
			}

			// Jail Time
			DateTime createdtime = DateTime.MinValue;
			try
			{
				createdtime = DateTime.Parse(xNode.Attributes["CreatedTime"].Value);
			}
			catch { }

			comment.CreatedTime = createdtime;


			// Reason
			comment.Comment = xNode.Attributes["Comment"].Value;

			return comment;
		}
	}
}
