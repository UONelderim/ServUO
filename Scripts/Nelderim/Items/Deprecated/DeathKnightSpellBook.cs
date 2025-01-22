namespace Server.Items
{
	public class DeathKnightSpellbook : Spellbook
	{

		[CommandProperty( AccessLevel.GameMaster )]

		public override SpellbookType SpellbookType{ get{ return SpellbookType.DeathKnight; } }
		public override int BookOffset{ get{ return 750; } }
		public override int BookCount{ get{ return 15; } }


		public DeathKnightSpellbook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			//writer.Write( (Mobile)owner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		//	owner = reader.ReadMobile();
			ReplaceWith(new DeathKnightBook(Content, false));
		}
	}
}
