//==============================================// 
// Created by Dupre               			// 
//==============================================// 

#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class SecureTent : BaseContainer
	{
		private Mobile m_Player;
		private SecureTent m_Chest;

		public override int DefaultGumpID { get { return 0x3C; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(16, 51, 168, 73); }
		}

		public SecureTent(Mobile player) : base(0xE80)
		{
			this.LiftOverride = true;
			m_Player = player;
			this.ItemID = 2482;
			this.Visible = true;
			this.Movable = false;
			MaxItems = 100;
		}

		//*******************************************************************************************
		/*
		     public void RemoveKeys( Mobile m )
			 {
				 uint keyValue = 0;
	 
				 if ( m_Chest != null )
					 keyValue = m_Chest.KeyValue;
	 
				 if ( keyValue == 0 && m_Chest != null )
					 keyValue = m_Chest.KeyValue;
	 
				 Key.RemoveKeys( m, keyValue );
			 }
	 
			 public uint CreateKeys( Mobile m )
			 {
				 uint value = Key.RandomValue();
	 
				 Key packKey = new Key( KeyType.Gold, value, this );
				 Key bankKey = new Key( KeyType.Gold, value, this );
	 
				 packKey.MaxRange = 10;
				 bankKey.MaxRange = 10;
	 
				 packKey.Name = "a tent key";
				 bankKey.Name = "a tent key";
	 
				 BankBox box = m.BankBox;
	 
				 if ( !box.TryDropItem( m, bankKey, false ) )
					 bankKey.Delete();
				 else
					 m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502484 ); // A ship's key is now in my safety deposit box.
	 
				 if ( m.AddToBackpack( packKey ) )
					 m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502485 ); // A ship's key is now in my backpack.
				 else
					 m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502483 ); // A ship's key is now at my feet.
	 
				 return value;
			 }
	   */
		//*******************************************************************************************

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Player
		{
			get
			{
				return m_Player;
			}
			set
			{
				m_Player = value;
				InvalidateProperties();
			}
		}

		public override int MaxWeight
		{
			get
			{
				return 400;
			}
		}

		public SecureTent(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version 

			writer.Write(m_Player);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			m_Player = (PlayerMobile)reader.ReadMobile();
		}

		public override TimeSpan DecayTime
		{
			get
			{
				return TimeSpan.FromMinutes(30.0);
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (m_Player != null)
				list.Add("A Secure Travel Bag");
			else
				base.AddNameProperty(list);
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_Player != null)
			{
				LabelTo(from, "A Secure Travel Bag");

				if (CheckContentDisplay(from))
					LabelTo(from, "({0} items, {1} stones)", TotalItems, TotalWeight);
			}
			else
			{
				base.OnSingleClick(from);
			}
		}

		public override bool IsAccessibleTo(Mobile m)
		{
			if ((m == m_Player || m.AccessLevel >= AccessLevel.GameMaster))
			{
				return true;
			}

			return false;

			return m == m_Player && base.IsAccessibleTo(m);
		}

		//*************************************************
		//public bool CheckKey( uint keyValue )
		//{
		//	if ( m_Chest != null && m_Chest.KeyValue == keyValue )
		//		return true;
		//
		//	return false;
		//}
		//*************************************************
	}
}
