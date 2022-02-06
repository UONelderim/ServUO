/*
 * Created by SharpDevelop.
 * User: Shazzy
 * Date: 11/17/2005
 * Time: 6:25 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Server;

namespace Server.Items
{
	
	public class SantasQuestLog: BaseBook
	{
		private const string TITLE = "Zapis zadania od Pana";
		private const string AUTHOR = "Elfy Pana";
		private const int PAGES = 4;
		private const bool WRITABLE = false;
		
		[Constructable]
		public SantasQuestLog() : base( ( 0x0FBD ), TITLE, AUTHOR, PAGES, WRITABLE )
		{
			LootType = LootType.Blessed;
			// NOTE: There are 8 lines per page and
			// approx 22 to 24 characters per line.
			//  0----+----1----+----2----+
			int cnt = 0;
			string[] lines;

			lines = new string[]
			{
				"Znajdz i oswoj", 
				"renifery i zwroc je",
				"do Pana.",
                "Wystarczy, ze powiesz 'panie';' i ",
				"zwrocisz ich uwage.",
                "On nagrodzi Cie",
				"za Twoje trudy", 
                "w odnalezieniu ich.",
				

			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"Musisz znalezc renifery o imionach:",
				"Rudolph, Dasher,",
				"Dancer, Prancer,",
				"Vixen. Comet,",
				"Cupid, Donner,", 
				"i Blitzen.",
				"I przyprowadzic je bezpiecznie",
				"do Pana.",
			};
			Pages[cnt++].Lines = lines;
			
			lines = new string[]
			{
				"Zbierz wszystkie",
				"buty reniferow",
				"oraz specjalny mlot",
				"(2-kliknij na młot",
				"gdy zbierzesz",
				"wszystkie buty",
				"w swoim plecaku, by",
				"otrzymac prezent Pana).",
			};
			Pages[cnt++].Lines = lines;
			
			lines = new string[]
			{
				"Wez prezent, który zrobiłeś",
				"i zanies go",
				"do Pana.",
				"     ",
				"Baw sie dobrze", 
				"niech Twe swieta Pana",
				"sa pelne radosci!",
				" - Elfy Pana",
			};
			Pages[cnt++].Lines = lines;



		}

		public SantasQuestLog( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); 
		}
	}
}
