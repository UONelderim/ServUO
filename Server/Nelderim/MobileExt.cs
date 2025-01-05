#region References

#endregion

using System.Collections.Generic;
using Server.ContextMenus;

namespace Server
{
	public partial class Mobile
	{
		// HiddenGM

		[CommandProperty(AccessLevel.Seer, AccessLevel.Administrator)]
		public bool HiddenGM { get; set; }

		[CommandProperty(AccessLevel.Counselor, AccessLevel.Administrator)]
		public AccessLevel TrueAccessLevel { get => m_AccessLevel; set => m_AccessLevel = value; }
		
		//Maska smierci

		public bool MaskOfDeathEffect { get; set; }
		
		public virtual void NAddProperties(ObjectPropertyList list)
		{
			AddLabelsProperty(list);
		}

		public virtual void NContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			
		}
	}
}
