using System.Collections.Generic;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName("zwloki szamana jaszczuroludzi")]
	public class AntyWarriorMob : BaseCreature
	{
		public override InhumanSpeech SpeechType => InhumanSpeech.Lizardman;

		[Constructable]
		public AntyWarriorMob() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "szaman jaszczuroludzi";
			Body = 33;

			BaseSoundID = 417;

			SetStr(96, 120);
			SetDex(86, 105);
			SetInt(136, 160);

			SetHits(98, 172);

			SetDamage(8, 10);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 25, 30);
			SetResistance(ResistanceType.Fire, 50, 60);
			SetResistance(ResistanceType.Cold, 5, 10);
			SetResistance(ResistanceType.Poison, 90, 90);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.EvalInt, 80.1, 90.0);
			SetSkill(SkillName.Magery, 80.1, 90.0);
			SetSkill(SkillName.MagicResist, 85.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new VolcanicAsh());
			}

			base.OnCarve(from, corpse, with);
		}

		public override bool CanRummageCorpses => true;
		public override int Meat => 1;
		public override int Hides => 4;
		public override HideType HideType => HideType.Spined;

		public AntyWarriorMob(Serial serial) : base(serial)
		{
		}

		public void DrainLife()
		{
			List<Mobile> targets = new();

			IPooledEnumerable eable = GetMobilesInRange(2);
			foreach (Mobile m in eable)
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned ||
				                          ((BaseCreature)m).Team != Team))
					targets.Add(m);
				else if (m.Player)
					targets.Add(m);
			}

			eable.Free();

			foreach (var m in targets)
			{
				DoHarmful(m);

				m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
				m.PlaySound(0x231);

				m.SendMessage("Czujesz jak Twoje sily zyciowe zaczynaja odplywac!");

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

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
