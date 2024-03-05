#region References

using System.Linq;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki dowodcy orkow")]
	public class KapitanIIILegionuOrkow : BaseCreature
	{
		// 10.10.2012 :: zombie
		public override double DifficultyScalar { get { return 1.05; } }
		// zombie

		[Constructable]
		public KapitanIIILegionuOrkow() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Body = 189;

			Name = "Kapitan III Legionu Orków";
			BaseSoundID = 0x45A;
			Hue = 1180;

			SetStr(767, 945);
			SetDex(66, 75);
			SetInt(46, 70);

			SetHits(4800, 5500);

			SetDamage(20, 25);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 55, 75);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 80, 100);
			SetResistance(ResistanceType.Poison, 100, 101);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.Macing, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 125.1, 140.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			PackItem(new ShadowIronOre(20));
			PackItem(new IronIngot(10));


			if (0.05 > Utility.RandomDouble())
				PackItem(new OrcishKinMask());

			if (0.2 > Utility.RandomDouble())
				PackItem(new BolaBall());
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
			AddLoot(LootPack.Rich);
		}

		public override bool BardImmune { get { return !Core.AOS; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int Meat { get { return 2; } }

		public override bool IsEnemy(Mobile m)
		{
			if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
				return false;

			return base.IsEnemy(m);
		}

		public override void AggressiveAction(Mobile aggressor, bool criminal)
		{
			base.AggressiveAction(aggressor, criminal);

			Item item = aggressor.FindItemOnLayer(Layer.Helm);

			if (item is OrcishKinMask)
			{
				AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
				item.Delete();
				aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
				aggressor.PlaySound(0x307);
			}
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override bool AutoDispel { get { return true; } }

		public override void OnDamagedBySpell(Mobile caster)
		{
			if (caster == this)
				return;

			SpawnOrcLord(caster);
		}

		public void SpawnOrcLord(Mobile target)
		{
			Map map = target.Map;

			if (map == null)
				return;

			int orcs = 0;

			var eable = GetMobilesInRange(10);
			foreach (Mobile m in eable)
			{
				if (m is OrcishLord)
					++orcs;
			}
			eable.Free();

			if (orcs < 10)
			{
				BaseCreature orc = new SpawnedOrcishLord();

				orc.Team = this.Team;

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

				orc.MoveToWorld(loc, map);

				orc.Combatant = target;
			}
		}

		public KapitanIIILegionuOrkow(Serial serial) : base(serial)
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
