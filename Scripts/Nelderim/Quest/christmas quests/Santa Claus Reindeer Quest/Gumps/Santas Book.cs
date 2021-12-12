//
//  Added book/gump 10JUN2016 by Tukaram
//  Makes quest work with the Enhanced Client, and gives the player a checklist for taming reindeer. 
//  Warning - You can change pages and it will maintain the check marks, but if you close the gump the list clears.
//

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class SantasBook : Item
    {
        [Constructable]
        public SantasBook() : base(0xFF2)
        {
            Name = "Ksiega Pomocnika Pana";
            Movable = true;
            Hue = 1925;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {

            from.SendGump(new SantasBookGump());
           
        }
    
        public SantasBook( Serial serial ) : base( serial )
            {
            }

        public override void Serialize( GenericWriter writer )
		    {
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 ); // version
		    }

		public override void Deserialize( GenericReader reader )
		    {
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		    }
	    }
    }

///////////////////////////////////Gump Starts Here///////////////////////////
namespace Server
{
    public class SantasBookGump : Gump
    {
        public SantasBookGump()
            : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            ///////////////////////////////////Page Break///////////////////////////
            AddPage(1);
            AddImageTiled(14, 10, 377, 433, 2524);
            AddImageTiled(14, 425, 379, 18, 3501);
            AddImage(14, 18, 2712, 37);
            AddImage(14, 220, 2712, 37);
            AddTextEntry(120, 44, 167, 25, 33, 0, @"Zagubione renifery Pana");
            AddHtml(35, 83, 346, 281, @"<BODY>Znajdź i oswój każdego z reniferów i zwróć je Mikołajowi. Po prostu powiedz „Panie”, aby zwrócić jego uwagę. Wynagrodzi twoje wysiłki, podobnie jak jego zgubiony renifer.
Musisz znaleźć: Rudolfa, Dashera, Tancerza, Prancera, Vixena. Kometa, Kupidyn, Donner i Blitzen. Zabierz je bezpiecznie z powrotem do Pana.
Zbierz wszystkie buty renifera, a także specjalny młot. Kliknij dwukrotnie młotkiem wszystkie buty w plecaku, aby zaprezentować Pana. Zabierz prezent, który zrobiłeś i zanieś go do Pana.
Baw się i niech Twoje Święta będą wypełnione szczęściem! - Elfy
</BODY>", (bool)false, (bool)true);

            AddImageTiled(14, 7, 379, 18, 3501);
            AddItem(44, 17, 14943);
            AddItem(311, 17, 14951);
            AddItem(61, 361, 14983);
            AddItem(79, 357, 14984);

            AddButton(163, 385, 247, 248, 0, GumpButtonType.Reply, 0);
            AddButton(320, 385, 5541, 5542, (int)Buttons.Button1, GumpButtonType.Page, 2); //Forward

            ///////////////////////////////////Page Break///////////////////////////
            AddPage(2);
            AddImageTiled(14, 10, 377, 433, 2524);
            AddImageTiled(14, 425, 379, 18, 3501);
            AddImage(14, 18, 2712, 37);
            AddImage(14, 220, 2712, 37);
            AddTextEntry(120, 44, 167, 25, 33, 0, @"Zagubione renifery Pana");

            AddCheck(55, 85, 210, 211, false, 1);
            AddCheck(55, 110, 210, 211, false, 1);
            AddCheck(55, 135, 210, 211, false, 1);
            AddCheck(55, 160, 210, 211, false, 1);
            AddCheck(55, 185, 210, 211, false, 1);
            AddCheck(55, 210, 210, 211, false, 1);
            AddCheck(55, 235, 210, 211, false, 1);
            AddCheck(55, 260, 210, 211, false, 1);
            AddCheck(55, 285, 210, 211, false, 1);

            AddLabel(80, 85, 0, @"Dasher");
            AddLabel(80, 110, 0, @"Dancer");
            AddLabel(80, 135, 0, @"Prancer");
            AddLabel(80, 160, 0, @"Vixen");
            AddLabel(80, 185, 0, @"Comet");
            AddLabel(80, 210, 0, @"Cupid");
            AddLabel(80, 235, 0, @"Donner");
            AddLabel(80, 260, 0, @"Blitzen");
            AddLabel(80, 285, 0, @"Rudolph");

            AddHtml(35, 285, 346, 281, @"<BODY>Użyj tej listy kontrolnej, aby śledzić, które z reniferów Pana zostały oswojone. Nie zamykaj tej książki, w przeciwnym razie znaczniki wyboru zostaną utracone. - Elfy</BODY>", (bool)false, (bool)true);

            AddImageTiled(14, 7, 379, 18, 3501);
            AddItem(44, 17, 14943);
            AddItem(311, 17, 14951);
            AddItem(61, 361, 14983);
            AddItem(79, 357, 14984);

            AddButton(163, 385, 247, 248, 0, GumpButtonType.Reply, 0);
            AddButton(320, 385, 5538, 5539, (int)Buttons.Button2, GumpButtonType.Page, 1); //back
        }
        ///////////////////////////////////Page Break///////////////////////////

        public enum Buttons
        {
            Button1,
            Button2,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    { break; }
            }
        }
    }
}
