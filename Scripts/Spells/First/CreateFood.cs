using Server.Items;
using System;
using Server.Gumps;
using Server.Network;

namespace Server.Spells.First
{
    public class CreateFoodSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Create Food", "In Mani Ylem",
            224,
            9011,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.MandrakeRoot);
        internal static readonly FoodInfo[] m_Food = new FoodInfo[]
        {
            new FoodInfo(typeof(Grapes), "a grape bunch"),
            new FoodInfo(typeof(Ham), "a ham"),
            new FoodInfo(typeof(CheeseWedge), "a wedge of cheese"),
            new FoodInfo(typeof(Muffins), "muffins"),
            new FoodInfo(typeof(FishSteak), "a fish steak"),
            new FoodInfo(typeof(Ribs), "cut of ribs"),
            new FoodInfo(typeof(CookedBird), "a cooked bird"),
            new FoodInfo(typeof(Sausage), "sausage"),
            new FoodInfo(typeof(Apple), "an apple"),
            new FoodInfo(typeof(Peach), "a peach")
        };
        public CreateFoodSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        
        public override void OnCast()
        {
            if (CheckSequence())
            {
	            Caster.SendGump(new CreateFoodGump(Caster));
            }

            FinishSequence();
        }
    }

    public class FoodInfo
    {
        private Type m_Type;
        private string m_Name;
        public FoodInfo(Type type, string name)
        {
            m_Type = type;
            m_Name = name;
            //Hacky but whatever
            var temp = Create();
            ItemId = temp.ItemID;
            temp.Delete();
        }

        public Type Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        public int ItemId { get;  }
        public Item Create()
        {
            return Loot.Construct(m_Type);
        }
    }

    public class CreateFoodGump : Gump
    {
	    const int Margin = 5;
	    const int ButtonWidth = 80;
	    const int ButtonHeight = 60;
	    const int ButtonArt = 0x919;
	    const int ColumnsCount = 3;

	    private long _CastTime;
	    private Mobile _Caster;
	    private FoodInfo[] _FoodInfo;
	    public CreateFoodGump(Mobile caster) : base(50, 50)
	    {
		    _CastTime = Core.TickCount;
		    caster.CloseGump<CreateFoodGump>();
		    _Caster = caster;
		    var foodInfo = CreateFoodSpell.m_Food;

		    var rowsCount = (int)Math.Ceiling(foodInfo.Length / (double)ColumnsCount);

		    var totalWidth = ButtonWidth * ColumnsCount + Margin * 2;
		    var totalHeight = ButtonHeight * rowsCount + Margin * 2;
			    
		    AddPage(0);
		    AddBackground(0, 0, totalWidth, totalHeight, 3000);

		    for (int i = 0; i < foodInfo.Length; i++)
		    {
			    var entry = foodInfo[i];
			    var x = i % ColumnsCount;
			    var y = i / ColumnsCount;
			    AddImageTiledButton(Margin + x * ButtonWidth, Margin + y * ButtonHeight, 
				    ButtonArt, ButtonArt, i + 1, GumpButtonType.Reply, 0, 
				    entry.ItemId, 0, 17, 23); 
		    }
	    }

	    public override void OnResponse(NetState sender, RelayInfo info)
	    {
		    if(_CastTime + 10000 < Core.TickCount) //10s
		    {
			    _Caster.SendLocalizedMessage(500641); //Straciles koncentracje, a zaklecie rozproszylo sie. 
			    return;
		    }

		    var index = info.ButtonID - 1;
		    if(index < 0 || index >= CreateFoodSpell.m_Food.Length)
		    {
			    _Caster.SendMessage("OSZUST!");
			    return;
		    }
		    var foodInfo = CreateFoodSpell.m_Food[index];
		    var food = foodInfo.Create();
		    
		    if (food != null)
		    {
		        _Caster.AddToBackpack(food);
		    
		        // You magically create food in your backpack:
		        _Caster.SendLocalizedMessage(1042695, true, " " + foodInfo.Name);
		    
		        _Caster.FixedParticles(0, 10, 5, 2003, EffectLayer.RightHand);
		        _Caster.PlaySound(0x1E2);
		    }
	    }
    }
}
