namespace Server.Items
{

	public class PlainTobacco : BaseTobacco
    {
        public static void OnSmoke(Mobile m)
        {
            m.SendMessage("Dym tytoniowy napelnia twoje pluca.");

            m.Emote("*wypuszcza z ust kleby fajkowego dymu*");

            m.PlaySound(0x226);
            SmokeTimer a = new SmokeTimer(m);
            a.Start();

            m.RevealingAction();
        }

        [Constructable]
		public PlainTobacco() : this(1)
		{
		}

		[Constructable]
		public PlainTobacco(int amount) : base(amount)
		{
			Name = "tyton pospolity";
			Hue = 2129;
		}

		public PlainTobacco(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

}