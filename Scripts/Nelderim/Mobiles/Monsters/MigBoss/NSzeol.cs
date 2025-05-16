#region References

using Nelderim;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("Zgliszcza Pajaka")]
	public class NSzeol : BaseCreature
	{
		public override bool BardImmune => true;
		public override double SwitchTargetChance => 2.0;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Lethal;
		public override Poison HitPoison => Poison.Lethal;

		[Constructable]
		public NSzeol() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Body = 173;
			Hue = 2835;
			Name = "Szeol - Przeklety Pajak";

			BaseSoundID = 0x183;

			SetStr(505, 800);
			SetDex(132, 160);
			SetInt(402, 600);
			SetHits(13000);
			SetStam(205, 300);

			SetDamage(21, 33);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 75, 80);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 60, 70);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.MagicResist, 120.7, 140.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			SetSpecialAbility(SpecialAbility.Webbing);
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.UndeadScrolls);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved)
			{
				corpse.DropItem(new Brimstone());
			}

			base.OnCarve(from, corpse, with);
		}
		
		public NSzeol(Serial serial) : base(serial)
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
