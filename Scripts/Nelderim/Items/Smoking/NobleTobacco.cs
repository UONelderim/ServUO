using System;

namespace Server.Items
{
	public class NobleTobacco : BaseTobacco
	{
		public static void OnSmoke(Mobile m)
		{
			m.SendMessage("Dym tytoniowy napelnia twoje pluca, czujesz przyjemne mrowienie w ustach.");

			m.Emote("*wypuszcza z ust wirujace kleby fajkowego dymu*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m, TimeSpan.FromSeconds(10), 0);
			a.Start();

			m.RevealingAction();
		}

		[Constructable]
		public NobleTobacco() : this(1)
		{
		}

		[Constructable]
		public NobleTobacco(int amount) : base(amount)
		{
			Name = "tyton szlachetny";
			Hue = 2126;
		}

		public NobleTobacco(Serial serial) : base(serial)
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