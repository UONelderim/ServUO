using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	public class PrzyspieszenieWilkolaka : SilverRing
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(OnLogin);
			EventSink.Logout += new LogoutEventHandler(OnLogout);
		}

		private static void OnLogin(LoginEventArgs e)
		{
			Item ring = e.Mobile.FindItemOnLayer(Layer.Ring);
			if (ring != null && ring is PrzyspieszenieWilkolaka)
			{
				ring.OnEquip(e.Mobile);
			}
		}

		private static void OnLogout(LogoutEventArgs e)
		{
			Item ring = e.Mobile.FindItemOnLayer(Layer.Ring);
			if (ring != null && ring is PrzyspieszenieWilkolaka)
			{
				ring.OnRemoved(e.Mobile);
			}
		}

		private Timer m_Timer;

		[Constructable]
		public PrzyspieszenieWilkolaka()
		{
			Weight = 1.0;
			Hue = 1153;
			Name = "Przyspieszenie Wilkolaka";
			LootType = LootType.Blessed;
			SkillBonuses.SetValues(0, SkillName.Fencing, 30.0);
			Label1 = "dotkniecie piersciena powoduje, iz Twe konczyny pracuja szybciej";
		}

		public override bool OnEquip(Mobile from)
		{
			if (from != null && from.Map != Map.Internal)
			{
				from.Send(SpeedControl.MountSpeed);
				if (m_Timer == null || !m_Timer.Running)
					m_Timer = new InternalTimer(from, this);

				IMount mount = from.Mount;
				if (mount != null)
				{
					mount.Rider = null;
				}

				BaseMount.SetMountPrevention(from, BlockMountType.Dazed, TimeSpan.FromDays(90));
				AnimalFormContext ctx = new AnimalFormContext(new Timer(TimeSpan.Zero), null, true,
					typeof(PrzyspieszenieWilkolaka), null);
				AnimalForm.AddContext(from, ctx);
			}

			return base.OnEquip(from);
		}

		public override void OnRemoved(IEntity parent)
		{
			if (parent != null && parent is Mobile m)
			{
				m.Send(SpeedControl.Disable);
				BaseMount.SetMountPrevention(m, BlockMountType.Dazed, TimeSpan.FromSeconds(10));
				AnimalForm.RemoveContext(m, false);
			}

			if (m_Timer != null && m_Timer.Running)
			{
				m_Timer.Stop();
			}

			base.OnRemoved(parent);
		}

		public PrzyspieszenieWilkolaka(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;
			private PrzyspieszenieWilkolaka m_Ring;

			public InternalTimer(Mobile m, PrzyspieszenieWilkolaka ring) : base(TimeSpan.Zero, TimeSpan.FromSeconds(3))
			{
				m_Mobile = m;
				m_Ring = ring;
				Start();
			}

			protected override void OnTick()
			{
				if (m_Mobile != null && !m_Mobile.Deleted && m_Mobile.Player && m_Mobile.Map != Map.Internal &&
				    m_Ring != null && m_Mobile.Equals(m_Ring.Parent))
				{
					m_Mobile.Hits -= 5;
					m_Mobile.Mana -= 5;
				}
				else
				{
					Stop();
				}
			}
		}
	}
}
