using Server.Engines.Craft;
using Server.Targeting;
using System;

namespace Server.Items
{
    public class UtilityItem
    {
        static public int RandomChoice(int itemID1, int itemID2)
        {
            int iRet = 0;
            switch (Utility.Random(2))
            {
                default:
                case 0:
                    iRet = itemID1;
                    break;
                case 1:
                    iRet = itemID2;
                    break;
            }
            return iRet;
        }
    }

    // ********** Dough **********
    public class Dough : Item, IQuality
    {
        private ItemQuality _Quality;

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality { get { return _Quality; } set { _Quality = value; InvalidateProperties(); } }

        public bool PlayerConstructed => true;

        [Constructable]
        public Dough()
            : base(0x103d)
        {
            Stackable = true;
            Weight = 1.0;
        }

        public override bool WillStack(Mobile from, Item item)
        {
            if (item is IQuality && ((IQuality)item).Quality != _Quality)
            {
                return false;
            }

            return base.WillStack(from, item);
        }

        public int OnCraft(int quality,
	        bool makersMark,
	        Mobile from,
	        CraftSystem craftSystem,
	        Type typeRes,
	        Type typeRes2,
	        ITool tool,
	        CraftItem craftItem,
	        int resHue)
        {
            Quality = (ItemQuality)quality;

            return quality;
        }

        public override void AddCraftedProperties(ObjectPropertyList list)
        {
            if (_Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // Exceptional
            }
        }

        public Dough(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write((int)_Quality);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version > 0)
                _Quality = (ItemQuality)reader.ReadInt();
        }

#if false
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			from.Target = new InternalTarget( this );
		}
#endif

        private class InternalTarget : Target
        {
            private readonly Dough m_Item;

            public InternalTarget(Dough item)
                : base(1, false, TargetFlags.None)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (targeted is Eggs)
                {
                    m_Item.Delete();

                    ((Eggs)targeted).Consume();

                    from.AddToBackpack(new UnbakedQuiche());
                    from.AddToBackpack(new Eggshells());
                }
                else if (targeted is CheeseWheel)
                {
                    m_Item.Delete();

                    ((CheeseWheel)targeted).Consume();

                    from.AddToBackpack(new CheesePizza());
                }
                else if (targeted is Sausage)
                {
                    m_Item.Delete();

                    ((Sausage)targeted).Consume();

                    from.AddToBackpack(new SausagePizza());
                }
                else if (targeted is Apple)
                {
                    m_Item.Delete();

                    ((Apple)targeted).Consume();

                    from.AddToBackpack(new UnbakedApplePie());
                }
                else if (targeted is Peach)
                {
                    m_Item.Delete();

                    ((Peach)targeted).Consume();

                    from.AddToBackpack(new UnbakedPeachCobbler());
                }
            }
        }
    }

    // ********** SweetDough **********
    public class SweetDough : Item
    {
        private ItemQuality _Quality;

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality { get { return _Quality; } set { _Quality = value; InvalidateProperties(); } }

        public override int LabelNumber => 1041340;// sweet dough

        [Constructable]
        public SweetDough()
            : base(0x103d)
        {
            Stackable = true;
            Weight = 1.0;
            Hue = 150;
        }

        public override void AddCraftedProperties(ObjectPropertyList list)
        {
            if (_Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // Exceptional
            }
        }

        public SweetDough(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write((int)_Quality);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version > 0)
                _Quality = (ItemQuality)reader.ReadInt();

            if (Hue == 51)
                Hue = 150;
        }

#if false
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			from.Target = new InternalTarget( this );
		}
#endif

        private class InternalTarget : Target
        {
            private readonly SweetDough m_Item;

            public InternalTarget(SweetDough item)
                : base(1, false, TargetFlags.None)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (targeted is BowlFlour)
                {
                    m_Item.Delete();
                    ((BowlFlour)targeted).Delete();

                    from.AddToBackpack(new CakeMix());
                }
                else if (targeted is Campfire)
                {
                    from.PlaySound(0x225);
                    m_Item.Delete();
                    InternalTimer t = new InternalTimer(from, (Campfire)targeted);
                    t.Start();
                }
            }

            private class InternalTimer : Timer
            {
                private readonly Mobile m_From;
                private readonly Campfire m_Campfire;

                public InternalTimer(Mobile from, Campfire campfire)
                    : base(TimeSpan.FromSeconds(5.0))
                {
                    m_From = from;
                    m_Campfire = campfire;
                }

                protected override void OnTick()
                {
                    if (m_From.GetDistanceToSqrt(m_Campfire) > 3)
                    {
                        m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                        return;
                    }

                    if (m_From.CheckSkill(SkillName.Cooking, 0, 10))
                    {
                        if (m_From.AddToBackpack(new Muffins()))
                            m_From.PlaySound(0x57);
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                    }
                }
            }
        }
    }

    // ********** JarHoney **********
    public class JarHoney : Item
    {
        [Constructable]
        public JarHoney()
            : base(0x9ec)
        {
            Weight = 1.0;
            Stackable = true;
        }

        public JarHoney(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Stackable = true;
        }

        /*public override void OnDoubleClick( Mobile from )
        {
        if ( !Movable )
        return;

        from.Target = new InternalTarget( this );
        }*/

        private class InternalTarget : Target
        {
            private readonly JarHoney m_Item;

            public InternalTarget(JarHoney item)
                : base(1, false, TargetFlags.None)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (targeted is Dough)
                {
                    m_Item.Delete();
                    ((Dough)targeted).Consume();

                    from.AddToBackpack(new SweetDough());
                }

                if (targeted is BowlFlour)
                {
                    m_Item.Consume();
                    ((BowlFlour)targeted).Delete();

                    from.AddToBackpack(new CookieMix());
                }
            }
        }
    }

    // ********** BowlFlour **********
    public class BowlFlour : Item
    {
        [Constructable]
        public BowlFlour()
            : base(0xa1e)
        {
            Weight = 1.0;
        }

        public BowlFlour(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    // ********** WoodenBowl **********
    public class WoodenBowl : Item
    {
        [Constructable]
        public WoodenBowl()
            : base(0x15f8)
        {
            Weight = 1.0;
        }

        public WoodenBowl(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    // ********** SackFlour **********
    public class SackFlour : Item, IQuality
    {
        private ItemQuality _Quality;

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality { get { return _Quality; } set { _Quality = value; InvalidateProperties(); } }

        public bool PlayerConstructed => true;

        [Constructable]
        public SackFlour()
            : this(1)
        {
        }

        [Constructable]
        public SackFlour(int amount)
            : base(0x1039)
        {
            Weight = 5.0;

            Stackable = true;
            Amount = amount;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            SackFlourOpen flour = new SackFlourOpen
            {
                Location = Location
            };

            if (Parent is Container)
            {
                ((Container)Parent).DropItem(flour);
            }
            else
            {
                flour.MoveToWorld(GetWorldLocation(), Map);
            }

            if (Amount > 1)
            {
                Amount--;
            }
            else
            {
                Delete();
            }
        }

        public int OnCraft(int quality,
	        bool makersMark,
	        Mobile from,
	        CraftSystem craftSystem,
	        Type typeRes,
	        Type typeRes2,
	        ITool tool,
	        CraftItem craftItem,
	        int resHue)
        {
            Quality = (ItemQuality)quality;

            return quality;
        }

        public override void AddCraftedProperties(ObjectPropertyList list)
        {
            if (_Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // Exceptional
            }
        }

        public SackFlour(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(4); // version

            writer.Write((int)_Quality);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 4:
                    _Quality = (ItemQuality)reader.ReadInt();
                    break;
                case 3:
                    _Quality = (ItemQuality)reader.ReadInt();
                    reader.ReadInt();
                    Stackable = true;
                    break;
				case 2:
				case 1:
					reader.ReadInt();
					break;
			}
        }
    }

    // ********** SackFlourOpen **********
    public class SackFlourOpen : Item
    {
        public override int LabelNumber => 1024166;  // open sack of flour

        [Constructable]
        public SackFlourOpen() : base(0x103A)
        {
            Weight = 4.0;
        }

        public SackFlourOpen(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    // ********** Eggshells **********
    public class Eggshells : Item
    {
        [Constructable]
        public Eggshells()
            : base(0x9b4)
        {
            Weight = 0.5;
        }

        public Eggshells(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WheatSheaf : Item
    {
        [Constructable]
        public WheatSheaf()
            : this(1)
        {
        }

        [Constructable]
        public WheatSheaf(int amount)
            : base(7869)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            from.BeginTarget(4, false, TargetFlags.None, OnTarget);
        }

        public virtual void OnTarget(Mobile from, object obj)
        {
            if (obj is AddonComponent)
                obj = (obj as AddonComponent).Addon;

            IFlourMill mill = obj as IFlourMill;

            if (mill != null)
            {
                int needs = mill.MaxFlour - mill.CurFlour;

                if (needs > Amount)
                    needs = Amount;

                mill.CurFlour += needs;
                Consume(needs);
            }
        }

        public WheatSheaf(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
