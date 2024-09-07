using System;
using Nelderim;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki Zhoaminth")]
	public class Zhoaminth : BaseCreature
	{
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => true;
		public override bool CanRummageCorpses => true;
		public override Poison PoisonImmune => Poison.Lethal;

		private DateTime m_LastManaReg = DateTime.MinValue;
		private static TimeSpan ManaRegenDelay = TimeSpan.FromSeconds(30);
		private Bow m_Bow;

		private static int FightRange = 11;

		[Constructable]
		public Zhoaminth() : base(AIType.AI_Mage, FightMode.Closest, FightRange + 1, FightRange, 0.25, 0.5)
		{
			Name = "Zhoaminth - zapomniane zlo";
			Body = 0x310;
			Hue = 2947;
			BaseSoundID = 357;

			SetStr(1286, 1385);
			SetDex(210, 265);
			SetInt(800, 900);

			SetMana(8000);
			SetHits(18000);

			SetDamage(22, 24);

			SetDamageType(ResistanceType.Cold, 50);
			SetDamageType(ResistanceType.Poison, 50);
			SetDamageType(ResistanceType.Physical, 0);

			SetResistance(ResistanceType.Physical, 90, 100);
			SetResistance(ResistanceType.Fire, 80, 90);
			SetResistance(ResistanceType.Cold, 60, 70);
			SetResistance(ResistanceType.Poison, 55, 65);
			SetResistance(ResistanceType.Energy, 70, 80);

			SetSkill(SkillName.Anatomy, 25.1, 50.0);
			SetSkill(SkillName.EvalInt, 110);
			SetSkill(SkillName.Magery, 115.5, 130.0);
			SetSkill(SkillName.Meditation, 105.1, 120.0);
			SetSkill(SkillName.MagicResist, 120.5, 130.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 160.0, 160.0);
			SetSkill(SkillName.Archery, 120.0, 120.0);

			m_Bow = new Bow();
			m_Bow.Attributes.SpellChanneling = 1;
			m_Bow.MaxRange = FightRange;
			AddItem(m_Bow);
			PackItem(new Arrow(Utility.Random(50, 60)));

			AddItem(new LightSource());

			//SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				corpse.DropItem(new Bloodspawn());
				corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override bool OnBeforeDeath()
		{
			RemoveItem(m_Bow);

			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.DeathKnightScrolls);
		}

		public override void OnThink()
		{
			base.OnThink();

			if (Combatant != null)
			{
				if ( DateTime.Now - m_LastManaReg >= ManaRegenDelay)
				{
					DoManaRegen();
					m_LastManaReg = DateTime.Now;
				}
			}
		}

		private void DoManaRegen()
		{
			if (Mana < 600)
			{
				// TODO: Wyszukiwac w poblizu specjalne itemy bedace zrodlami energii. Gracz moze je niszczyc aby temu zapobiec.

				Mana = 660;
				Say("*przywoluje magiczna energie z pobliskich zrodel mocy*");
				FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			}
		}

		public Zhoaminth(Serial serial) : base(serial)
		{
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
