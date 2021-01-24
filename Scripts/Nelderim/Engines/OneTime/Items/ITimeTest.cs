using Server.OneTime;

namespace Server
{
    class ITimeTest : Item, IOneTime //Add IOneTime interface to item or mobile script to use One Time
    {
        public int OneTimeType { get; set; } //This is the getter/setter for the type of timer and is needed for the Interface

        [Constructable]
        public ITimeTest() : base(0xF07)
        {
            Hue = 1175;

            OneTimeType = 3; //second : 1 = tick, 2 = millisecond, 3 = second, 4 = minute, 5 = hour, 6 = day (Pick a time interval 1-6)
        }

        public ITimeTest(Serial serial) : base(serial)
        {
        }

        public void OneTimeTick() //This is the method that will run when the timer event is raised and is needed for the Interface
        {
            if (Visible)
            {
                if (Hue == 1175)
                    Hue = 1153;
                else
                    Hue = 1175;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(OneTimeType);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            OneTimeType = reader.ReadInt();
        }
    }
}
