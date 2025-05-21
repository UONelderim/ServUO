#region References

using System;
using Nelderim.Gains;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class GainBooster : Item
	{
		private double _GainFactor;
		private TimeSpan _Duration;
		public override int LabelNumber => 1064800; // Gain Booster

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double GainFactor
		{
			get => _GainFactor;
			set
			{
				_GainFactor = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public TimeSpan Duration
		{
			get => _Duration;
			set
			{
				_Duration = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public GainBooster() : this(2.0, 60)
		{
		}

		[Constructable]
		public GainBooster(double factor) : this(factor, 60)
		{
		}

		
		[Constructable]
		public GainBooster(double factor, int minutes) : base(0x14F0)
		{
			Hue = 897;
			Weight = 1.0;
			GainFactor = factor;
			Duration = TimeSpan.FromMinutes(minutes);
		}

		public GainBooster(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			PlayerMobile pm = (PlayerMobile)from;
			if (Gains.Get(pm).ActivateGainBoost(GainFactor, Duration))
			{
				from.SendMessage(0x40, $"Aktywowales Gain Booster o mnożniku x{GainFactor} który będzie trwać do {DateTime.Now + Duration}");
				Delete();
			}
			else
				from.SendMessage("Masz już aktywny Gain Booster");
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			
			list.Add("Mnożnik przyrostów: x{0}", GainFactor); // potions remaining: ~1_val~
			list.Add("Czas działania: {0}", Duration); // potions remaining: ~1_val~
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			
			writer.Write(GainFactor);
			writer.Write(Duration);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					GainFactor = reader.ReadDouble();
					Duration = reader.ReadTimeSpan();
					break;
				}
			}
		}
	}
}
