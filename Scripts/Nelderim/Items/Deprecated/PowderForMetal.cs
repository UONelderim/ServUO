namespace Server.Items
{
	public class PowderForMetal : Item, IUsesRemaining
	{
		private int m_UsesRemaining;

		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining {
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		public PowderForMetal() : this(5) {
		}

		public PowderForMetal(int charges) : base(4102) {
			Name = "Proszek wzmocnienia metalu";
			Weight = 1.0;
			Hue = 2419;
			UsesRemaining = charges;
		}

		public PowderForMetal(Serial serial) : base(serial) {
		}

		public override void Serialize(GenericWriter writer) {
			base.Serialize(writer);

			writer.Write((int)0);
			writer.Write((int)m_UsesRemaining);
		}

		public override void Deserialize(GenericReader reader) {
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version) {
				case 0: {
						m_UsesRemaining = reader.ReadInt();
						break;
					}
			}
			ReplaceWith(new BlacksmithyPowderOfTemperament(m_UsesRemaining));
		}
	}
}
