using System;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Engines.HunterKiller;
using Server.Multis;
using Server.Spells;

namespace Server.Items
{
	public partial class Key : Item, IResource, IQuality
	{
		private static Hashtable m_ShipJumpCooldown = new Hashtable(); // no need to serialize, just let it reset on server restart

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (m_Link != null && typeof(BaseBoat).IsAssignableFrom(m_Link.GetType()))
			{
				list.Add(new ContextMenuFindShip(from, this));
				list.Add(new ContextMenuJumpShip(from, this));
			}
		}

		private class ContextMenuFindShip : ContextMenuEntry
		{
			Mobile m_From;
			Key m_Key;
			public ContextMenuFindShip(Mobile from, Key key) : base(6249)
			{
				m_From = from;
				m_Key = key;
			}

			public override void OnClick()
			{
				if (m_Key.IsChildOf(m_From.Backpack) || m_From.AccessLevel >= AccessLevel.Counselor)
				{
					if (m_Key.Link is BaseBoat)
					{
						BaseBoat boat = m_Key.Link as BaseBoat;

						if (boat.Deleted)
						{
							m_From.SendMessage("Ten statek juz nie istnieje.");
						}
						else if (!boat.CheckKey((m_Key.KeyValue)))
						{
							m_From.SendMessage("Cos jest nie tak z tym kluczem.");
						}
						else
						{
							m_From.SendMessage("Polozenie statku: " + boat.Map + " " + boat.Location);
						}
					}
					else
					{
						m_From.SendMessage("Ten klucz nie jest polaczony ze statkiem.");
					}
				}
				else
				{
					m_From.SendMessage("Klucz do statku musi sie znajdowac w twoim plecaku.");
				}
			}
		}

		private class ContextMenuJumpShip : ContextMenuEntry
		{
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
						if (Key.m_ShipJumpCooldown.Contains(m_Mobile))
						{
							Key.m_ShipJumpCooldown.Remove(m_Mobile);
						}
					}
				}
			}

			Mobile m_From;
			Key m_Key;
			public ContextMenuJumpShip(Mobile from, Key key) : base(6250)
			{
				m_From = from;
				m_Key = key;
			}

			public override void OnClick()
			{
				if (m_Key.IsChildOf(m_From.Backpack) || m_From.AccessLevel >= AccessLevel.Counselor)
				{
					if (!m_ShipJumpCooldown.Contains(m_From) || m_From.AccessLevel >= AccessLevel.Counselor)
					{
						if (m_Key.Link is BaseBoat)
						{
							BaseBoat boat = m_Key.Link as BaseBoat;

							if (boat.Deleted)
							{
								m_From.SendMessage("Ten statek juz nie istnieje.");
							}
							else if (!boat.CheckKey((m_Key.KeyValue)))
							{
								m_From.SendMessage("Cos jest nie tak z tym kluczem.");
							}
							else if (Engines.VvV.VvVSigil.ExistsOn(m_From))
							{
								m_From.SendLocalizedMessage(1019004); // You are not allowed to travel there.
							}
							else if (m_From.Criminal)
							{
								m_From.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
							}
							else if (SpellHelper.CheckCombat(m_From))
							{
								m_From.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
							}
							else
							{
								if (m_From.AccessLevel == AccessLevel.Player)
									m_ShipJumpCooldown.Add(m_From, new JumpInfo(m_From));

								Teleport(m_From, boat.GetMarkedLocation(), boat.Map);

								m_From.SendMessage("Teleportowales sie na swoj okret.");
							}
						}
						else
						{
							m_From.SendMessage("Ten klucz nie jest polaczony ze statkiem.");
						}
					}
					else
					{
						m_From.SendMessage("Wykorzystales juz niedawno ta mozliwosc. Musisz odczekac przed ponownym uzyciem.");
					}
				}
				else
				{
					m_From.SendMessage("Klucz do statku musi sie znajdowac w twoim plecaku.");
				}
			}

			private void Teleport(Mobile from, Point3D loc, Map map)
			{
				from.PlaySound(0x1FC);
				from.MoveToWorld(loc, map);
				from.PlaySound(0x1FC);
			}
		}
	}
}
