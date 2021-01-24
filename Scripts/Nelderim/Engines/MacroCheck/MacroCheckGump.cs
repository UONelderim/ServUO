using Server.Gumps;
using Server.Network;

namespace Server.Engines
{
    public class MacroCheckGump : Gump
    {    
        private CheckPlayer m_Check;

        public MacroCheckGump( CheckPlayer check ) : base( Utility.RandomMinMax( 10 , 80 ) , Utility.RandomMinMax( 10 , 50 ) )
        {
            m_Check = check;
                        
            this.Closable=false;
            this.Disposable=true;
            this.Dragable=false;
            this.Resizable=false;
            this.AddPage(0);    
            int ktory = Utility.RandomMinMax(0,2);
            if (ktory == 0)
            {
                this.AddImage(71,80,2200);
                int random3 = Utility.RandomMinMax(0,7);
                int i;
                for (i=0; i<=3; i++)
                {
                    if (random3 == i)
                    {
                        this.AddButton(120+(i*30),230,2225+i,2225+i,1,GumpButtonType.Reply,0);
                    }
                    else
                    {
                        this.AddButton(120+(i*30),230,2225+i,2225+i,2,GumpButtonType.Reply,0);
                    }
                }
                for (i=4; i<=7; i++)
                {
                    if (random3 == i)
                    {
                        this.AddButton(270+((i-4)*30),230,2225+i,2225+i,1,GumpButtonType.Reply,0);
                    }
                    else
                    {
                        this.AddButton(270+((i-4)*30),230,2225+i,2225+i,2,GumpButtonType.Reply,0);
                    }
                }
                this.AddLabel(274,140,32,"Wybierz cyfre "+(random3+1));
                this.AddLabel(130,100,32,"Macro");
                this.AddLabel(300,100,32,"Check");
                this.AddLabel(120, 140, 32, "Daj znac Bogom,");
                this.AddLabel(120, 160, 32, "ze zyjesz!");
            } 
            else if (ktory==1)
            {
                this.AddBackground(71,80,400,120,2620);
                int random2 = Utility.RandomMinMax(0,11);
                for (int i=0; i<=11; i++)
                {
                    if (i == random2)
                    {
                        this.AddButton(85+(i*30),100,1155,1155, 1 , GumpButtonType.Reply,0); // czerwony
                    }
                    else
                    {
                        this.AddButton(85+(i*30),100,1152,1152, 2 , GumpButtonType.Reply,0); // czerwony
                    }                
                }
                this.AddLabel(180, 140, 32, "Daj znac Bogom, ze zyjesz");
                this.AddLabel(180, 160, 32, "naciskajac wyrozniajacy sie przycisk!");
            }
            else if (ktory == 2)
            {
                this.AddBackground(71,80,300,350,2620);
                this.AddImage(96, 100, 104);        
                int random = Utility.RandomMinMax(0,7);
                int[] id = new int[8];            
                id[0] = 2;
                id[1] = 2;
                id[2] = 2;
                id[3] = 2;
                id[4] = 2;
                id[5] = 2;
                id[6] = 2;
                id[7] = 2;        
                id[random] = 1;        
                this.AddButton(188,107,112,112, id[0], GumpButtonType.Reply,0); // miecz            
                this.AddButton(251,130,107,107, id[1], GumpButtonType.Reply,0); // kielich            
                this.AddButton(277,191,105,105, id[2], GumpButtonType.Reply,0); // serce            
                this.AddButton(251,255,109,109, id[3], GumpButtonType.Reply,0); // waga            
                this.AddButton(188,281,106,106, id[4], GumpButtonType.Reply,0); // reka            
                this.AddButton(127,255,111,111, id[5], GumpButtonType.Reply,0); // krzyz            
                this.AddButton(101,194,110,110, id[6], GumpButtonType.Reply,0); // kropla            
                this.AddButton(127,130,108,108, id[7], GumpButtonType.Reply,0); // laska        
                int strzalkaID = 4500 + random;            
                this.AddImage(198,202,strzalkaID);                    
                this.AddLabel(135,329,32,"Macro");
                this.AddLabel(265,329,32,"Check");
                this.AddLabel(120, 370, 32, "Daj znac Bogom, ze zyjesz");
                this.AddLabel(120, 390, 32, "naciskajac przycisk wskazany strzalka w centrum tarczy!");
}

        }
        
        public override void OnResponse( NetState state, RelayInfo info)
        {
            int val = info.ButtonID;
            
            if( val == 1 )
                m_Check.PlayerRequest( true );
            else
                m_Check.PlayerRequest( false );
        }
                    
    }
}
