using Nelderim.Towns;

namespace Server.Mobiles
{
    public abstract partial class BaseVendor
    {
        // Towns

        [CommandProperty( AccessLevel.Administrator )]
        public Towns TownAssigned
        {
            get { return TownsVendor.Get(this).TownAssigned; }
            set { TownsVendor.Get(this).TownAssigned = value; }
        }

        [CommandProperty( AccessLevel.Administrator )]
        public TownBuildingName TownBuildingAssigned
        {
            get { return TownsVendor.Get(this).TownBuildingAssigned; }
            set { TownsVendor.Get(this).TownBuildingAssigned = value; }
        }

        public bool IsAssignedBuildingWorking()
        {
            if ( TownAssigned == Towns.None )
            {
                return true;
            }
            else
            {
                if ( TownDatabase.GetBuildingStatus( TownAssigned, TownBuildingAssigned ) == TownBuildingStatus.Dziala )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void OnLazySpeech()
        {
            string[] responses = new string[] {
                "To nie karczma! Zachowuj sie chamie niemyty!",
                "Heeeeeee?",
                "Nie rozumiem.",
                "To do mnie mamroczesz?",
                "Sam spierdalaj!",
                "Wypad! Bo wezwe straz!",
                "Ta, jasne.",
                "Coraz glupsi ci mieszczanie.",
                "Terefere",
                "Tiruriru",
                "Co tam belkoczesz pod nosem.",
                "Mow wyrazniej bo nie rozumiem.",
                "Masz jakies uposledzenie umyslowe Panie?",
                "Nie rozumiem o co ci chodzi.",
                "A moze tak troche szacunku?"};
            string response = responses[Utility.Random( responses.Length )];
            Say( response );
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Smuggler
        {
            get { return TownsVendor.Get(this).TradesWithCriminals; }
            set { TownsVendor.Get(this).TradesWithCriminals = value; }
        }

        // blokada nietolerancji
        private bool m_IntolerativeBlocked;

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Blocked
        {
            get { return m_IntolerativeBlocked; }
            set { m_IntolerativeBlocked = value; }
        }
    }
}
