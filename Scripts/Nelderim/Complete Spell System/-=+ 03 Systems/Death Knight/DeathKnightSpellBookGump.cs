using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Misc; 
using Server.Network; 
using Server.Spells; 
using Server.Spells.DeathKnight; 
using Server.Prompts; 

namespace Server.Gumps 
{ 
	public class DeathKnightSpellbookGump : Gump 
	{
		private DeathKnightSpellbook m_Book; 


		public bool HasSpell(Mobile from, int spellID)
		{
			return (m_Book.HasSpell(spellID));
		}

		public DeathKnightSpellbookGump( Mobile from, DeathKnightSpellbook book, int page ) : base( 50, 100 ) 
		{
			m_Book = book; 

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			

            AddPage(0);
            AddImage(100, 100, 2203);

			int PriorPage = page - 1;
				if ( PriorPage < 1 ){ PriorPage = 19; }
			int NextPage = page + 1;
			string sGrave = "";



			if ( page == 1 )
			{
				int SpellsInBook = 14;
				int SafetyCatch = 0;
				int SpellsListed = 749;
				string SpellName = "";

				int nHTMLx = 146;
				int nHTMLy = 108;

				int nBUTTONx = 124;
				int nBUTTONy = 112;

				while ( SpellsInBook > 0 )
				{
					SpellsListed++;
					SafetyCatch++;

					if ( this.HasSpell( from, SpellsListed) )
					{
						SpellsInBook--;

						if ( SpellsListed == 750 ){ SpellName = "Wygnanie"; }
						else if ( SpellsListed == 751 ){ SpellName = "Dotyk Demona"; }
						else if ( SpellsListed == 752 ){ SpellName = "Pakt Ze Smiercia"; }
						else if ( SpellsListed == 753 ){ SpellName = "Ponury Zniwiarz"; }
						else if ( SpellsListed == 754 ){ SpellName = "Reka Wiedzmy"; }
						else if ( SpellsListed == 755 ){ SpellName = "Ogien Piekielny"; }
						else if ( SpellsListed == 756 ){ SpellName = "Promien Smierci"; }
						else if ( SpellsListed == 757 ){ SpellName = "Kula Smierci"; }
						else if ( SpellsListed == 758 ){ SpellName = "Tarcza Nienawisci"; }
						else if ( SpellsListed == 759 ){ SpellName = "Zniwiarz Dusz"; }
						else if ( SpellsListed == 760 ){ SpellName = "Wytrzymalosc Stali"; }
						else if ( SpellsListed == 761 ){ SpellName = "Uderzenie"; }
						else if ( SpellsListed == 762 ){ SpellName = "Skora Sukkuba"; }
						else if ( SpellsListed == 763 ){ SpellName = "Gniew"; }

						AddHtml( nHTMLx, nHTMLy, 182, 26, @"<BODY><BASEFONT Color=#111111><BIG>" + SpellName + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
						AddButton(nBUTTONx, nBUTTONy, 30008, 30008, SpellsListed, GumpButtonType.Reply, 0);

						nHTMLy = nHTMLy + 25;
						if ( SpellsInBook == 7 ){ nHTMLx = 310; nHTMLy = 108; }

						nBUTTONy = nBUTTONy + 25;
						if ( SpellsInBook == 7 ){ nBUTTONx = 280; nBUTTONy = 112; }
					}

					if ( SafetyCatch > 14 ){ SpellsInBook = 0; }
				}
			}
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{
			Mobile from = state.Mobile; 

			if ( info.ButtonID < 700 && info.ButtonID > 0 )
			{
				from.SendSound( 0x55 );
				int page = info.ButtonID;
				if ( page < 1 ){ page = 19; }
				if ( page > 19 ){ page = 1; }
				from.SendGump( new DeathKnightSpellbookGump( from, m_Book, page ) );
			}
			else if ( info.ButtonID > 700 )
			{
				if ( info.ButtonID == 750 ){ new BanishSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 751 ){ new DemonicTouchSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 752 ){ new DevilPactSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 753 ){ new GrimReaperSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 754 ){ new HagHandSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 755 ){ new HellfireSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 756 ){ new LucifersBoltSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 757 ){ new OrbOfOrcusSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 758 ){ new ShieldOfHateSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 759 ){ new SoulReaperSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 760 ){ new StrengthOfSteelSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 761 ){ new StrikeSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 762 ){ new SuccubusSkinSpell( from, null ).Cast(); }
				else if ( info.ButtonID == 763 ){ new WrathSpell( from, null ).Cast(); }

				from.SendGump( new DeathKnightSpellbookGump( from, m_Book, 1 ) );
			}
		}
	}
}