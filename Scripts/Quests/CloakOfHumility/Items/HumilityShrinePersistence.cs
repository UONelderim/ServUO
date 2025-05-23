using Server.Items;
using Server.Mobiles;
namespace Server.Engines.Quests
{
    public class HumilityShrinePersistence : Item
    {
        [Constructable]
        public HumilityShrinePersistence() : base(219)
        {
            Movable = false;
        }

        private Rectangle2D m_Rec = new Rectangle2D(4273, 3696, 2, 2);

        public override bool HandlesOnMovement => true;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m is PlayerMobile && m_Rec.Contains(m) && m.Backpack != null)
            {
                Item item = m.Backpack.FindItemByType(typeof(GreyCloak));

                if (item == null)
                    item = m.FindItemOnLayer(Layer.Cloak);

                if (item != null && item is GreyCloak && ((GreyCloak)item).Owner == m)
                {
                    m.SendLocalizedMessage(1075897); // As you near the shrine a strange energy envelops you. Suddenly, your cloak is transformed into the Cloak of Humility!

                    m.Backpack.DropItem(new HumilityCloak());
                    item.Delete();
                }
            }
        }

        public HumilityShrinePersistence(Serial serial)
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

        public static void SetupMobiles()
        {
            BaseCreature next = new Gareth();
            next.MoveToWorld(new Point3D(2023, 2841, 20), Map.Trammel);
            next.Home = next.Location;
            next.RangeHome = 5;

            next = new Gareth();
            next.MoveToWorld(new Point3D(2023, 2841, 20), Map.Felucca);
            next.Home = next.Location;
            next.RangeHome = 5;

            next = new Dierdre();
            next.MoveToWorld(new Point3D(1442, 1600, 20), Map.Felucca);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Jason();
            next.MoveToWorld(new Point3D(610, 2197, 0), Siege.SiegeShard ? Map.Felucca : Map.Trammel);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Kevin();
            next.MoveToWorld(new Point3D(2464, 439, 15), Siege.SiegeShard ? Map.Felucca : Map.Trammel);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Maribel();
            next.MoveToWorld(new Point3D(1443, 1701, 0), Siege.SiegeShard ? Map.Felucca : Map.Trammel);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Nelson();
            next.MoveToWorld(new Point3D(3441, 2623, 36), Siege.SiegeShard ? Map.Felucca : Map.Trammel);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Sean();
            next.MoveToWorld(new Point3D(2442, 471, 15), Map.Felucca);
            next.Home = next.Location;
            next.RangeHome = 40;

            next = new Walton();
            next.MoveToWorld(new Point3D(610, 2197, 0), Map.Felucca);
            next.Home = next.Location;
            next.RangeHome = 40;
        }
    }
}
