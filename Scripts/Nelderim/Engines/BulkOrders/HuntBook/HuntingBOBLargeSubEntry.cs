using System;

namespace Server.Engines.BulkOrders
{
	public class HuntingBOBLargeSubEntry
	{
		private Type m_ItemType;
		private int m_AmountCur;
		private int m_Number;
		private int m_Graphic;
		private int m_Level;

		public Type ItemType{ get{ return m_ItemType; } }
		public int AmountCur{ get{ return m_AmountCur; } }
		public int Number{ get{ return m_Number; } }
		public int Graphic{ get{ return m_Graphic; } }
		public int Level{ get{ return m_Level; } }

		public HuntingBOBLargeSubEntry( LargeBulkEntry lbe )
		{
			m_ItemType = lbe.Details.Type;
			m_AmountCur = lbe.Amount;
			m_Number = lbe.Details.Number;
			m_Graphic = lbe.Details.Graphic;
			m_Level = lbe.Details.Graphic;
		}

		public HuntingBOBLargeSubEntry( GenericReader reader )
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
					m_Number = reader.ReadEncodedInt();
					m_Graphic = reader.ReadEncodedInt();
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
			writer.WriteEncodedInt( (int) m_Number );
			writer.WriteEncodedInt( (int) m_Graphic );
			writer.WriteEncodedInt( (int) m_Level );
		}
	}
}