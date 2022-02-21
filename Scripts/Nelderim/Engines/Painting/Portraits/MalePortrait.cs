#region References

using System;

#endregion

namespace Server.Items
{
	[FlipableAttribute(0xEA2, 0xEA1)]
	public class MalePortrait : Item
	{
		[Constructable]
		public MalePortrait(string artistName, string subject) : base(0xEA2)
		{
			if (artistName == subject)
			{
				Name = String.Format("Autoportret {0}", artistName, subject);
			}
			else
			{
				Name = String.Format("Portret {0} namalowany przez {1}", artistName, subject);

				Weight = 3.0;
				Hue = 0;
			}
		}

		public MalePortrait(Serial serial) : base(serial)
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
