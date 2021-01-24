using System;
using System.Collections.Generic;
using Server.Mobiles;
using System.IO;
using System.Xml;

namespace Server.Nelderim
{
	public class RumorsSystem
	{
		private static List<RumorRecord> m_RumorsList;
		private static string SaveFolder = "Data/NelderimRegions";
		public static AccessLevel m_AccessLevel = AccessLevel.Administrator;
		
		public static List<RumorRecord> RumorsList
		{
			get { return m_RumorsList; }
		}
			
		static RumorsSystem()
		{
            m_RumorsList = new List<RumorRecord>();
		}

		public static void Initialize()
		{	
			EventSink.WorldSave += new WorldSaveEventHandler( OnWorldSave );
			EventSink.Shutdown += new ShutdownEventHandler( OnShutdown );
		}
			
		public static void Load()
		{
			Console.Write("Rumors System: Wczytywanie...");
			try
			{
				m_RumorsList.Clear();
	
				string RumorsList = Path.Combine( SaveFolder, "RumorsList.xml" );
	
				if ( File.Exists( RumorsList ) )
				{
					m_RumorsList.AddRange( LoadRumorsList( RumorsList ) );
				}
                Console.WriteLine( "Gotowe" );
				
				TownCrier.UpdateNews();
			}
			catch( Exception e )
			{
				Console.WriteLine("Error");
				Console.WriteLine( e.ToString() );
			}
		}
		
		private static List<RumorRecord> LoadRumorsList( string filename )
		{
			List<RumorRecord> rumors = new List<RumorRecord>();

			XmlDocument dom = new XmlDocument();

			if ( System.IO.File.Exists( filename ) )
			{
				try
				{
					dom.Load( filename );
				}
				catch{}

				if ( dom == null || dom.ChildNodes.Count < 2 )
				{
					return rumors;
				}

				XmlNode xItems = dom.ChildNodes[ 1 ];

				foreach( XmlNode xRumor in xItems.ChildNodes )
				{
					RumorRecord Rumor = null;

					try // If modified manually, some entries might be broken
					{
						Rumor = new RumorRecord( xRumor );
					}
					finally
					{
						if ( Rumor != null )
						{
							rumors.Add( Rumor );
						}
					}
				}
			}

			return rumors;
		}
		
		private static void OnWorldSave(WorldSaveEventArgs e)
		{
			Save();
			TownCrier.UpdateNews();
		}
		
		private static void OnShutdown( ShutdownEventArgs e )
		{
			Save();
		}
		
		private static void Save()
		{
			if ( !Directory.Exists( SaveFolder ) )
				Directory.CreateDirectory( SaveFolder );
			
			string RumorsList = Path.Combine( SaveFolder, "RumorsList.xml" );
			
			if( m_RumorsList.Count > 0 )
				SaveRumorsList( RumorsList, m_RumorsList );
		}
		
		private static void SaveRumorsList( string filename, List<RumorRecord> rumorslist )
		{
			try
			{
				XmlDocument dom = new XmlDocument();
	
				XmlNode xDeclaration = dom.CreateXmlDeclaration( "1.0", null, null );
				dom.AppendChild( xDeclaration );
	
				XmlNode xItems = dom.CreateElement( "RumorsList" );
	
				foreach( RumorRecord rumor in rumorslist )
				{
					XmlNode xRecord = rumor.GetXmlNode( dom );
					xItems.AppendChild( xRecord );
				}
	
				dom.AppendChild( xItems );
				dom.Save( filename );
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public static void AddRumor( RumorRecord rumor )
		{
			m_RumorsList.Add( rumor );
		}
		
		public static void DeleteRumor( RumorRecord rumor )
		{
			m_RumorsList.Remove( rumor );
		}
		
		public static List<RumorRecord> GetRumors( Mobile mobile, PriorityLevel priority )
		{
			return GetRumors( mobile, priority, NewsType.Rumor );
		}
		
		public static List<RumorRecord> GetRumors( Mobile mobile, NewsType type )
		{
			return GetRumors( mobile, PriorityLevel.None, type );
		}
		
		public static List<RumorRecord> GetRumors( Mobile mobile, PriorityLevel priority, NewsType type )
		{
			String regionname = mobile.Region.Name;
			RegionsEngineRegion reg = RegionsEngine.GetRegion( regionname );
			
			return ( type == NewsType.All ) ? GetRumors( reg, priority, type ) : Validate( GetRumors( reg, priority, type ) );
		}
		
		public static List<RumorRecord> GetRumors( RegionsEngineRegion reg, PriorityLevel priority, NewsType type )
		{
			try
			{
				List<RumorRecord> rumorslist;
				
				if ( reg.Name == "Default" )
					rumorslist = new List<RumorRecord>();
				else 
					rumorslist = GetRumors( RegionsEngine.GetRegion( reg.Parent ), priority, type );
				
				foreach ( RumorRecord r in m_RumorsList )
				{
					if( CheckRegions( r.ExcludedRegions, reg.Name ) )
					   rumorslist.Remove( r );
					else if( !rumorslist.Contains( r ) && CheckRegions( r.Regions, reg.Name ) 
					        && ( type == NewsType.All || ( r.Type == type && priority <= r.Priority ) ) )
					   rumorslist.Add( r );
				}
				
				rumorslist.Sort();
				
				return rumorslist;
			}
			catch( Exception e )
			{
				Console.WriteLine( e.ToString() );
				return new List<RumorRecord>();;
			}
		}
		
		public static List<RumorRecord> GetRumorsList( string regioname )
		{
			List<RumorRecord> list = new List<RumorRecord>();
			
			try
			{
				for( int i = 0; i < m_RumorsList.Count; i++ )
				{
					RumorRecord rumor = ( RumorRecord ) m_RumorsList[ i ];
					
					if( rumor.CheckRegions( regioname ) && !list.Contains( rumor ) )
						list.Add( rumor );
				}
				
				list.Sort();
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
			
			return list;
		}
	
		private static bool CheckRegions( List<string> regions, string regioname )
		{
			foreach( string regname in regions )
			{
				if( regname == regioname )
				{
					return true;
				}
			}
			return false;
		}
		
		private static List<RumorRecord> Validate( List<RumorRecord> rl )
		{
			for ( int i = rl.Count - 1; i >= 0; i-- )
			{
				RumorRecord r = rl[ i ] as RumorRecord;
					
				if ( r.Expired || !r.IsValid() )
					rl.Remove( r );
			}
			
			return rl;
		}
	}
}


