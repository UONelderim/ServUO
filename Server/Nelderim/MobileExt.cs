#region References

#endregion

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
			
		}
	}
}
