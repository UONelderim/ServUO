#region References

using Nelderim;
using Server.Items;
using System;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza burugha")]
	public class NBurugh : BasePeerless
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Lethal;
		public override Poison HitPoison => Poison.Lethal;

		[Constructable]
		public NBurugh() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Body = 256;
			Hue = 2207;
			BaseSoundID = 357;
			Name = "burugh - siewca zarazy";

			SetStr(1100, 1200);
			SetDex(80, 90);
			SetInt(600, 650);
			SetHits(14000);
			SetStam(205, 300);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 45);
			SetResistance(ResistanceType.Fire, 55);
			SetResistance(ResistanceType.Cold, 75);
			SetResistance(ResistanceType.Poison, 95);
			SetResistance(ResistanceType.Energy, 75);

			SetSkill(SkillName.MagicResist, 110.0, 110.0);
			SetSkill(SkillName.Tactics, 100.0, 100.0);
			SetSkill(SkillName.Wrestling, 100.0, 100.0);
			SetSkill(SkillName.EvalInt, 120.0, 120.0);
			SetSkill(SkillName.Magery, 120.0, 120.0);

			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved)
			{
				if (Utility.RandomDouble() < 0.40)
					corpse.DropItem(new EyeOfNewt());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
			AddLoot(NelderimLoot.UndeadScrolls);
		}
		
		public NBurugh(Serial serial) : base(serial)
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
