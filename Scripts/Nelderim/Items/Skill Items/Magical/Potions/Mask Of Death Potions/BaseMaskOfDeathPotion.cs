#region References

using System;
using System.Collections;

#endregion

namespace Server.Items
{
	public abstract class BaseMaskOfDeathPotion : BasePotion
	{
		public abstract TimeSpan Duration { get; }
		public abstract double Delay { get; }

		public static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect(Mobile m)
		{
			return m_Table.Contains(m);
		}

		private static void Expire_Callback(object state)
		{
			Mobile m = (Mobile)state;

			m.SendLocalizedMessage(503325); // You are no longer ignored by monsters.

			m.MaskOfDeathEffect = false;

			m_Table.Remove(m);
		}

		public BaseMaskOfDeathPotion(PotionEffect effect) : base(0xF06, effect)
		{
			Hue = 0x482;
		}

		public BaseMaskOfDeathPotion(Serial serial) : base(serial)
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

		public void ApplyMaskOfDeath(Mobile from)
		{
			from.MaskOfDeathEffect = true;
		}


		public override void Drink(Mobile from)
		{
			if (from.BeginAction(typeof(BaseMaskOfDeathPotion)))
			{
				if (!UnderEffect(from))
				{
					Timer t = (Timer)m_Table[from];

					if (t != null)
						t.Stop();

					m_Table[from] = t = Timer.DelayCall(Duration, new TimerStateCallback(Expire_Callback), from);

					//Effects.SendPacket( from, from.Map, new GraphicalEffect( EffectType.FixedFrom, from.Serial, Serial.Zero, 0x375A, from.Location, from.Location, 10, 15, true, false ) );//Default
					from.FixedParticles(0x3779, 10, 15, 5002, EffectLayer.Head);
					from.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
					from.FixedParticles(0x3778, 10, 30, 5010, EffectLayer.Head);

					ApplyMaskOfDeath(from);

					from.SendLocalizedMessage(503326); // You are now ignored by monsters.

					Consume();
				}

				Timer.DelayCall(TimeSpan.FromMinutes(Delay), new TimerStateCallback(Release), from);
			}
			else
			{
				from.SendMessage("You must wait 30 minutes to use this potion");
			}
		}


		private static void Release(object state)
		{
			((Mobile)state).EndAction(typeof(BaseMaskOfDeathPotion));
		}
	}
}
