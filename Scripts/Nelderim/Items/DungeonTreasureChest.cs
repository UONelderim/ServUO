using System;
using Nelderim;

namespace Server.Items
{
	public abstract class DungeonTreasureChest : LockableContainer
	{
		public override bool Decays => true;
		public override TimeSpan DecayTime => TimeSpan.FromMinutes(1440.0);

		public abstract void FillChest();
		public override bool IsDecoContainer => false;

		public DungeonTreasureChest(int itemId) : base(itemId)
		{
			Movable = false;
			Locked = true;
			FillChest();
			InvalidateProperties();
		}

		public DungeonTreasureChest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if (version == 0)
				reader.ReadInt(); //Level
		}
	}

	public class DungeonTreasureChestFirst : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFirst() : base(Utility.RandomList(0xE7E, 0x9A9, 0xE3E, 0xE3F))
		{
			RequiredSkill = LockLevel = 52;
			TrapType = TrapType.DartTrap;
			TrapPower = Utility.RandomMinMax(0, 25);
		}

		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(50, 100)));

			if (Utility.RandomBool())
			{
				DropItem(new Bolt(30));
			}
			else
			{
				DropItem(new Arrow(30));
			}

			if (Utility.RandomBool())
			{
				Item reagent = Loot.RandomReagent();
				reagent.Amount = Utility.RandomMinMax(10, 15);
				DropItem(reagent);
			}
		}

		public DungeonTreasureChestFirst(Serial serial) : base(serial)
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

	public class DungeonTreasureChestSecond : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestSecond() : base(Utility.RandomList(0xE3C, 0xE3D))
		{
			RequiredSkill = LockLevel = 62;
			TrapType = TrapType.PoisonTrap;
			TrapPower = Utility.RandomMinMax(0, 35);
		}

		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(100, 150)));

			if (0.7 > Utility.RandomDouble())
			{
				switch (Utility.Random(3))
				{
					case 0:
						DropItem(new Bolt(60));
						break;
					case 1:
						DropItem(new Arrow(60));
						break;
					case 2:
						DropItem(new Sandals());
						break;
				}
			}

			if (Utility.RandomBool())
			{
				Item reagent = Loot.RandomReagent();
				reagent.Amount = Utility.RandomMinMax(16, 20);
				DropItem(reagent);
			}
		}

		public DungeonTreasureChestSecond(Serial serial) : base(serial)
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

	public class DungeonTreasureChestThird : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestThird() : base(Utility.RandomList(0xE77, 0xE7F))
		{
			RequiredSkill = LockLevel = 72;
			TrapType = TrapType.ExplosionTrap;
			TrapPower = Utility.RandomMinMax(0, 45);
		}


		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(150, 250)));

			if (0.10 > Utility.RandomDouble())
				DropItem(new PowerGeneratorKey());

			if (Utility.RandomBool())
			{
				if (Utility.RandomBool())
				{
					DropItem(new Bolt(90));
				}
				else
				{
					DropItem(new Arrow(90));
				}
			}

			if (Utility.RandomBool())
			{
				Item reagent = Loot.RandomReagent();
				reagent.Amount = Utility.RandomMinMax(21, 25);
				DropItem(reagent);
			}

			AddLoot(_LootPack);
		}

		private static readonly LootPack _LootPack = new(new[]
		{
			new NelderimLootPackEntry(true, true, NelderimLoot.NelderimItems, 100.0, 1, 3, 3, 20, 70, true)
		});

		public DungeonTreasureChestThird(Serial serial) : base(serial)
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

	public class DungeonTreasureChestFourth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFourth() : base(Utility.RandomList(0xE43, 0xE42))
		{
			RequiredSkill = LockLevel = 84;
			TrapType = TrapType.PoisonTrap;
			TrapPower = Utility.RandomMinMax(0, 35);
		}

		public DungeonTreasureChestFourth(Serial serial) : base(serial)
		{
		}

		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(250, 400)));

			if (0.15 > Utility.RandomDouble())
				DropItem(new PowerGeneratorKey());

			AddLoot(_LootPack);
			if (Utility.RandomBool())
			{
				Item gem = Loot.RandomGem();
				gem.Amount = Utility.RandomMinMax(2, 3);
				DropItem(gem);
			}
		}

		private static readonly LootPack _LootPack = new(new[]
		{
			new NelderimLootPackEntry(true, true, NelderimLoot.NelderimItems, 100.0, 2, 3, 4, 40, 80, true)
		});

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

	public class DungeonTreasureChestFifth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFifth() : base(Utility.RandomList(0xE41, 0xE40))
		{
			RequiredSkill = LockLevel = 92;
			TrapType = TrapType.ExplosionTrap;
			TrapPower = Utility.RandomMinMax(0, 45);
		}

		public DungeonTreasureChestFifth(Serial serial) : base(serial)
		{
		}

		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(500, 600)));

			if (0.20 > Utility.RandomDouble())
				DropItem(new PowerGeneratorKey());

			AddLoot(_LootPack);
			if (Utility.RandomBool())
			{
				Item gem = Loot.RandomGem();
				gem.Amount = Utility.RandomMinMax(4, 6);
				DropItem(gem);
			}
		}

		private static readonly LootPack _LootPack = new(new[]
		{
			new NelderimLootPackEntry(true, true, NelderimLoot.NelderimItems, 100.0, 3, 4, 5, 60, 100, true)
		});

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

	public class DungeonTreasureChestSixth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestSixth() : base(Utility.RandomList(0x9AB, 0xE7C))
		{
			RequiredSkill = LockLevel = 100;
			TrapType = TrapType.ExplosionTrap;
			TrapPower = Utility.RandomMinMax(0, 55);
		}

		public DungeonTreasureChestSixth(Serial serial) : base(serial)
		{
		}

		public override void FillChest()
		{
			DropItem(new Gold(Utility.RandomMinMax(700, 800)));

			if (0.25 > Utility.RandomDouble())
				DropItem(new PowerGeneratorKey());

			AddLoot(_LootPack);
			if (Utility.RandomBool())
			{
				Item gem = Loot.RandomGem();
				gem.Amount = Utility.RandomMinMax(7, 9);
				DropItem(gem);
			}

			if (0.2 > Utility.RandomDouble())
			{
				DropItem(new FireworksWand());
			}
		}

		private static readonly LootPack _LootPack = new(new[]
		{
			new NelderimLootPackEntry(true, true, NelderimLoot.NelderimItems, 100.0, 4, 5, 5, 80, 100, true)
		});


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
