using System;
using Nelderim;

namespace Server
{
	public partial class Mobile
	{
		[CommandProperty(AccessLevel.Administrator, true)]
		public string ModifiedBy
		{
			get => Labels.Get(this).ModifiedBy;
			set => Labels.Get(this).ModifiedBy = value;
		}

		[CommandProperty(AccessLevel.Administrator, true)]
		public DateTime ModifiedDate
		{
			get => Labels.Get(this).ModifiedDate;
			set => Labels.Get(this).ModifiedDate = value;
		}

		private string[] m_Labels
		{
			get => Labels.Get(this).Labels;
			set
			{
				Labels.Get(this).Labels = value;
				InvalidateProperties();
			}
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
		public string Label0
		{
			get => m_Labels[0];
			set
			{
				m_Labels[0] = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string Label1
		{
			get => m_Labels[1];
			set
			{
				m_Labels[1] = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string Label2
		{
			get => m_Labels[2];
			set
			{
				m_Labels[2] = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string Label3
		{
			get => m_Labels[3];
			set
			{
				m_Labels[3] = value;
				InvalidateProperties();
			}
		}
		
		

		[CommandProperty(AccessLevel.GameMaster)]
		public string LabelOfCreator
		{
			get => m_Labels[4];
			set
			{
				m_Labels[4] = value;
				InvalidateProperties();
			}
		}
	}
}
