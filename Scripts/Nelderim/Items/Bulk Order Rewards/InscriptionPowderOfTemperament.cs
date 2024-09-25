#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class InscriptionPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefInscription.CraftSystem;

		[Constructable]
		public InscriptionPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public InscriptionPowderOfTemperament(int uses) : base(uses)
		{
			Name = "Proszek wzmocnienia wyrobow skryby";
			Hue = 1083;
		}

		public InscriptionPowderOfTemperament(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
