#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	public class BlacksmihyPowderOfTemperament : SpecializedPowderOfTemperament
	{
		public override CraftSystem CraftSystem => DefBlacksmithy.CraftSystem;

		[Constructable]
		public BlacksmihyPowderOfTemperament() : this(5)
		{
		}

		[Constructable]
		public BlacksmihyPowderOfTemperament(int uses) : base(uses)
		{
		}

		public BlacksmihyPowderOfTemperament(Serial serial) : base(serial)
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
