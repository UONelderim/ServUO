#region References

using Nelderim;
using Server.Items;
using System;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza saraga")]
	public class NSarag : BasePeerless
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Lethal;

		[Constructable]
		public NSarag() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Body = 308;
			Hue = 1182;
			Name = "sarag - przedwieczny nieumarly";

			BaseSoundID = 0x48D;

			SetStr(900, 1000);
			SetDex(120, 130);
			SetInt(500, 500);
			SetHits(10000);

			SetDamage(20, 30);

			SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Cold, 70);

			SetResistance(ResistanceType.Physical, 70);
			SetResistance(ResistanceType.Fire, 70);
			SetResistance(ResistanceType.Cold, 70);
			SetResistance(ResistanceType.Poison, 70);
			SetResistance(ResistanceType.Energy, 70);

			SetSkill(SkillName.MagicResist, 110.0);
			SetSkill(SkillName.Tactics, 110.0);
			SetSkill(SkillName.Wrestling, 110.0);
			SetSkill(SkillName.EvalInt, 110.0);
			SetSkill(SkillName.Magery, 110.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;

			if (Utility.RandomDouble() < 0.3)
				PackItem(new ObsidianStone());

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override double WeaponAbilityChance => 0.1;

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.40)
					corpse.DropItem(new Pumice());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.RangerScrolls);
		}
		
		public NSarag(Serial serial) : base(serial)
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
