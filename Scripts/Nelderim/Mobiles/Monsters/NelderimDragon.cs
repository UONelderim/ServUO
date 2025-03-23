#region References

using Nelderim;
using Server.Items;
using System;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki smoczyska")]
	public class NelderimDragon : BasePeerless
	{
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;

		[Constructable]
		public NelderimDragon() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("dragon");

			Body = Utility.RandomList(12, 59);
			BaseSoundID = 362;
			Hue = 1034;

			SetStr(1232, 1400);
			SetDex(76, 82);
			SetInt(76, 85);

			SetHits( 12000 );

			SetDamage(35, 37);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 60, 75);
			SetResistance(ResistanceType.Fire, 70, 80);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 45, 50);
			SetResistance(ResistanceType.Energy, 45, 50);

			SetSkill(SkillName.EvalInt, 99.1, 120.0);
			SetSkill(SkillName.Magery, 99.1, 120.0);
			SetSkill(SkillName.MagicResist, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 100.1, 120.0);
			SetSkill(SkillName.Wrestling, 100.1, 120.0);

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.30)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.ClericScrolls);
		}

		public override bool AutoDispel => true;
		public override int TreasureMapLevel => 5;
		public override int Meat => 8;
		public override int Hides => 20;
		public override HideType HideType => HideType.Barbed;
		public override int Scales => 5;
		public override ScaleType ScaleType => (Body == 12 ? ScaleType.Yellow : ScaleType.Red);
		public override FoodType FavoriteFood => FoodType.Meat;
		
		public NelderimDragon(Serial serial) : base(serial)
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
