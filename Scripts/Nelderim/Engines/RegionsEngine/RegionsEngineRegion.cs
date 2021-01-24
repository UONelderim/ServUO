namespace Server.Nelderim
{
	public class RegionsEngineRegion
	{
		private string m_Name;
		private Population m_Population;
		private string m_Parent;
		private GuardEngine[] m_Guards;
		private bool[] m_Schools;
		private bool[] m_Followers;
		private int m_TameLimit;
		private int[] m_Intolerance;
        private double[] m_ResourceVeins;
		
		public RegionsEngineRegion( string name, string parent )
		{
			m_Name = name;
			m_Parent = parent;
			m_Population = null;
			m_Guards = new GuardEngine[] { null, null, null, null, null, null, null };
			m_Schools = null;
			m_Followers = null;
			m_TameLimit = 0;
			m_Intolerance = null;
            m_ResourceVeins = null;
		}
		
		public string Name
		{
			get { return m_Name; }
		}
		
		public string Parent
		{
			get { return m_Parent; }
		}
		
		public Population RegionPopulation
		{
			get { return m_Population; }
			set { m_Population = value; }
		}
		
		public Race GetRace
		{
			get { return m_Population.GetRace; }
		}
		
		public GuardEngine[] Guards
		{
			get { return m_Guards; }
			set { m_Guards = value; }
		}
		
		public bool[] Schools
		{
			get { return m_Schools; }
			set { m_Schools = value; }
		}
		
		public bool MageryIsBanned
		{
			get { return m_Schools[0]; }
			set { m_Schools[0] = value; }
		}
		
		public bool ChivalryIsBanned
		{
			get { return m_Schools[1]; }
			set { m_Schools[1] = value; }
		}
		
		public bool NecromantionIsBanned
		{
			get { return m_Schools[2]; }
			set { m_Schools[2] = value; }
		}
		public bool DruidismIsBanned
		{
			get { return m_Schools[3]; }
			set { m_Schools[3] = value; }
		}
		
		public bool[] BannedFollowers
		{
			get { return m_Followers; }
			set { m_Followers = value; }
		}
		
		public bool SummonsAreBanned
		{
			get { return m_Followers[0]; }
			set { m_Followers[0] = value; }
		}
		
		public bool FamiliarsAreBanned
		{
			get { return m_Followers[1]; }
			set { m_Followers[1] = value; }
		}
		
		public bool PetsAreBanned
		{
			get { return m_Followers[2]; }
			set { m_Followers[2] = value; }
		}
		
		public int TameLimit
		{
			get { return m_TameLimit; }
			set { m_TameLimit = value; }
		}
		
		public int[] Intolerance
		{
			get { return m_Intolerance; }
			set { m_Intolerance = value; }
		}

		public double[] ResourceVeins
		{
			get { return m_ResourceVeins; }
			set { m_ResourceVeins = value; }
		}

 	}
}
