using System;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Tournament;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;
using Server.Spells.Sixth;

namespace Server.Regions
{
	public class NArenaRegion : Region
	{
		public class FightTimer : Timer
		{
			readonly Mobile m_Target;

			public FightTimer(Mobile target, int span) : base(TimeSpan.FromMinutes(span))
			{
				m_Target = target;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				if (m_Target.Region is NArenaRegion && (m_Target.Region as NArenaRegion).IsFighter(m_Target))
				{
					m_Target.SendLocalizedMessage(505256); // Uplynal wykupiony czas na arenie.
					(m_Target.Region as NArenaRegion).EndFight(m_Target, EOFReason.EndOfTime);
				}
			}
		}

		public class AfterFightBlockTimer : Timer
		{
			readonly Mobile m_Target;

			public AfterFightBlockTimer(Mobile target) : base(TimeSpan.FromSeconds(5))
			{
				Protection.Add(target);
				m_Target = target;
				m_Target.Frozen = true;
				// 5-sekundowa blokada bezpieczenstwa. Anuluj wszystkie agresywne akcje!
				m_Target.SendLocalizedMessage(505257, "", 38);
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				m_Target.Frozen = false;
				m_Target.SendLocalizedMessage(505258, "", 167); // Blokada bezpieczenstwa zniesiona.
				Protection.Remove(m_Target);
			}
		}

		public class BeforeFightBlockTimer : Timer
		{
			readonly Mobile m_Target;

			public BeforeFightBlockTimer(Mobile target) : base(TimeSpan.FromSeconds(5))
			{
				m_Target = target;
				m_Target.Frozen = true;
				// 5-sekundowa blokada startowa. Gotuj sie!
				m_Target.SendLocalizedMessage(505259, "", 38);
				Priority = TimerPriority.EveryTick;
			}

			protected override void OnTick()
			{
				m_Target.Frozen = false;
				m_Target.SendLocalizedMessage(505260, "", 167); // Blokada zniesiona. WALKA!!!
			}
		}

		public class Fighter
		{
			public Mobile Foe { get; }
			public DateTime Start { get; }
			public FightType Fight { get; }
			public NArenaRegion Arena { get; }
			public FightTimer Timer { get; }

			public Fighter(NArenaRegion a, Mobile m, FightType ft)
			{
				Arena = a;
				Foe = m;
				Fight = ft;
				Timer = new FightTimer(m, (int)ft);
				Start = DateTime.Now;
				Timer.Start();
			}
		}

		private static ArrayList m_Protection;
		public ArenaTrainer Owner { get; set; }

		public ArrayList Fighters { get; }

		public static ArrayList Protection
		{
			get
			{
				if (m_Protection == null)
					m_Protection = new ArrayList();

				return m_Protection;
			}
			set
			{
				m_Protection = value;
			}
		}

		public NArenaRegion(string name, Map map, Rectangle3D[] area, ArenaTrainer owner) : base(owner.ArenaName,
			owner.Map, 100, area)
		{
			Owner = owner;
			Fighters = new ArrayList();
			DefaultLogoutDelay = TimeSpan.FromSeconds(30);
		}

		public static void Initialize()
		{
			EventSink.Login += OnLogin;
			EventSink.Logout += OnLogout;
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			global = 6;
		}

		public override bool OnBeginSpellCast(Mobile m, ISpell s)
		{
			FightType ft = FightType.None;

			if (m.AccessLevel >= AccessLevel.Counselor)
				return base.OnBeginSpellCast(m, s);

			if (IsFighter(m) &&
			    ((Owner.NoMagery && !(s is PaladinSpell) && !(s is NecromancerSpell) && !(s is SamuraiSpell) &&
			      !(s is NinjaSpell)) // 07.02.28 :: emfor
			     || (s is PaladinSpell && Owner.NoChivalry) || (s is NecromancerSpell && Owner.NoNecro)))
			{
				m.SendLocalizedMessage(505261); // Zasady walki zabraniaja stosowania tej magii!
				return false;
			}

			if (Owner.NoHiding && s is InvisibilitySpell && IsFighter(m, out ft))
			{
				m.SendLocalizedMessage(505262); // "Zasady walki zabraniaja uzycia tego!!!"

				if ((int)ft < (int)FightType.ShortTraining)
				{
					Owner.Say(505263); // Zasady walki zabraniaja ukrywania sie!

					EndFight(m, EOFReason.Defeat);

					Mobile op = (Fighters[0] as Fighter).Foe;

					if (op != null && op.Alive)
						EndFight(op, EOFReason.Victory);
				}

				return false;
			}

			if (s is GateTravelSpell || s is RecallSpell || s is MarkSpell)
			{
				m.SendLocalizedMessage(505264); // You cannot cast that spell here.
				return false;
			}

			return base.OnBeginSpellCast(m, s);
		}

		public override bool OnDoubleClick(Mobile m, object o)
		{
			if (m.AccessLevel > AccessLevel.Player)
				return true;

			if (Owner.NoAlchemy && o is BasePotion)
			{
				m.SendLocalizedMessage(505262); // "Zasady walki zabraniaja uzycia tego!!!"
				return false;
			}

			if (Owner.NoHealing && o is Bandage)
			{
				m.SendLocalizedMessage(505262); // "Zasady walki zabraniaja uzycia tego!!!"
				return false;
			}

			if (Owner.NoMounts && (o is BaseMount || o is EtherealMount))
			{
				m.SendLocalizedMessage(505265); // Zasady walki zabraniaja dosiadania wierzchowcow!!!
				return false;
			}

			return base.OnDoubleClick(m, o);
		}

		public override bool OnSkillUse(Mobile m, int Skill)
		{
			FightType ft = FightType.None;

			if (m.AccessLevel > AccessLevel.Counselor)
				return true;

			if (Owner.NoHiding && Skill == (int)SkillName.Hiding && IsFighter(m, out ft))
			{
				m.SendLocalizedMessage(505262); // "Zasady walki zabraniaja uzycia tego!!!"

				if ((int)ft < (int)FightType.ShortTraining)
				{
					Owner.Say(505263); // Zasady walki zabraniaja ukrywania sie!

					EndFight(m, EOFReason.Defeat);

					Mobile op = (Fighters[0] as Fighter).Foe;

					if (op != null && op.Alive)
						EndFight(op, EOFReason.Victory);
				}

				return false;
			}

			if (Owner.NoNecro && Skill == (int)SkillName.SpiritSpeak && IsFighter(m))
			{
				m.SendLocalizedMessage(505266); // Zasady walki zabraniaja uzycia tej umiejetnosci!!!

				return false;
			}

			if (Skill == (int)SkillName.Stealing || Skill == (int)SkillName.Snooping && IsFighter(m))
			{
				m.SendLocalizedMessage(505266); // Zasady walki zabraniaja uzycia tej umiejetnosci!!!

				Owner.Say(505267); //Zasady areny zabraniaja okradania przeciwnikow!

				if ((int)ft < (int)FightType.ShortTraining)
				{
					EndFight(m, EOFReason.Defeat);

					Mobile op = (Fighters[0] as Fighter).Foe;

					if (op != null && op.Alive)
						EndFight(op, EOFReason.Victory);
				}
				else
					Extort(m);

				return false;
			}

			return true;
		}

		public static void OnLogin(LoginEventArgs e)
		{
			Mobile m = e.Mobile;
			Region region = m.Region;

			if (region is NArenaRegion)
				((NArenaRegion)region).Extort(m);
		}

		public static void OnLogout(LogoutEventArgs e)
		{
			e.Mobile.Region.OnExit(e.Mobile);
		}

		public override void OnEnter(Mobile m)
		{
			try
			{
				if (IsFighter(m))
				{
					if (Owner.NoMounts && m.Mounted && m.Mount != null)
					{
						m.SendLocalizedMessage(505265); // Zasady walki zabraniaja dosiadania wierzchowcow!!!

						if (m.Mount is EtherealMount)
						{
							EtherealMount mount = m.Mount as EtherealMount;

							mount.UnmountMe();
						}
						else if (m.Mount is BaseCreature)
						{
							BaseCreature bc = m.Mount as BaseCreature;

							(bc as BaseMount).Rider = null;
							bc.ControlOrder = OrderType.Stay;
							bc.ControlTarget = null;

							Extort(bc);
						}
					}

					//Najpierw sprawdzamy zasady wprowadzania kontrolowancow
					if (m is BaseCreature && !(m is BaseMount))
					{
						BaseCreature bc = m as BaseCreature;
						Mobile cm = bc.ControlMaster;

						if (Owner.NoFamiliars && m is BaseFamiliar)
						{
							if (cm != null)
								cm.SendLocalizedMessage(
									505268); // Zasady areny nie pozwalaja wprowadzic na nia tej istoty.

							Unsummon(m);
						}
						else if (Owner.NoSummons && IsSummon(m))
						{
							cm = bc.SummonMaster;

							if (cm != null)
								cm.SendLocalizedMessage(505268);

							Unsummon(m);
						}
						else if (Owner.NoControlledSummons && IsControlledSummon(m))
						{
							if (cm != null)
								cm.SendLocalizedMessage(505268);

							Unsummon(m);
						}
						else if (Owner.NoControls && !(IsControlledSummon(m) || IsSummon(m)
						                                                     || m is BaseFamiliar))
						{
							if (cm != null)
								cm.SendLocalizedMessage(505268);

							bc.ControlOrder = OrderType.Stay;
							bc.ControlTarget = null;

							Extort(m);
						}
					}

					foreach (Mobile mobile in AllPlayers)
					{
						if (mobile == m)
							continue;

						if (IsFighter(mobile))
						{
							LookAgain(mobile);

							if (m.NetState != null)
								MobileMoving.Send(m.NetState, mobile);
						}
					}

					foreach (Mobile mobile in AllMobiles)
					{
						if (mobile == m)
							continue;

						if (IsFighter(mobile) && m.NetState != null)
						{
							MobileMoving.Send(m.NetState, mobile);
						}
					}

					LookAgain(m);
					m.SendLocalizedMessage(505269, Owner.ArenaName); // Wkraczasz na arene ~1_NAME~.
				}
				else if (Owner.NoEnter && !(m is BaseNelderimGuard))
					Extort(m);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				Extort(m);
			}
		}

		/*
		// 07.05.12 :: emfor :: start
		public override void OnMobileAdd( Mobile m )
		{
	  if( m is BaseMount && m_Owner.NoControls )
	  {
		  BaseCreature bc = m as BaseCreature;
		  
		  Mobile pm = bc.ControlMaster;
		  pm.SendMessage("Twoje zwierze zostalo usuniete z areny!");
		  
		  bc.ControlOrder = OrderType.Stay;
					bc.ControlTarget = null;
		  
		  ArenaRegion.Extort( m );
	  }
	}
	// 07.05.12 :: emfor :: end
	*/

		public override void OnExit(Mobile m)
		{
			try
			{
				if (IsFighter(m))
				{
					if (m is PlayerMobile)
						EndFight(m, EOFReason.Exit);
					else
						ClearAgressors(m);

					foreach (Mobile mobile in AllMobiles)
					{
						if (mobile == m)
							continue;

						if (IsFighter(mobile) && m.NetState != null)
							MobileMoving.Send(m.NetState, mobile);
					}

					foreach (Mobile mobile in AllPlayers)
					{
						if (mobile == m)
							continue;

						if (IsFighter(mobile))
						{
							LookAgain(mobile);

							if (m.NetState != null)
								MobileMoving.Send(m.NetState, mobile);

							if (mobile.NetState != null)
								MobileMoving.Send(mobile.NetState, m);
						}
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				Owner.RestartArena();
			}
		}

		public override bool OnBeforeDeath(Mobile m)
		{
			FightType ft;

			try
			{
				if (IsFighter(m, out ft))
				{
					if (m is BaseCreature && !(m is BaseFamiliar || IsSummon(m) || IsControlledSummon(m)))
					{
						Rejuvenate(m, false);
						Teleport(m, Owner.EndOfFightPoint);
						ClearAgressors(m);
						(m as BaseCreature).ControlOrder = OrderType.Stay;
						(m as BaseCreature).ControlTarget = null;

						if (ft != FightType.ShortTraining && ft != FightType.LongTraining)
							m.Frozen = true;
					}
					else
					{
						switch (ft)
						{
							case FightType.ShortTraining:
							case FightType.LongTraining:
							{
								Rejuvenate(m, false);
								RejuvenateEffect(m);
								m.SendLocalizedMessage(505270, "", 38); // smierc nie dosiega wojownikow areny!
								break;
							}

							case FightType.ShortDuel:
							case FightType.LongDuel:
							case FightType.Tournament:
							{
								EndFight(m, EOFReason.Defeat);

								Mobile op = (Fighters[0] as Fighter).Foe;

								if (op != null && op.Alive)
									EndFight(op, EOFReason.Victory);

								break;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				Owner.RestartArena();
			}

			return false;
		}

		public static bool Protected(Mobile m)
		{
			return Protection.Contains(m);
		}

		public bool AddFighter(Mobile m, FightType ft)
		{
			try
			{
				RemoveFighter(m);
				Fighters.Add(new Fighter(this, m, ft));
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool AddFighter(Mobile blue, Mobile red, FightType ft)
		{
			try
			{
				RemoveFighter(blue);
				RemoveFighter(red);

				Fighters.Add(new Fighter(this, blue, ft));
				Fighters.Add(new Fighter(this, red, ft));

				Point3D oldBlueLocation = blue.Location;
				Point3D oldRedLocation = red.Location;
				Map map = blue.Map;

				new BeforeFightBlockTimer(blue).Start();
				new BeforeFightBlockTimer(red).Start();

				Teleport(blue, Owner.CornerBlue);
				Teleport(red, Owner.CornerRed);

				IPooledEnumerable eable = map.GetMobilesInRange(oldBlueLocation, 8);
				ArrayList toMove = new ArrayList();
				ArrayList toUnsummon = new ArrayList();

				foreach (Mobile mob in eable)
				{
					if (mob is BaseFamiliar || IsSummon(mob) || IsControlledSummon(mob))
						toUnsummon.Add(mob);
					else if (IsControlled(mob, blue) && !Owner.NoControls)
						toMove.Add(mob);
				}

				foreach (Mobile mob in toMove)
					mob.MoveToWorld(blue.Location, blue.Map);

				eable.Free();
				toMove.Clear();

				eable = map.GetMobilesInRange(oldRedLocation, 8);

				foreach (Mobile mob in eable)
				{
					if (mob is BaseFamiliar || IsSummon(mob) || IsControlledSummon(mob))
						toUnsummon.Add(mob);
					else if (IsControlled(mob, red) && !Owner.NoControls)
						toMove.Add(mob);
				}

				foreach (Mobile mob in toMove)
					mob.MoveToWorld(red.Location, red.Map);

				for (int i = toUnsummon.Count - 1; i >= 0; i--)
					Unsummon(toUnsummon[i] as Mobile);

				eable.Free();
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				return false;
			}

			return true;
		}

		public bool AddFighter(TournamentFight tf)
		{
			try
			{
				Mobile blue = tf.Blue;
				Mobile red = tf.Red;

				RemoveFighter(blue);
				RemoveFighter(red);

				Fighters.Add(new Fighter(this, blue, FightType.Tournament));
				Fighters.Add(new Fighter(this, red, FightType.Tournament));

				new BeforeFightBlockTimer(blue).Start();
				new BeforeFightBlockTimer(red).Start();

				Point3D oldBlueLocation = blue.Location;
				Point3D oldRedLocation = red.Location;
				Map map = blue.Map;

				Teleport(blue, Owner.CornerBlue);
				Teleport(red, Owner.CornerRed);


				IPooledEnumerable eable = map.GetMobilesInRange(oldBlueLocation, 8);
				ArrayList toMove = new ArrayList();
				ArrayList toUnsummon = new ArrayList();

				foreach (Mobile mob in eable)
				{
					if (mob is BaseFamiliar || IsSummon(mob) || IsControlledSummon(mob))
						toUnsummon.Add(mob);
					else if (IsControlled(mob, blue))
						toMove.Add(mob);
				}

				foreach (Mobile mob in toMove)
					mob.MoveToWorld(blue.Location, blue.Map);

				eable.Free();
				toMove.Clear();

				eable = map.GetMobilesInRange(oldRedLocation, 8);

				foreach (Mobile mob in eable)
				{
					if (mob is BaseFamiliar || IsSummon(mob) || IsControlledSummon(mob))
						toUnsummon.Add(mob);
					else if (IsControlled(mob, blue))
						toMove.Add(mob);
				}

				foreach (Mobile mob in toMove)
					mob.MoveToWorld(red.Location, red.Map);

				for (int i = toUnsummon.Count - 1; i >= 0; i--)
					Unsummon(toUnsummon[i] as Mobile);

				eable.Free();

				// LookAgain( red );
				// LookAgain( blue );
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				return false;
			}

			return true;
		}

		public void EndFight(Mobile m)
		{
			EndFight(m, EOFReason.DefaultReason);
		}

		public void EndFight(Mobile m, EOFReason reason)
		{
			FightType ft = FightType.None;

			try
			{
				if (m != null && IsFighter(m, out ft))
				{
					m.Combatant = null;
					m.Warmode = false;
					m.Target.Cancel(m);

					ClearAgressors(m);
					RemoveFighter(m);
					LookAgain(m);

					if (m.Followers > 0)
					{
						ArrayList toDelete = new ArrayList();

						foreach (Mobile mob in AllMobiles)
						{
							if (IsControlled(mob, m) || IsSummoned(mob, m))
							{
								if (IsSummon(mob) || IsControlledSummon(mob))
									toDelete.Add(mob);
								else

								{
									(mob as BaseCreature).ControlOrder = OrderType.Follow;
									(mob as BaseCreature).ControlTarget = m;

									ClearAgressors(mob);
								}
							}
						}

						for (int i = toDelete.Count - 1; i >= 0; i--)
						{
							Unsummon(toDelete[i] as Mobile);
							toDelete.RemoveAt(i);
						}

						if (m.Followers > 0)
						{
							IPooledEnumerable eable = Owner.Map.GetMobilesInRange(Owner.EndOfFightPoint, 1);

							foreach (Mobile mob in eable)
							{
								if (IsControlled(mob, m) && mob.Frozen)
									mob.Frozen = false;
							}
						}
					}

					if ((int)reason > (int)EOFReason.Exit)
					{
						Rejuvenate(m, true);
						new AfterFightBlockTimer(m).Start();

						if (reason != EOFReason.Victory)
						{
							m.SetLocation(Owner.EndOfFightPoint, true);

							ArrayList move = new ArrayList();

							// nie powinno byc juz summonow, wiec je olewamy
							foreach (Mobile mob in AllMobiles)
							{
								if (IsControlled(mob, m))
									move.Add(mob);
							}

							foreach (Mobile mob in move)
								mob.MoveToWorld(m.Location, m.Map);

							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 11), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 7), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 3), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3728, 13);
							m.PlaySound(0x228);
						}

						if (reason == EOFReason.Victory)
						{
							m.FixedParticles(0x3779, 5, 20, 5002, EffectLayer.Head);
							m.PlaySound(490);
							m.SendLocalizedMessage((m.Female) ? 505271 : 505272, "",
								167); // Zwyciezylas! | Zwyciezyles!
							Owner.Say(505273, m.Name); // Zwyciezca jest ~1_NAME~!
						}
						else if (reason == EOFReason.Defeat)
						{
							// "Zostalas pokonana!!!" : "Zostalas pokonany!!!"
							m.SendLocalizedMessage((m.Female) ? 505274 : 505275, "", 167);
							// ~1_NAME~ zostala haniebnie pokonana!!! : ~1_NAME~ zostal haniebnie pokonany!!!
							Owner.Say(m.Female ? 505276 : 505277, m.Name);
						}
					}

					else if (reason == EOFReason.Exit)
					{
						if ((int)ft < (int)FightType.ShortTraining)
						{
							// "Zostalas pokonana!!!" : "Zostalas pokonany!!!"
							m.SendLocalizedMessage((m.Female) ? 505274 : 505275, "", 167);
							// ~1_NAME~ haniebnie uciekla z areny!!! : ~1_NAME~ haniebnie uciekl z areny!!!
							Owner.Say(m.Female ? 505278 : 505279, m.Name);

							Mobile op = (Fighters[0] as Fighter).Foe;

							if (op != null && op.Alive)
								EndFight(op, EOFReason.Victory);
						}
					}

					if (ft == FightType.Tournament)
					{
						if (reason == EOFReason.Victory)
							Owner.PassTournamentFightWinner(m);
						else if (reason == EOFReason.EndOfTime)
							Owner.Tournament.PushFight(TournamentPushFight.Restart);
						else if (reason == EOFReason.ForceRestart)
							m.SendLocalizedMessage(
								505280); // Pojedynek zostanie powtorzony! Klepsydra na przygotowanie!
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				Owner.RestartArena();
			}
		}

		public bool IsFighter(Mobile m)
		{
			FightType ft;

			return IsFighter(m, out ft);
		}

		public bool IsFighter(Mobile mob, out FightType ft)
		{
			Mobile m = mob;
			ft = FightType.None;

			if (mob == null)
				return false;

			if (mob is BaseCreature)
			{
				BaseCreature bc = mob as BaseCreature;

				if (bc.Controlled && bc.ControlMaster != null)
					m = bc.ControlMaster;
				else if (bc.Summoned && bc.SummonMaster != null)
					m = bc.SummonMaster;
				else
					return false;
			}

			try
			{
				foreach (Fighter f in Fighters)
				{
					if (f.Foe == m)
					{
						ft = f.Fight;
						return true;
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			return false;
		}

		private bool RemoveFighter(Mobile m)
		{
			Fighter fight = null;

			try
			{
				foreach (Fighter f in Fighters)
				{
					if (m == f.Foe)
					{
						f.Timer.Stop();
						fight = f;
						break;
					}
				}

				if (fight != null)
				{
					Fighters.Remove(fight);

					m.Delta(MobileDelta.Noto);
					m.InvalidateProperties();
					Mobile.ProcessDeltaQueue();

					return true;
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			return false;
		}

		private void ClearAgressors(Mobile m)
		{
			try
			{
				ArrayList rev = new ArrayList();

				foreach (Mobile mobile in AllMobiles)
				{
					if (mobile != m && IsFighter(mobile) && mobile is Revenant &&
					    (mobile as Revenant).ConstantFocus == m)
						rev.Add(mobile);
				}

				foreach (Mobile mobile in rev)
					Unsummon(mobile);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			try
			{
				List<AggressorInfo> list = m.Aggressors;

				for (int i = list.Count - 1; i >= 0; i--)
				{
					AggressorInfo info = list[i];

					if (IsFighter(info.Attacker) && !info.Expired)
					{
						if (info.Attacker.Combatant == m)
							info.Attacker.Combatant = null;

						List<AggressorInfo> list2 = info.Attacker.Aggressed;

						for (int j = 0; j < list2.Count; j++)
						{
							AggressorInfo info2 = list2[j];

							if (info2.Defender == m)
							{
								list2.RemoveAt(j);
								info2.Free();
							}

							list.RemoveAt(i);
							info.Free();
						}
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}

			try
			{
				List<AggressorInfo> list = m.Aggressed;

				for (int i = list.Count - 1; i >= 0; i--)
				{
					AggressorInfo info = list[i];

					if (IsFighter(info.Defender) && !info.Expired)
					{
						if (info.Defender.Combatant == m)
							info.Defender.Combatant = null;

						List<AggressorInfo> list2 = info.Defender.Aggressors;

						for (int j = 0; j < list2.Count; j++)
						{
							AggressorInfo info2 = list2[j];

							if (info2.Attacker == m)
							{
								list2.RemoveAt(j);
								info2.Free();
							}
						}

						list.RemoveAt(i);
						info.Free();
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public void CloseBussiness()
		{
			try
			{
				for (int i = 0; i < Fighters.Count; i++)
				{
					Fighter f = Fighters[i] as Fighter;

					TimeSpan timeRemain = DateTime.Now - f.Start;

					int amount = (int)(timeRemain.Minutes / (double)f.Fight)
						* ((f.Fight > FightType.LongDuel) ? 5 : 50) * 5 / 6;

					Banker.Deposit(f.Foe, amount);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public bool Busy(bool hardBusy)
		{
			try
			{
				if (hardBusy && Fighters.Count > 0)
					return true;

				for (int i = 0; i < Fighters.Count; i++)
				{
					if (((Fighter)Fighters[i]).Fight < FightType.ShortTraining)
						return true;
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				return true;
			}

			return false;
		}

		private void Extort(Mobile m)
		{
			try
			{
				if (m != null && m.AccessLevel == AccessLevel.Player)
				{
					// Hej! ~1_NAME~ nie masz prawa przebywac na arenie!
					this.Owner.Say(505281, m.Name);
					// Nie masz prawa przebywac na arenie! Oplac wstep!
					m.SendLocalizedMessage(505282);

					if (this.Owner.NoEnter)
						Teleport(m, this.Owner.ExtortPoint);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public void CleanArena(string message)
		{
			try
			{
				if (message != null && MobileCount > 0)
					Owner.Say(message);

				ArrayList toExtort = new ArrayList();

				foreach (Mobile m in AllMobiles)
				{
					if (!IsFighter(m) && m.AccessLevel == AccessLevel.Player)
						toExtort.Add(m);
				}

				for (int i = 0; i < toExtort.Count; i++)
					Teleport(toExtort[i] as Mobile, Owner.ExtortPoint);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public void CleanArena(int message)
		{
			try
			{
				if (MobileCount > 0)
					Owner.Say(message);

				ArrayList toExtort = new ArrayList();

				foreach (Mobile m in AllMobiles)
				{
					if (!IsFighter(m) && m.AccessLevel == AccessLevel.Player)
						toExtort.Add(m);
				}

				for (int i = 0; i < toExtort.Count; i++)
					Teleport(toExtort[i] as Mobile, Owner.ExtortPoint);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		public static void Rejuvenate(Mobile m, bool full)
		{
			if (m == null)
				return;

			if (full)
			{
				m.Paralyzed = false;
				m.Poison = null;
				AnimalForm.RemoveContext(m, true);
				IncognitoSpell.StopTimer(m);
				DisguiseTimers.StopTimer(m);
				CurseSpell.RemoveEffect(m);
				StrangleSpell.RemoveCurse(m);
				m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Dex));
				m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Int));
				m.RemoveStatMod(String.Format("[Magic] {0} Offset", StatType.Str));
				m.Stam = m.StamMax;
				m.Mana = m.ManaMax;
			}

			m.Hits = m.HitsMax;
		}

		public static void RejuvenateEffect(Mobile m)
		{
			if (m == null)
				return;

			m.FixedParticles(0x3779, 5, 20, 5002, EffectLayer.Head);
			m.PlaySound(490);
		}

		public static void Teleport(Mobile m, Point3D loc)
		{
			if (m == null)
				return;

			m.SetLocation(loc, true);

			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z - 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z - 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 11), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 7), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 3), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3728, 13);
			m.PlaySound(0x228);
		}

		public static bool IsSummon(Mobile m)
		{
			return (m is EnergyVortex || m is BladeSpirits || m is Revenant);
		}

		public static bool IsControlledSummon(Mobile m)
		{
			return (m is SummonedAirElemental || m is SummonedEarthElemental || m is SummonedFireElemental
			        || m is SummonedWaterElemental || m is SummonedDaemon);
		}

		public static bool IsControlled(Mobile who)
		{
			BaseCreature bc = who as BaseCreature;

			return (bc != null && bc.Controlled && bc.ControlMaster != null);
		}

		public static bool IsControlled(Mobile who, Mobile whom)
		{
			return IsControlled(who as BaseCreature, whom);
		}

		public static bool IsControlled(BaseCreature who, Mobile whom)
		{
			return (who != null && who.Controlled && who.ControlMaster == whom);
		}

		public static bool IsSummoned(Mobile who)
		{
			BaseCreature bc = who as BaseCreature;

			return (bc != null && bc.Summoned && bc.SummonMaster != null);
		}

		public static bool IsSummoned(Mobile who, Mobile whom)
		{
			return IsSummoned(who as BaseCreature, whom);
		}

		public static bool IsSummoned(BaseCreature who, Mobile whom)
		{
			return (who != null && who.Summoned && who.SummonMaster == whom);
		}

		public static bool HasOwner(Mobile who)
		{
			return (GetOwner(who as BaseCreature) != null);
		}

		public static Mobile GetOwner(Mobile who)
		{
			return GetOwner(who as BaseCreature);
		}

		public static Mobile GetOwner(BaseCreature who)
		{
			if (who == null)
				return null;

			return (who.ControlMaster == null) ? who.SummonMaster : who.ControlMaster;
		}

		public static void Unsummon(Mobile m)
		{
			Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 1,
				13, 2100, 3, 5042, 0);
			m.PlaySound(0x201);
			m.Delete();
		}

		public static bool CanSee(Mobile from, Mobile target)
		{
			// Console.WriteLine( "CanSee( {0}, {1} )", from.Name, target.Name );
			NArenaRegion fr = from.Region as NArenaRegion;
			bool fif = (fr != null && fr.IsFighter(from));

			if (fif)
			{
				// Console.WriteLine( "{0} is fighter", from.Name );
				NArenaRegion tr = target.Region as NArenaRegion;
				bool tif = (tr != null && tr.IsFighter(target));
				bool sameregion = (fr == tr);

				if (sameregion && tif)
				{
					// Console.WriteLine( "{0} is fighter too", target.Name );
					return true;
				}

				// Console.WriteLine( "but {0} ins't", target.Name );
				return false;
			}

			return true;
		}

		private void LookAgain(Mobile m)
		{
			try
			{
				if (m == null || m.Map == null || m.NetState == null)
					return;

				IPooledEnumerable eable = m.Map.GetMobilesInRange(m.Location);

				foreach (Mobile mob in eable)
				{
					if (!m.CanSee(mob))
					{
						m.NetState.Send(mob.RemovePacket);
					}
					else
					{
						MobileIncoming.Send(m.NetState, mob);
						m.NetState.Send(mob.NGetOPLPacket(m));
					}
				}

				eable.Free();
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}
	}
}
