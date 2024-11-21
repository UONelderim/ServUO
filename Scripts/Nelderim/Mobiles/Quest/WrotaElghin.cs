#region References

using System;

#endregion

namespace Server.Items
{
	public class WrotaElghin : Moongate
	{
		private static readonly TimeSpan m_DDT = TimeSpan.FromSeconds(300.0); // czas rozpadu w sekundach

		public override int LabelNumber => 1048047; // a Moongate

		[Constructable]
		public WrotaElghin()
		{
			Timer.DelayCall(m_DDT, Delete);
			Name = "Wrota Elghin";
			Hue = 2892;
			Dispellable = false;
			TargetMap = Map.Felucca;
			Target = new Point3D(6350, 11, -1);
		}

		public WrotaElghin(Serial serial)
			: base(serial)
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
