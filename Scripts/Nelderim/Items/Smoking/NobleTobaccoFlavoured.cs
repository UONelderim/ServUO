using System;

namespace Server.Items
{

	public class NobleTobaccoApple : BaseTobaccoFlavoured
	{
		public override void OnSmoke(Mobile m)
		{
			m.SendMessage("Jablkowy dym tytoniowy napelnia twoje pluca, czujesz przyjemne mrowienie w ustach.");

			m.Emote("*wypuszcza z ust wirujace kleby fajkowego dymu roztaczajac jablkowy aromat*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m, TimeSpan.FromSeconds(10), SmokeHue);
			a.Start();

			m.RevealingAction();
		}

		[Constructable]
		public NobleTobaccoApple() : this(1)
		{
		}

		[Constructable]
		public NobleTobaccoApple(int amount) : base(amount)
		{
			Name = "tyton szlachetny";
			Hue = 2126;
			Flavour = TobaccoFlavour.Apple;
		}

		public NobleTobaccoApple(Serial serial) : base(serial)
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

	public class NobleTobaccoPear : BaseTobaccoFlavoured
	{
		public override void OnSmoke(Mobile m)
		{
			m.SendMessage("Gruszkowy dym tytoniowy napelnia twoje pluca, czujesz przyjemne mrowienie w ustach.");

			m.Emote("*wypuszcza z ust wirujace kleby fajkowego dymu roztaczajac gruszkowy aromat*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m, TimeSpan.FromSeconds(10), SmokeHue);
			a.Start();

			m.RevealingAction();
		}


		[Constructable]
		public NobleTobaccoPear() : this(1)
		{
		}

		[Constructable]
		public NobleTobaccoPear(int amount) : base(amount)
		{
			Name = "tyton szlachetny";
			Hue = 2126;
			Flavour = TobaccoFlavour.Pear;
		}

		public NobleTobaccoPear(Serial serial) : base(serial)
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

	public class NobleTobaccoLemon : BaseTobaccoFlavoured
	{
		public override void OnSmoke(Mobile m)
		{
			m.SendMessage("Cytrusowy dym tytoniowy napelnia twoje pluca, czujesz przyjemne mrowienie w ustach.");

			m.Emote("*wypuszcza z ust wirujace kleby fajkowego dymu roztaczajac cytrusowy aromat*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m, TimeSpan.FromSeconds(10), SmokeHue);
			a.Start();

			m.RevealingAction();
		}


		[Constructable]
		public NobleTobaccoLemon() : this(1)
		{
		}

		[Constructable]
		public NobleTobaccoLemon(int amount) : base(amount)
		{
			Name = "tyton szlachetny";
			Hue = 2126;
			Flavour = TobaccoFlavour.Lemon;
		}

		public NobleTobaccoLemon(Serial serial) : base(serial)
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