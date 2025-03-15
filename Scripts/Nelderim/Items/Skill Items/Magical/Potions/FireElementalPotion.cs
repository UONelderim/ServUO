#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class FireElementalPotion : BaseElementalPotion
	{
		private static readonly Type m_CreatureType = typeof(SummonedFireElemental);

		private static readonly int[] m_LandTiles = { 500, 503, };

		private static readonly int[] m_ItemIDs =
		{
			0x12EE, 0x134D, 0x136E, 0x136E, 0x137E, 0x137E, 0x1380, 0x1380, 0x1382, 0x1382, 0x1A19, 0x1A7E, 0x2AE4,
			0x2AEB, 0x2B3E, 0x2B65, 0x3286, 0x32B1, 0x343B, 0x346C, 0x3546, 0x3561, 0x01F4, 0x01F5, 0x01F6, 0x01F7,
			0xA12, 0xA0F, 0xF0D, 0x398C /*potka pozogi*/
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
		public FireElementalPotion()
			: base(PotionEffect.FireElemental)
		{
			Weight = 0.5;
			Movable = true;
			Hue = 38;
			Name = "Mikstura Zywiolaka Ognia";
		}

		public FireElementalPotion(Serial serial)
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
