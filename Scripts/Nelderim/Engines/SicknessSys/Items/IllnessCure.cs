#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys
{
	public class IllnessCure : Item
	{
		private IllnessType IllType { get; set; }

		[Constructable]
		public IllnessCure(VirusCell cell) : base(0xF07)
		{
			IllType = cell.Illness;

			Name = "lek na" + cell.Sickness + "";
			Hue = cell.PM.Hue;
			Weight = 1.0;

			Movable = false;

			if (cell.InDebug && cell.GM != null)
			{
				cell.GM.SendMessage(120, "Stworzone przez : " + Name + " : " + Hue);
			}
		}

		public IllnessCure(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.HasFreeHand())
			{
				Item cell = from.Backpack.FindItemByType(typeof(VirusCell));

				if (cell != null)
				{
					VirusCell Cell = cell as VirusCell;

					if (Cell.Illness == IllType)
					{
						if (Cell.InDebug && Cell.GM != null)
						{
							Cell.GM.SendMessage(120, "Wypijasz miksture");
						}

						SicknessCure.Cure(from as PlayerMobile, Cell);

						Delete();
					}
					else
					{
						from.SendMessage("Jestes zarazony " + Cell.Illness);
					}
				}
				else
				{
					from.SendMessage("Nie jestes zarazony zadna choroba!");
				}
			}
			else
			{
				from.SendMessage("Aby to wypic, musisz miec wolna reke!");
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write((int)IllType);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			IllType = (IllnessType)reader.ReadInt();
		}
	}
}
