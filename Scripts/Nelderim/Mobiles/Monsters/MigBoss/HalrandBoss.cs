using Server.Items;
using System;
using System.Collections.Generic;
using Server.Custom.Misc;

namespace Server.Mobiles
{
	public class HalrandBoss : BaseCreature
	{
		private static readonly Type[] UniqueList = [typeof(InquisitorsArms), typeof(LegsOfTheFallenKing), typeof(ColdBreeze)]; 

		private static readonly Type[] SharedList = [typeof(MadmansHatchet), typeof(MinersPickaxe), typeof(VampiricBladedWhip)]; 

		private static readonly Type[] DecorativeList = [typeof(BlabberBlade), typeof(BowOfHarps), typeof(Erotica), typeof(SatanicHelm), typeof(ShieldOfIce)]; 
		
		private static readonly double[] m_Offsets =
		{
			Math.Cos(000.0 / 180.0 * Math.PI), Math.Sin(000.0 / 180.0 * Math.PI), Math.Cos(040.0 / 180.0 * Math.PI),
			Math.Sin(040.0 / 180.0 * Math.PI), Math.Cos(080.0 / 180.0 * Math.PI), Math.Sin(080.0 / 180.0 * Math.PI),
			Math.Cos(120.0 / 180.0 * Math.PI), Math.Sin(120.0 / 180.0 * Math.PI), Math.Cos(160.0 / 180.0 * Math.PI),
			Math.Sin(160.0 / 180.0 * Math.PI), Math.Cos(200.0 / 180.0 * Math.PI), Math.Sin(200.0 / 180.0 * Math.PI),
			Math.Cos(240.0 / 180.0 * Math.PI), Math.Sin(240.0 / 180.0 * Math.PI), Math.Cos(280.0 / 180.0 * Math.PI),
			Math.Sin(280.0 / 180.0 * Math.PI), Math.Cos(320.0 / 180.0 * Math.PI), Math.Sin(320.0 / 180.0 * Math.PI),
		};

		private static readonly int _MaxAbilityInterval = 15; //seconds
		private static readonly int _MinAbilityInterval = 10; //seconds
		private DateTime _NextAbilityTime;

		private bool _IsTrueForm;

		private List<GreaterArcaneDaemon> _ArcaneDaemons = [];
		public override bool TaintedLifeAura => true;
		public override bool TeleportsPets => true;


		[Constructable]
		public HalrandBoss()
			: base(AIType.AI_Paladin, FightMode.Weakest, 18, 1, 0.2, 0.4)
		{
			Name = "Duch Halranda Wulfrosta";
			BodyValue = 400;
			Hue = 2904; // zmienić na ducha wraz z ciuchami? 
			Female = false;

			SetStr(900, 1000);
			SetDex(125, 135);
			SetInt(1000, 1200);

			Kills = 2000;

			SetDamageType(ResistanceType.Cold, 50);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 70, 90);
			SetResistance(ResistanceType.Cold, 90, 100);
			SetResistance(ResistanceType.Poison, 70, 80);
			SetResistance(ResistanceType.Energy, 60, 60);

			SetSkill(SkillName.Wrestling, 90.1, 100.0);
			SetSkill(SkillName.Swords, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 90.2, 110.0);
			SetSkill(SkillName.MagicResist, 120.2, 160.0);
			SetSkill(SkillName.Magery, 120.0);
			SetSkill(SkillName.EvalInt, 120.0);
			SetSkill(SkillName.Meditation, 120.0);
			SetSkill(SkillName.Necromancy, 120.0);
			SetSkill(SkillName.DetectHidden, 120.0);
			SetSkill(SkillName.Tracking, 120.0);
			SetSkill(SkillName.SpiritSpeak, 90.0);
			SetSkill(SkillName.Chivalry, 120.0);
			SetSkill(SkillName.Meditation, 120.0);

			SetWearable(new SpiritOfTheTotem{Name = "wilcza maska"}, 1560);
			SetWearable(new PlateDo(), 1560);
			SetWearable(new JackalsCollar(), 1560);
			SetWearable(new PlateGloves(), 1560);
			SetWearable(new StuddedArms(), 1560);
			SetWearable(new PlateHaidate(), 1560);
			SetWearable(new HoodedShroudOfShadows(), 1187);
			SetWearable(new ZyronicClaw());
			HairItemID = 0x203C;
			HairHue = Race.RandomHairHue();
		}

		public HalrandBoss(Serial serial)
			: base(serial)
		{
		}
		
		public override bool AutoDispel => false;
		public override bool Unprovokable => true;
		public override Poison PoisonImmune => Poison.Lethal;

		[CommandProperty(AccessLevel.GameMaster)]
		public override int HitsMax => _IsTrueForm ? 65000 : 1000;

		[CommandProperty(AccessLevel.GameMaster)]
		public override int ManaMax => 5000;

		public override bool DisallowAllMoves => false;

		public override bool TeleportsTo => true;

		public void Morph()
		{
			if (_IsTrueForm)
				return;

			_IsTrueForm = true;

			Name = "Prawdziwa forma Halranda Wulfrosta";
			BodyValue = 400;
			Female = false;
			Hue = 1175;

			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;
			
			SetDamageType(ResistanceType.Cold, 50);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 70, 90);
			SetResistance(ResistanceType.Cold, 90, 100);
			SetResistance(ResistanceType.Poison, 70, 80);
			SetResistance(ResistanceType.Energy, 60, 60);

			SetSkill(SkillName.Wrestling, 90.1, 100.0);
			SetSkill(SkillName.Swords, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 90.2, 110.0);
			SetSkill(SkillName.MagicResist, 120.2, 160.0);
			SetSkill(SkillName.Magery, 120.0);
			SetSkill(SkillName.EvalInt, 120.0);
			SetSkill(SkillName.Meditation, 120.0);
			SetSkill(SkillName.Necromancy, 120.0);
			SetSkill(SkillName.DetectHidden, 120.0);
			SetSkill(SkillName.Tracking, 120.0);
			SetSkill(SkillName.SpiritSpeak, 90.0);
			SetSkill(SkillName.Chivalry, 120.0);
			SetSkill(SkillName.Meditation, 120.0);

			ProcessDelta();

			Say("JAK ŚMIECIE WY ŚMIECIE!? JA ZBUDOWAŁEM TEN ŚWIAT!!");

			if (Map != null)
			{
				for (int i = 0; i < m_Offsets.Length; i += 2)
				{
					double rx = m_Offsets[i];
					double ry = m_Offsets[i + 1];

					int dist = 0;
					bool ok = false;
					int x = 0, y = 0, z = 0;

					while (!ok && dist < 10)
					{
						int rdist = 10 + dist;

						x = X + (int)(rx * rdist);
						y = Y + (int)(ry * rdist);
						z = Map.GetAverageZ(x, y);

						if (!(ok = Map.CanFit(x, y, Z, 16, false, false)))
							ok = Map.CanFit(x, y, z, 16, false, false);

						if (dist >= 0)
							dist = -(dist + 1);
						else
							dist = -(dist - 1);
					}

					if (!ok)
						continue;

					var spawn = new GreaterArcaneDaemon{ Team = Team };

					spawn.MoveToWorld(new Point3D(x, y, z), Map);

					_ArcaneDaemons.Add(spawn);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(2); // version

			writer.Write(_IsTrueForm);
			writer.WriteMobileList(_ArcaneDaemons);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			if(version < 2)
				reader.ReadBool(); //IsSpawned
			_IsTrueForm = reader.ReadBool();
			_ArcaneDaemons = reader.ReadStrongMobileList<GreaterArcaneDaemon>();

		}

		public override void OnThink()
		{
			if (DateTime.Now >= _NextAbilityTime)
			{
				ThrowSnowball();
				_NextAbilityTime = DateTime.Now +
				                    TimeSpan.FromSeconds(Utility.RandomMinMax(_MinAbilityInterval,
					                    _MaxAbilityInterval));
			}

			base.OnThink();
		}

		private void ThrowSnowball()
		{
			var targets = new HashSet<Mobile>();
			foreach (var aggressorInfo in Aggressors) targets.Add(aggressorInfo.Attacker);
			foreach (var aggressorInfo in Aggressed) targets.Add(aggressorInfo.Defender);

			if (targets.Count > 0)
			{
				Say("Lap To Gowno!");
				foreach (var target in targets)
				{
					if (Utility.RandomDouble() < 0.8) 
						continue;
					
					PlaySound(0x145);
					Animate(9, 1, 1, true, false, 0);

					Effects.SendMovingEffect(this, target, 0x913, 7, 0, false, true, 1161, 0);
					AOS.Damage(target, Utility.RandomMinMax(10, 18), 0, 0, 0, 100, 0);
				}
			}
		}
		
		public override void GenerateLoot()
		{
			AddLoot(LootPack.MysticScrolls);
		}

		public override bool OnBeforeDeath()
		{
			if (_IsTrueForm)
			{
				foreach (var m in _ArcaneDaemons) 
					m.Kill();
				_ArcaneDaemons.Clear();
				
				if (!NoKillAwards)
					ArtifactHelper.DistributeArtifact(this, GetArtifact());

				return base.OnBeforeDeath();
			}

			Morph();
			return false;
		}

		private static Item GetArtifact()
		{
			return Utility.RandomDouble() switch
			{
				<= 0.05 => Loot.Construct(UniqueList),
				<= 0.15 => Loot.Construct(SharedList),
				<= 0.30 => Loot.Construct(DecorativeList),
				_ => null
			};
		}
	}
}
