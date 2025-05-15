#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki prastarego ognistego smoka")]
	public class PrastaryOgnistySmok : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }

		[Constructable]
		public PrastaryOgnistySmok() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.2, 0.4)
		{
			Name = "prastary ognisty smok";

			Body = 46;
			BaseSoundID = 362;
			Hue = 1070;

			SetStr(1096, 1125);
			SetDex(116, 135);
			SetInt(536, 575);

			SetHits(878, 995);

			SetDamage(25, 35);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 60, 75);
			SetResistance(ResistanceType.Fire, 90, 100);
			SetResistance(ResistanceType.Cold, 35, 50);
			SetResistance(ResistanceType.Poison, 55, 65);
			SetResistance(ResistanceType.Energy, 55, 65);

			SetSkill(SkillName.EvalInt, 80.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 120.0);
			SetSkill(SkillName.MagicResist, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 100.1, 120.0);
			SetSkill(SkillName.Wrestling, 100.1, 120.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 65;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new RedDragonsHeart());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DragonsBlood());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new VolcanicAsh());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich);
			AddLoot(LootPack.Gems, 4);
			AddLoot(LootPack.LootItem<PazurStarozytnegoOgnistegoSmoka>(50.0));
		}

		public override bool AutoDispel => false;
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 8; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public PrastaryOgnistySmok(Serial serial) : base(serial)
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
		}
	}
}
