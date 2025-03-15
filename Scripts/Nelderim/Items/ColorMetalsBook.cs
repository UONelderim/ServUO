using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class ColorMetalsBook : Item
	{
		// Koszt nauki uzycia kolorowych metali w majsterkowaniu:
		public static int Price { get{ return 10000; } }

		public static bool LearnColorMetals(Mobile from)
		{
			PlayerMobile pm = from as PlayerMobile;
			if ( pm == null || from.Skills[SkillName.Tinkering].Base < 100.0 )
			{
				pm.SendMessage( "Aby zrozumiec tresc tej nauki musisz wpierw osiagnac mistrzostwo w majsterkowaniu." );
				return false;
			}
			else if ( pm.ColorMetal )
			{
				pm.SendMessage( "Posiadasz juz ta wiedze." );
				return false;
			}
			else
			{
				pm.ColorMetal = true;
				pm.SendMessage( "Nauczyles sie jak uzywac kolorowych metali w majsterkowaniu." );
				return true;
			}
		}

		public override string DefaultName
		{
			get { return "Jak majsterkowac z roznymi metalami"; }
		}

		[Constructable]
		public ColorMetalsBook() : base( 0xFF4 )
		{
			Weight = 1.0;
		}

		public ColorMetalsBook( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else 
			{
				if (LearnColorMetals(from))
				{
					Delete();
				}
			}
		}
	}
}