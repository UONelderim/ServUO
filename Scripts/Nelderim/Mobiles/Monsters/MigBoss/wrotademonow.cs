#region References

using System;

#endregion

namespace Server.Items
{
	public class wrotademonow : Moongate
	{
		private static readonly TimeSpan m_DDT = TimeSpan.FromSeconds(60.0); // czas rozpadu w sekundach

		public override int LabelNumber { get { return 1048047; } } // a Moongate

		[Constructable]
		public wrotademonow()
		{
			Timer.DelayCall(m_DDT, Delete);
			Name = "Wrota Demonow";
			Hue = 0x455;
			Dispellable = false;

			TargetMap = Map.Felucca;

			Target = new Point3D(5992, 687, 0);
		}

		public wrotademonow(Serial serial) : base(serial)
		{
		}


		public override bool OnMoveOver(Mobile m)
		{
			base.OnMoveOver(m);

			if (!ValidateUse(m, false))
			{
				return false;
			}

			return true;
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
