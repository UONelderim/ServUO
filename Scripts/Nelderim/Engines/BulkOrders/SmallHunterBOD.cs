#region References

using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Engines.BulkOrders
{
	public class SmallHunterBOD : SmallBOD
	{
		public override BODType BODType => BODType.Hunter;

		private static readonly TimeSpan m_HuntProtection = TimeSpan.FromSeconds(15.0);

		//Since we don't use material, we can store collected points in it :)
		public override BulkMaterialType Material => (BulkMaterialType)CollectedPoints;
		
		private static double ScalePoints(double difficulty)
		{
			return Math.Pow(difficulty, 0.625);
		}

		private double _CollectedPoints;

		[CommandProperty(AccessLevel.GameMaster)]
		public double CollectedPoints
		{
			get => _CollectedPoints;
			set
			{
				_CollectedPoints = value;
				InvalidateProperties();
			}
		}

		public override int ComputeFame()
		{
			return 0;
		}

		public override int ComputeGold()
		{
			return HunterRewardCalculator.Instance.ComputeGold(this);
		}

		public override List<Item> ComputeRewards(bool full)
		{
			var list = new List<Item>();

			var rewardGroup =
				HunterRewardCalculator.Instance.LookupRewards(HunterRewardCalculator.Instance.ComputePoints(this));

			if (rewardGroup == null) return list;
			if (full)
			{
				foreach (var reward in rewardGroup.Items)
				{
					var item = reward.Construct();

					if (item != null)
						list.Add(item);
				}
			}
			else
			{
				var item = rewardGroup.AcquireItem()?.Construct();

				if (item != null)
					list.Add(item);
			}

			return list;
		}

		public static SmallHunterBOD CreateRandomFor(Mobile m, double theirSkill)
		{
			SmallBulkEntry[] entries;
			double easyChance;
			double mediumChance;

			switch (theirSkill)
			{
				case >= 105.1:
					easyChance = 0.22; 
					mediumChance = 0.68;
					break;
				case >= 90.1:
					easyChance = 0.26;
					mediumChance = 0.8; 
					break;
				case >= 70.1:
					easyChance = 0.78; 
					mediumChance = 1.0;
					break;
				default:
					easyChance = 1.0;
					mediumChance = 0.0;
					break;
			}

			var rand = Utility.RandomDouble();

			if (easyChance >= rand)
				entries = SmallBulkEntry.Easy;
			else if (mediumChance >= rand)
				entries = SmallBulkEntry.Medium;
			else
				entries = SmallBulkEntry.Hard;

			if (entries.Length <= 0) return null;

			var amountMax = theirSkill switch
			{
				>= 70.1 => Utility.RandomList(10, 15, 20, 20),
				>= 50.1 => Utility.RandomList(10, 15, 15, 20),
				_ => Utility.RandomList(10, 10, 15, 20)
			};

			var entry = Utility.RandomList(entries);

			if (entry != null)
				return new SmallHunterBOD(entry, amountMax);

			return null;
		}

		private SmallHunterBOD(SmallBulkEntry entry, int amountMax)
		{
			Hue = 0xA8E;
			AmountMax = amountMax;
			Type = entry.Type;
			Graphic = entry.Graphic;
			Number = entry.Number;
		}

		[Constructable]
		public SmallHunterBOD()
		{
			var entries = Utility.RandomList(1, 2, 3) switch
			{
				3 => SmallBulkEntry.Hard,
				2 => SmallBulkEntry.Medium,
				_ => SmallBulkEntry.Easy
			};

			if (entries.Length <= 0) return;
			
			var entry = Utility.RandomList(entries);

			Hue = 0xA8E;
			AmountMax =  Utility.RandomList(10, 15, 20);
			Type = entry.Type;
			Number = entry.Number;
			Graphic = entry.Graphic;
		}

		public SmallHunterBOD(int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, BulkMaterialType mat, int hue)
		{
			Hue = 0xA8E;
			AmountMax = amountMax;
			AmountCur = amountCur;
			Type = type;
			Number = number;
			Graphic = graphic;
			RequireExceptional = reqExceptional;
			CollectedPoints = (double)mat;
			GraphicHue = hue;
		}
		
		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			
			list.Add(1060658, "{0}\t{1}", "Zebrane punkty", $"{CollectedPoints:F2}"); // ~1_val~: ~2_val~
		}

		public SmallHunterBOD(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write(_CollectedPoints);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			_CollectedPoints = reader.ReadDouble();
		}

		public override void EndCombine(Mobile from, object o)
		{
			if (o is Corpse corpse && corpse.Owner is BaseCreature bc)
			{
				var mobType = corpse.Owner.GetType();

				if (AmountCur >= AmountMax)
				{
					from.SendLocalizedMessage(
						1045166); // The maximum amount of requested items have already been combined to this deed.
				}
				else if (Type == null || (mobType != Type && !mobType.IsSubclassOf(Type)))
				{
					from.SendLocalizedMessage(1045169); // The item is not in the request.
				}
				else if (bc.IsChampionSpawn || bc.Summoned)
				{
					from.SendMessage("Te zwłoki nie mogą zostać oddane.");
				}
				else if (bc.CollectedByHunter)
				{
					from.SendMessage("Te zwłoki zostaly juz oddane.");
				}
				else if (!CanBeCollected(from, corpse))
				{
					from.SendMessage("Te zwłoki naleza do kogos innego.");
				}
				else
				{
					bc.CollectedByHunter = true;
					++AmountCur;

					from.SendLocalizedMessage(1045170); // The item has been combined with the deed.
					from.SendGump(new SmallBODGump(from, this));
					_CollectedPoints += ScalePoints(bc.Difficulty / AmountMax);

					if (AmountCur < AmountMax)
						BeginCombine(from);
				}
			}
			else
			{
				from.SendMessage("Te zwłoki są zbyt stare, żebyś mógł je dodać do zamówienia.");
			}
		}

		public bool CanBeCollected(Mobile from, Corpse c)
		{
			if (from is PlayerMobile && c?.Owner is BaseCreature mob)
			{
				if (c.TimeOfDeath + m_HuntProtection > DateTime.Now)
				{
					var maxDamage = mob.GetLootingRights().Highest(r => r.m_Damage);
					return from == maxDamage.m_Mobile;
				}
				return true;
			}
			return false;
		}
	}
}
