#region References

using Nelderim;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza gorogona")]
	public class NGorogon : BaseCreature
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Lethal;
		public override Poison HitPoison => Poison.Deadly;

		[Constructable]
		public NGorogon() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Body = 249;
			Hue = 1570;
			Name = "gorogon";

			BaseSoundID = 0x183;

			SetStr(1200, 1300);
			SetDex(180, 190);
			SetInt(120, 130);
			SetHits(12000);
			SetStam(205, 300);

			SetDamage(19, 31);

			SetDamageType(ResistanceType.Physical, 70);
			SetDamageType(ResistanceType.Poison, 30);

			SetResistance(ResistanceType.Physical, 80);
			SetResistance(ResistanceType.Fire, 60);
			SetResistance(ResistanceType.Cold, 50);
			SetResistance(ResistanceType.Poison, 50);
			SetResistance(ResistanceType.Energy, 50);

			SetSkill(SkillName.MagicResist, 100.0, 100.0);
			SetSkill(SkillName.Tactics, 130.6, 150.0);
			SetSkill(SkillName.Wrestling, 130.6, 150.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
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
			AddLoot(LootPack.SuperBoss);
			AddLoot(NelderimLoot.UndeadScrolls);
		}

		public NGorogon(Serial serial) : base(serial)
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
