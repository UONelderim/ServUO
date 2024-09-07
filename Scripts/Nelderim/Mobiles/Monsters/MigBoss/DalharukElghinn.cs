#region References

using System;
using System.Collections;
using System.Collections.Generic;
using Nelderim;
using Server.Items;
using Server.Spells;
using Server.Spells.Sixth;
using Server.Targeting;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki Dalharuk'a Elghinn'a")]
	public class DalharukElghinn : BaseCreature
	{
		public override double DifficultyScalar => 1.10;
		public override bool BardImmune => false;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override Poison PoisonImmune => Poison.Greater;
		public override int TreasureMapLevel => 5;
		public override int Meat => 19;
		public override int Hides => 60;
		public override HideType HideType => HideType.Barbed;
		public override FoodType FavoriteFood => FoodType.Meat;
		public override Poison HitPoison => Utility.RandomBool() ? Poison.Deadly : Poison.Lethal;
		public virtual int StrikingRange => 12;

		[Constructable]
		public DalharukElghinn() : base(AIType.AI_Boss, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "Dalharuk Elghinn";

			Body = 312;
			BaseSoundID = 362;
			Hue = 2156;

			SetStr(1500, 1600);
			SetDex(120, 130);
			SetInt(600, 800);

			SetHits(22000);

			SetDamage(18, 25);

			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Energy, 80);
			SetDamageType(ResistanceType.Physical, 0);

			SetResistance(ResistanceType.Physical, 80);
			SetResistance(ResistanceType.Fire, 65);
			SetResistance(ResistanceType.Cold, 70);
			SetResistance(ResistanceType.Poison, 50);
			SetResistance(ResistanceType.Energy, 90);

			SetSkill(SkillName.EvalInt, 110.1, 120.2);
			SetSkill(SkillName.Magery, 155.1, 160.0);
			SetSkill(SkillName.MagicResist, 110.1, 120.0);
			SetSkill(SkillName.Tactics, 110.1, 120.0);
			SetSkill(SkillName.Wrestling, 70.1, 80.0);
			SetSkill(SkillName.DetectHidden, 90.1, 120.5);
			SetSkill(SkillName.Focus, 20.2, 30.0);

			m_Change = DateTime.Now;
			m_Stomp = DateTime.Now;

			SetWeaponAbility(WeaponAbility.Bladeweave);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
		}

		public override void GenerateLoot()
		{
			AddLoot( NelderimLoot.DeathKnightScrolls );
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new FireRuby());
			}

			base.OnCarve(from, corpse, with);
		}

		public DalharukElghinn(Serial serial) : base(serial)
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

			m_Change = DateTime.Now;
			m_Stomp = DateTime.Now;
		}

		public override void OnDamagedBySpell(Mobile attacker)
		{
			base.OnDamagedBySpell(attacker);

			DoCounter(attacker);
			SpawnDemon(attacker);
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			DoCounter(attacker);
		}

		public void SpawnDemon(Mobile target)
		{
			Map map = target.Map;

			if (map == null)
				return;

			int demons = 0;

			var eable = GetMobilesInRange(10);
			foreach (Mobile m in eable)
			{
				if (m is BaseCreature)
					++demons;
			}
			eable.Free();

			if (demons < 10)
			{
				Type[] demonTypes =
				{
					typeof(DemonicznySluga), typeof(LesserDaemon), typeof(LesserMoloch), typeof(CommonHordeDaemon)
				};
				Type demonType = demonTypes[Utility.Random(demonTypes.Length)];

				BaseCreature demon = Activator.CreateInstance(demonType) as BaseCreature;
				demon.Summoned = true;
				demon.Team = this.Team;

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

				demon.MoveToWorld(loc, map);

				demon.Combatant = target;
			}
		}

		private void DoCounter(Mobile attacker)
		{
			if (this.Map == null)
				return;

			if (attacker is BaseCreature && ((BaseCreature)attacker).BardProvoked)
				return;

			if (0.05 > Utility.RandomDouble())
			{
				/* Counterattack with Hit Poison Area
				 * 20-25 damage, unresistable
				 * Lethal poison, 100% of the time
				 * Particle effect: Type: "2" From: "0x4061A107" To: "0x0" ItemId: "0x36BD" ItemIdName: "explosion" FromLocation: "(296 615, 17)" ToLocation: "(296 615, 17)" Speed: "1" Duration: "10" FixedDirection: "True" Explode: "False" Hue: "0xA6" RenderMode: "0x0" Effect: "0x1F78" ExplodeEffect: "0x1" ExplodeSound: "0x0" Serial: "0x4061A107" Layer: "255" Unknown: "0x0"
				 * Doesn't work on provoked monsters
				 */

				Mobile target = null;

				if (attacker is BaseCreature)
				{
					Mobile m = ((BaseCreature)attacker).GetMaster();

					if (m != null)
						target = m;
				}

				if (target == null || !target.InRange(this, 18))
					target = attacker;

				this.Animate(10, 4, 1, true, false, 0);

				ArrayList targets = new ArrayList();

				var eable = target.GetMobilesInRange(8);
				foreach (Mobile m in eable)
				{
					if (m == this || !CanBeHarmful(m))
						continue;

					if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned ||
					                          ((BaseCreature)m).Team != this.Team))
						targets.Add(m);
					else if (m.Player && m.Alive)
						targets.Add(m);
				}
				eable.Free();

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];

					DoHarmful(m);

					AOS.Damage(m, this, Utility.RandomMinMax(10, 15), true, 0, 0, 0, 100, 0);

					m.FixedParticles(0x36BD, 1, 10, 0x1F78, 0xA6, 0, (EffectLayer)255);
					m.ApplyPoison(this, Poison.Lethal);
				}
			}
		}

		public override void OnThink()
		{
			base.OnThink();

			if (Combatant != null)
			{
				if (m_Change < DateTime.Now && Utility.RandomDouble() < 0.2)
					ChangeOpponent();

				if (m_Stomp < DateTime.Now && Utility.RandomDouble() < 0.1)
					HoofStomp();
			}
			// exit ilsh 1313, 936, 32
		}

		public override int Damage(int amount, Mobile from)
		{
			if (Combatant != null && !(Hits > HitsMax * 0.05) && !(Utility.RandomDouble() > 0.1))
			{
				new InvisibilitySpell(this, null).Cast();

				Target target = Target;

				if (target != null)
					target.Invoke(this, this);

				Timer.DelayCall(TimeSpan.FromSeconds(2), Teleport);
			}

			return base.Damage(amount, from);
		}

		private DateTime m_Change;
		private DateTime m_Stomp;

		public void Teleport()
		{
			// 20 tries to teleport
			for (int tries = 0; tries < 20; tries++)
			{
				int x = Utility.RandomMinMax(5, 7);
				int y = Utility.RandomMinMax(5, 7);

				if (Utility.RandomBool())
					x *= -1;

				if (Utility.RandomBool())
					y *= -1;

				Point3D p = new Point3D(X + x, Y + y, 0);
				IPoint3D po = new LandTarget(p, Map);

				if (po == null)
					continue;

				SpellHelper.GetSurfaceTop(ref po);

				if (InRange(p, 12) && InLOS(p) && Map.CanSpawnMobile(po.X, po.Y, po.Z))
				{
					Point3D from = Location;
					Point3D to = new Point3D(po);

					Location = to;
					ProcessDelta();

					FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
					PlaySound(0x1FE);

					break;
				}
			}

			RevealingAction();
		}

		public void ChangeOpponent()
		{
			Mobile agro, best = null;
			double distance, random = Utility.RandomDouble();

			if (random < 0.75)
			{
				// find random target relatively close
				for (int i = 0; i < Aggressors.Count && best == null; i++)
				{
					agro = Validate(Aggressors[i].Attacker);

					if (agro == null)
						continue;

					distance = StrikingRange - GetDistanceToSqrt(agro);

					if (distance > 0 && distance < StrikingRange - 2 && InLOS(agro.Location))
					{
						distance /= StrikingRange;

						if (random < distance)
							best = agro;
					}
				}
			}
			else
			{
				int damage = 0;

				// find a player who dealt most damage
				for (int i = 0; i < DamageEntries.Count; i++)
				{
					agro = Validate(DamageEntries[i].Damager);

					if (agro == null)
						continue;

					distance = GetDistanceToSqrt(agro);

					if (distance < StrikingRange && DamageEntries[i].DamageGiven > damage && InLOS(agro.Location))
					{
						best = agro;
						damage = DamageEntries[i].DamageGiven;
					}
				}
			}

			if (best != null)
			{
				// teleport
				best.Location = GetSpawnPosition(Location, Map, 1);
				best.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
				best.PlaySound(0x1FE);

				Timer.DelayCall(TimeSpan.FromSeconds(1), delegate
				{
					// poison
					best.ApplyPoison(this, HitPoison);
					best.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
					best.PlaySound(0x474);
				});

				m_Change = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
			}
		}

		public void HoofStomp()
		{
			var eable = GetMobilesInRange(StrikingRange);
			foreach (Mobile m in eable)
			{
				Mobile valid = Validate(m);

				if (valid != null && Affect(valid))
					valid.SendLocalizedMessage(
						1075081); // *Dreadhorn�s eyes light up, his mouth almost a grin, as he slams one hoof to the ground!*
			}
			eable.Free();

			// earthquake
			PlaySound(0x2F3);

			Timer.DelayCall(TimeSpan.FromSeconds(30), delegate { StopAffecting(); });

			m_Stomp = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
		}

		public Mobile Validate(Mobile m)
		{
			Mobile agro;

			if (m is BaseCreature)
				agro = ((BaseCreature)m).ControlMaster;
			else
				agro = m;

			if (!CanBeHarmful(agro, false) || !agro.Player || Combatant == agro)
				return null;

			return agro;
		}

		private static Dictionary<Mobile, bool> m_Affected;

		public static bool IsUnderInfluence(Mobile mobile)
		{
			if (m_Affected != null)
			{
				if (m_Affected.ContainsKey(mobile))
					return true;
			}

			return false;
		}

		public static bool Affect(Mobile mobile)
		{
			if (m_Affected == null)
				m_Affected = new Dictionary<Mobile, bool>();

			if (!m_Affected.ContainsKey(mobile))
			{
				m_Affected.Add(mobile, true);
				return true;
			}

			return false;
		}

		public static void StopAffecting()
		{
			if (m_Affected != null)
				m_Affected.Clear();
		}
	}
}
