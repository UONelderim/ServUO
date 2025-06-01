#region References

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nelderim.Towns;
using Server.Nelderim;

#endregion

namespace Server.Mobiles
{
	public abstract partial class BaseVendor
	{
		// Towns
		public virtual bool SupportsBribes => true;

		[CommandProperty(AccessLevel.Administrator)]
		public Towns TownAssigned
		{
			get => TownsVendor.Get(this).TownAssigned;
			set => TownsVendor.Get(this).TownAssigned = value;
		}

		[CommandProperty(AccessLevel.Administrator)]
		public TownBuildingName TownBuildingAssigned
		{
			get => TownsVendor.Get(this).TownBuildingAssigned;
			set => TownsVendor.Get(this).TownBuildingAssigned = value;
		}

		public bool IsAssignedBuildingWorking()
		{
			if (TownAssigned == Towns.None)
			{
				return true;
			}

			if (TownDatabase.GetBuildingStatus(TownAssigned, TownBuildingAssigned) == TownBuildingStatus.Dziala)
			{
				return true;
			}

			return false;
		}

		public void OnLazySpeech()
		{
			string[] responses =
			{
				"To nie karczma! Zachowuj sie chamie niemyty!", "Heeeeeee?", "Nie rozumiem.",
				"To do mnie mamroczesz?", "Sam spierdalaj!", "Wypad! Bo wezwe straz!", "Ta, jasne.",
				"Coraz glupsi ci mieszczanie.", "Terefere", "Tiruriru", "Co tam belkoczesz pod nosem.",
				"Mow wyrazniej bo nie rozumiem.", "Masz jakies uposledzenie umyslowe Panie?",
				"Nie rozumiem o co ci chodzi.", "A moze tak troche szacunku?"
			};
			string response = responses[Utility.Random(responses.Length)];
			Say(response);
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Smuggler
		{
			get => TownsVendor.Get(this).TradesWithCriminals;
			set => TownsVendor.Get(this).TradesWithCriminals = value;
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool RequiresCustomerIdentity => !Smuggler; // domyslnie tylko szmugler obsluzy kapturnika

        // blokada nietolerancji
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Blocked { get; set; }

        //Plotki
        public virtual void SayRumor( Mobile from, SpeechEventArgs e )
        {
            try
            {
                String speech = e.Speech;
                List<RumorRecord> RumorsList = RumorsSystem.GetRumors( this, PriorityLevel.VeryLow );

                if ( RumorsList != null && RumorsList.Count > 0 )
                {
                    RumorsList.Reverse();

                    for ( int index = 0; index < RumorsList.Count; index++ )
                    {
                        RumorRecord rumor = RumorsList[index];

                        String[] keywords = rumor.KeyWord.Split( ' ' );
                        for ( int a = 0; a < keywords.Length; a++ )
                        {
                            if ( Regex.IsMatch( speech, keywords[a].Replace(",",""), RegexOptions.IgnoreCase ) && !e.Handled )
                            {
                                e.Handled = true;
                                Direction = GetDirectionTo( from.Location );

                                if ( !CheckVendorAccess( from ) )
                                    return;

                                from.SendGump( new SayRumorGump( this, rumor ) );

                                Say( 00505131, rumor.Text ); // "Slyszalem ze, ~1_RUMOR~"
                                return;
                            }
                        }
                    }
                }
            }
            catch ( Exception exc )
            {
                Console.WriteLine( exc.ToString() );
            }
        }

        public virtual void SayAboutRumors( Mobile from )
        {
            try
            {
                List<RumorRecord> RumorsList = RumorsSystem.GetRumors( this, PriorityLevel.VeryLow );

                Direction = GetDirectionTo( from.Location );

                if ( !CheckVendorAccess( from ) )
                    return;

                if ( RumorsList == null || RumorsList.Count == 0 )
                {
                    Say("Pfff... nuda, cisza, spokoj... nic do rozmowy przy mlodym winie."); // 
                    return;
                }

                switch ( RumorsList.Count )
                {
                    case 1:
                        Say( from.Female ? 505525 : 505524, ( RumorsList[ 0 ]).Title ); // Co ciekawego? Slyszalas o ___?
                        break;
                    case 2:
                        Say( 505523, String.Format( "{0}\t{1}", ( RumorsList[ 0 ]).Title, (RumorsList[ 1 ]).Title ) ); // Mam dwie nowiny. O ___ i o ___.
                        break;
                    default:
                        string text = "";
                        
                        for( int i=0; i < RumorsList.Count; i++ )
                        {
                            RumorRecord rum = RumorsList[ i ];
        
                            text += String.Format( "{0}{1}{2}", ( i + 1 == RumorsList.Count ) ? " lub o " : " ", rum.Title, ( i + 1 == RumorsList.Count ) ? "" : "," );
                        }
                        
                        Say( 505522, text ); // AH! Mnogosc nowin! Chcesz, opowiem Ci o ___
                        break;
                }

            }
            catch ( Exception exc )
            {
                Console.WriteLine( exc.ToString() );
            }
        }

        private bool NelderimCheckVendorAccess(Mobile from, bool verbose)
        {
	        if (!(from is PlayerMobile))
	        {
		        if(verbose)
					Yell(00505124); // Odejdz!
		        return false;
	        }

	        var pm = from as PlayerMobile;

	        if (pm.AccessLevel > AccessLevel.VIP) //GM
	        {
		        return true;
	        }

	        if (pm.Warmode)
	        {
		        if(verbose)
					Yell(00505125, from.Race.GetName(Cases.Wolacz)); // Odloz bron ~1_RACE~!
		        return false;
	        }
	        
	        if(!CanSee(pm) || !InLOS(pm)) {
		        if(verbose)
					Emote(505163); // Rozglada sie nerwowo
		        return false;
	        }

	        if (!(this is AnimalTrainer))
	        {
		        if (pm.Mounted)
		        {
			        if(verbose)
						Yell(00505126,
							from.Race.GetName(Cases.Wolacz)); // Zejdz z wierzchowca ~1_RACE~ nim sie do mnie odezwiesz!
			        return false;
		        }
	        }

            if (RequiresCustomerIdentity && pm.IdentityHidden) // kaptur sprawia ze npc nie obsluguje postaci
            {
                if (verbose)
                    Say("Nie obsluguje kogos, jesli nie moge spojrzec na jego twarz!");

                return false;
            }

            if ((from.Kills >= 5 || from.Criminal) && !Smuggler)
	        {
		        if(verbose)
					Yell(00505127); // Takich jak ty tu nie obslugujemy!
		        return false;
	        }

	        // Dzialanie budynku
	        if (!IsAssignedBuildingWorking())
	        {
		        if(verbose)
					Say(1063883); // Miasto nie oplacilo moich uslug. Nieczynne.
		        return false;
	        }
	        
	        if (NelderimRegionSystem.ActIntolerativeHarmful(this, from, false))
	        {
		        if (Blocked)
		        {
			        new IntoleranceGuardTimer(from).Start();
			        if(verbose)
						Yell(00505128, from.Race.GetName(Cases.Biernik)); // Mam dosc brudasow szwendajacych sie po okolicy! Straaaaaz!!! Lapac ~1_RACE~!
		        }
		        else
		        {
			        if(verbose)
						Yell(00505129, from.Race.GetName(Cases.Mianownik)); // Kolejny ~1_RACE~ smie zawracac mi glowe! Won!

			        _BlockTimer = new IntoleranceBlockTimer(this);
			        _BlockTimer.Start();
		        }
		        return false;
	        }

	        if (Blocked && Race != from.Race)
	        {
		        _BlockTimer.Stop();
		        _BlockTimer = new IntoleranceBlockTimer(this);
		        _BlockTimer.Start();
		        if(verbose)
			        Yell(00505130, from.Race.GetName(Cases.Wolacz)); // Won! Won! Wooooon! ~1_RACE~!
		        return false;
	        }
	        
	        return true;
        }
        
        private IntoleranceBlockTimer _BlockTimer;
        
        private class IntoleranceBlockTimer : Timer
        {
	        private BaseVendor m_Blocked;

	        public IntoleranceBlockTimer( BaseVendor blocked ) : base( TimeSpan.FromSeconds( 20 + Utility.Random( 340 ) ) )
	        {
		        m_Blocked = blocked;
		        Priority = TimerPriority.FiveSeconds;
		        m_Blocked.Blocked = true;
	        }

	        protected override void OnTick()
	        {
		        try
		        {
			        if ( !m_Blocked.Deleted )
			        {
				        m_Blocked.Blocked = false;
				        m_Blocked.Say( 00505141 ); // Mam nadzieje, ze nie bede mial wiecej niechcianych klientow!
			        }
		        }
		        catch ( Exception e )
		        {
			        Console.WriteLine( e.ToString() );
		        }
	        }
        }

        private class IntoleranceGuardTimer : Timer
        {
	        private Mobile m_target;

	        public IntoleranceGuardTimer( Mobile target) : base( TimeSpan.FromSeconds( 20 ) )
	        {
		        m_target = target;
		        Priority = TimerPriority.FiveSeconds;
		        target.SendLocalizedMessage( 00505144, "", 0x25 ); // Straz niezdrowo sie Toba interesuje! Lepiej zejdz jej z oczu!
	        }

	        protected override void OnTick()
	        {
		        try
		        {
			        if ( !m_target.Deleted )
			        {
				        m_target.Criminal = true;
				        m_target.SendLocalizedMessage( 505133, "", 0x25 ); // Zdaje sie, ze popadles w tarapaty! Kryminalisto!
			        }
		        }
		        catch ( Exception e )
		        {
			        Console.WriteLine( e.ToString() );
		        }
	        }
        }
        
        public override void OnRegionChange(Region Old, Region New)
        {
	        base.OnRegionChange(Old, New);
	        NelderimRegionSystem.OnRegionChange(this, Old, New);
        }
        
        protected override void OnCreate()
		{
	        base.OnCreate();
	        InitOutfit();
		}

        public override bool UseLanguages => true;
	}
}
