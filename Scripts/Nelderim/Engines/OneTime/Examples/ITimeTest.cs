using Server.Items;
using Server.Mobiles;
using Server.OneTime;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    class ITimeTest : Item, IOneTime  //Add IOneTime interface to item or mobile script to use One Time
    {
        public int OneTimeType { get; set; } //This is the getter/setter for the type of timer and is needed for the Interface

        [Constructable]
        public ITimeTest() : base(0xF07)
        {
            Hue = 1175;

            OneTimeType = 3; //second : <Do Not Use>1 = tick, 2 = millisecond<Do Not Use>, 3 = second, 4 = minute, 5 = hour, 6 = day (Pick a time interval 3-6) <Tick and Milli Removed - CPU HOGS>
        }

        public ITimeTest(Serial serial) : base(serial)
        {
        }

        private Point3D PlayerOldLoc;
        private List<Item> itms = new List<Item>();

        public void OneTimeTick() //This is the method that will run when the timer event is raised and is needed for the Interface
        {
            if (Hue == 1175)
                Hue = 1153;
            else
                Hue = 1175;


            //the following code is another example of how to add control to the PlayerMobile without editing the PlayerMobile.cs
            //When you spawn a ITimeTest into the world, place it into your backpack to invoke the code!
            if (itms.Count > 0)
            {
                foreach (Item item in itms)
                {
                    item.Delete();
                }
            }

            if (RootParent != null)
            {
                if (RootParent is PlayerMobile pm)
                {
                    if (PlayerOldLoc != pm.Location)
                    {
                        ITimeTest testMarker = new ITimeTest();

                        testMarker.MoveToWorld(PlayerOldLoc, pm.Map);

                        PlayerOldLoc = pm.Location;

                        pm.Say("You Moved!");

                        IEnumerable<Item> items = from c in pm.GetItemsInRange(50)
                                                  where c.Visible
                                                  select c as Item;

                        itms.AddRange(items);
                    }
                    else
                    {
                        pm.Say("Your Standing Still!");
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(OneTimeType); //Save One Time Type
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            OneTimeType = reader.ReadInt(); //Load One Time Type
        }
    }
}
