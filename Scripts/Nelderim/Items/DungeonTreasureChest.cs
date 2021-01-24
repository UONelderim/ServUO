using System;

namespace Server.Items
{
	public enum LevelType
	{
		First = 1,
		Second = 2,
		Third = 3,
		Fourth = 4,
		Fifth = 5,
		Sixth = 6
	}

	public class DungeonTreasureChest : LockableContainer
	{
		private LevelType m_Level;

		public override bool Decays { get { return true; } }
		public override TimeSpan DecayTime { get { return TimeSpan.FromMinutes(1440.0); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public LevelType Level
		{
			get
			{
				return m_Level;
			}
			set
			{
				m_Level = value;

				SetRequiments(m_Level);

				ClearContents();

				FillChest(m_Level);

				InvalidateProperties();
			}
		}

		public void SetRequiments(LevelType level)
		{
			switch ( m_Level )
			{
				case LevelType.First:
					{
						RequiredSkill = LockLevel = 52;
						TrapType = TrapType.DartTrap;
						TrapPower = Utility.RandomMinMax(0, 25);
						break;
					}
				case LevelType.Second:
					{
						RequiredSkill = LockLevel = 62;
						TrapType = TrapType.PoisonTrap;
						TrapPower = Utility.RandomMinMax(0, 35);
						break;
					}
				case LevelType.Third:
					{
						RequiredSkill = LockLevel = 72;
						TrapType = TrapType.ExplosionTrap;
						TrapPower = Utility.RandomMinMax(0, 45);
						break;
					}
				case LevelType.Fourth:
					{
						RequiredSkill = LockLevel = 84;
						TrapType = TrapType.PoisonTrap;
						TrapPower = Utility.RandomMinMax(0, 35); ;
						break;
					}
				case LevelType.Fifth:
					{
						RequiredSkill = LockLevel = 92;
						TrapType = TrapType.ExplosionTrap;
						TrapPower = Utility.RandomMinMax(0, 45); ;
						break;
					}
				case LevelType.Sixth:
					{
						RequiredSkill = LockLevel = 100;
						TrapType = TrapType.ExplosionTrap;
						TrapPower = Utility.RandomMinMax(0, 55); ;
						break;
					}
			}
		}

		public static int CalculateType(LevelType level)
		{
			int[] m_ItemIDs = new int[15];

			switch ( level )
			{
				case LevelType.First:
					{
						m_ItemIDs = new int[]
						   {
							0xE7E, 0x9A9, 0xE3E, 0xE3F
						   };
						break;
					}
				case LevelType.Second:
					{
						m_ItemIDs = new int[]
						   {
							0xE3C, 0xE3D
						   };
						break;
					}
				case LevelType.Third:
					{
						m_ItemIDs = new int[]
						   {
							0xE77, 0xE7F
						   };
						break;
					}
				case LevelType.Fourth:
					{
						m_ItemIDs = new int[]
						   {
							0xE43, 0xE42
						   };
						break;
					}
				case LevelType.Fifth:
					{
						m_ItemIDs = new int[]
						   {
							0xE41, 0xE40
						   };
						break;
					}
				case LevelType.Sixth:
					{
						m_ItemIDs = new int[]
						   {
							0x9AB, 0xE7C
						   };
						break;
					}
			}

			int itemID = Utility.RandomList(m_ItemIDs);

			return itemID;
		}

		public void FillChest(LevelType level)
		{
			switch ( level )
			{
				case LevelType.First:
					{
						DropItem(new Gold(Utility.RandomMinMax(50, 100)));

						if ( 1 > Utility.RandomDouble() )
						{
							int chance = Utility.Random(2);

							switch ( chance )
							{
								case 0: DropItem(new Bolt(30)); break;
								case 1: DropItem(new Arrow(30)); break;
							}

							if ( 0.5 > Utility.RandomDouble() )
							{
								Item reagent = Loot.RandomReagent();
								reagent.Amount = Utility.RandomMinMax(10, 15);
								DropItem(reagent);
							}
						}

						break;
					}

				case LevelType.Second:
					{
						DropItem(new Gold(Utility.RandomMinMax(100, 150)));

						if ( 0.7 > Utility.RandomDouble() )
						{
							int chance = Utility.Random(3);

							switch ( chance )
							{
								case 0: DropItem(new Bolt(60)); break;
								case 1: DropItem(new Arrow(60)); break;
								case 2: DropItem(new Sandals()); break;
							}
						}

						if ( 0.5 > Utility.RandomDouble() )
						{
							Item reagent = Loot.RandomReagent();
							reagent.Amount = Utility.RandomMinMax(16, 20);
							DropItem(reagent);
						}

						break;
					}

				case LevelType.Third:
					{
						DropItem(new Gold(Utility.RandomMinMax(150, 250)));

						if ( 0.5 > Utility.RandomDouble() )
						{
							int chance = Utility.Random(2);

							switch ( chance )
							{
								case 0: DropItem(new Bolt(90)); break;
								case 1: DropItem(new Arrow(90)); break;
							}
						}

						if ( 0.5 > Utility.RandomDouble() )
						{
							Item reagent = Loot.RandomReagent();
							reagent.Amount = Utility.RandomMinMax(21, 25);
							DropItem(reagent);
						}

						int chance_new = Utility.Random(16);

						switch ( chance_new )
						{
							case 0:
								{
									BaseWeapon weapon = Loot.RandomWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 1:
								{
									BaseWeapon weapon = Loot.RandomWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 2:
								{
									BaseWeapon weapon = Loot.RandomWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 3:
								{
									BaseWeapon weapon = Loot.RandomWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 4:
								{
									BaseWeapon weapon = Loot.RandomRangedWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 5:
								{
									BaseWeapon weapon = Loot.RandomRangedWeapon();
									BaseRunicTool.ApplyAttributesTo(weapon, 3, 20, 70);
									DropItem(weapon);
									break;
								}
							case 6:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 7:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 8:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 9:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 10:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 11:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo(armor, 3, 20, 70);
									DropItem(armor);
									break;
								}
							case 12:
								{
									BaseHat hat = Loot.RandomHat();
									BaseRunicTool.ApplyAttributesTo(hat, 3, 20, 70);
									DropItem(hat);
									break;
								}
							case 13:
								{
									BaseJewel jewel = Loot.RandomJewelry();
									BaseRunicTool.ApplyAttributesTo(jewel, 3, 20, 70);
									DropItem(jewel);
									break;
								}
							case 14:
								{
									BaseJewel jewel = Loot.RandomJewelry();
									BaseRunicTool.ApplyAttributesTo(jewel, 3, 20, 70);
									DropItem(jewel);
									break;
								}
							case 15:
								{
									BaseJewel jewel = Loot.RandomJewelry();
									BaseRunicTool.ApplyAttributesTo(jewel, 3, 20, 70);
									DropItem(jewel);
									break;
								}

						}


						break;
					}

				case LevelType.Fourth:
					{
						DropItem(new Gold(Utility.RandomMinMax(250, 400)));

						for ( int i = 0; i < 2; i++ )
						{

							int chance_new = Utility.Random(16);

							switch ( chance_new )
							{
								case 0:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 3, 40, 80);
										DropItem(weapon);
										break;
									}
								case 1:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 3, 40, 80);
										DropItem(weapon);
										break;
									}
								case 2:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 40, 80);
										DropItem(weapon);
										break;
									}
								case 3:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 40, 80);
										DropItem(weapon);
										break;
									}
								case 4:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 3, 40, 80);
										DropItem(weapon);
										break;
									}
								case 5:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 40, 80);
										DropItem(weapon);
										break;
									}
								case 6:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 3, 40, 80);
										DropItem(armor);
										break;
									}
								case 7:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 3, 40, 80);
										DropItem(armor);
										break;
									}
								case 8:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 3, 40, 80);
										DropItem(armor);
										break;
									}
								case 9:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 40, 80);
										DropItem(armor);
										break;
									}
								case 10:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 40, 80);
										DropItem(armor);
										break;
									}
								case 11:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 40, 80);
										DropItem(armor);
										break;
									}
								case 12:
									{
										BaseHat hat = Loot.RandomHat();
										BaseRunicTool.ApplyAttributesTo(hat, 4, 40, 80);
										DropItem(hat);
										break;
									}
								case 13:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 3, 40, 80);
										DropItem(jewel);
										break;
									}
								case 14:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 4, 40, 80);
										DropItem(jewel);
										break;
									}
								case 15:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 4, 40, 80);
										DropItem(jewel);
										break;
									}
							}

						}


						if ( 0.5 > Utility.RandomDouble() )
						{
							Item gem = Loot.RandomGem();
							gem.Amount = Utility.RandomMinMax(2, 3);
							DropItem(gem);
						}

						break;
					}

				case LevelType.Fifth:
					{
						DropItem(new Gold(Utility.RandomMinMax(500, 600)));

						for ( int i = 0; i < 3; i++ )
						{
							int chance_new = Utility.Random(16);

							switch ( chance_new )
							{
								case 0:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 60, 100);
										DropItem(weapon);
										break;
									}
								case 1:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 60, 100);
										DropItem(weapon);
										break;
									}
								case 2:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 60, 100);
										DropItem(weapon);
										break;
									}
								case 3:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 60, 100);
										DropItem(weapon);
										break;
									}
								case 4:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 4, 60, 100);
										DropItem(weapon);
										break;
									}
								case 5:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 60, 100);
										DropItem(weapon);
										break;
									}
								case 6:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 60, 100);
										DropItem(armor);
										break;
									}
								case 7:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 60, 100);
										DropItem(armor);
										break;
									}
								case 8:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 4, 60, 100);
										DropItem(armor);
										break;
									}
								case 9:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 60, 100);
										DropItem(armor);
										break;
									}
								case 10:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 60, 100);
										DropItem(armor);
										break;
									}
								case 11:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 60, 100);
										DropItem(armor);
										break;
									}
								case 12:
									{
										BaseHat hat = Loot.RandomHat();
										BaseRunicTool.ApplyAttributesTo(hat, 5, 60, 100);
										DropItem(hat);
										break;
									}
								case 13:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 4, 60, 100);
										DropItem(jewel);
										break;
									}
								case 14:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 5, 60, 100);
										DropItem(jewel);
										break;
									}
								case 15:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 5, 60, 100);
										DropItem(jewel);
										break;
									}
							}
						}

						if ( 0.5 > Utility.RandomDouble() )
						{
							Item gem = Loot.RandomGem();
							gem.Amount = Utility.RandomMinMax(4, 6);
							DropItem(gem);
						}

						break;
					}

				case LevelType.Sixth:
					{
						DropItem(new Gold(Utility.RandomMinMax(700, 800)));

						for ( int i = 0; i < 4; i++ )
						{
							int chance_new = Utility.Random(16);

							switch ( chance_new )
							{
								case 0:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 1:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 2:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 3:
									{
										BaseWeapon weapon = Loot.RandomWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 4:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 5:
									{
										BaseWeapon weapon = Loot.RandomRangedWeapon();
										BaseRunicTool.ApplyAttributesTo(weapon, 5, 80, 100);
										DropItem(weapon);
										break;
									}
								case 6:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 7:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 8:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 9:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 10:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 11:
									{
										BaseArmor armor = Loot.RandomArmor();
										BaseRunicTool.ApplyAttributesTo(armor, 5, 80, 100);
										DropItem(armor);
										break;
									}
								case 12:
									{
										BaseHat hat = Loot.RandomHat();
										BaseRunicTool.ApplyAttributesTo(hat, 5, 80, 100);
										DropItem(hat);
										break;
									}
								case 13:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 5, 80, 100);
										DropItem(jewel);
										break;
									}
								case 14:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 5, 80, 100);
										DropItem(jewel);
										break;
									}
								case 15:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo(jewel, 5, 80, 100);
										DropItem(jewel);
										break;
									}
							}
						}


						if ( 0.5 > Utility.RandomDouble() )
						{
							Item gem = Loot.RandomGem();
							gem.Amount = Utility.RandomMinMax(7, 9);
							DropItem(gem);
						}

						if ( 0.2 > Utility.RandomDouble() )
						{
							//BaseWand wand = FireworksWand();
							AddItem(new FireworksWand());
						}

						break;
					}
			}
		}


		public void ClearContents()
		{
			for ( int i = this.Items.Count - 1; i >= 0; --i )
			{
				if ( i < this.Items.Count )
					((Item)this.Items[i]).Delete();
			}
		}

		public override bool IsDecoContainer
		{
			get { return false; }
		}

		[Constructable]
		public DungeonTreasureChest(LevelType level) : base(CalculateType(level))
		{
			Movable = false;
			Locked = true;

			Level = level;
		}

		public DungeonTreasureChest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Level);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Level = (LevelType)reader.ReadInt();
		}
	}

	public class DungeonTreasureChestFirst : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFirst() : base(LevelType.First)
		{
		}

		public DungeonTreasureChestFirst(Serial serial) : base(serial)
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

	public class DungeonTreasureChestSecond : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestSecond() : base(LevelType.Second)
		{
		}

		public DungeonTreasureChestSecond(Serial serial) : base(serial)
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

	public class DungeonTreasureChestThird : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestThird() : base(LevelType.Third)
		{
		}

		public DungeonTreasureChestThird(Serial serial) : base(serial)
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

	public class DungeonTreasureChestFourth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFourth() : base(LevelType.Fourth)
		{
		}

		public DungeonTreasureChestFourth(Serial serial) : base(serial)
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

	public class DungeonTreasureChestFifth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFifth() : base(LevelType.Fifth)
		{
		}

		public DungeonTreasureChestFifth(Serial serial) : base(serial)
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

	public class DungeonTreasureChestSixth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestSixth() : base(LevelType.Sixth)
		{
		}

		public DungeonTreasureChestSixth(Serial serial) : base(serial)
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
