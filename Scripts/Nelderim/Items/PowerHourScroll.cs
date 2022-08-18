#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class PowerHourScroll : Item
	{
		public override int LabelNumber { get { return 1064800; } } // Zwoj PowerHour

		[Constructable]
		public PowerHourScroll() : base(0x14F0)
		{
			base.Hue = 0x381;
			base.Weight = 1.0;
		}

		public PowerHourScroll(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			PlayerMobile pm = (PlayerMobile)from;

			// if (pm.HasPowerHour)
			// {
			// 	// Player is currently on PowerHour
			// 	pm.SendLocalizedMessage(1064802); // Jestes w trakcie PowerHour!
			// }
			// else if (pm.AllowPowerHour)
			// {
			// 	// Player hasnt used PowerHour
			// 	pm.SendLocalizedMessage(1064801); // Nie zuzyto jeszcze PowerHour tego dnia!
			// }
			// else
			// {
			// 	// Reset PowerHour
			// 	pm.LastPowerHour = DateTime.Now.AddDays(-1);
			// 	pm.SendLocalizedMessage(
			// 		1064803); // Zuzycie PowerHour zostalo zresetowane, mozesz uruchomic je ponownie.
			// 	// Remove scroll
			// 	Delete();
			// }
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

			switch (version)
			{
				case 0:
				{
					break;
				}
			}
		}
	}
}
