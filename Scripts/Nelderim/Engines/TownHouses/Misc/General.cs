// Check PackUpHouse() for that crash on item delete.  It causes a crash in RemoveMulti (Core)

#region References

using System;
using System.Collections;
using System.Linq;
using Server;
using Server.Commands;
using Server.Multis;

#endregion

namespace Knives.TownHouses
{
	public class General
	{
		public static string Version { get { return "2.01"; } }

		// This setting determines the suggested gold value for a single square of a home
		//  which then derives price, lockdowns and secures.
		public static int SuggestionFactor { get { return 600; } }

		// This setting determines if players need License in order to rent out their property
		public static bool RequireRenterLicense { get { return true; } }

		public static void Configure()
		{
			EventSink.WorldSave += OnSave;
		}

		public static void Initialize()
		{
			EventSink.Login += OnLogin;
			EventSink.Speech += HandleSpeech;
			EventSink.ServerStarted += OnStarted;
			CommandSystem.Register("wynajem", AccessLevel.Player, Wynajem_OnCommand);
		}

		private static void OnStarted()
		{
			foreach (TownHouse house in TownHouse.AllTownHouses)
			{
				house.InitSectorDefinition();
				house.ForSaleSign.UpdateRegion();
			}
		}

		public static void OnSave(WorldSaveEventArgs e)
		{
			foreach (TownHouseSign sign in new ArrayList(TownHouseSign.AllSigns))
				sign.ValidateOwnership();

			foreach (TownHouse house in new ArrayList(TownHouse.AllTownHouses))
				if (house.Deleted)
				{
					TownHouse.AllTownHouses.Remove(house);
				}
		}

		private static void OnLogin(LoginEventArgs e)
		{
			foreach (BaseHouse house in BaseHouse.GetHouses(e.Mobile))
				if (house is TownHouse)
				{
					TownHouse th = (TownHouse)house;
					th.ForSaleSign.CheckDemolishTimer();
					TownHouseInfo(th, e.Mobile);
				}
		}

		private static void HandleSpeech(SpeechEventArgs e)
		{
			//
			// Wylaczona obsluga komend glosowych
			//
			//ArrayList houses = new ArrayList(BaseHouse.GetHouses( e.Mobile ));

			//if ( houses == null )
			//	return;

			//foreach( BaseHouse house in houses )
			//{
			//	if (!house.Region.GetMobiles().Contains(e.Mobile))
			//      continue;

			//	if ( house is TownHouse )
			//		house.OnSpeech( e );

			//	if ( house.Owner == e.Mobile
			//	 && e.Speech.ToLower() == "create rental contract"
			//	 && CanRent( e.Mobile, house, true ) )
			//	{
			//		e.Mobile.AddToBackpack( new RentalContract() );
			//		e.Mobile.SendMessage( "A rental contract has been placed in your bag." );
			//	}

			//	if ( house.Owner == e.Mobile
			//	 && e.Speech.ToLower() == "check storage" )
			//	{
			//		int count = 0;

			//		e.Mobile.SendMessage( "You have {0} lockdowns and {1} secures available.", RemainingSecures( house ), RemainingLocks( house ) );

			//		if ( (count = AllRentalLocks( house )) != 0 )
			//			e.Mobile.SendMessage( "Current rentals are using {0} of your lockdowns.", count );
			//		if ( (count = AllRentalSecures( house )) != 0 )
			//			e.Mobile.SendMessage( "Current rentals are using {0} of your secures.", count );
			//	}
			//}
		}

		private static bool CanRent(Mobile m, BaseHouse house, bool say)
		{
			if (house is TownHouse && ((TownHouse)house).ForSaleSign.PriceType != "Sale")
			{
				if (say)
					m.SendMessage("You must own your property to rent it.");

				return false;
			}

			if (RequireRenterLicense)
			{
				RentalLicense lic = m.Backpack.FindItemByType(typeof(RentalLicense)) as RentalLicense;

				if (lic != null && lic.Owner == null)
					lic.Owner = m;

				if (lic == null || lic.Owner != m)
				{
					if (say)
						m.SendMessage("You must have a renter's license to rent your property.");

					return false;
				}
			}

			if (EntireHouseContracted(house))
			{
				if (say)
					m.SendMessage("This entire house already has a rental contract.");

				return false;
			}

			if (RemainingSecures(house) < 0 || RemainingLocks(house) < 0)
			{
				if (say)
					m.SendMessage("You don't have the storage available to rent property.");

				return false;
			}

			return true;
		}

		private static void Wynajem_OnCommand(CommandEventArgs e)
		{
			if (!e.Mobile.CheckAlive()) return;

			ArrayList houses = new ArrayList(BaseHouse.GetHouses(e.Mobile));

			if (houses == null) return;

			foreach (BaseHouse house in houses)
			{
				if (house.Region.AllMobiles.Contains(e.Mobile) && house is TownHouse && house.Owner == e.Mobile)
				{
					TownHouse tHouse = (TownHouse)house;
					if (!TownHouseInfo(tHouse, e.Mobile))
					{
						e.Mobile.SendMessage("Ten dom nie jest wynajmowany");
					}
				}
			}
		}

		private static bool TownHouseInfo(TownHouse th, Mobile m)
		{
			TownHouseSign thSign = th.ForSaleSign;
			if (thSign.RentByTime != TimeSpan.Zero)
			{
				m.SendMessage("Twoj dom {0}", thSign.Name);
				m.SendMessage("Cykl wynajmu konczy sie za {0} dni, {1}:{2}:{3}.",
					(thSign.RentTime - DateTime.Now).Days,
					(thSign.RentTime - DateTime.Now).Hours,
					(thSign.RentTime - DateTime.Now).Minutes,
					(thSign.RentTime - DateTime.Now).Seconds);
				m.SendMessage("Cykl wynajmu trwa {0} dni.", thSign.RentByTime.Days);
				m.SendMessage("Cykl wynajmu kosztuje {0} sztuk zlota.", thSign.Price);
				return true;
			}

			return false;
		}

		#region Rental Info

		public static bool EntireHouseContracted(BaseHouse house)
		{
			foreach (Item item in TownHouseSign.AllSigns)
				if (item is RentalContract && house == ((RentalContract)item).ParentHouse)
					if (((RentalContract)item).EntireHouse)
						return true;

			return false;
		}

		public static bool HasContract(BaseHouse house)
		{
			foreach (Item item in TownHouseSign.AllSigns)
				if (item is RentalContract && house == ((RentalContract)item).ParentHouse)
					return true;

			return false;
		}

		public static bool HasOtherContract(BaseHouse house, RentalContract contract)
		{
			foreach (Item item in TownHouseSign.AllSigns)
				if (item is RentalContract && item != contract && house == ((RentalContract)item).ParentHouse)
					return true;

			return false;
		}

		public static int RemainingSecures(BaseHouse house)
		{
			if (house == null)
				return 0;

			return house.GetAosMaxSecures() - house.GetAosCurSecures(out _, out _, out _, out _);
		}

		public static int RemainingLocks(BaseHouse house)
		{
			if (house == null)
				return 0;

			return house.GetAosMaxLockdowns() - house.GetAosCurLockdowns();
		}

		public static int AllRentalSecures(BaseHouse house)
		{
			int count = 0;

			foreach (TownHouseSign sign in TownHouseSign.AllSigns)
				if (sign is RentalContract && ((RentalContract)sign).ParentHouse == house)
					count += sign.Secures;

			return count;
		}

		public static int AllRentalLocks(BaseHouse house)
		{
			int count = 0;

			foreach (TownHouseSign sign in TownHouseSign.AllSigns)
				if (sign is RentalContract && ((RentalContract)sign).ParentHouse == house)
					count += sign.Locks;

			return count;
		}

		#endregion
	}
}
