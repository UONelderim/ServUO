#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class WaterElementalPotion : BaseElementalPotion
	{
		private static readonly Type m_CreatureType = typeof(SummonedWaterElemental);

		private static readonly int[] m_LandTiles = { 0x00A8, 0x00A9, 0x0136, 0x00AA, 0x00AB, 0x0137 };

		private static readonly int[] m_ItemIDs = { 0x1787, 6076, 13421, 13528, 0x1F9D, 0x99B  /*dzbanki z woda*/, 0x0E7B /*beczka z woda od dostawcy*/  };

		public override Type CreatureType { get { return m_CreatureType; } }

		public override int[] LandTiles
		{
			get
			{
				return m_LandTiles;
			}
		}

		public override int[] ItemIDs
		{
			get
			{
				return m_ItemIDs;
			}
		}

		[Constructable]
		public WaterElementalPotion()
			: base(PotionEffect.WaterElemental)
		{
			Weight = 0.5;
			Movable = true;
			Hue = 88;
			Name = "Mikstura Zywiolaka Wody";
		}

		public WaterElementalPotion(Serial serial)
			: base(serial)
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
