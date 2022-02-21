#region References

using System;
using System.Threading.Tasks;
using Server.Mobiles;
using Server.OneTime.Events;

#endregion

namespace Server
{
	class StickyGrass : Item
	{
		private int Time = Utility.Random(1, 3);

		private int Count { get; set; }
		private int OrgZ { get; set; }

		private bool Update;
		private int NewID { get; set; }
		private int NewHue { get; set; }
		private int NewZ { get; set; }

		private bool WillDelete { get; set; }

		[Constructable]
		public StickyGrass()
		{
			OneTimeSecEvent.SecTimerTick += TimerTick;

			Count = 0;
			OrgZ = 999;

			Update = false;

			switch (Utility.Random(2))
			{
				case 0:
					ItemID = 3378;
					break;
				case 1:
					ItemID = 3379;
					break;
			}

			NewID = ItemID;
			NewHue = 0;
		}

		public StickyGrass(Serial serial) : base(serial)
		{
		}

		public override void Delete()
		{
			if (!WillDelete)
			{
				Visible = false;

				WillDelete = true;
			}
			else
			{
				base.Delete();
			}
		}

		private void TimerTick(object sender, EventArgs e)
		{
			if (Visible)
			{
				if (OrgZ == 999)
				{
					OrgZ = Z;
					NewZ = Z;
				}

				if (Update)
				{
					Update = false;
					ItemID = NewID;
					Hue = NewHue;
					Z = NewZ;
				}

				Task.Factory.StartNew(() => RunCheckAsync());
			}
			else
			{
				if (Z != OrgZ)
					Z = OrgZ;
			}
		}

		private void RunCheckAsync()
		{
			bool IsPlayer = false;

			IPooledEnumerable<Mobile> Mobs = GetMobilesInRange(3);

			if (Mobs != null)
			{
				foreach (Mobile mob in Mobs)
				{
					if (mob is PlayerMobile)
					{
						bool GoodLoad = true;

						if (X == mob.X)
						{
							if (Y == mob.Y)
								GoodLoad = false;
						}

						if (GoodLoad)
							IsPlayer = true;
					}
				}

				Mobs.Free();
			}

			if (Count >= Time)
			{
				if (IsPlayer)
				{
					if (ItemID == 3378)
						NewID = 3379;
					else
						NewID = 3378;

					int Grab = Utility.Random(1, 3);

					if (Grab == 1)
					{
						if (Z > OrgZ)
						{
							NewZ--;
							NewHue = 0;
						}
						else
						{
							NewZ++;
							NewHue = 67;
						}
					}
				}
				else
				{
					if (Z > OrgZ)
					{
						NewZ--;
						NewHue = 0;
					}
				}

				Update = true;

				Count = 0;
			}

			Count++;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(Time);

			writer.Write(OrgZ);

			writer.Write(WillDelete);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			Time = reader.ReadInt();

			OrgZ = reader.ReadInt();

			WillDelete = reader.ReadBool();

			Time = Utility.Random(1, 3);

			Count = 0;

			Update = false;
			NewID = ItemID;
			NewHue = Hue;
			NewZ = Z;

			if (!Deleted)
			{
				if (!WillDelete)
					OneTimeSecEvent.SecTimerTick += TimerTick;
			}
		}
	}
}
