#region References

using System;
using System.Text.RegularExpressions;
using Nelderim.Towns;
using Server.Gumps;
using Server.Nelderim;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;

#endregion

namespace Server.Mobiles
{
	public partial class BaseAI
	{
		protected int m_DefaultRange;

		protected DateTime m_NextIntoleranceCheck;
		protected DateTime m_NextRangeChange;
		protected DateTime m_NextTargetChange;

		public bool IsInHarmfulField
		{
			get
			{
				var eable = m_Mobile.GetItemsInRange(1);
				foreach (Item it in eable)
				{
					if (it is FireFieldSpell.FireFieldItem || it is PoisonFieldSpell.InternalItem ||
					    it is ParalyzeFieldSpell.InternalItem || it is EnergyFieldSpell.InternalItem)
					{
						return true;
					}
				}
				eable.Free();

				return false;
			}
		}

		private void TryTargetChange()
		{
			if (DateTime.Now > m_NextTargetChange && !m_Mobile.Controlled && !m_Mobile.Summoned &&
			    m_Mobile.Combatant != null)
			{
				IDamageable newTarget = null;

				// sprawdzenie ataku na mastera
				if (m_Mobile.Combatant is BaseCreature)
				{
					Mobile owner = ((BaseCreature)m_Mobile.Combatant).ControlMaster;

					if (owner != null)
						if (Utility.RandomDouble() < m_Mobile.AttackMasterChance &&
						    m_Mobile.GetDistanceToSqrt(owner) < 3 && m_Mobile.InLOS(owner))
							newTarget = owner;
				}

				// sprawdzenie ataku na inny cel
				if (newTarget == null && ChangeCombatant(m_Mobile, m_Mobile.Combatant))
				{
					if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
						newTarget = m_Mobile.FocusMob;
				}

				if (newTarget != null)
				{
					m_Mobile.DebugSay("Zapragnalem zmienic cel na {0}", newTarget.Name);

					m_Mobile.Combatant = newTarget;
					m_Mobile.FocusMob = null;
					m_NextTargetChange = DateTime.Now + TimeSpan.FromSeconds(15);
				}
				else
					m_NextTargetChange = DateTime.Now + TimeSpan.FromSeconds(10);
			}
		}

		private bool ChangeCombatant(Mobile m, IDamageable combatant)
		{
			double chance = 1.0;

			try
			{
				if (m == null)
					return false;

				if (combatant == null)
					return true;

				BaseCreature bc = m as BaseCreature;

				if (bc != null)
					chance = bc.SwitchTargetChance;

				if (chance <= 0 || m.Hits < m.HitsMax * 0.1 || combatant.Hits < m.HitsMax * 0.4)
					return false;
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				return false;
			}

			return Utility.RandomDouble() < chance;
		}

		protected void OnCombatantFled(IDamageable combatant)
		{
			m_Mobile.DebugSay("My combatant has fled, so I am on guard");
			Action = ActionType.Guard;

			if (m_Mobile is BaseNelderimGuard)
				m_Mobile.SetLocation(m_Mobile.Home, false);
		}

		protected void OnGuardActionAttack(IDamageable combatant)
		{
			if (!(m_Mobile is BaseNelderimGuard))
				return;

			int rand;

			if (m_Mobile.FocusMob is Mobile m && m.Player && (rand = Utility.Random(0, 10)) > 5)
			{
				string msg = String.Empty;
				switch (rand)
				{
					case 6:
						msg = String.Format("Ha! {0}! Ty psi pomiocie! Dopadne Cie i ukarze w imie Sprawiedliwosci!",
							m_Mobile.FocusMob.Name);
						break;
					case 7:
					case 8:
						msg = String.Format("{0}! Stoj!", m_Mobile.FocusMob.Name);
						break;
					default:
						msg = "Stoj!";
						break;
				}

				m_Mobile.Yell(msg);
			}
		}

		protected void OnGuardActionWander()
		{
			if (!(m_Mobile is BaseNelderimGuard))
				return;

			if ((m_Mobile.GetDistanceToSqrt(m_Mobile.Home) > m_Mobile.RangePerception * 3 ||
			     !m_Mobile.InLOS(m_Mobile.Home)) && !(m_Mobile.Home == new Point3D(0, 0, 0)))
			{
				m_Mobile.DebugSay("I am to far");
				m_Mobile.SetLocation(m_Mobile.Home, false);
			}

			Map map = m_Mobile.Map;

			if (map != null && DateTime.Now >= m_NextIntoleranceCheck)
			{
				m_NextIntoleranceCheck = DateTime.Now + TimeSpan.FromSeconds(10.0);

				IPooledEnumerable eable = map.GetMobilesInRange(m_Mobile.Location, m_Mobile.RangePerception);

				foreach (Mobile m in eable)
				{
					if (m is PlayerMobile && m.Player && m.AccessLevel == AccessLevel.Player
					    && m.Kills < 5 && m.Criminal == false && m.Alive && !m.Hidden)
					{
						if (!(m as PlayerMobile).Noticed && Utility.Random(m_Mobile.RangePerception - 1) >
						    m_Mobile.GetDistanceToSqrt(m.Location))
						{
							if (NelderimRegionSystem.ActIntolerativeHarmful(m_Mobile, m, true))
							{
								((PlayerMobile)m).Noticed = true;
								new GuardTimer(m, m_Mobile).Start();
								break;
							}
						}
					}
				}
			}

			NelderimRumors();
		}

		protected class GuardTimer : Timer
		{
			private Mobile m_source;
			private readonly Mobile m_target;

			public GuardTimer(Mobile target, Mobile source) : base(TimeSpan.FromSeconds(20))
			{
				m_target = target;
				m_source = source;
				Priority = TimerPriority.FiveSeconds;
				target.SendLocalizedMessage(00505144, "",
					0x25); // Straz niezdrowo sie Toba interesuje! Lepiej zejdz jej z oczu!
			}

			protected override void OnTick()
			{
				try
				{
					if (!m_target.Deleted)
					{
						m_target.SendMessage(0x25, "Zdaje sie, ze popadles w tarapaty! Kryminalisto!");
						m_target.Criminal = true;
						((PlayerMobile)m_target).Noticed = false;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
		}

		protected void NelderimRumors()
		{
			Map map = m_Mobile.Map;

			if (map != null)
			{
				IPooledEnumerable eable = map.GetMobilesInRange(m_Mobile.Location, 7);
				bool say = false;

				foreach (Mobile m in eable)
				{
					if (m is PlayerMobile && !m.Hidden && m.Alive &&
					    Utility.RandomDouble() < m_Mobile.CurrentSpeed / 240.0 && m_Mobile.Activation(m))
					{
						say = true;
						// Console.WriteLine( "Plotka {0} dla gracza {1} o godzinie {2}", m_Mobile.Name, m.Name, DateTime.Now );
						break;
					}
				}

				if (say)
					m_Mobile.AnnounceRandomRumor(PriorityLevel.Medium);
			}
		}

		private void NelderimOnSpeech(SpeechEventArgs e)
		{
			m_Mobile.DebugSay("Listening...");

			bool isOwner = (e.Mobile == m_Mobile.ControlMaster);
			bool isFriend = (!isOwner && m_Mobile.IsPetFriend(e.Mobile));

			if (!e.Handled && e.Mobile.Alive && (isOwner || isFriend))
			{
				string speech = e.Speech.ToLower();

				if (Regex.IsMatch(e.Speech, "zabijcie", RegexOptions.IgnoreCase) ||
				    Regex.IsMatch(e.Speech, "atakujcie", RegexOptions.IgnoreCase))
				{
					if (!isOwner)
						return;

					BeginPickTarget(e.Mobile, OrderType.Attack);
				}

				if (Regex.IsMatch(e.Speech, "broncie", RegexOptions.IgnoreCase) ||
				    Regex.IsMatch(e.Speech, "chroncie", RegexOptions.IgnoreCase) ||
				    Regex.IsMatch(e.Speech, "chroncie", RegexOptions.IgnoreCase) ||
				    Regex.IsMatch(e.Speech, "broncie", RegexOptions.IgnoreCase))
				{
					if (!isOwner)
						return;

					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Guard;
					}
				}

				else if (Regex.IsMatch(e.Speech, "chodzcie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "chodzmy", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "chodzcie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "chodzmy", RegexOptions.IgnoreCase))
				{
					if (!isOwner)
						return;

					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Come;
					}
				}

				else if (Regex.IsMatch(e.Speech, "idzcie za", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "idzcie za", RegexOptions.IgnoreCase))
				{
					BeginPickTarget(e.Mobile, OrderType.Follow);
				}
				else if (Regex.IsMatch(e.Speech, "stojcie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "stac", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "stojcie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "stac", RegexOptions.IgnoreCase))
				{
					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Stay;
					}
				}
				else if (Regex.IsMatch(e.Speech, "stop", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "zatrzymajcie sie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "zatrzymajcie sie", RegexOptions.IgnoreCase))
				{
					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Stop;
					}
				}

				else if (Regex.IsMatch(e.Speech, "chroncie mnie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "broncie mnie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "chroncie mnie", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "broncie mnie", RegexOptions.IgnoreCase))
				{
					if (!isOwner)
						return;

					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = e.Mobile;
						m_Mobile.ControlOrder = OrderType.Guard;
					}
				}

				else if (Regex.IsMatch(e.Speech, "za mna", RegexOptions.IgnoreCase) ||
				         Regex.IsMatch(e.Speech, "za mna", RegexOptions.IgnoreCase))
				{
					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						m_Mobile.ControlTarget = e.Mobile;
						m_Mobile.ControlOrder = OrderType.Follow;
					}
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "zabij", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "atakuj", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					if (!m_Mobile.IsDeadPet)
						BeginPickTarget(e.Mobile, OrderType.Attack);
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "bron", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "chron", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "chron", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "bron", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;
					if (!m_Mobile.IsDeadPet)
						BeginPickTarget(e.Mobile, OrderType.Guard);
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "chodz", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "chodz", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					m_Mobile.ControlTarget = e.Mobile;
					m_Mobile.ControlOrder = OrderType.Come;
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "idz za mna", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "idz za mna", RegexOptions.IgnoreCase)))
				{
					m_Mobile.ControlTarget = e.Mobile;
					m_Mobile.ControlOrder = OrderType.Follow;
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "idz za", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "idz za", RegexOptions.IgnoreCase)))
				{
					BeginPickTarget(e.Mobile, OrderType.Follow);
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "stoj", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "stoj", RegexOptions.IgnoreCase)))
				{
					m_Mobile.ControlTarget = null;
					m_Mobile.ControlOrder = OrderType.Stay;
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "zatrzymaj sie", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "zatrzymaj sie", RegexOptions.IgnoreCase)))
				{
					m_Mobile.ControlTarget = null;
					m_Mobile.ControlOrder = OrderType.Stop;
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "chron mnie", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "chron mnie", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "bron mnie", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "bron mnie", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;
					if (!m_Mobile.IsDeadPet)
					{
						m_Mobile.ControlTarget = e.Mobile;
						m_Mobile.ControlOrder = OrderType.Guard;
					}
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "upusc", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "upusc", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					if (!m_Mobile.IsDeadPet && !m_Mobile.Summoned)
					{
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Drop;
					}
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "uwalniam cie", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "uwalniam cie", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;
					if (!m_Mobile.Summoned)
					{
						e.Mobile.SendGump(new ConfirmReleaseGump(e.Mobile, m_Mobile));
					}
					else
					{
						m_Mobile.Say(1043255,
							m_Mobile.Name); // ~1_NAME~ appears to have decided that is better off without a master!
						m_Mobile.ControlTarget = null;
						m_Mobile.ControlOrder = OrderType.Release;
					}
				}
				else if ((Regex.IsMatch(e.Speech, "jestescie wolne", RegexOptions.IgnoreCase) ||
				          Regex.IsMatch(e.Speech, "jestescie wolne", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					if (m_Mobile.CheckControlChance(e.Mobile))
					{
						if (!m_Mobile.Summoned)
						{
							e.Mobile.SendGump(new ConfirmReleaseGump(e.Mobile, m_Mobile));
						}
						else
						{
							m_Mobile.Say(1043255,
								m_Mobile.Name); // ~1_NAME~ appears to have decided that is better off without a master!
							m_Mobile.ControlTarget = null;
							m_Mobile.ControlOrder = OrderType.Release;
						}
					}
				}

				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "przekaz", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					if (!m_Mobile.IsDeadPet)
					{
						if (m_Mobile.Summoned)
							e.Mobile.SendLocalizedMessage(
								1005487); // You cannot transfer ownership of a summoned creature.
						else if (e.Mobile.HasTrade)
							e.Mobile.SendLocalizedMessage(1010507); // You cannot transfer a pet with a trade pending
						else
							BeginPickTarget(e.Mobile, OrderType.Transfer);
					}
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "patroluj", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					m_Mobile.ControlTarget = null;
					m_Mobile.ControlOrder = OrderType.Patrol;
				}
				else if ((WasNamed(speech) && m_Mobile.CheckControlChance(e.Mobile)) &&
				         (Regex.IsMatch(e.Speech, "przyjaciel", RegexOptions.IgnoreCase)))
				{
					if (!isOwner)
						return;

					if (m_Mobile.Summoned)
						e.Mobile.SendLocalizedMessage(1005481); // Summoned creatures are loyal only to their summoners.
					else if (e.Mobile.HasTrade)
						e.Mobile.SendLocalizedMessage(1070947); // You cannot friend a pet with a trade pending
					else
						BeginPickTarget(e.Mobile, OrderType.Friend);
				}
			}
		}
		
		public static bool IsSpidersFriend(Mobile m)
		{
			if (m.Race == Race.NDrow || TownDatabase.IsCitizenOfGivenTown(m, Towns.LDelmah))
				return true;

			if (m is BaseCreature bc)
			{
				if (bc.Controlled)
				{
					Mobile master = bc.ControlMaster;
					if (master?.Race == Race.NDrow || TownDatabase.IsCitizenOfGivenTown(master, Towns.LDelmah))
						return true;
				}
				if (bc.Summoned)
				{
					Mobile master = bc.SummonMaster;
					if (master?.Race == Race.NDrow || TownDatabase.IsCitizenOfGivenTown(master, Towns.LDelmah))
						return true;
				}
			}
			return false;
		}
	}
}
