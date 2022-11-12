#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wladcy minotaurow")]
	public class MinotaurBoss : BaseCreature
	{
		public override double DifficultyScalar => 1.05;
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.75;
		public override double SwitchTargetChance => 0.15;
		public override Poison PoisonImmune => Poison.Lethal;
		public override int Meat => 2;
		public override bool CanRummageCorpses => true;
		public override bool AutoDispel => true;
		public override bool AllureImmune => true;

		[Constructable]
		public MinotaurBoss() : base(AIType.AI_Melee, FightMode.Closest, 13, 1, 0.25, 0.5)
		{
			Body = 280;

			Name = "wladca minotaurow";
			BaseSoundID = 0x45A;

			SetStr(767, 945);
			SetDex(106, 115);
			SetInt(46, 70);

			SetHits(5000, 6000);

			SetDamage(20, 30);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 125.1, 140.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 70;

			SetWeaponAbility(WeaponAbility.CrushingBlow);
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			ArtifactHelper.ArtifactDistribution(this);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
			AddLoot(LootPack.Gems, 6);
		}

		public override void OnDamagedBySpell(Mobile caster)
		{
			if (caster == this)
				return;

			SpawnMinotaur(caster);
		}

		public void SpawnMinotaur(Mobile target)
		{
			Map map = target.Map;

			if (map == null)
				return;

			int minos = 0;

			foreach (Mobile m in this.GetMobilesInRange(10))
			{
				if (m is Minotaur)
					++minos;
			}

			if (minos < 10)
			{
				BaseCreature mino = new MinotaurSummon();

				mino.Team = this.Team;

				Point3D loc = target.Location;
				bool validLocation = false;

				for (int j = 0; !validLocation && j < 10; ++j)
				{
					int x = target.X + Utility.Random(3) - 1;
					int y = target.Y + Utility.Random(3) - 1;
					int z = map.GetAverageZ(x, y);

					if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
						loc = new Point3D(x, y, Z);
					else if (validLocation = map.CanFit(x, y, z, 16, false, false))
						loc = new Point3D(x, y, z);
				}

				mino.MoveToWorld(loc, map);

				mino.Combatant = target;
			}
		}

		public MinotaurBoss(Serial serial) : base(serial)
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
