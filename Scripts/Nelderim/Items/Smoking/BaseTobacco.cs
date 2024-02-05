using System;

namespace Server.Items
{
	public interface ISmokable
	{
		void OnSmoke(Mobile m);
	}

	public abstract class BaseTobacco : Item, ISmokable
	{
		public virtual void OnSmoke(Mobile m)
		{
			m.SendMessage("Dym tytoniowy napelnia twoje pluca.");

			m.Emote("*wypuszcza z ust kleby fajkowego dymu*");

			m.PlaySound(0x226);
			SmokeTimer a = new SmokeTimer(m);
			a.Start();

			m.RevealingAction();
		}

		protected class SmokeTimer : Timer
		{
			private static TimeSpan ParticlesDuration => TimeSpan.FromMilliseconds(500);

			private Mobile m_Smoker;
			private int m_Hue;

			public SmokeTimer(Mobile smoker) : this(smoker, TimeSpan.FromSeconds(5), 0)
			{
			}

			public SmokeTimer(Mobile smoker, TimeSpan duration, int hue) : base(TimeSpan.Zero, ParticlesDuration, (int)(duration.TotalMilliseconds / ParticlesDuration.TotalMilliseconds))
			{
				m_Smoker = smoker;
				m_Hue = hue;
			}
			protected override void OnTick()
			{
				int speed = 10;
				int duration = 10;
				int itemId = 0x3728;
				int hue = m_Hue;
				int effect = 9539;
				Effects.SendLocationParticles(EffectItem.Create(m_Smoker.Location, m_Smoker.Map, EffectItem.DefaultDuration), itemId, speed, duration, hue, 7, effect, 0);
			}
		}

		public BaseTobacco() : this(1)
		{
		}

		public BaseTobacco(int amount) : base(0x11EB)
		{
			Stackable = true;
			Weight = 0.025;
			Amount = amount;
		}

		public BaseTobacco(Serial serial) : base(serial)
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