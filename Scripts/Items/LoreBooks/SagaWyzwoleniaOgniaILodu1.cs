using System;
using Server;

namespace Server.Items
{
	public partial class KsiegaSagaWyzwoleniaOgniaILodu1 : RedBook
	{
		[Constructable]
		public KsiegaSagaWyzwoleniaOgniaILodu1() : base(false)
		{
			//Writable = false;
		}

		// Intended for defined books only
		public KsiegaSagaWyzwoleniaOgniaILodu1(bool writable) : base(0xFF1, writable)
		{
		}

		public KsiegaSagaWyzwoleniaOgniaILodu1(Serial serial) : base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}
	}
}