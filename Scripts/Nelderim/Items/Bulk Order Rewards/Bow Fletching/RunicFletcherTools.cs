#region References

using Server.Engines.Craft;

#endregion

namespace Server.Items
{
	[FlipableAttribute(0x13E4, 0x13E3)]
	public class RunicFletcherTools : BaseRunicTool
	{
		public override CraftSystem CraftSystem { get { return DefBowFletching.CraftSystem; } }

		public override int LabelNumber
		{
			get
			{
				int index = CraftResources.GetIndex(Resource);

				if (index >= 1 && index <= 6)
					return 3000282 + index;

				return 3000282; // runiczne narzedzia lukmistrza
			}
		}

		public RunicFletcherTools(CraftResource resource) : base(resource, 4130)
		{
			Weight = 4.0;
			Hue = CraftResources.GetHue(resource);
		}

		public RunicFletcherTools(CraftResource resource, int uses) : base(resource, uses, 4130)
		{
			Weight = 4.0;
			Hue = CraftResources.GetHue(resource);
		}

		public RunicFletcherTools(Serial serial) : base(serial)
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
			
			ReplaceWith(new RunicFletcherTool(Resource, UsesRemaining));
		}
	}
}
