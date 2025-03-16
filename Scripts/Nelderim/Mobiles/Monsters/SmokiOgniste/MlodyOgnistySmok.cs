#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki mlodego ognistego smoka")]
	public class MlodyOgnistySmok : BaseCreature
	{
		[Constructable]
		public MlodyOgnistySmok() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "mlody ognisty smok";

			Body = 61;
			BaseSoundID = 362;
			Hue = Utility.RandomList(0, 1568, 1569, 1570, 1571, 1572, 1071);

			SetStr(401, 430);
			SetDex(133, 152);
			SetInt(101, 140);

			SetHits(241, 258);

			SetDamage(10, 15);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Fire, 60);

			SetResistance(ResistanceType.Physical, 45, 50);
			SetResistance(ResistanceType.Fire, 70, 80);
			SetResistance(ResistanceType.Cold, 15, 30);
			SetResistance(ResistanceType.Poison, 20, 30);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 65.1, 80.0);
			SetSkill(SkillName.Tactics, 65.1, 90.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 46;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 88.3;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new DragonsBlood());
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new VolcanicAsh());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 1);
			AddLoot(LootPack.MageryRegs, 3);
		}

		public override int TreasureMapLevel { get { return 2; } }
		public override int Meat { get { return 4; } }
		public override int Hides { get { return 5; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 2; } }
		public override ScaleType ScaleType { get { return (Body == 60 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

		public MlodyOgnistySmok(Serial serial) : base(serial)
		{
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

			if (Hue == 2586)
			{
				Hue = 1071;
			}
		}
	}
}
