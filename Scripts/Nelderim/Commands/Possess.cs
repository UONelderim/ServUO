#region References

using System;
using System.Collections;
using System.Reflection;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server
{
	public class Possess
	{
		public static Layer[] ItemLayers =
		{
			Layer.FirstValid, Layer.TwoHanded, Layer.Shoes, Layer.Pants, Layer.Shirt, Layer.Helm, Layer.Gloves,
			Layer.Ring, Layer.Neck, Layer.Hair, Layer.Waist, Layer.InnerTorso, Layer.Bracelet, Layer.FacialHair,
			Layer.MiddleTorso, Layer.Earrings, Layer.Arms, Layer.Cloak, Layer.OuterTorso, Layer.OuterLegs,
			Layer.Mount
		};

//LastUserValid	?
		public static void Initialize()
		{
			CommandSystem.Register("Possess", AccessLevel.Counselor, Possess_OnCommand);
			CommandSystem.Register("PossessRed", AccessLevel.Counselor, PossessRed_OnCommand);
			CommandSystem.Register("PossessBlue", AccessLevel.Counselor, PossessBlue_OnCommand);
			CommandSystem.Register("PossessGrey", AccessLevel.Counselor, PossessGrey_OnCommand);
			CommandSystem.Register("Unpossess", AccessLevel.Counselor, UnPossess_OnCommand);
		}

		public static void CopyProps(Mobile from, Mobile to)
		{
			// Location handled separately
			string[] PropsToChange =
			{
				"RawDex", "RawInt", "RawStr", "Body", "BodyMod", "BodyValue", "Criminal", "Direction",
				"DisplayGuildTitle", "EmoteHue", "FacialHairHue", "FacialHairItemID", "Fame", "Female", "Guild",
				"GuildFealty", "GuildTitle", "HairHue", "HairItemID", "Hits", "Hue", "HueMod", "Karma", "Kills",
				"MagicDamageAbsorb", "Mana", "MeleeDamageAbsorb", "Name", "NameHue", "NameMod", "Paralyzed",
				"Poison", "ShortTermMurders", "SpeechHue", "Stam", "Title", "VirtualArmor", "VirtualArmorMod",
				"Warmode", "WhisperHue", "YellHue"
			};

			try
			{
				Type t = typeof(Mobile);
				PropertyInfo pi;

				for (int i = 0; i < PropsToChange.Length; i++)
				{
					pi = t.GetProperty(PropsToChange[i],
						BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
					if (PropsToChange[i] == "Hue" && (int)pi.GetValue(from, null) == 0) // 2004.01.23
						continue;
					pi.SetValue(to, pi.GetValue(from, null), null);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Possess: CopyProps Exception: {0}", e.Message);
			}
		}

		public static void CopySkills(Mobile from, Mobile to)
		{
			try
			{
				for (int i = 0; i < from.Skills.Length; i++)
				{
					to.Skills[i].Base = from.Skills[i].Base;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Possess: CopySkills Exception: {0}", e.Message);
			}
		}

		public static void MoveItems(Mobile from, Mobile to)
		{
			ArrayList PossessItems = new ArrayList(from.Items);

			try
			{
				for (int i = 0; i < PossessItems.Count; i++)
				{
					Item item = (Item)PossessItems[i];

					if (Array.IndexOf(ItemLayers, item.Layer) != -1)
					{
						// steal item from source & equip it ;]
						to.EquipItem(item);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Possess: MoveItems Exception: {0}", e.Message);
			}
		}

		private class PossessTarget : Target
		{
			public PossessTarget() : base(-1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from_mob, object o)
			{
				PlayerMobile from = from_mob as PlayerMobile;

				if (o is Mobile)
				{
					// Possess handler
					Mobile m = (Mobile)o;

					if (m.Account != null)
					{
						from.SendMessage("Nie mozesz wcielic sie w innego gracza!");
						return;
					}

					// 05.03.26 :: troyan :: badamy jakiego typu kriminalnego jest mob i ustawiamy odpowienie hue
					int notoriety = NotorietyHandlers.MobileNotoriety(@from, (Mobile)o);
					IMount mount;

					string log = from_mob.AccessLevel + " " + CommandLogging.Format(from_mob) + " ";
					log += "GM " + from_mob.Name + " possessed " + CommandLogging.Format(o) + " [Possess]";

					CommandLogging.WriteLine(from_mob, log);

					if (from.m_PossessMob != null)
					{
						// unpossess first
						Mobile StorageMob1 = from.m_PossessStorageMob;
						Mobile m1 = from.m_PossessMob;
						mount = from.Mount;
						MoveItems(from, m1);
						m1.Location = from.Location;
						m1.Direction = from.Direction;
						m1.Map = from.Map;
						m1.Frozen = false;
						if (mount != null)
							mount.Rider = m1;

						mount = StorageMob1.Mount;
						CopyProps(StorageMob1, from);
						CopySkills(StorageMob1, from);
						MoveItems(StorageMob1, from);
						if (mount != null)
							mount.Rider = from;
						StorageMob1.Delete();
						from.m_PossessMob = null;
						from.m_PossessStorageMob = null;

						if (from.SavagePaintExpiration > TimeSpan.Zero) // 2004.01.23
						{
							from.BodyMod = (from.Female ? 184 : 183);
							from.HueMod = 0;
						}
						else
							from.HueMod = -1;

						from.Hidden = true;
					}

					// create internal mobile that will store our changed props etc
					Mobile StorageMob = new Mobile();

					StorageMob.AccessLevel = from.AccessLevel;

					from.m_PossessMob = m;
					from.m_PossessStorageMob = StorageMob;

					mount = from.Mount;
					CopyProps(from, StorageMob);
					CopySkills(from, StorageMob);
					MoveItems(from, StorageMob);
					if (mount != null)
						mount.Rider = StorageMob;
					StorageMob.InvalidateProperties();

					mount = m.Mount;
					CopyProps(m, from);

					switch (notoriety)
					{
						case Notoriety.Innocent:
						case Notoriety.Ally:
						case Notoriety.Invulnerable:
							from.Kills = 0;
							from.Criminal = false;
							from.NameHue = 0x59;
							break;
						case Notoriety.CanBeAttacked:
							from.Kills = 0;
							from.Criminal = false;
							from.NameHue = 0x3B2;
							break;
						case Notoriety.Criminal:
							from.Kills = 0;
							from.Criminal = true;
							from.NameHue = 0x3B2;
							break;
						case Notoriety.Murderer:
							from.Kills = 6;
							from.Criminal = true;
							from.NameHue = 0x25;
							break;
					}

					CopySkills(m, from);
					MoveItems(m, from);
					from.Location = m.Location;
					if (mount != null)
						mount.Rider = from;

					// Move target to internal map
					m.Frozen = true;
					m.Map = Map.Internal;

					from.InvalidateProperties(); // recalc vitals etc
					from.Hidden = false;
				}
				else // 2004.01.23
				{
					from.SendMessage("Mozesz wcielac sie tylko w istoty zywe.");
				}
			}
		}

		private class PossessRedTarget : Target
		{
			public PossessRedTarget() : base(-1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from_mob, object o)
			{
				PlayerMobile from = from_mob as PlayerMobile;

				if (o is Mobile)
				{
					// Possess handler
					Mobile m = (Mobile)o;

					if (m.Account != null)
					{
						from.SendMessage("You cannot possess other players!");
						return;
					}

					string log = from_mob.AccessLevel + " " + CommandLogging.Format(from_mob) + " ";
					log += "GM " + from_mob.Name + " possessed " + CommandLogging.Format(o) + " [Possess]";

					CommandLogging.WriteLine(from_mob, log);

					if (from.m_PossessMob != null)
					{
						// unpossess first
						Mobile StorageMob1 = from.m_PossessStorageMob;
						Mobile m1 = from.m_PossessMob;
						MoveItems(from, m1);
						m1.Location = from.Location;
						m1.Direction = from.Direction;
						m1.Map = from.Map;
						m1.Frozen = false;

						CopyProps(StorageMob1, from);
						CopySkills(StorageMob1, from);
						MoveItems(StorageMob1, from);
						StorageMob1.Delete();
						from.m_PossessMob = null;
						from.m_PossessStorageMob = null;

						if (from.SavagePaintExpiration > TimeSpan.Zero) // 2004.01.23
						{
							from.BodyMod = (from.Female ? 184 : 183);
							from.HueMod = 0;
						}
						else
							from.HueMod = -1;


						from.Hidden = true;
					}

					// create internal mobile that will store our changed props etc
					Mobile StorageMob = new Mobile();

					StorageMob.AccessLevel = from.AccessLevel;

					from.m_PossessMob = m;
					from.m_PossessStorageMob = StorageMob;

					CopyProps(from, StorageMob);
					CopySkills(from, StorageMob);
					MoveItems(from, StorageMob);
					StorageMob.InvalidateProperties();

					CopyProps(m, from);

					//if (m.NameHue == -1)	// 2005.03.17
					from.NameHue = 0x25;
					from.Kills = 25;
					from.Criminal = true;

					CopySkills(m, from);
					MoveItems(m, from);
					from.Location = m.Location;

					// Move target to internal map
					m.Frozen = true;
					m.Map = Map.Internal;

					from.InvalidateProperties(); // recalc vitals etc
					from.Hidden = false;
				}
				else // 2004.01.23
				{
					from.SendMessage("You may only possess Mobiles.");
				}
			}
		}

		private class PossessBlueTarget : Target
		{
			public PossessBlueTarget() : base(-1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from_mob, object o)
			{
				PlayerMobile from = from_mob as PlayerMobile;
				if (o is Mobile)
				{
					// Possess handler
					Mobile m = (Mobile)o;

					if (m.Account != null)
					{
						from.SendMessage("You cannot possess other players!");
						return;
					}

					string log = from_mob.AccessLevel + " " + CommandLogging.Format(from_mob) + " ";
					log += "GM " + from_mob.Name + " possessed " + CommandLogging.Format(o) + " [Possess]";

					CommandLogging.WriteLine(from_mob, log);

					if (from.m_PossessMob != null)
					{
						// unpossess first
						Mobile StorageMob1 = from.m_PossessStorageMob;
						Mobile m1 = from.m_PossessMob;
						MoveItems(from, m1);
						m1.Location = from.Location;
						m1.Direction = from.Direction;
						m1.Map = from.Map;
						m1.Frozen = false;

						CopyProps(StorageMob1, from);
						CopySkills(StorageMob1, from);
						MoveItems(StorageMob1, from);
						StorageMob1.Delete();
						from.m_PossessMob = null;
						from.m_PossessStorageMob = null;

						if (from.SavagePaintExpiration > TimeSpan.Zero) // 2004.01.23
						{
							from.BodyMod = (from.Female ? 184 : 183);
							from.HueMod = 0;
						}
						else
							from.HueMod = -1;


						from.Hidden = true;
					}

					// create internal mobile that will store our changed props etc
					Mobile StorageMob = new Mobile();

					StorageMob.AccessLevel = from.AccessLevel;

					from.m_PossessMob = m;
					from.m_PossessStorageMob = StorageMob;

					CopyProps(from, StorageMob);
					CopySkills(from, StorageMob);
					MoveItems(from, StorageMob);
					StorageMob.InvalidateProperties();

					CopyProps(m, from);

					//if (m.NameHue == -1)	// 2005.03.17
					from.NameHue = 0x59;
					from.Kills = 0;
					from.Criminal = false;

					CopySkills(m, from);
					MoveItems(m, from);
					from.Location = m.Location;

					// Move target to internal map
					m.Frozen = true;
					m.Map = Map.Internal;

					from.InvalidateProperties(); // recalc vitals etc
					from.Hidden = false;
				}
				else // 2004.01.23
				{
					from.SendMessage("You may only possess Mobiles.");
				}
			}
		}

		private class PossessGreyTarget : Target
		{
			public PossessGreyTarget() : base(-1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from_mob, object o)
			{
				PlayerMobile from = from_mob as PlayerMobile;
				if (o is Mobile)
				{
					// Possess handler
					Mobile m = (Mobile)o;

					if (m.Account != null)
					{
						from.SendMessage("You cannot possess other players!");
						return;
					}

					string log = from_mob.AccessLevel + " " + CommandLogging.Format(from_mob) + " ";
					log += "GM " + from_mob.Name + " possessed " + CommandLogging.Format(o) + " [Possess]";

					CommandLogging.WriteLine(from_mob, log);

					if (from.m_PossessMob != null)
					{
						// unpossess first
						Mobile StorageMob1 = from.m_PossessStorageMob;
						Mobile m1 = from.m_PossessMob;
						MoveItems(from, m1);
						m1.Location = from.Location;
						m1.Direction = from.Direction;
						m1.Map = from.Map;
						m1.Frozen = false;

						CopyProps(StorageMob1, from);
						CopySkills(StorageMob1, from);
						MoveItems(StorageMob1, from);
						StorageMob1.Delete();
						from.m_PossessMob = null;
						from.m_PossessStorageMob = null;

						if (from.SavagePaintExpiration > TimeSpan.Zero) // 2004.01.23
						{
							from.BodyMod = (from.Female ? 184 : 183);
							from.HueMod = 0;
						}
						else
							from.HueMod = -1;

						from.Hidden = true;
					}

					// create internal mobile that will store our changed props etc
					Mobile StorageMob = new Mobile();

					StorageMob.AccessLevel = from.AccessLevel;

					from.m_PossessMob = m;
					from.m_PossessStorageMob = StorageMob;

					CopyProps(from, StorageMob);
					CopySkills(from, StorageMob);
					MoveItems(from, StorageMob);
					StorageMob.InvalidateProperties();

					CopyProps(m, from);

					//if (m.NameHue == -1)	// 2005.03.17
					from.NameHue = 0x3B2;
					from.Criminal = true;

					CopySkills(m, from);
					MoveItems(m, from);
					from.Location = m.Location;

					// Move target to internal map
					m.Frozen = true;
					m.Map = Map.Internal;

					from.InvalidateProperties(); // recalc vitals etc
					from.Hidden = false;
				}
				else // 2004.01.23
				{
					from.SendMessage("You may only possess Mobiles.");
				}
			}
		}

		[Usage("Possess")]
		[Description("Wcielanie sie w mobka.")]
		private static void Possess_OnCommand(CommandEventArgs e)
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;
			from.SendMessage(0x26,
				"Wroc do swojej postaci przed resatrtem lub wylaczeniem serwera, w przeciwnym razie pozostaniesz w niej na zawsze!");
			e.Mobile.Target = new PossessTarget();
		}

		[Usage("PossessRed")]
		[Description("Wcielanie sie w NPC z Czerwonym imieniem.")]
		private static void PossessRed_OnCommand(CommandEventArgs e)
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;
			from.SendMessage(0x26,
				"Wroc do swojej postaci przed resatrtem lub wylaczeniem serwera, w przeciwnym razie pozostaniesz w niej na zawsze!");
			e.Mobile.Target = new PossessRedTarget();
		}

		[Usage("PossessBlue")]
		[Description("Wcielanie sie w NPC z Niebieskim imieniem.")]
		private static void PossessBlue_OnCommand(CommandEventArgs e)
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;
			from.SendMessage(0x26,
				"Wroc do swojej postaci przed resatrtem lub wylaczeniem serwera, w przeciwnym razie pozostaniesz w niej na zawsze!");
			e.Mobile.Target = new PossessBlueTarget();
		}

		[Usage("PossessGrey")]
		[Description("Wcielanie sie w NPC z Szarym imieniem.")]
		private static void PossessGrey_OnCommand(CommandEventArgs e)
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;
			from.SendMessage(0x26,
				"Wroc do swojej postaci przed resatrtem lub wylaczeniem serwera, w przeciwnym razie pozostaniesz w niej na zawsze!");
			e.Mobile.Target = new PossessGreyTarget();
		}

		[Usage("Unpossess")]
		[Description("Reverses efect of previous possess command.")]
		private static void UnPossess_OnCommand(CommandEventArgs e)
		{
			PlayerMobile from = (PlayerMobile)e.Mobile;

			if (from.m_PossessStorageMob == null)
			{
				from.SendLocalizedMessage(505714); // You are not currently possessing any mobile.
				return;
			}

			from.AccessLevel = from.TrueAccessLevel;
			Mobile StorageMob = from.m_PossessStorageMob;
			Mobile m = from.m_PossessMob;
			MoveItems(from, m);
			m.Location = from.Location;
			m.Direction = from.Direction;
			m.Map = from.Map;
			m.Frozen = false;

			CopyProps(StorageMob, from);
			CopySkills(StorageMob, from);
			MoveItems(StorageMob, from);

			StorageMob.Delete();
			from.m_PossessMob = null;
			from.m_PossessStorageMob = null;

			if (from.SavagePaintExpiration > TimeSpan.Zero) // 2004.01.23
			{
				from.BodyMod = (from.Female ? 184 : 183);
				from.HueMod = 0;
			}
			else
				from.HueMod = -1;

			from.Hidden = true;
		}
	}
}
