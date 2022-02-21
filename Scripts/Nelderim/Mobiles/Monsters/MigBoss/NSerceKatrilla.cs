namespace Server.Items
{
	public class NSerceKatrilla : Item
	{
		//private static TimeSpan m_DDT = TimeSpan.FromMinutes( 1440.0 ); // Czas rozpadu w minutach

		[Constructable]
		public NSerceKatrilla() : base(0x1CED)
		{
			//Timer.DelayCall( m_DDT, new TimerCallback( Delete) ); 
			Name = "Spowite Krwia Serce Wielkiego Ksiecia Demonow Katrilla";
			Weight = 1.0;
			Hue = 0x676;
			LootType = LootType.Cursed;
		}

		public NSerceKatrilla(Serial serial) : base(serial)
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
