/// <summary>
///  LogoS :: 4.03.2005
/// </summary>
using System;
using Server;
using Server.Mobiles;

namespace Arya.Jail
{

	public class JailType
	{
		private bool m_Active;
		private Point3D[] m_JailCells;
		private Point3D m_JailExit;
		private Point3D m_JailEnter;
		private Rectangle3D[] m_JailRegion;
		private string m_Name;
		
		public JailType( bool active , string name , Point3D[] jailcells , Point3D jailexit , Point3D jailenter , Rectangle3D[] jailregion ) 
		{
			m_Active = active;
			m_JailCells = jailcells;
			m_JailExit = jailexit;
			m_JailEnter = jailenter;
			m_JailRegion = jailregion;
			m_Name = name;
		}
		
		public JailType()
		{
			m_Active = true;
			m_JailCells = new Point3D[]{};
			m_JailRegion = new Rectangle3D[]{};
		}
		
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; }
		}
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		public Point3D[] JailCells
		{
			get { return m_JailCells; }
			set { m_JailCells = value; }
		}
		public Point3D JailEnter
		{
			get { return m_JailEnter; }
			set { m_JailEnter = value; }
		}
		public Point3D JailExit
		{
			get { return m_JailExit; }
			set { m_JailExit = value; }
		}
		
		public Rectangle3D[] JailRegion
		{
			get { return m_JailRegion; }
			set { m_JailRegion = value; }
		}
		
		public bool IsInJail( Mobile m )
		{
			for( int i = 0; i < m_JailRegion.Length ; i++ )
			{
				Rectangle3D jailregion = ( Rectangle3D )m_JailRegion[i];

				if( jailregion.Contains( m.Location ) )
					return true;
				
			}
			return false;
		}	
		
		public JailType WhichJail( Mobile m )
		{
			for( int i = 0; i < m_JailRegion.Length ; i++ )
			{
				Rectangle3D jailregion = ( Rectangle3D )m_JailRegion[i];

				if( jailregion.Contains( m.Location ) )
					return this;
				
			}
			return null;
		}	
		
		 
	}
}
