#region References

using System.Collections.Generic;
using CPA = Server.CommandPropertyAttribute;

#endregion


namespace Server.Mobiles
{
	public class CleanNPC : BaseVendor
	{
		[Constructable]
		public CleanNPC() : base("")
		{
		}

		public override void InitOutfit()
		{
		}

		public CleanNPC(Serial serial) : base(serial)
		{
		}
		
		protected override List<SBInfo> SBInfos { get; }
		public override void InitSBInfo()
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
