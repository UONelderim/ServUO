#region References

using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class UnearthedBones : Container
	{
		readonly Mobile m_From;
		public override bool IsDecoContainer { get { return false; } }

		[Constructable]
		public UnearthedBones()
			: base(Utility.Random(0xECA, 9))
		{
			Name = "wystajace z ziemi kosci";
			DropItem(new Bone());

			AddGold();
			AddLoot();
		}

		[Constructable]
		public UnearthedBones(Mobile from)
			: base(Utility.Random(0xECA, 9))
		{
			Name = "wystajace z ziemi kosci";
			DropItem(new Bone());
			m_From = from;
			AddGold();
			AddLoot();
		}

		public UnearthedBones(Serial serial)
			: base(serial)
		{
		}

		public void AddGold()
		{
			DropItem(new Gold((int)(Utility.RandomDouble() * 100)));
		}

		public void AddItem()
		{
			switch (Utility.Random(5))
			{
				case 0:
					AddWeapon();
					break;
				case 1:
					AddShield();
					break;
				case 2:
					AddRanged();
					break;
				case 3:
					AddArmor();
					break;
				case 4:
					AddJewelry();
					break;
			}
		}

		public void AddLoot()
		{
			int rnd = Utility.RandomMinMax(1, 5);

			for (int i = 0; i < rnd; i++)
			{
				AddItem();
			}
		}


		public void AddWeapon()
		{
			Item weapon = Loot.RandomWeapon();
			AddAttrib(weapon);
		}

		public void AddRanged()
		{
			Item ranged = Loot.RandomRangedWeapon();
			AddAttrib(ranged);
		}

		public void AddShield()
		{
			Item shield = Loot.RandomShield();
			AddAttrib(shield);
		}

		public void AddArmor()
		{
			Item armor = Loot.RandomArmorOrHat();
			AddAttrib(armor);
		}

		public void AddJewelry()
		{
			Item jewelry = Loot.RandomJewelry();
			AddAttrib(jewelry);
		}

		public void AddAttrib(Item item)
		{
			int props = 1 + (Utility.RandomMinMax(0, 2));

			int luckChance;
			if (m_From != null)
				luckChance = m_From is PlayerMobile ? ((PlayerMobile)m_From).Luck : m_From.Luck;
			else
				luckChance = (int)(Utility.RandomDouble() * 100);


			int min = 1;
			int max = 100;

			if (item is BaseWeapon)
				BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, false, luckChance, props, min, max);
			else if (item is BaseArmor)
				BaseRunicTool.ApplyAttributesTo((BaseArmor)item, false, luckChance, props, min, max);
			else if (item is BaseRanged)
				BaseRunicTool.ApplyAttributesTo((BaseRanged)item, false, luckChance, props, min, max);
			else if (item is BaseShield)
				BaseRunicTool.ApplyAttributesTo((BaseShield)item, false, luckChance, props, min, max);
			else if (item is BaseShield)
				BaseRunicTool.ApplyAttributesTo((BaseJewel)item, false, luckChance, props, min, max);


			DropItem(item);
		}

		public void AddGem()
		{
			Item gem = Loot.RandomGem();
			DropItem(gem);
		}

		public override void OnDoubleClick(Mobile m)
		{
			base.OnDoubleClick(m);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
