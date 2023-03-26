#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class BlacksmithyPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefBlacksmithy.CraftSystem;

		[Constructable]
		public BlacksmithyPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public BlacksmithyPowderOfTemperament(int uses) : base(uses)
		{
		}

		public BlacksmithyPowderOfTemperament(Serial serial) : base(serial)
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
