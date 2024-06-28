using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targets;
using Server.Network;
using Server.Targeting;
using System.Collections;
using Server.Multis;
using Server.Spells;

namespace Server.Commands
{
	public class BoatCommands
	{
		public static void Initialize()
		{
			Register("StatekSkocz", AccessLevel.Player, new CommandEventHandler(BoatJumpCommand));
			Register("StatekZnajdz", AccessLevel.Player, new CommandEventHandler(BoatFindCommand));
		}
		public static void Register(string command, AccessLevel access, CommandEventHandler handler)
		{
			CommandSystem.Register(command, access, handler);
		}

		[Usage("StatekSkocz")]
		[Description("Wskaz klucz do statku, aby teleportowac sie na okret.")]
		public static void BoatJumpCommand(CommandEventArgs e)
		{
			string toSay = e.ArgString.Trim();

			e.Mobile.Target = new BoatJumpTarget();
		}

		public class BoatJumpTarget : Target
		{
			private static Hashtable m_ShipJumpCooldown = new Hashtable(); // no need to serialize, just let it reset on server restart

			private class JumpInfo
			{
				private Timer m_Timer;

				public JumpInfo(Mobile m)
				{
					m_Timer = new CooldownTimer(m);
				}

				private class CooldownTimer : Timer
				{
					private Mobile m_Mobile;
					public CooldownTimer(Mobile pm) : base(TimeSpan.FromHours(12))
					{
						m_Mobile = pm;
					}

					protected override void OnTick()
					{
						Stop();
						if (BoatJumpTarget.m_ShipJumpCooldown.Contains(m_Mobile))
						{
							BoatJumpTarget.m_ShipJumpCooldown.Remove(m_Mobile);
						}
					}
				}
			}

			public BoatJumpTarget() : base(12, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Key)
				{
					Key key = (Key)targeted;

					if (key.IsChildOf(from.Backpack) || from.AccessLevel >= AccessLevel.Counselor)
					{
						if (!m_ShipJumpCooldown.Contains(from) || from.AccessLevel >= AccessLevel.Counselor)
						{
							if (key.Link is BaseBoat)
							{
								BaseBoat boat = key.Link as BaseBoat;

								if (boat.Deleted)
								{
									from.SendMessage("Ten statek juz nie istnieje.");
								}
								else if (!boat.CheckKey((key.KeyValue)))
								{
									from.SendMessage("Cos jest nie tak z tym kluczem.");
								}
								if (Engines.VvV.VvVSigil.ExistsOn(from))
								{
									from.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
								}
								else if (from.Criminal)
								{
									from.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
								}
								else if (SpellHelper.CheckCombat(from))
								{
									from.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
								}
								else
								{
									if (from.AccessLevel == AccessLevel.Player)
										m_ShipJumpCooldown.Add(from, new JumpInfo(from));

									Teleport(from, boat.GetMarkedLocation(), boat.Map);

									from.SendMessage("Teleportowales sie na swoj okret.");
								}
							}
							else
							{
								from.SendMessage("Ten klucz nie jest polaczony ze statkiem.");
							}
						}
						else
						{
							from.SendMessage("Wykorzystales juz niedawno ta komende. Musisz odczekac przed ponownym uzyciem.");
						}
					}
					else
					{
						from.SendMessage("Klucz do statku musi sie znajdowac w twoim plecaku.");
					}
				}
				else
				{
					from.SendMessage("Ta komenda dziala tylko na klucz do statku.");
				}
			}

			private void Teleport(Mobile from, Point3D loc, Map map)
			{
				from.PlaySound(0x1FC);
				from.MoveToWorld(loc, map);
				from.PlaySound(0x1FC);
			}
		}

		[Usage("StatekZnajdz")]
		[Description("Wskaz klucz do statku, aby wyswietlic jego wspolrzedne na mapie")]
		public static void BoatFindCommand(CommandEventArgs e)
		{
			string toSay = e.ArgString.Trim();

			e.Mobile.Target = new BoatFindTarget();
		}

		public class BoatFindTarget : Target
		{
			public BoatFindTarget() : base(12, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Key)
				{
					Key key = (Key)targeted;

					if (key.IsChildOf(from.Backpack) || from.AccessLevel >= AccessLevel.Counselor)
					{
						if (key.Link is BaseBoat)
						{
							BaseBoat boat = key.Link as BaseBoat;

							if (boat.Deleted)
							{
								from.SendMessage("Ten statek juz nie istnieje.");
							}
							else if (!boat.CheckKey((key.KeyValue)))
							{
								from.SendMessage("Cos jest nie tak z tym kluczem.");
							}
							else
							{
								from.SendMessage("Polozenie statku: " + boat.Map + " " + boat.Location);
							}
						}
						else
						{
							from.SendMessage("Ten klucz nie jest polaczony ze statkiem.");
						}
					}
					else
					{
						from.SendMessage("Klucz do statku musi sie znajdowac w twoim plecaku.");
					}
				}
				else
				{
					from.SendMessage("Ta komenda dziala tylko na klucz do statku.");
				}
			}
		}
	}
}
