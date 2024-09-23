namespace Server.Items
{

	public enum TobaccoFlavour
	{
		None = 0,
		Apple,
		Pear,
		Lemon,
	}

	public abstract class BaseTobaccoFlavoured : BaseTobacco
	{
		private TobaccoFlavour m_Flavour;
		public TobaccoFlavour Flavour
		{
			get
			{
				return m_Flavour;
			}
			set
			{
				m_Flavour = value;
				InvalidateProperties();
			}
		}

		static protected int SmokeHue(TobaccoFlavour flavour)
		{
			switch (flavour)
			{
				case TobaccoFlavour.None: return 0;
				case TobaccoFlavour.Apple: return 41;
				case TobaccoFlavour.Pear: return 51;
				case TobaccoFlavour.Lemon: return 55;
				default: return 0;
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			switch (Flavour)
			{
				case TobaccoFlavour.None:
					break;
				case TobaccoFlavour.Apple:
					list.Add(1061201, "jablkiem"); // Aromatyzowany ~1_val~
					break;
				case TobaccoFlavour.Pear:
					list.Add(1061201, "gruszka"); // Aromatyzowany ~1_val~
					break;
				case TobaccoFlavour.Lemon:
					list.Add(1061201, "cytryna"); // Aromatyzowany ~1_val~
					break;
				default:
					list.Add(1061201, "czyms dziwnym"); // Aromatyzowany ~1_val~
					break;
			}
		}

		//public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, Type typeRes2, BaseTool tool, CraftItem craftItem, int resHue)
		//{
		//    TobaccoFlavour tf = TobaccoFlavour.None;
		//    foreach (CraftRes res in craftItem.Ressources)
		//    {
		//        if (res.ItemType == typeof(Apple))
		//            tf = TobaccoFlavour.Apple;
		//        else if (res.ItemType == typeof(Pear))
		//            tf = TobaccoFlavour.Pear;
		//        else if (res.ItemType == typeof(Lemon))
		//            tf = TobaccoFlavour.Lemon;

		//        if (tf != TobaccoFlavour.None)
		//            break;
		//    }

		//    Flavour = tf;

		//    return 1; // regular quality
		//}

		public BaseTobaccoFlavoured() : base()
		{
		}

		public BaseTobaccoFlavoured(int amount) : base(amount)
		{
		}

		public BaseTobaccoFlavoured(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write((int)Flavour);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			Flavour = (TobaccoFlavour)reader.ReadInt();
		}
	}

}