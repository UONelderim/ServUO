#region References

using System;

#endregion

namespace Server.Items
{
	public class NGreaterStrengthPotion : BaseStrengthPotion
	{
		public override int StrOffset { get { return 20; } }

		public override TimeSpan Duration { get { return TimeSpan.FromMinutes(20.0); } }

		public override int LabelNumber { get { return 1072002; } }

		[Constructable]
		public NGreaterStrengthPotion() : base(PotionEffect.NStrengthGreater)
		{
			Name = "potężna mikstura siły";
			Stackable = true;
			Weight = 0.5;
		}

		public NGreaterStrengthPotion(Serial serial) : base(serial)
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
