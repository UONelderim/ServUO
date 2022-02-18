#region References

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.ContextMenus;
using Server.Items;
using Server.Targets;

#endregion

namespace Server.Mobiles
{
	public class Fence : AnimalTrainer
	{
		#region Baza

		public enum FenceType
		{
			Fence,
			Smuggler,
			Stable,
			Barkeeper
		}

		private FenceType m_Type;
		private List<SBInfo> m_SBInfos = new List<SBInfo>();

		protected override List<SBInfo> SBInfos
		{
			get { return m_SBInfos; }
		}

		public override NpcGuild NpcGuild
		{
			get { return NpcGuild.ThievesGuild; }
		}

		public override bool HandlesOnSpeech(Mobile from)
		{
			return true;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public FenceType Type
		{
			get { return m_Type; }
			set
			{
				if (m_Type != value)
				{
					m_Type = value;
					m_SBInfos = new List<SBInfo>();
					LoadSBInfo();
					ClearSkills();

					switch (m_Type)
					{
						case FenceType.Fence:
							Title = "- paser";
							SetSkill(SkillName.DetectHidden, 85.0, 100.0);
							SetSkill(SkillName.Hiding, 60.0, 80.0);
							SetSkill(SkillName.Snooping, 80.0, 100.0);
							break;
						case FenceType.Smuggler:
							Title = "- szmugler";
							break;
						case FenceType.Barkeeper:
							Title = "- barman";
							break;
						case FenceType.Stable:
							Title = "- parobek";
							break;
					}
				}
			}
		}

		[Constructable]
		public Fence()
		{
			Title = "- paser";
			m_Type = FenceType.Fence;
			Smuggler = true;
		}

		public Fence(Serial serial) : base(serial)
		{
		}

		public override void InitSBInfo()
		{
			switch (m_Type)
			{
				case FenceType.Fence:
					m_SBInfos.Add(new SBFence());
					break;
				case FenceType.Smuggler:
					m_SBInfos.Add(new SBSmuggler());
					break;
				case FenceType.Barkeeper:
					m_SBInfos.Add(new SBRedBarkeeper());
					break;
				case FenceType.Stable:
					m_SBInfos.Add(new SBStable());
					break;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
			writer.Write((int)m_Type);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			Type = (FenceType)reader.ReadInt();
		}

		private void ClearSkills()
		{
			Skills skills = this.Skills;

			for (int i = 0; i < skills.Length; i++)
			{
				skills[i].Base = 0;
			}
		}

		private void StablingHandle(SpeechEventArgs e)
		{
			if (Regex.IsMatch(e.Speech, "opiek", RegexOptions.IgnoreCase))
			{
				e.Handled = true;
				if (e.Speech.ToLower() == "opiek" || Regex.IsMatch(e.Speech, "^opiek..?$", RegexOptions.IgnoreCase))
					OnLazySpeech();
				else BeginStable(e.Mobile);
			}
			else if (Regex.IsMatch(e.Speech, "oddaj", RegexOptions.IgnoreCase) &&
			         Regex.IsMatch(e.Speech, "wszyst", RegexOptions.IgnoreCase))
			{
				e.Handled = true;
				Claim(e.Mobile);
			}
			else if (Regex.IsMatch(e.Speech, "oddaj", RegexOptions.IgnoreCase))
			{
				e.Handled = true;
				if (e.Speech.ToLower() == "oddaj") OnLazySpeech();
				else BeginClaimList(e.Mobile);
			}
			else if (Regex.IsMatch(e.Speech, "zmniejsz", RegexOptions.IgnoreCase))
			{
				e.Handled = true;
				if (e.Speech.ToLower() == "zmniejsz" ||
				    Regex.IsMatch(e.Speech, "^zmniejsz..?$", RegexOptions.IgnoreCase)) OnLazySpeech();
				else e.Mobile.Target = new ShrinkTarget(this);
			}
		}

		public void CheckOperation(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;

			if (!CheckVendorAccess(from))
				return;

			string[] split = e.Speech.Split(' ');

			if (split.Length >= 2)
			{
				int amount;

				try
				{
					amount = Convert.ToInt32(split[1]);
				}
				catch
				{
					return;
				}

				if (amount < 5000)
				{
					Say(00505089); // Nie bede ryzykowac dla tak marnej kwoty!
				}
				else if (amount > 1000000)
				{
					Say(00505090); // Tyle zlota? Nie zaryzykuje!
				}
				else
				{
					int charge = (int)(amount * 0.2);

					BankCheck check = new BankCheck(amount);

					if (!Banker.Withdraw(from, amount + charge))
					{
						Say(00505091); // "Glupcze! Nie masz dosc zlota w banku!"
						check.Delete();
					}
					else
					{
						if (!e.Mobile.Backpack.TryDropItem(e.Mobile, check, false))
							Say(00505093,
								charge.ToString()); // Trzymaj ten czek! To bylo spore ryzyko... warte {0} sztuk zlota
						else
						{
							check.Location = e.Mobile.Location;
							Emote(00505092); // Rzuca czek na ziemie
							Say(00505093,
								charge.ToString()); // Trzymaj ten czek! To bylo spore ryzyko... warte {0} sztuk zlota
						}
					}
				}
			}
		}

		public void WhitdrawOperation(SpeechEventArgs e, double charge)
		{
			Mobile from = e.Mobile;
			if (!CheckVendorAccess(from))
				return;

			string[] split = e.Speech.Split(' ');

			if (split.Length >= 2)
			{
				int amount;

				try
				{
					amount = Convert.ToInt32(split[1]);
				}
				catch
				{
					return;
				}

				if (amount > 5000)
				{
					Say(500381); // Thou canst not withdraw so much at one time!
				}
				else if (amount > 0)
				{
					int payment = (int)(amount * charge);

					if (!Banker.Withdraw(from, amount + payment))
					{
						Say(500384); // Ah, art thou trying to fool me? Thou hast not so much gold!
					}
					else
					{
						e.Mobile.AddToBackpack(new Gold(amount));

						Say(00505103, payment.ToString()); // "Bierz to zloto! Ma fatyga kosztowala {0} sztuk zlota"
					}
				}
			}
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			base.OnSpeech(e);

			if (e.Handled || !e.Mobile.InRange(this, 3))
				return;

			if (!CheckVendorAccess(e.Mobile))
				return;

			int[] keywords = e.Keywords;
			string speech = e.Speech.ToLower();
			Mobile from = e.Mobile;

			#region Stajenny

			if (Type == FenceType.Stable)
			{
				#region Zwierzeta

				StablingHandle(e);

				#endregion
			}

			#endregion

			#region Paser (+Stajenny)

			if (Type == FenceType.Fence)
			{
				#region Zwierzeta

				StablingHandle(e);

				#endregion

				#region "Czek"

				if (Regex.IsMatch(e.Speech, "czek", RegexOptions.IgnoreCase))
				{
					e.Handled = true;
					CheckOperation(e);
				}

				#endregion

				#region "Pobrac"

				else if (Regex.IsMatch(e.Speech, "pobrac", RegexOptions.IgnoreCase))
				{
					e.Handled = true;
					WhitdrawOperation(e, 0.1); // 10% dla pasera
				}

				#endregion

				#region "Saldo"

				else if (Regex.IsMatch(e.Speech, "saldo", RegexOptions.IgnoreCase))
				{
					if (CheckVendorAccess(from))
					{
						e.Handled = true;
						if (e.Speech.ToLower() == "saldo") OnLazySpeech();
						else
						{
							BankBox box = e.Mobile.BankBox;

							if (box != null && Banker.Withdraw(from, 100))
								this.Say(00505104,
									(Banker.GetBalance(from) + 100)
									.ToString()); // Masz w banku {0} zlota... Minus 100 za moja fatyge
							else
								this.Say(00505105); // Nie masz nawet zlota, by mnie oplacic!
						}
					}
				}

				#endregion
			}

			#endregion

			#region Szmugler

			else if (Type == FenceType.Smuggler)
			{
				#region "Pobrac"

				if (Regex.IsMatch(e.Speech, "pobrac", RegexOptions.IgnoreCase))
				{
					e.Handled = true;
					WhitdrawOperation(e, 0.2); // 20% dla szmuglera
				}

				#endregion
			}

			#endregion

			#region Barkeeper

			else if (Type == FenceType.Barkeeper)
			{
				#region Nowa tozsamosc

				if ((Regex.IsMatch(e.Speech, "nowa", RegexOptions.IgnoreCase) ||
				     Regex.IsMatch(e.Speech, "zmieni", RegexOptions.IgnoreCase)) &&
				    Regex.IsMatch(e.Speech, "tozsamosc", RegexOptions.IgnoreCase))
				{
					//e.Handled = true;
					//ProcessAmnesty(e.Mobile);
				}

				#endregion

				#region Ujawnij tozsamosc

				else if (Regex.IsMatch(e.Speech, "ujawnic", RegexOptions.IgnoreCase)
				         && Regex.IsMatch(e.Speech, "tozsamosc", RegexOptions.IgnoreCase))
				{
					//e.Handled = true;
					//PlayerMobile pm = e.Mobile as PlayerMobile;
					//
					//if (pm != null && pm.HiddenCriminal)
					//    e.Mobile.SendGump(new ConfirmEndOfAmnestyGump(pm));
				}

				#endregion
			}

			#endregion
		}

		public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if (from.Alive && CheckVendorAccess(from))
			{
				switch (Type)
				{
					case FenceType.Stable:
					{
						#region Stajnia

						list.Add(new StableEntry(this, from));

						if (from.Stabled.Count > 0)
							list.Add(new ClaimAllEntry(this, from));

						#endregion

						break;
					}
					case FenceType.Fence:
					{
						#region Stajnia

						list.Add(new StableEntry(this, from));

						if (from.Stabled.Count > 0)
							list.Add(new ClaimAllEntry(this, from));

						#endregion

						break;
					}
					case FenceType.Smuggler:
					{
						break;
					}
					case FenceType.Barkeeper:
					{
						#region Tozsamosc

						PlayerMobile pm = from as PlayerMobile;
						//if ( pm != null && pm.HiddenCriminal )
						//    list.Add( new UnhideEntry( this, pm ) );
						//else if ( pm != null )
						//    list.Add( new AmnestyEntry( this, from ) );

						#endregion

						break;
					}
				}
			}

			base.AddCustomContextEntries(from, list);
		}

		#endregion

		#region Stajenny

		// Wszystko korzysta z klasy AnimalTrainer

		#endregion

		#region Bankier

		public bool OperationDepositGold(Mobile from, double chargePercent, Item dropped)
		{
			if (!(dropped is Gold))
				return false;

			if (dropped.Amount < 1000)
			{
				Say(505696); // Nie rozsmieszaj mnie! Nie przyjmuje na przechowanie mniej niz 1000 sztuk zlota
				return false;
			}

			int charge = (int)(dropped.Amount * chargePercent);

			if (Banker.Deposit(from, dropped.Amount - charge))
			{
				Say(505697, charge.ToString()); // Zabieram {0} sztuk zlota za fatyge
				from.SendSound(from.BankBox.GetDroppedSound(dropped));
				return true;
			}

			Say(505698); // Nie ma miejsca w banku na wiecej zlota!
			return false;
		}

		public bool OperationDepositCheck(Mobile from, double chargePercent, Item dropped)
		{
			if (!(dropped is BankCheck))
				return false;

			int amount = (dropped as BankCheck).Worth;

			int charge = (int)(amount * chargePercent);

			if (Banker.Deposit(from, amount - charge))
			{
				Say(505697, charge.ToString()); // Zabieram {0} sztuk zlota za fatyge
				from.SendSound(from.BankBox.GetDroppedSound(dropped));
				return true;
			}

			Say(505698); // Nie ma miejsca w banku na wiecej zlota!
			return false;
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (base.OnDragDrop(from, dropped))
				return true;

			switch (Type)
			{
				case FenceType.Stable:
				{
					break;
				}
				case FenceType.Fence:
				{
					if (dropped is Gold)
					{
						return OperationDepositGold(from, 0.1, dropped);
					}

					if (dropped is BankCheck)
					{
						return OperationDepositCheck(from, 0.2, dropped);
					}

					break;
				}
				case FenceType.Smuggler:
				{
					if (dropped is Gold)
					{
						return OperationDepositGold(from, 0.2, dropped);
					}

					if (dropped is BankCheck)
					{
						Say(505700); // Na co mi to?! Tylko zloto!
						return false;
					}

					break;
				}
				case FenceType.Barkeeper:
				{
					break;
				}
			}

			return false;
		}

		#endregion

		#region Amnestia

		/*
		private class AmnestyEntry : ContextMenuEntry
		{
		    private Fence m_Mobile;
		    private Mobile m_From;

		    public AmnestyEntry( Fence mobile, Mobile from ) : base( 50008, 4 ) // Nowa tozsamosc
		    {
		        m_Mobile = mobile;
		        m_From = from;
		    }

		    public override void OnClick()
		    {
		        m_From.Say( 00505107 ); // Ciiii... chce ukryc swa prawdziwa tozsamosc
		        m_Mobile.ProcessAmnesty( m_From );
		    }
		}
		
		private class UnhideEntry : ContextMenuEntry
		{
		    private Fence m_Mobile;
		    private PlayerMobile m_From;

		    public UnhideEntry( Fence mobile, PlayerMobile from ) : base( 50009, 4 ) // Ujawnij tozsamosc
		    {
		        m_Mobile = mobile;
		        m_From = from;
		    }

		    public override void OnClick()
		    {
		        if ( m_From != null )
		        {
		            string args = String.Format( "{0}\t{1}", m_From.HiddenRealName, m_From.Name );
		            
		            m_From.Say( 00505122, args ); // Mam dosc ukrywania! Nazywam sie ~1_NAME~, a nie ~2_NAME~!
		            m_From.SendGump( new ConfirmEndOfAmnestyGump( m_From ) );
		        }
		    }
		}
		
		public void ProcessAmnesty( Mobile from )
		{
		    try
		    {
		        if ( from is PlayerMobile && ( ( PlayerMobile ) from ).HiddenCriminal )
		        {
		            Emote( 00505108 ); // *Smieje sie gromko*
		            Say( 00505109, ( ( PlayerMobile ) from ).HiddenRealName ); // "żarty sobie robisz ~1_NAME~!"
		            return;
		        }    
		        
		        if ( !from.Player )
		        {
		            Emote( 00505110 ); // *Rechoce*
		            Say( 00505111 ); // Nie potrzebujesz nowej tozsamosci!
		            return;
		        }
		        
		        int price = ( from.Kills > 5 ) ? from.Kills * 10000 : 50000;
		        
		        Say( 00505112, price.ToString() ); // "Nowa tozsamosc kosztowac Cie bedzie ~1_GOLD~ sztuk zlota"
		        
		        if ( Banker.GetBalance( from ) < price )
		        {
		            Emote( 00505113 ); // *Szczerzy zeby*
		            Say( 00505114 ); // "Ale przeciez tyle nie masz , prawda?"
		            return;
		        }
		        
		        // Zbieramy niezbedne informacje
		        
		        Say( 00505115 ); // "I jesli jestes chetny, to przystapmy do interesow"
		        
		        string name = RaceHandler.GetRandomName( from.Race, from.Female );
		        
		        from.SendLocalizedMessage( 00505116, name, 0x25 ); // Twe nowe imie to ~1_NAME~. Wpisz "Zgadzam sie", jesli akceptujesz warunki.
		        from.Prompt = new AmnestyPrompt( from, this, price, name );
		    }    
		    catch ( Exception exc )
		    {
		        Console.WriteLine( exc.ToString() );
		    }
		}
		
		public class AmnestyPrompt : Prompt
		{
		    private Mobile m_From;
		    private Mobile m_Vendor;
		    private int m_Price;
		    private string m_Name;
		    
		    public AmnestyPrompt( Mobile from, Mobile vendor, int price, string name )
		    {
		        m_From = from;
		        m_Vendor = vendor;
		        m_Price = price;
		        m_Name = name;
		    }
	
		    public override void OnResponse( Mobile from, string text )
		    {
		        if ( !text.ToLower().Equals( "zgadzam sie" ) )
		        {
		            from.SendLocalizedMessage( 00505117, m_Name, 0x25 ); // ~1_NAME~ nie podoba sie?
		            
		            m_Name = RaceHandler.GetRandomName( m_From.Race, m_From.Female );
		            
		            from.SendLocalizedMessage( 00505118, m_Name, 0x25 ); // To moze ~1_NAME~? Wpisz "Zgadzam sie" dla akceptacji warunkow.
		            from.Prompt = new AmnestyPrompt( m_From, m_Vendor, m_Price, m_Name );
		        }
		        else
		        {
		            if ( from is PlayerMobile && Banker.Withdraw( m_From, m_Price ) )
		            {
		                ( ( PlayerMobile ) from ).OnAmnestyBegin( m_Name );
		                from.SendLocalizedMessage( 00505119, "", 0x25 ); // Nowa tozsamosc - nowe zycie!
		                m_Vendor.Say( 00505120 ); // "Dobrze sie z Toba robi interesy!"
		            }
		            else
		                m_Vendor.Say( 00505091 ); // Glupcze! Nie masz dosc zlota w banku!
		        }
		    }
	
		    public override void OnCancel( Mobile from )
		    {
		        from.SendLocalizedMessage( 502980 ); // Message entry cancelled.
		        m_Vendor.Say( 00505121 ); // "Nie chesz, nie musisz. Tylkio nie zawracaj mi wiecej glowy!"
		    }
		}
		*/

		#endregion
	}
}
