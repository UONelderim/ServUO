using System;

namespace Server.Engines.BulkOrders
{
	public class HuntingBOBSmallEntry
	{
		private Type m_ItemType;
		private int m_AmountCur, m_AmountMax;
		private int m_Number;
		private int m_Graphic;
		private int m_Price;
		private int m_Level;

		public Type ItemType{ get{ return m_ItemType; } }
		public int AmountCur{ get{ return m_AmountCur; } }
		public int AmountMax{ get{ return m_AmountMax; } }
		public int Number{ get{ return m_Number; } }
		public int Graphic{ get{ return m_Graphic; } }
		public int Level{ get{ return m_Level; } }
		public int Price{ get{ return m_Price; } set{ m_Price = value; } }

		public Item Reconstruct()
		{
			SmallHunterBOD bod = null;

			bod = new SmallHunterBOD( m_AmountCur, m_AmountMax, m_ItemType, m_Number, m_Graphic, m_Level );

			return bod;
		}

		public HuntingBOBSmallEntry( SmallHunterBOD bod )
		{
			m_ItemType = bod.Type;
			m_AmountCur = bod.AmountCur;
			m_AmountMax = bod.AmountMax;
			m_Number = bod.Number;
			m_Graphic = bod.Graphic;
			m_Level = bod.Level;
		}

		public HuntingBOBSmallEntry( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
				{
					string type = reader.ReadString();

					if ( type != null )
						m_ItemType = ScriptCompiler.FindTypeByFullName( type );
					m_AmountCur = reader.ReadEncodedInt();
					m_AmountMax = reader.ReadEncodedInt();
					m_Number = reader.ReadEncodedInt();
					m_Graphic = reader.ReadEncodedInt();
					m_Price = reader.ReadEncodedInt();
					m_Level = reader.ReadEncodedInt();

					break;
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( 0 ); // version

			writer.Write( m_ItemType == null ? null : m_ItemType.FullName );
			writer.WriteEncodedInt( (int) m_AmountCur );
			writer.WriteEncodedInt( (int) m_AmountMax );
			writer.WriteEncodedInt( (int) m_Number );
			writer.WriteEncodedInt( (int) m_Graphic );
			writer.WriteEncodedInt( (int) m_Price );
			writer.WriteEncodedInt( (int) m_Level );
		}
	}
}