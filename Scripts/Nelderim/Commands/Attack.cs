#region References

using System;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server.Commands
{
	public class AttackCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("Attack", AccessLevel.Counselor, Attack_OnCommand);
		}

		[Usage("Attack")]
		[Description("Prowokuje atak jednego celu na drugi cel.")]
		private static void Attack_OnCommand(CommandEventArgs args)
		{
			args.Mobile.SendMessage("Wskaz prowokowany cel...");
			args.Mobile.Target = new AttackerTarget();
		}

		private class AttackerTarget : Target
		{
			public AttackerTarget() : base(12, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				try
				{
					if (targeted is BaseCreature)
					{
						BaseCreature Attacker = targeted as BaseCreature;

						if (!Attacker.Deleted && Attacker.Alive)
						{
							from.SendMessage("Wskaz cel ataku...");
							from.Target = new AttackedTarget(Attacker);
						}
						else
							from.SendMessage("Nie mozesz tego sprowokowac do walki..");
					}
					else
					{
						from.SendMessage("Nie mozesz tego sprowokowac do walki..");
					}
				}
				catch (Exception exc)
				{
					Console.WriteLine(exc.ToString());
					from.SendMessage("Wystapil nieznany blad polecenia!");
				}
			}
		}

		private class AttackedTarget : Target
		{
			private readonly BaseCreature m_Attacker;

			public AttackedTarget(BaseCreature Attacker) : base(12, false, TargetFlags.None)
			{
				m_Attacker = Attacker;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				try
				{
					if (targeted is BaseCreature)
					{
						BaseCreature Attacked = targeted as BaseCreature;

						if (!Attacked.Deleted && Attacked.Alive && Attacked != m_Attacker)
						{
							m_Attacker.AIObject.Action = ActionType.Combat;
							m_Attacker.Combatant = Attacked;

							Attacked.AIObject.Action = ActionType.Combat;
							Attacked.Combatant = m_Attacker;

							string log = from.AccessLevel + " " + CommandLogging.Format(from);
							log += " provoked " + CommandLogging.Format(m_Attacker);
							log += " to Attack " + CommandLogging.Format(targeted) + " [Attack]";

							CommandLogging.WriteLine(from, log);
						}
						else
							from.SendMessage("Nie mozesz tego wybrac za cel prowokacji...");
					}
					else if (targeted is PlayerMobile)
					{
						PlayerMobile Attacked = targeted as PlayerMobile;

						if (!Attacked.Deleted && Attacked.Alive && Attacked.AccessLevel < AccessLevel.Counselor)
						{
							m_Attacker.AIObject.Action = ActionType.Combat;
							m_Attacker.Combatant = Attacked;

							string log = from.AccessLevel + " " + CommandLogging.Format(from);
							log += " provoked " + CommandLogging.Format(m_Attacker);
							log += " to Attack " + CommandLogging.Format(targeted) + " [Attack]";

							CommandLogging.WriteLine(from, log);
						}
						else
							from.SendMessage("Nie mozesz tego wybrac za cel prowokacji...");
					}
					else
						from.SendMessage("Nie mozesz tego wybrac za cel prowokacji...");
				}
				catch (Exception exc)
				{
					Console.WriteLine(exc.ToString());
					from.SendMessage("Wystapil nieznany blad polecenia!");
				}
			}
		}
	}
}
