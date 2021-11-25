using System;
using Server;

namespace Knives.TownHouses
{
	public class TownHouseConfirmGump : GumpPlusLight
	{
		private TownHouseSign c_Sign;
		private bool c_Items;

		public TownHouseConfirmGump( Mobile m, TownHouseSign sign ) : base( m, 100, 100 )
		{
			c_Sign = sign;
		}

		protected override void BuildGump()
		{
            int width = 200;
			int y = 0;

			AddHtml( 0, y+=10, width, String.Format( "<CENTER>{0} ten dom?", c_Sign.RentByTime == TimeSpan.Zero ? "Kupic" : "Wynajac" ));
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			if ( c_Sign.RentByTime == TimeSpan.Zero )
				AddHtml( 0, y+=25, width, String.Format( "<CENTER>{0}: {1}", "Cena", c_Sign.Free ? "Darmowy" : "" + c_Sign.Price ));
			else if ( c_Sign.RecurRent )
				AddHtml( 0, y+=25, width, String.Format( "<CENTER>{0}: {1}", c_Sign.PriceType, c_Sign.Price ));
			else
				AddHtml( 0, y+=25, width, String.Format( "<CENTER>{0}: {1}", "Jeden " + c_Sign.PriceTypeShort, c_Sign.Price ));

			if ( c_Sign.KeepItems )
			{
				AddHtml( 0, y+=20, width, "<CENTER>Koszt przedmiot�w: " + c_Sign.ItemsPrice);
				AddButton( 20, y, c_Items ? 0xD3 : 0xD2, "Przedmioty", new GumpCallback( Items ) );
			}

            AddHtml(0, y += 20, width, "<CENTER>Zablokowanych: " + c_Sign.Locks);
			AddHtml( 0, y+=20, width, "<CENTER>Zabezpieczonych: " + c_Sign.Secures);

			AddButton( 10, y+=25, 0xFB1, 0xFB3, "Anuluj", new GumpCallback( Cancel ) );
			AddButton( width-40, y, 0xFB7, 0xFB9, "Potwierdz", new GumpCallback( Confirm ) );

            AddBackgroundZero(0, 0, width, y+40, 0x13BE);
        }

		private void Items()
		{
			c_Items = !c_Items;

			NewGump();
		}

        private void Cancel()
        {
        }

		private void Confirm()
		{
			c_Sign.Purchase( Owner, c_Items );
		}
	}
}