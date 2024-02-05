using System;

namespace Server.Items
{

	public class SwampTobacco : BaseTobacco
	{
		public override void OnSmoke(Mobile m)
		{
			m.SendMessage("Dym z bagiennego ziela napelnia twoje pluca, czujesz niesamowita lekkosc.");

			m.Emote("*wypuszcza z ust kleby dymu z bagiennego ziela*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m, TimeSpan.FromSeconds(20), 87);
			a.Start();

			int hue = 87;
			Timer.DelayCall(TimeSpan.FromSeconds(13), delegate { CoughEffect(m, hue); });
			m.FixedParticles(0x376A, 9, 32, 5030, hue, 0, EffectLayer.Waist);

			m.RevealingAction();
		}

		private void CoughEffect(Mobile smoker, int hue)
		{
			if (smoker.Female)
				smoker.PlaySound(785);
			else
				smoker.PlaySound(1056);

			if (!smoker.Mounted)
				smoker.Animate(33, 5, 1, true, false, 0);

			smoker.FixedParticles(0x376A, 9, 32, 5030, hue, 0, EffectLayer.Waist);

			switch (Utility.Random(4))
			{
				case 0:
					smoker.Emote("*dym z bagiennego ziela wydaje sie ulatywac nawet z uszu postaci*");
					break;
				case 1:
					smoker.Emote("*dym z bagiennego ziela zawirowal fantazyjnie wokol glowy postaci*");
					break;
				case 2:
				default:
					smoker.Emote("*wykaszluje niewielkie klebki dymu bagiennego ziela*");
					break;
			}
		}

		[Constructable]
		public SwampTobacco() : this(1)
		{
		}

		[Constructable]
		public SwampTobacco(int amount) : base(amount)
		{
			Name = "bagienne ziele";
			Hue = 2130; //82;
		}

		public SwampTobacco(Serial serial) : base(serial)
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