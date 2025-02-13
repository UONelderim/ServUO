#region References

using System;
using Server;

#endregion

namespace Nelderim
{
	public class LabelsInfo : NExtensionInfo
	{
		public LabelsInfo()
		{
			Labels = new string[5];
		}

		public string[] Labels { get; set; }

		public string ModifiedBy { get; set; }

		public DateTime ModifiedDate { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 );
			writer.Write(Labels.Length);
			foreach (string label in Labels)
				writer.Write(label);

			writer.Write(ModifiedBy);
			writer.Write(ModifiedDate);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			int labels_count = reader.ReadInt();
			Labels = new string[labels_count];
			for (int j = 0; j < labels_count; j++)
				Labels[j] = reader.ReadString();

			ModifiedBy = reader.ReadString();
			ModifiedDate = reader.ReadDateTime();
		}
	}
}
