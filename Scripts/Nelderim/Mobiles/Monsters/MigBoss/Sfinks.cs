#region References

using Nelderim;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza sfinksa")]
	public class Sfinks : BaseCreature
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => true;

		[Constructable]
		public Sfinks() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Stary Sfinks";

			BaseSoundID = 362;
			Body = 788;
			SetStr(1200, 1300);
			SetDex(180, 190);
			SetInt(120, 130);
			SetHits(12000);
			SetStam(205, 300);

			SetDamage(19, 31);

			SetDamageType(ResistanceType.Physical, 70);
			SetDamageType(ResistanceType.Cold, 30);

			SetResistance(ResistanceType.Physical, 80);
			SetResistance(ResistanceType.Fire, 60);
			SetResistance(ResistanceType.Cold, 80);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 50);

			SetSkill(SkillName.MagicResist, 100.0, 100.0);
			SetSkill(SkillName.Tactics, 130.6, 150.0);
			SetSkill(SkillName.Wrestling, 130.6, 150.0);

			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
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
			AddLoot(NelderimLoot.BardScrolls);
		}

		public Sfinks(Serial serial) : base(serial)
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
