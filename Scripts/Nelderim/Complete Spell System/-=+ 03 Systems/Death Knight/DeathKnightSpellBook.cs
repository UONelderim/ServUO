using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;


namespace Server.Items
{
	public class DeathKnightSpellbook : Spellbook
	{
		//public Mobile owner;

		[CommandProperty( AccessLevel.GameMaster )]
		//public Mobile Owner { get{ return owner; } set{ owner = value; } }

		public override SpellbookType SpellbookType{ get{ return SpellbookType.DeathKnight; } }
		public override int BookOffset{ get{ return 750; } }
		public override int BookCount{ get{ return 15; } }



		[Constructable]
		public DeathKnightSpellbook() : this( (ulong)0 )
		{
		}


		[Constructable]
		public DeathKnightSpellbook( ulong content/*, Mobile gifted*/ ) : base( content, 3834 )
		{
			Hue = 0x969;
			
			//owner = gifted;

			string sEvil = "Zly";
			switch ( Utility.RandomMinMax( 0, 7 ) ) 
			{
				case 0: sEvil = "Zlego";			break;
				case 1: sEvil = "Mrocznego";			break;
				case 2: sEvil = "Przebieglego";		break;
				case 3: sEvil = "Szalonego";		break;
				case 4: sEvil = "Zepsutego";		break;
				case 5: sEvil = "Nienawistnego";		break;
				case 6: sEvil = "Chorego";	break;
				case 7: sEvil = "Posepnego";	break;
			}

			switch ( Utility.RandomMinMax( 1, 2 ) ) 
			{
				case 1: this.Name = "Ksiega " + sEvil + " Rycerza";	break;
				case 2: this.Name = "Ksiega " + sEvil + " Rycerza";	break;
			}
		}

public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			/*if ( owner != from )
			{
				from.SendMessage( "These pages appears as scribbles to you." );
			}
			else*/ if ( Parent == from || ( pack != null && Parent == pack ) )
			{
				from.SendSound( 0x55 );
				from.CloseGump( typeof( DeathKnightSpellbookGump ) );
				from.SendGump( new DeathKnightSpellbookGump( from, this, 1 ) );
			}
			else from.SendMessage("Ta ksiega musi znajdowac sie Twoim glownym plecaku");
		}


 /*       public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( owner != null ){ list.Add( 1070722, "For " + owner.Name + "" ); }
        }*/

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
		}
	}
}
