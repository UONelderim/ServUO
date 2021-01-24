using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadSpellbook : CSpellbook
	{
		public override School School{ get{ return School.Undead; } }

		[Constructable]
		public UndeadSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public UndeadSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public UndeadSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 38;
			Name = "Księga Okultyzmu";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				Container pack = from.Backpack;
				if( !(Parent == from || (pack != null && Parent == pack)) )
				{
					from.SendMessage( "Ta książka musi znajdować się w głównym plecaku, by można jej uzywać." );
					return;
				}
				else if( SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this.School ) )
				{
					return;
				}
			}

			from.CloseGump( typeof( UndeadSpellbookGump ) );
			from.SendGump( new UndeadSpellbookGump( this ) );
		}

		public UndeadSpellbook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
