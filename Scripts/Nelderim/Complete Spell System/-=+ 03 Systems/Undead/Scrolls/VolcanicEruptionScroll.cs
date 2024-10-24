namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadVolcanicEruptionScroll : CSpellScroll
	{
		[Constructable]
		public UndeadVolcanicEruptionScroll() : this(1)
		{
		}

		[Constructable]
		public UndeadVolcanicEruptionScroll(int amount) : base(typeof(UndeadVolcanicEruptionSpell), 0xE39, amount)
		{
			Name = "Erupcja Wulkaniczna";
			Hue = 38;
		}

		public UndeadVolcanicEruptionScroll(Serial serial) : base(serial)
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
