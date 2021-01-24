using System;
using Server;
using Server.Network;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Engines.Apiculture;
using Server.Targeting;

namespace Server.Items
{
	public class apiLargeWaxPot : Item
	{
		public static readonly int MaxWax = 999; //the maximum amount the pot can hold

		private int m_UsesRemaining;
		private int m_Beeswax;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MeltedBeeswax
		{
			get { return m_Beeswax; }
			set { if(value<0)value=0;if(value>MaxWax)value=MaxWax;m_Beeswax = value; InvalidateProperties(); }
		}

		[Constructable]
		public apiLargeWaxPot() : this( 50 )
		{
		}
		
		[Constructable]
		public apiLargeWaxPot( int uses ) : base( 2541 )
		{
			m_UsesRemaining = uses;
			Name = "Duzy garnek na wosk";
			Weight = 5.0;
			m_Beeswax = 0;
		}

		public apiLargeWaxPot( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~

			if( MeltedBeeswax < 1 )
				list.Add( 1049644 , "Pusty" );
			else
				list.Add( 1060663,"{0}\t{1}" ,"Stopiony czysty wosk", MeltedBeeswax.ToString() );
		}

		public virtual void DisplayDurabilityTo( Mobile m )
		{
			LabelToAffix( m, 1017323, AffixType.Append, ": " + m_UsesRemaining.ToString() ); // Durability
		}

		public override void OnAosSingleClick( Mobile from )
		{
			DisplayDurabilityTo( from );

			base.OnAosSingleClick( from );
		}

		public override void OnDoubleClick(Mobile from)
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				from.PrivateOverheadMessage( 0, 1154, false,  "Wybierz czysty wosk pszczeli by go stopic lub ten garnek by wyjac z niego wosk.", from.NetState );
				BeginAdd( from );
			}
		}

		public void BeginAdd( Mobile from )
		{
			from.Target = new AddPureWaxTarget( this );
		}

		public void EndAdd( Mobile from, object o )
		{
			if ( o is Item && ((Item)o).IsChildOf( from.Backpack ) )
			{
				if( o is PureRawBeeswax )
				{
					//error checking
					if ( UsesRemaining < 1 )
					{
						from.PrivateOverheadMessage( 0, 1154, false,  "Ten garnek jest zbyt uszkodzony, by moc go nadal uzywac.", from.NetState );
						return;
					}
					else if ( m_Beeswax >= MaxWax )
					{
						from.PrivateOverheadMessage( 0, 1154, false,  "Ten garnek nie zmiesci wiecej wosku.", from.NetState );
						return;
					}
					else if( !BeeHiveHelper.Find( from, BeeHiveHelper.m_HeatSources ) )
					{
						from.PrivateOverheadMessage( 0, 1154, false,  "Musisz byc blisko zrodla ciepla, by moc topic wosk.", from.NetState );
						return;
					}

                    PureRawBeeswax wax = (PureRawBeeswax)o;

					if( (wax.Amount + MeltedBeeswax) > MaxWax )
					{
						wax.Amount -= (MaxWax - MeltedBeeswax);
						MeltedBeeswax = MaxWax;
					}
					else
					{
						MeltedBeeswax += wax.Amount;
						wax.Delete();
					}
					
					from.PrivateOverheadMessage( 0, 1154, false,  "Topisz wosk i mieszasz go w garnku.", from.NetState );

					this.ItemID = 5162; //change the graphic					

					from.PlaySound( 43 ); //bellow sound
					//from.PlaySound( 0x21 ); //bubbling sound

					UsesRemaining--;

					if ( MeltedBeeswax < MaxWax )
						BeginAdd( from );
				}
				else if ( o == this )
				{
					//empty the pot
					if( MeltedBeeswax < 1 )
						from.PrivateOverheadMessage( 0, 1154, false, "W garnku nie ma wosku.", from.NetState );
					else
					{
						Item wax = new PureRawBeeswax( MeltedBeeswax );

						if ( !from.PlaceInBackpack( wax ) )
						{
							wax.Delete();
							from.PrivateOverheadMessage( 0, 1154, false,  "Nie masz miejsca w plecaku, by wyciagnac wosk!", from.NetState );
							return;
						}

						MeltedBeeswax = 0;
					
						ItemID = 2541; //empty pot

						from.PrivateOverheadMessage( 0, 1154, false,  "Oprozniles garnek i umiesciles czysty wosk pszczeli w plecaku.", from.NetState );
					}
				}
				else
					from.PrivateOverheadMessage( 0, 1154, false,  "Mozesz topic jedynie czysty wosk pszczeli w tym garnku.", from.NetState );
			}
			else
			{
				from.PrivateOverheadMessage( 0, 1154, false,  "Wosk musi znajdowac sie w twoim plecaku by moc go stopic.", from.NetState );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_UsesRemaining );
			writer.Write( (int) m_Beeswax );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_UsesRemaining = reader.ReadInt();
					m_Beeswax = reader.ReadInt();
					break;
				}
			}
		}
	}

	public class AddPureWaxTarget : Target
	{
		private apiLargeWaxPot m_pot;

		public AddPureWaxTarget( apiLargeWaxPot pot ) : base( 18, false, TargetFlags.None )
		{
			m_pot = pot;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_pot.Deleted || !m_pot.IsChildOf( from.Backpack ) )
				return;

			m_pot.EndAdd( from, targeted );
		}
	}
}
