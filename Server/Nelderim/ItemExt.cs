#region References

using System;
using System.Linq;
using Nelderim;

#endregion

namespace Server
{
	public partial class Item
	{
		private const int StealableFlag = 0x00200000;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Stealable
		{
			get { return GetSavedFlag(StealableFlag); }
			set { SetSavedFlag(StealableFlag, value); }
		}

		// Nelderim labels
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
		
		public virtual void AddLabelsProperty( ObjectPropertyList list )
		{
			var joinedLabels = String.Join('\n', m_Labels.SkipLast(1).Where(l => !String.IsNullOrEmpty(l)));
			if (!String.IsNullOrEmpty(joinedLabels))
				list.Add(joinedLabels);
		}
	}
}
