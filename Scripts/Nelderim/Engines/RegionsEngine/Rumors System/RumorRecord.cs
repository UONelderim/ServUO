using System;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;

namespace Server.Nelderim
{
	public class RumorRecord : IComparable
	{
		#region Fields
		
		private string m_Title;
		private string m_Coppice;
		private string m_Text;
		private string m_KeyWord;
		private PriorityLevel m_Priority;
		private List<string> m_Regions;
		private List<string> m_ExcludedRegions;
		private DateTime m_EndRumor;
		private DateTime m_StartRumor;
		private Mobile m_RumorMobile;
		private NewsType m_Type;
		
		#endregion
		#region Properties
		
		public String Title { get { return m_Title; } set { m_Title = value; } }
		
		public String Coppice { get { return m_Coppice; } set { m_Coppice = value; } }

		public String Text { get { return m_Text; } set { m_Text = value; } }
		
		public String KeyWord { get { return m_KeyWord; } set { m_KeyWord = value; } }
		
		public List<string> Regions{ get { return m_Regions; } }
		
		public List<string> ExcludedRegions{ get { return m_ExcludedRegions; } }
		
		public PriorityLevel Priority { get { return m_Priority; } set { m_Priority = value; } }
		
		public DateTime EndRumor { get { return m_EndRumor; } set { m_EndRumor = value; } }
		
		public DateTime StartRumor { get { return m_StartRumor; } set { m_StartRumor = value; } }
		
		public Mobile RumorMobile { get { return m_RumorMobile; } set { m_RumorMobile = value; } }
		
		public NewsType Type { get { return m_Type; } set { m_Type = value; } }
		
		public bool Expired { get { return m_EndRumor < DateTime.Now || m_StartRumor > DateTime.Now; } }
		
		#endregion
		#region ctor
		
		public RumorRecord( string title , string coppice , string text , string keyword , PriorityLevel priority , DateTime start , DateTime end , Mobile rumormobile, NewsType type )
		{
			
			m_Title = title;
			m_Coppice = coppice;
			m_Text = text;
			m_KeyWord = keyword;
			m_Priority = priority;
			m_StartRumor = start;
			m_EndRumor = end;
			m_RumorMobile = rumormobile;
			m_Type = type;
			m_Regions = new List<string>();
			m_ExcludedRegions = new List<string>();
	    }
		
		public RumorRecord( RumorRecord record )
		{
			m_Title = record.Title + " (kopia)";
			m_Coppice = record.Coppice;
			m_Text = record.Text;
			m_KeyWord = record.KeyWord;
			m_Priority = record.Priority;
			m_StartRumor = record.StartRumor;
			m_EndRumor = record.EndRumor;
			m_RumorMobile = record.RumorMobile;
			m_Type = record.Type;
			m_Regions = new List<string>();
			m_ExcludedRegions = new List<string>();
	    }
		
		public RumorRecord( XmlNode xNode )
	    {
			m_Regions = new List<string>();
			m_ExcludedRegions = new List<string>();
			
			#region Title
			
			try
			{
				m_Title = xNode.Attributes[ "Title" ].Value;
				if ( m_Title == null ) m_Title = "";
			}
			catch
			{
				m_Title = "";
			}
			
			#endregion
			#region Coppice
			
			try
			{
				m_Coppice = xNode.Attributes[ "Coppice" ].Value;
				if ( m_Coppice == null ) m_Coppice = "";
			}
			catch
			{
				m_Coppice = "";
			}
			
			#endregion
			#region KeyWord
			
			try
			{
				m_KeyWord = xNode.Attributes[ "KeyWord" ].Value;
				if ( m_KeyWord == null ) m_KeyWord = "";
			}
			catch
			{
				m_KeyWord = "";
			}
			
			#endregion
			#region Text
			
			try
			{
				m_Text = xNode.Attributes[ "Text" ].Value;
				if ( m_Text == null ) m_Text = "";
			}
			catch
			{
				m_Text = "";
			}
			
			#endregion
			#region Rumor Mobile
			
			try
			{
				int serial = -1;
				
				serial = Convert.ToInt32( xNode.Attributes[ "RumorMobile" ].Value, 16 );
				
				if ( serial != -1 )
					m_RumorMobile = World.FindMobile( new Serial(serial) );
				else
					m_RumorMobile = null;
			}
			catch
			{
				m_RumorMobile = null;
			}
			
			#endregion
			#region Regions
			
			try
			{
				if ( xNode.ChildNodes.Count > 0 )
				{
					XmlNode xRegionsName = xNode.ChildNodes[ 0 ];
	
					foreach( XmlNode xCom in xRegionsName.ChildNodes )
					{
						AddRegion( xCom.Attributes[ "Name" ].Value );
					}
				}
			}
			catch
			{
			}
			
			#endregion
			#region ExcludedRegions
			
			try
			{
				if ( xNode.ChildNodes.Count > 0 )
				{
					XmlNode xRegionsName = xNode.ChildNodes[ 1 ];
	
					foreach( XmlNode xCom in xRegionsName.ChildNodes )
					{
						AddRegionExclude( xCom.Attributes[ "Name" ].Value );
					}
				}
			}
			catch
			{
			}
			
			#endregion
			#region Priority
			
			try
			{
				m_Priority = ( PriorityLevel )( int.Parse( xNode.Attributes[ "Priority" ].Value ) );
			}
			catch
			{
				m_Priority = PriorityLevel.None;
			}
			
			#endregion
			#region Type
			
			try
			{
				m_Type = ( NewsType )( int.Parse( xNode.Attributes[ "Type" ].Value ) );
			}
			catch
			{
				m_Type = NewsType.Rumor;
			}
			
			#endregion
			#region End
			
			try
			{
				m_EndRumor = DateTime.ParseExact( xNode.Attributes[ "EndRumor" ].Value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture );
			}
			catch
			{
				m_EndRumor = DateTime.MinValue;
			}
			
			#endregion
			#region Start
			
			try
			{
				m_StartRumor = DateTime.ParseExact( xNode.Attributes[ "StartRumor" ].Value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture );
			}
			catch
			{
				m_StartRumor = DateTime.MinValue;
			}
			
			#endregion
		}
	    
		public RumorRecord()
		{
			m_Regions = new List<string>();
			m_ExcludedRegions = new List<string>();
			m_Priority = PriorityLevel.VeryLow;
			m_Type = NewsType.Rumor;
	    }
		
		#endregion
		#region Methods
		
	    public double DoublePriority( RumorType type )
	    {
			double chance = 0.00;
			
			if( type == RumorType.BuyandSell )
			{
				switch( m_Priority )
				{
					case PriorityLevel.Low: chance = 0.01; break;
					case PriorityLevel.Medium: chance = 0.025; break;
					case PriorityLevel.High: chance = 0.05; break;
					case PriorityLevel.VeryHigh: chance = 0.1; break;
				}
			}
			else if( type == RumorType.Vendor )
			{
				switch( m_Priority )
				{
					case PriorityLevel.Medium: chance = 0.005; break;
					case PriorityLevel.High: chance = 0.1; break;
					case PriorityLevel.VeryHigh: chance = 0.2; break;
				}
			}
			else if( type == RumorType.Guard )
			{
				switch( m_Priority )
				{			
					case PriorityLevel.High: chance = 0.0025; break;
					case PriorityLevel.VeryHigh: chance = 0.005; break;
				}
			}
			return chance;
		}
		
		public bool CheckRegions( string regionName )
		{
			try
			{
				foreach( string reg in m_Regions )
				{
					if( reg == regionName )
					{
						foreach( string reg2 in m_ExcludedRegions )
						{
							if( reg2 == regionName ) 
								return true;
						}
						
						return false;
					}
				}
				
				return true;
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
				
			return false;
		}
	   
		public void AddRegion( string regionName )
	    {
			try
			{
				RegionsEngineRegion region = RegionsEngine.GetRegion( regionName );
				
				foreach( string reg in m_ExcludedRegions )
				{
					if( reg == region.Name )
					{
						m_ExcludedRegions.Remove( reg );
						return;
					}
				}
				
				foreach( string reg in m_Regions )
				{
					if( reg == region.Name )
						return;
				}
				
				// ArrayList string
				if( region != null )
				{
					m_Regions.Add( regionName );
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public void AddRegionExclude( string regionName )
	    {
			try
			{
				RegionsEngineRegion region = RegionsEngine.GetRegion( regionName );
				
				foreach( string reg in m_ExcludedRegions )
				{
					if( reg == region.Name )
						return;
				}
				
				// ArrayList string
				if( region != null )
				{
					m_ExcludedRegions.Add( regionName );
				}
			}
			catch ( Exception exc )
			{
				Console.WriteLine( exc.ToString() );
			}
		}
		
		public XmlNode GetXmlNode( XmlDocument dom )
		{
			XmlNode xNode = dom.CreateElement( "Rumor" );
			
			#region Title
			
			try
			{
				XmlAttribute title = dom.CreateAttribute( "Title" );
				title.Value = m_Title;
				xNode.Attributes.Append( title );
			}
			catch
			{
			}
			
			#endregion
			#region Coppice
			
			try
			{
				XmlAttribute coppice = dom.CreateAttribute( "Coppice" );
				coppice.Value = m_Coppice;
				xNode.Attributes.Append( coppice );
			}
			catch
			{
			}
			
			#endregion
			#region KeyWord
			
			try
			{
				XmlAttribute keyword = dom.CreateAttribute( "KeyWord" );
				keyword.Value = m_KeyWord;
				xNode.Attributes.Append( keyword );
			}
			catch
			{
			}
			
			#endregion
			#region Text
			
			try
			{
				XmlAttribute text = dom.CreateAttribute( "Text" );
				text.Value = m_Text;
				xNode.Attributes.Append( text );
			}
			catch
			{
			}
			
			#endregion
			#region Rumor Mobile
			
			try
			{
				XmlAttribute mobile = dom.CreateAttribute( "RumorMobile" );
				mobile.Value = m_RumorMobile.Serial.ToString();
				xNode.Attributes.Append( mobile );
			}
			catch
			{
			}
			
			#endregion
			#region Regions
			
			try
			{
				if ( m_Regions.Count > 0 )
				{
					XmlNode xRegions = dom.CreateElement( "Regions" );
					
					foreach( string r in m_Regions )
					{
						XmlNode xCom = dom.CreateElement( "Region" );
						
						XmlAttribute com = dom.CreateAttribute( "Name" );
						com.Value = r;
						xCom.Attributes.Append( com );
	
						xRegions.AppendChild( xCom );
					}
	
					xNode.AppendChild( xRegions );
				}
			}
			catch
			{
			}
			
			#endregion
			#region ExcludedRegions
			
			try
			{
				if ( m_Regions.Count > 0 )
				{
					XmlNode xRegions = dom.CreateElement( "ExcludedRegions" );
					
					foreach( string r in m_ExcludedRegions )
					{
						XmlNode xCom = dom.CreateElement( "Region" );
						
						XmlAttribute com = dom.CreateAttribute( "Name" );
						com.Value = r;
						xCom.Attributes.Append( com );
	
						xRegions.AppendChild( xCom );
					}
	
					xNode.AppendChild( xRegions );
				}
			}
			catch
			{
			}
			
			#endregion
			#region Priority
			
			try
			{
				XmlAttribute priority = dom.CreateAttribute( "Priority" );
				priority.Value =  ( (int) m_Priority ).ToString();
				xNode.Attributes.Append( priority );
			}
			catch {}
			
			#endregion
			#region Type
			
			try
			{
				XmlAttribute type = dom.CreateAttribute( "Type" );
				type.Value =  ( (int) m_Type ).ToString();
				xNode.Attributes.Append( type );
			}
			catch {}
			
			#endregion
			#region End
			
			try
			{
				XmlAttribute endrumor = dom.CreateAttribute( "EndRumor" );
				endrumor.Value = m_EndRumor.ToString("dd.MM.yyyy HH:mm:ss");
				xNode.Attributes.Append( endrumor );
			}
			catch
			{
			}
			
			#endregion
			#region Start
			
			try
			{
				XmlAttribute startrumor = dom.CreateAttribute( "StartRumor" );
				startrumor.Value = m_StartRumor.ToString("dd.MM.yyyy HH:mm:ss");
				xNode.Attributes.Append( startrumor );
			}
			catch
			{
			}
			
			#endregion
			
			return xNode;
		}
		
		public bool IsValid()
		{
			if( Title.Length == 0 && m_Type != NewsType.News )
				return false;
				
			if( Coppice.Length == 0 && m_Type == NewsType.Rumor )
				return false;
			
			if( KeyWord.Length == 0 && m_Type == NewsType.Rumor )
				return false;
								
			if( Text.Length < 10 )
				return false;
			
			return true;
		}
		
		public int CompareTo( object o )
		{
			if ( o == null )
				return 1;
	     	 
			if ( !(o is RumorRecord) )
				throw new ArgumentException();
	      
			RumorRecord rec = ( RumorRecord ) o;
			
			if ( !Expired && IsValid() && ( rec.Expired || !rec.IsValid() ) )
			    return 1;
			    
			if ( ( Expired || !IsValid() ) && !rec.Expired && rec.IsValid() )
			    return -1;   
			
			if ( m_Type > rec.Type )
				return 1;
			
			if ( m_Type < rec.Type )
				return -1;
	      
			if ( m_Priority > rec.Priority )
				return 1;
			
			if ( m_Priority < rec.Priority )
				return -1;
	      
			return m_EndRumor.CompareTo( rec.EndRumor );
		}
 
		#endregion
	}				
}


