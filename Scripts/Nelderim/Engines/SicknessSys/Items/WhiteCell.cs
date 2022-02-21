#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Items
{
	public class WhiteCell : Item
	{
		public PlayerMobile PM { get; set; }
		public int ViralResistance { get; set; }

		public int DefaultBody { get; set; }
		public int DefaultBodyHue { get; set; }

		public int DefaultHairHue { get; set; }
		public int DefaultFacialHue { get; set; }

		[Constructable]
		public WhiteCell(Mobile pm, VirusCell cell) : base(0xF13)
		{
			PM = pm as PlayerMobile;
			ViralResistance = 0;

			Name = pm.Name + "'s White Cell";

			Hue = 1153;

			Stackable = false;
			Visible = false;
			Movable = false;

			DefaultBody = cell.DefaultBody;
			DefaultBodyHue = cell.DefaultBodyHue;

			DefaultHairHue = cell.DefaultHairHue;
			DefaultFacialHue = cell.DefaultFacialHue;

			LootType = LootType.Blessed;
		}

		public WhiteCell(Serial serial) : base(serial)
		{
		}

		public override double DefaultWeight
		{
			get
			{
				return 0.0;
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			PM.Say("*" + ViralResistance + "*");

			base.OnDoubleClick(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(PM);
			writer.Write(ViralResistance);

			writer.Write(DefaultBody);
			writer.Write(DefaultBodyHue);

			writer.Write(DefaultHairHue);
			writer.Write(DefaultFacialHue);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			PM = reader.ReadMobile() as PlayerMobile;
			ViralResistance = reader.ReadInt();

			DefaultBody = reader.ReadInt();
			DefaultBodyHue = reader.ReadInt();

			DefaultHairHue = reader.ReadInt();
			DefaultFacialHue = reader.ReadInt();
		}
	}
}
