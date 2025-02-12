using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;


namespace Server.Items
{
	public class GluttonousBlade : Kryss
	{
		private Timer m_Timer;
		private DateTime _ActiveUntil;

		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int MinDamage => 15;
		public override int MaxDamage => 18;

		[Constructable]
		public GluttonousBlade()
		{
			Name = "Krys Przekletego Glodu";
			LootType = LootType.Cursed;

			_ActiveUntil = DateTime.UtcNow + TimeSpan.FromMinutes(10);
			Activate();
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime ActiveUntil
		{
			get => _ActiveUntil;
			set
			{
				_ActiveUntil = value;
				InvalidateProperties();
			}
		}

		private TimeSpan TimeLeft => ActiveUntil - DateTime.UtcNow;

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (TimeLeft <= TimeSpan.Zero)
			{
				list.Add("[Jestem głodny!]");
				return;
			}

			var hours = (int)TimeLeft.TotalHours;
			var minutes = TimeLeft.Minutes;

			list.Add($"[Bede jeszcze najedzony: {hours} dni i {minutes} godzin ziemi Nelderim]");
		}

		public override void OnHit(Mobile attacker, IDamageable defender, double damageBonus)
		{
			if (defender is BaseCreature && 0.03 > Utility.RandomDouble())
			{
				defender.Hits -= 50;
				defender.PlaySound(1067);

				attacker.SendMessage("Klątwa ostrza zaczyna się uaktywniać.");
				attacker.Hits -= 15;
				attacker.Mana -= 15;
				attacker.Stam -= 15;
				attacker.LocalOverheadMessage(MessageType.Regular, 0x20, true, "DAJ MI ZŁOTA!!!");
			}

			base.OnHit(attacker, defender, damageBonus);
		}

		private void Activate()
		{
			m_Timer?.Stop();

			Name = "Krys Przekletego Glodu";
			Hue = 2675;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 45;
			Attributes.DefendChance = -10;
			Attributes.SpellDamage = 3;
			WeaponAttributes.HitFireball = 30;
			WeaponAttributes.HitLeechMana = 35;
			WeaponAttributes.HitLightning = 10;
			Attributes.BonusStr = 3;
			Attributes.BonusDex = 3;
			Attributes.BonusInt = 3;

			m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), Slice);
		}

		private void Deactivate()
		{
			Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
				0x3728,
				8,
				20,
				5042);
			Effects.PlaySound(Location, Map, 0x201);

			m_Timer?.Stop();

			Name = "Nienazarty krys";
			Hue = 1287;
			Attributes.AttackChance = 0;
			Attributes.WeaponDamage = 0;
			Attributes.DefendChance = 0;
			WeaponAttributes.HitLeechMana = 0;
			Attributes.SpellDamage = 0;
			WeaponAttributes.HitFireball = 0;
			WeaponAttributes.HitLightning = 0;
			Attributes.BonusStr = 0;
			Attributes.BonusDex = 0;
			Attributes.BonusInt = 0;
		}

		public virtual void Slice()
		{
			if (TimeLeft <= TimeSpan.Zero)
			{
				Deactivate();
				return;
			}

			InvalidateProperties();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
			}
			else
			{
				from.SendMessage("Nakarm mnie!!!");
				from.BeginTarget(1, false, TargetFlags.None, OnTarget);
			}
		}

		private void OnTarget(Mobile from, object target)
		{
			if (target is Gold gold)
			{
				ActiveUntil = DateTime.UtcNow + TimeSpan.FromMinutes(gold.Amount / 100.0);
				from.SendMessage($"Bron konsumuje {gold.Amount} centarow");
				from.PlaySound(1073);
				Activate();
				gold.Delete();
			}
			else
			{
				from.PlaySound(1094);
				from.Say("Obrzydlistwo... To nie jest zloto!");
			}
		}

		public GluttonousBlade(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1);
			writer.Write(TimeLeft);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			var timeLeft = reader.ReadTimeSpan();
			_ActiveUntil = DateTime.UtcNow + timeLeft;

			if (timeLeft > TimeSpan.Zero)
				Activate();
		}
	}
}
