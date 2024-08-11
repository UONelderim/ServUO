#region References

using System.Collections;
using System.Linq;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Arachne")]
	public class Arachne : BaseCreature
	{
		public override double DifficultyScalar { get { return 1.05; } }

		[Constructable]
		public Arachne() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "Arachne";
			Body = 173;
			BaseSoundID = 1170;

			SetStr(488, 620);
			SetDex(121, 170);
			SetInt(498, 657);

			SetHits(1012, 1353);

			SetDamage(18, 28);

			SetDamageType(ResistanceType.Physical, 65);
			SetDamageType(ResistanceType.Energy, 35);

			SetResistance(ResistanceType.Physical, 80, 90);
			SetResistance(ResistanceType.Fire, 70, 80);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 50, 60);
			SetResistance(ResistanceType.Energy, 50, 60);

			SetSkill(SkillName.EvalInt, 100.1, 120.0);
			SetSkill(SkillName.Magery, 99.1, 120.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 100.5, 150.0);
			SetSkill(SkillName.Tactics, 80.1, 120.0);
			SetSkill(SkillName.Wrestling, 80.1, 120.0);

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 80;
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved)
			{
				if (Utility.RandomDouble() < 0.40)
					corpse.DropItem(new Brimstone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 4);
		}

		public override double AttackMasterChance { get { return 0.25; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			var eable = GetMobilesInRange(2);
			foreach (Mobile m in eable)
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned ||
				                          ((BaseCreature)m).Team != this.Team))
					list.Add(m);
				else if (m.Player)
					list.Add(m);
			}
			eable.Free();

			foreach (Mobile m in list)
			{
				DoHarmful(m);

				m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
				m.PlaySound(0x231);

				m.SendMessage("You feel the life drain out of you!");

				int toDrain = Utility.RandomMinMax(10, 40);

				Hits += toDrain;
				m.Damage(toDrain, this);
			}
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (0.1 >= Utility.RandomDouble())
				DrainLife();
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if (0.1 >= Utility.RandomDouble())
				DrainLife();
		}

		public Arachne(Serial serial) : base(serial)
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
