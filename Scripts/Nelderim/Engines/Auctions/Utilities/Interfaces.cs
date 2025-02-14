#region AuthorHeader

//
//	Interfaces version 1.0 - utilities version 2.0, by Xanthos
//

#endregion AuthorHeader

using Server.Mobiles;

namespace Xanthos.Interfaces
{

	//
	// Used by the auction system to validate the pet referred to by a shrink item.
	//
	public interface IShrinkItem
	{
		BaseCreature ShrunkenPet { get; }
	}
}
