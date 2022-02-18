#region References

using System;

#endregion

namespace Server.Items
{
	public class MaskOfDeathPotion : BaseMaskOfDeathPotion
	{
		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(45.0); } }
		public override double Delay { get { return 30.0; } }

		public override int LabelNumber { get { return 1072100; } } // a Mask of Death potion

		[Constructable]
		public MaskOfDeathPotion() : base(PotionEffect.MaskOfDeath)
		{
			Weight = 0.5;
		}

		public MaskOfDeathPotion(Serial serial) : base(serial)
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
	}
}
