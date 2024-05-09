using System;
using Server.ACC.CSS;

namespace Server.Gumps 
{ 
	public class DeathKnightSpellbookGump : CSpellbookGump
	{
		public override string TextHue => "336633";
		public override int BGImage => 2203;
		public override int SpellBtn => 2362;
		public override int SpellBtnP => 2361;
		public override string Label1 => "Umiejętności";
		public override string Label2 => "mrocznego rycerza";
		public override Type GumpType => typeof(DeathKnightSpellbookGump);

		public DeathKnightSpellbookGump(CSpellbook book) : base(book)
		{
		}
	}
}
