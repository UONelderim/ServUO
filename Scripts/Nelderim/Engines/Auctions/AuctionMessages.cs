namespace Arya.Auction
{
	public class AuctionMessages
	{
		public const string DELIVERY_SYSTEM_TITLE = "Dostawa";
		public const string DELIVERING = "Dostarczam...";
		public const string PUT_GOLD_IN_BANK = "Umiesc zloto w banku";
		public const string PUT_ITEM_IN_BANK = "Umiesc przedmiot w banku";
		public const string ITEM = "Przedmiot";
		public const string GOLD = "Zloto";
		public const string VIEW_AUCTIONS = "Wyswietl aukcje";
		public const string CLOSE = "Zamknij";
		public const string AUCTION_SYSTEM_TITLE = "Witaj w Domu Aukcyjnym";
		public const string CREATE_AUCTION = "Wystaw Przedmiot";
		public const string VIEW_ALL_AUCTIONS = "Wyswietl wszystkie aukcje";
		public const string VIEW_YOUR_AUCTIONS = "Wyswietl twoje aukcje";
		public const string VIEW_YOUR_BIDS = "Wyswietl twoje oferty";
		public const string VIEW_PENDENCIES = "Wyswietl twoje zakonczone aukcje";
		public const string EXIT = "Wyjdz";
		public const string AUCTION_SYSTEM_DISABLED = "Dom Aukcyjny jest chwilowo nieczynny.";
		public const string SEARCH = "Szukaj";
		public const string SORT = "Sortuj";
		public const string PAGE_FMT = "Strona {0}/{1}";
		public const string DISPLAY_COUNT_FMT = "Wyswietlam {0} przedmiotow";
		public const string DISPLAY_EMPTY = "Brak przedmiotow do wyswietlenia";
		public const string PREVIOUS_PAGE = "Poprzednia Strona";
		public const string NEXT_PAGE = "Nastepna Strona";
		public const string ERROR = "Cos poszlo nie tak. Powiadom administracje.";
		public const string ITEM_EXPIRED = "The selected item has expired. Please refresh the auction listing.";
		public const string MESSAGING_SYSTEM_TITLE = "Auction Messaging System";
		public const string AUCTION = "Aukcja:";
		public const string VIEW_DETAILS = "View details";
		public const string NOT_AVAILABLE = "Not available";
		public const string MESSAGE_DETAILS = "Message Details:";
		public const string PENDING_TIME_LEFT = "Time left for all part to take their decisions: {0} days and {1} hours."; //Merge with days_hours_fmt
		public const string AUCTION_NO_LONGER_EXISTS = "The auction no longer exists, therefore this message is no longer valid.";
		
		public const string SEARCH_SYSTEM_TITLE = "Auction House Search";
		public const string SEARCH_PROMPT = "Enter the terms to search for (leave blank for all items)";
		public const string SEARCH_TYPES = "Limit search to these types:";
		public const string SEARCH_MAPS = "Maps";
		public const string SEARCH_ARTIFACTS = "Artifacts";
		public const string SEARCH_POWER_SCROLLS = "Power and Stat Scrolls";
		public const string SEARCH_RESOURCES = "Resources";
		public const string SEARCH_JEWELS = "Jewels";
		public const string SEARCH_WEAPONS = "Weapons";
		public const string SEARCH_ARMOR = "Armor";
		public const string SEARCH_SHIELDS = "Shields";
		public const string SEARCH_REAGENTS = "Reagents";
		public const string SEARCH_POTIONS = "Potions";
		public const string SEARCH_BOD_LARGE = "BOD (Large)";
		public const string SEARCH_BOD_SMALL = "BOD (Small)";
		public const string SEARCH_CANCEL = "Cancel";
		public const string SEARCH_CURRENT = "Search only within your current results";
		
		public const string SORT_SYSTEM_TITLE = "Auction House Sorting System";
		public const string ITEM_NAME = "Name";
		public const string SORT_NAME_ASC = "Ascending";
		public const string SORT_NAME_DESC = "Descending";
		public const string SORT_DATE = "Date";
		public const string SORT_DATE_ASC = "Oldest first";
		public const string SORT_DATE_DESC = "Newest first";
		public const string TIME_LEFT = "Time Left";
		public const string SORT_TIME_LEFT_ASC = "Shortest first";
		public const string SORT_TIME_LEFT_DESC = "Longest first";
		public const string SORT_BID_COUNT = "Number of bids";
		public const string SORT_BID_COUNT_ASC = "Few first";
		public const string SORT_BID_COUNT_DESC = "Most first";
		public const string SORT_MIN_BID = "Minimum bid value";
		public const string SORT_BID_ASC = "Lowest first";
		public const string SORT_BID_DESC = "Highest first";
		public const string SORT_MAX_BID = "Highest bid value";
		public const string SORT_CANCEL = "Cancel Sorting";
		
		public const string ITEM_X_OF_FMT = "Item {0} of {1}";
		public const string STARTING_BID = "Starting Bid";
		public const string RESERVE = "Reserve";
		public const string HIGHEST_BID = "Highest Bid";
		public const string NO_BIDS_YET = "No bids yet";
		public const string VIEW_WEB_LINK = "Web Link";
		public const string VIEW_BUY_NOW_FMT = "Buy this item now for {0} gold";
		public const string DAYS_HOURS_FMT = "{0} Days {1} Hours";
		public const string HOURS_FMT = "{0} Hours";
		public const string MINUTES_FMT = "{0} Minutes";
		public const string SECONDS_FMT = "{0} Seconds";
		public const string PENDING = "Pending";
		public const string NA = "N/A";
		public const string BID_ON_ITEM = "Licytuj";
		public const string VIEW_BIDS = "View Bids";
		public const string ITEM_DESCRIPTION = "Owner's Description";
		public const string ITEM_HUE = "Item Hue";
		public const string AUCTION_EXPIRED = "This auction is closed and is no longer accepting bids";
		public const string INVALID_BID = "Invalid bid. Bid not accepted.";
		public const string BID_HISTORY = "Bidding History";
		public const string WHO = "Who";
		public const string AMOUNT = "Amount";
		public const string RETURN_TO_AUCTION = "Return to the Auction";
		public const string CREATURES_DIVISION = "Creatures Division";
		public const string STABLE_PET = "Stable the pet";
		public const string USE_TICKET = "Use this ticket to stable your pet.";
		public const string STABLE_INFO = "Stabled pets must be claimed within a week time from the stable."; //Maybe we can merge these two
		public const string STABLE_INFO2 = "within a week time from the stable.";
		public const string FREE_SERVICE = "You will not pay for this service.";
		public const string TERMINATION_TITLE = "AUCTION SYSTEM TERMINATION";
		public const string TERMINATION_WARNING = "You are about to terminate the auction system running on this server. This will cause all current auctions to end right now. All items will be returned to the original owners and the highest bidders will receive their money back.";
		public const string TERMINATION_CONFIRM = "Yes I want to terminate the system";
		public const string TERMINATION_CANCEL = "Do nothing and let the system running";
		public const string NEW_AUCTION_TITLE = "New Auction Configuration";
		public const string DURATION = "Duration";
		public const string DAYS = "Days";
		public const string SETUP_BUY_NOW = "Allow Buy Now For:";
		public const string SETUP_DESCRIPTION = "Description (Optional)";
		public const string SETUP_WEB_LINK = "Web Link (Optional)";

		public const string AGREEMENT = @"<basefont color=#FF0000>Auction Agreement<br>
<basefont color=#FFFFFF>By completing and submitting this form you agree to take part in the auction system. The item you are auctioning will be removed from your inventory and will be returned to you only if you cancel this auction, if the auction is unsuccesfull and the item isn't sold, or if staff forces the auction system to stop.
<basefont color=#FF0000>Starting Bid:<basefont color=#FFFFFF> This is the minimum bid accepted for this item. Set this value to something reasonable, and possibly lower than what you expect to collect for the item in the end.
<basefont color=#FF0000>Reserve:<basefont color=#FFFFFF> This value will not be know to the bidders, and you should consider it as a safe price for your item. If the final bid reaches this value, the sale will be automatically finalized by the system. If on the other hand the highest bid is somewhere between the starting bid and the reserve, you will be given the option of choosing whether to sell the item or not. You will have 7 days after the end of the auction to take this decision. If you don't, the auction system will assume you decided not to sell and will return the item to you and the money to the highest bidder. Bidders will not see the value of the reserve, but only a statement saying whether it has been reached or not.
<basefont color=#FF0000>Duration:<basefont color=#FFFFFF> This value specifies how many days the auction will last from its creation time. At the end of this period, the system will proceed to either finalize the sale, return the item and the highest bid, or wait for a decision in case of a reserve not reached issue.
<basefont color=#FF0000>Buy Now:<basefont color=#FFFFFF> This options allows you to specify a safe price at which you are willing to sell the item before the end of the auction. If the buyer is willing to pay this price, they will be able to purchase the item right away without any further bids.
<basefont color=#FF0000>Name:<basefont color=#FFFFFF> This should be a short name defining your auction. The system will suggest a name based on the item you're selling, but you might wish to change it in some circumstances.
<basefont color=#FF0000>Description:<basefont color=#FFFFFF> You can write pretty much anything you wish here about your item. Keep in mind that the item properties you see when moving your mouse over the item will be available to bidders automatically, so there's no need for you to describe those.
<basefont color=#FF0000>Web Link:<basefont color=#FFFFFF> You can add a web link to this auction, in case you have a web page with further information or discussion about this item
<br>
Once you commit this auction you will not be able to retrieve your item until the auction ends. Make sure you understand what this means before committing.";
		public const string AGREEMENT_ACCEPT = "I have read the auction agreement and wish to continue and commit this auction."; //Maybe we can merge these two
		public const string AGREEMENT_ACCEPT2 = "to continue and commit this auction.";
		public const string CANCEL_EXIT = "Cancel and exit";
		public const string STARTING_BID_TOO_LOW = "The starting bid must be at least 1 gold coin.";
		public const string RESERVE_TOO_LOW = "The reserve must be greater or equal than the starting bid";
		public const string DURATION_TOO_LOW = "An auction must last at least {0} days.";
		public const string DURATION_TOO_HIGH = "An auction can last at most {0} days.";
		public const string MISSING_NAME = "Please speicfy a name for your auction";
		public const string RESERVE_TOO_HIGH = "The reserve you specified is too high. Either lower it or raise the starting bid.";
		public const string SYSTEM_CLOSED = "The system has been closed";
		public const string ITEM_NOT_FOUND_ERROR = "The item carried by this check no longer exists due to reasons outside the auction system";
		public const string ITEM_DELIVERED = "The content of the check has been delivered to your bank box.";
		public const string AUCTION_CONTROL_MOVE_WARNING = "You are not supposed to remove this item manually. Ever.";
		public const string GOLD_CHECK_TITLE = "Gold Check from the Auction System";
		public const string RESULT_BID_OUTBID_FMT = "You have been outbid for the auction of {0}. Your bid was {1}.";
		public const string RESULT_BID_SYSTEM_STOPPED_FMT = "Auction system stopped. Returning your bid of {1} gold for {0}";
		public const string RESULT_BID_AUCTION_CANCELED_FMT = "Auction for {0} has been canceled by either you or the owner. Returning your bid.";
		public const string RESULT_BID_RESERVE_NOT_MET_FMT = "Your bid of {0} for {1} didn't meet the reserve and the owner decided not to accept your offer";
		public const string RESULT_BID_ITEM_REMOVED = "The auction has been cancelled because the auctioned item has been removed from the world.";
		public const string RESULT_BID_STAFF_REMOVED =
			"You have bid on an auction that has been removed by the shard staff. Your bid is now being returned to you.";
		public const string RESULT_BID_SUCCESS_FMT = "You have sold {0} through the auction system. The highest bid was {1}.";
		
		public const string INVALID_GOLD_CHECK_REASON_FMT = "{0} is not a valid reason for an auction gold check";
		public const string INVALID_ITEM_CHECK_REASON_FMT = "{0} is not a valid reason for an auction item check";
		public const string CREATURE_CHECK_TITLE = "Creature Check from the Auction System";
		public const string ITEM_CHECK_TITLE = "Item Check from the Auction System";
		
		public const string RESULT_NO_BIDS_FMT = "Your auction for {0} has terminated without bids.";
		public const string RESULT_CANCELED_FMT = "Your auction for {0} has been canceled";
		public const string RESULT_SYSTEM_STOPPED_FMT = "The auction system has been stopped and your auctioned item is being returned to you. ({0})";
		public const string RESULT_PENDING_TIMEOUT = "The auction was in pending state due to either reserve not being met or because one or more items have been deleted. No decision has been taken by the involved parts to resolve the auction therefore it has been ended unsuccesfully.";
		public const string RESULT_ITEM_REMOVED = "The auction has been cancelled because the auctioned item has been removed from the world.";
		public const string RESULT_STAFF_CLOSED =
			"Your auction has been closed by the shard staff and your item is now returned to you.";
		
		public const string RESULT_SUCCESS_FMT = "You have succesfully purchased {0} through the auction system. Your bid was {1}.";
		
		public const string ERR_CREATURE_OWNER = "You can't auction creatures that don't belong to you.";
		public const string ERR_CREATURE_DEAD = "You can't auction dead creatures";
		public const string ERR_CREATURE_SUMMONED = "You can't auction summoned creatures";
		public const string ERR_CREATURE_FAMILIAR = "You can't auction familiars";
		public const string ERR_CREATURE_BACKPACK = "Please unload your pet's backpack first";
		public const string ERR_CREATURE_REMOVED = "The pet represented by this check no longer exists";
		public const string ERR_SYSTEM_CLOSED = "Sorry we're closed at this time. Please try again later.";
		public const string ERR_ITEM_REMOVED = "This item no longer exists";
		public const string CONTROL_SLOTS_FMT = "Control Slots : {0}<br>";
		public const string BONDABLE_FMT = "Bondable : {0}<br>";
		public const string STR_FMT = "Str : {0}<br>";
		public const string DEX_FMT = "Dex : {0}<br>";
		public const string INT_FMT = "Int : {0}<br>";
		public const string INVALID = "Invalid";
		public const string ITEM_NOT_FOUND = "The item you selected has been removed and will be held under strict custody";
		public const string AUCTION_CANCELED_ITEM = "You cancel the auction and your item is returned to you";
		public const string AUCTION_CANCELED_PET = "You cancel the auction and your pet is returned to you";
		public const string ERR_BID_LOW = "Your bid isn't high enough";
		public const string ERR_BID_MIN = "Your bid doesn't reach the minimum bid";
		public const string ERR_STABLE_FULL = "Your stable is full. Please free some space before claiming this creature.";
		public const string ERR_OUTBID_FMT = "You have been outbid. An auction check of {0} gold coins has been deposited in your backpack or bankbox. View the auction details if you wish to place another bid.";

		public const string MSG_RESERVE_OWNER_FMT =
			"Your auction has ended, but the highest bid didn't reach the reserve you specified. You now have option to decide whether to sell your item or not.<br><br>The highest bid is {0}. Your reserve was {1}.";
		public const string MSG_RESERVE_OWNER_ITEM_INVALID = "<br><br>Some of the items auctioned have been deleted during the duration of the auction. The buyer will have to accept the new auction before it can be completed.";
		public const string MSG_RESERVE_OWNER_OK = "Yes I want to sell my item even if the reserve hasn't been met";
		public const string MSG_RESERVE_OWNER_CANCEL = "No I don't want to sell and I want my item returned to me";
		public const string MSG_RESERVE_BUYER_FMT = "Your bid didn't meet the reserve specified by the auction owner. The item owner will now have to decide whether to sell or not.<br><br>Your bid was {0}. The reserve is {1}.";
		public const string MSG_BUTTON_CLOSE = "Close this message";

		public const string MSG_ERROR_BUYER_FMT =
			"You have participated and won an auction. However due to external events one or more of the items auctioned no longer exist. Please review the auction by using the view details button and decide whether you wish to purchase the items anyway or not.<br><br>Your bid was {0}";

		public const string MSG_ERROR_BUYER_RESERVE_NOT_MET =
			"<br><br>Your bid didn't meet the reserve specified by the owner. The owner will not have to deicde whether they wish to sell or not";
		public const string MSG_ERROR_BUYER_OK = "Yes I want to purchase anyway";
		public const string MSG_ERROR_BUYER_CANCEL = "No I don't want to purchase and wish to have my money back";

		public const string MSG_ERROR_OWNER =
			"Some of the items you acutioned no longer exists because of external reasons. The buyer will now decide whether to purchase or not.";

		public const string NEW_AUCTION_PROMPT = "Please target the item you wish to put on auction...";
		public const string ERR_TOO_MANY_AUCTIONS_FMT = "You cannot have more than {0} auctions active on your account";
		public const string ERR_NOT_ITEM = "You can only auction items";
		public const string ERR_INVALID_ITEM = "You cannot put that on auction";
		public const string ERR_FROZEN_ITEM = "You cannot auction items that are not movable";
		public const string ERR_CONTAINER_INVALID_ITEM =
			"One of the items inside the container isn't allowed at the auction house";
		public const string ERR_CONTAINER_NESTED = "You cannot sell containers with items nested in other containers";

		public const string ERR_INVALID_PARENT =
			"You can only auction items that you have in your backpack or in your bank box";

		public const string ERR_NOT_ENOUGH_MONEY = "You don't have enough money in your bank to place this bid";
		public const string ERR_BID_INCREMENT_FMT = "Your bid must be at least {0} higher than the current bid";

		public const string ERR_AUCTION_EXPIRED =
			"The selected auction is no longer active. Please refresh the auctions list.";
		public const string ERR_BUY_NOW_TOO_LOW =
			"If you chose to use the Buy Now feature, please specify a value higher than the reserve";
		public const string ERR_BUY_NOW_NOT_ENOUGH_MONEY = "You don't have enough money in your bank to buy this item";
		public const string ERR_BANKBOX_FULL =
			"You don't have enough space in your bank to make this deposit. Please free some space and try again.";
		
		public const string ADMIN_TITLE = "Auction Control Panel";
		public const string ADMIN_PROPERTIES = "Properties";
		public const string ADMIN_ACCOUNT_FMT = "Account : {0}";
		public const string ADMIN_OWNER_FMT = "Auction Owner : {0}";
		public const string ADMIN_ONLINE = "Online";
		public const string ADMIN_OFFLINE = "Offline";
		public const string ADMIN_AUCTIONED_ITEM = "Auctioned Item";
		public const string ADMIN_BRING = "Place it in your backpack";
		public const string ADMIN_PUT_BACK = "Put the item back into the system";
		public const string ADMIN_AUCTION = "Auction";
		public const string ADMIN_END_AUCTION = "End auction now";
		public const string ADMIN_CLOSE_RETURN = "Close and return item to the owner";
		public const string ADMIN_CLOSE_BRING = "Close and put the item in your pack";
		public const string ADMIN_CLOSE_DELETE = "Close and delete the item";
		public const string ADMIN_OPEN_PANEL_BUTTON = "Auction Staff Panel";

		public const string COMMIT_SUCCESS_FMT =
			"{0} gold coins have been withdrawn from your bank account as payment for this service."; 
		public const string COMMIT_FAILED_FMT = "You don't have enough gold in your bank to pay for this serice. The cost of this auction is: {0}.";

		public const string LATE_BID_EXTENSION =
			"Your bid has been placed too close to the auction deadline so the auction duration has been extended to accept further bids.";

		public const string CONTAINER_FMT = "Container: {0}";
	}
}
