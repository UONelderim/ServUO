#region References

using System;
using Server;
using Server.Engines.BulkOrders;
using Server.Engines.Points;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Network;

#endregion

namespace Nelderim.Engines.ChaosChest
{
	public class ChaosChest : LockableContainer
	{
		public delegate Item ConstructCallback(int type);

		private static readonly RewardGroup rewardGroup = new RewardGroup(0,
			// new RewardItem(20, ZwojeAttach, 0),
			new RewardItem(12, Pigment, 0),
			new RewardItem(5, Potion, 0),
			new RewardItem(20, Dekor, 0),
			new RewardItem(9, Kolczyki, 0),
			new RewardItem(10, Runik, 0),
			new RewardItem(8, Proszek, 0),
			new RewardItem(7, Runik, 1),
			new RewardItem(3, Art, 0),
			new RewardItem(2, Art, 1),
			new RewardItem(2, Art, 2),
			new RewardItem(2, Art, 3)
		);

		private static Item ZwojeAttach(int type)
		{
			//XmlEnemyMaster na konkretnego bossa 10% szansy 10% wiecej dmg 24h
			return new BankCheck(50000); // Tu wstawic zwoj attacha jak bedzie
		}

		private static Item Pigment(int type)
		{
			return new BasePigment();
		}

		private static Item Potion(int type)
		{
			switch (Utility.Random(2))
			{
				case 1: return new PetBondingPotion();
				default: return new PetResurrectPotion();
			}
		}

		private static Item Dekor(int type)
		{
			switch (Utility.Random(3))
			{
				case 2:
					return new HorseShoes();
				case 1:
					return new ForgedMetal();
				default:
				{
					switch (Utility.Random(4))
					{
						case 3:
							return new IronWire();
						case 2:
							return new CopperWire();
						case 1:
							return new GoldWire();
						default:
							return new SilverWire();
					}
				}
			}
		}

		private static Item Kolczyki(int type)
		{
			BaseJewel earrings;
			switch (Utility.Random(2))
			{
				case 1:
					earrings = new SilverEarrings();
					break;
				default:
					earrings = new GoldEarrings();
					break;
			}

			// Ustawianie resista
			Array resistanceTypes = Enum.GetValues(typeof(AosElementAttribute));
			AosElementAttribute res =
				(AosElementAttribute)resistanceTypes.GetValue(Utility.Random(resistanceTypes.Length));
			earrings.Resistances[res] = 5;

			//Ustawianie mr/hr/sr
			switch (Utility.Random(3))
			{
				case 2:
					earrings.Attributes[AosAttribute.RegenMana] = 2;
					break;
				case 1:
					earrings.Attributes[AosAttribute.RegenHits] = 2;
					break;
				default:
					earrings.Attributes[AosAttribute.RegenStam] = 2;
					break;
			}
			//Dac 2 propsy o srednim natezeniu zamiast regen i resista

			TemporaryQuestObject tqo =
				new TemporaryQuestObject("Bardzo chaotyczne kolczyki", (double)10 * 24 * 60 /*10dni*/);
			XmlAttach.AttachTo(earrings, tqo);

			return earrings;
		}

		private static Item Runik(int type)
		{
			switch (type)
			{
				case 1:
				{
					switch (Utility.Random(3))
					{
						case 2: return new RunicHammer(CraftResource.Valorite, 1);
						case 1: return new RunicSewingKit(CraftResource.BarbedLeather, 1);
						default: return new RunicFletcherTool(CraftResource.Frostwood, 1);
					}
				}
				default:
				{
					switch (Utility.Random(3))
					{
						case 2: return new RunicHammer(CraftResource.Agapite, 3);
						case 1: return new RunicSewingKit(CraftResource.HornedLeather, 3);
						default: return new RunicFletcherTool(CraftResource.Heartwood, 3);
					}
				}
			}
		}

		private static Item Proszek(int type)
		{
			var reward = Utility.RandomList(
				typeof(BlacksmithyPowderOfTemperament), 
				typeof(BowFletchingPowderOfTemperament), 
				typeof(CarpentryPowderOfTemperament),
				typeof(TailoringPowderOfTemperament),
				typeof(TinkeringPowderOfTemperament));
			if (reward != null)
			{
				return (Item)Activator.CreateInstance(reward, 5);
			}

			return null;
		}

		private static Item Art(int type)
		{
			switch (type)
			{
				case 3: return ArtifactHelper.GetRandomArtifact(ArtGroup.Doom);
				case 2: return ArtifactHelper.GetRandomArtifact(ArtGroup.Miniboss);
				case 1: return ArtifactHelper.GetRandomArtifact(ArtGroup.Fishing);
				default: return ArtifactHelper.GetRandomArtifact(ArtGroup.CustomChamp);
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public ChaosSigils UnsealedChaosSigils { get; set; }

		[Constructable]
		public ChaosChest() : base(0x1445)
		{
			Name = "Skrzynia chaosu";
			UnsealedChaosSigils = ChaosSigilType.NONE;
			Locked = true;
			LockLevel = 0;
			GenerateLoot();
		}

		private void GenerateLoot()
		{
			RewardItem rewardItem = rewardGroup.AcquireItem();
			Item item = rewardItem.Construct();
			if (item != null)
				DropItem(item);
			else
				Console.WriteLine("[WARN]No Item for chaosChest: " + Serial);
		}

		public ChaosChest(Serial serial) : base(serial)
		{
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);

			list.Add("Pieczec {0} delikatnie podswietla sie.", ChaosChestQuest.CURRENT_STAGE);

			if (UnsealedChaosSigils.Value != ChaosSigilType.ALL)
			{
				for (int i = 1; i < (int)ChaosSigilType.ALL; i *= 2)
				{
					ChaosSigilType type = (ChaosSigilType)i;
					list.Add("Pieczec {0} jest {1}.", type, UnsealedChaosSigils.Get(type) ? "otwarta" : "zamknieta");
				}
			}
		}

		public bool IsSealed(ChaosKey chaosKey)
		{
			return !UnsealedChaosSigils.Get(chaosKey.ChaosSigilType);
		}

		public void Unseal(ChaosKey chaosKey)
		{
			UnsealedChaosSigils.Set(chaosKey.ChaosSigilType, true);
			PublicOverheadMessage(MessageType.Emote, 0, true, "*klucz blokuje sie w skrzyni*");

			InvalidateProperties();
			if (UnsealedChaosSigils.Value == ChaosSigilType.ALL)
			{
				Locked = false;
				PublicOverheadMessage(MessageType.Emote, 0, true, "*klik*");
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write((int)UnsealedChaosSigils.Value);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			UnsealedChaosSigils = new ChaosSigils((ChaosSigilType)reader.ReadInt());
		}
	}
}
