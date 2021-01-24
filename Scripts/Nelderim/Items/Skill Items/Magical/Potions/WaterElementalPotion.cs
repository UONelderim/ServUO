using System;
using Server.Network;
using Server;
using Server.Targets;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;
namespace Server.Items
{
	public class WaterElementalPotion : BaseElementalPotion
	{
		private static Type m_CreatureType = typeof(SummonedWaterElemental);

		private static int[] m_LandTiles = new int[] {
			0x00A8, 0x00A9, 0x0136,
			0x00AA, 0x00AB, 0x0137
		};

		private static int[] m_ItemIDs = new int[] {
			0x1787, 6076,
			13421, 13528, 0x1F9D
		};

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

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

}
