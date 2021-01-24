using System;
using System.Collections.Generic;
using System.IO;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
	public class SmallHunterBOD : SmallBOD
	{
		public override BODType BODType => BODType.Hunter;

		public int Level => Graphic;

		private static readonly TimeSpan m_HuntProtection = TimeSpan.FromSeconds( 15.0 );
		
		public override int ComputeFame()
		{
			return 0;
		}

		public override int ComputeGold()
		{
			return HunterRewardCalculator.Instance.ComputeGold( this );
		}

		public override List<Item> ComputeRewards( bool full )
		{
			List<Item> list = new List<Item>();

			RewardGroup rewardGroup = HunterRewardCalculator.Instance.LookupRewards(HunterRewardCalculator.Instance.ComputePoints(this));

			if (rewardGroup != null) {
				if (full) {
					for (int i = 0; i < rewardGroup.Items.Length; ++i) {
						Item item = rewardGroup.Items[i].Construct();

						if (item != null)
							list.Add(item);
					}
				} else {
					RewardItem rewardItem = rewardGroup.AcquireItem();

					if (rewardItem != null) {
						Item item = rewardItem.Construct();

						if (item != null)
							list.Add(item);
					}
				}
			}

			return list;
		}
		
		public static SmallHunterBOD CreateRandomFor( Mobile m, double theirSkill )
		{
			SmallBulkEntry[] entries;
			double propab1 = 0.0;   // prawdopodobienstwo zlecenia o poziomie trudnosci 1
			double propab2 = 0.0;   // prawdopodobienstwo (zalezne!) zlecenia o poziomie trudnosci 2
			
			if( theirSkill >= 105.1 )
			{
				propab1 = 0.22; // 9/(9+44+29)
				propab2 = 0.68;  // 44/(44+29)
			}
			else if( theirSkill >= 90.1 )
			{
				propab1 = 0.26; // 22/(22+50+13)
				propab2 = 0.8;  // 50/(50+13)
			}
			else if( theirSkill >= 70.1 )
			{
				propab1 = 0.78; // 70/(70+20+0)
				propab2 = 1.0;  // 20/(20+0)
			}
			else
			{
				propab1 = 1.0;
				propab2 = 0.0;
			}
			
			double rand = Utility.RandomDouble();
			
			if( propab1 >= rand )
				entries = SmallBulkEntry.Hunter1;
			else if( propab2 >= rand )
				entries = SmallBulkEntry.Hunter2;
			else
				entries = SmallBulkEntry.Hunter3;

			if ( entries.Length > 0 )
			{
				int amountMax;

				if ( theirSkill >= 70.1 )
					amountMax = Utility.RandomList( 10, 15, 20, 20 );
				else if ( theirSkill >= 50.1 )
					amountMax = Utility.RandomList( 10, 15, 15, 20 );
				else
					amountMax = Utility.RandomList( 10, 10, 15, 20 );

				SmallBulkEntry entry = entries[Utility.Random( entries.Length )];
				
				//Logowanie
				if ( !Directory.Exists( "Logs" ) )
						Directory.CreateDirectory( "Logs" );

				string directory = "Logs/HunterBulkOrders";

				if ( !Directory.Exists( directory ) )
					Directory.CreateDirectory( directory );

				try
				{
					StreamWriter m_Output = new StreamWriter( Path.Combine( directory, "GivenHunterBODs.log" ), true );
					m_Output.AutoFlush = true;
					string log = String.Format("Small\t{0}\t{1}\t{2}", DateTime.Now, entry.Type.ToString(), amountMax.ToString());
					m_Output.WriteLine( log );
					m_Output.Flush();
					m_Output.Close();
				}
				catch
				{
				}

				if ( entry != null )
					return new SmallHunterBOD( entry, amountMax );
			}

			return null;
		}

		private SmallHunterBOD( SmallBulkEntry entry, int amountMax )
		{
			this.Hue = 0xA8E;
			this.AmountMax = amountMax;
			this.Type = entry.Type;
			this.Number = entry.Number;
			this.Graphic = entry.Graphic;
		}

		[Constructable]
		public SmallHunterBOD()
		{
			SmallBulkEntry[] entries;
			int prop = Utility.RandomList(1,2,3);
			if( prop == 1 )
				entries = SmallBulkEntry.Hunter1;
			else if( prop == 2 )
				entries = SmallBulkEntry.Hunter2;
			else
				entries = SmallBulkEntry.Hunter3;
			
			if ( entries.Length > 0 )
			{
				int hue = 0xA8E;
				int amountMax = Utility.RandomList( 10, 15, 20 );

				SmallBulkEntry entry = entries[Utility.Random( entries.Length )];

				this.Hue = hue;
				this.AmountMax = amountMax;
				this.Type = entry.Type;
				this.Number = entry.Number;
				this.Graphic = entry.Graphic;
			}
		}

		public SmallHunterBOD( int amountCur, int amountMax, Type type, int number, int graphic, int level)
		{
			this.Hue = 0xA8E;
			this.AmountMax = amountMax;
			this.AmountCur = amountCur;
			this.Type = type;
			this.Number = number;
			this.Graphic = graphic;
		}

		public SmallHunterBOD( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void EndCombine( Mobile from, object o )
		{
			if ( o is Corpse && o != null && (o as Corpse).Owner != null )
			{
				Type objectType = (o as Corpse).Owner.GetType();

				if ( AmountCur >= AmountMax )
				{
					from.SendLocalizedMessage( 1045166 ); // The maximum amount of requested items have already been combined to this deed.
				}
				else if ( Type == null || (objectType != Type && !objectType.IsSubclassOf( Type )) )
				{
					from.SendLocalizedMessage( 1045169 ); // The item is not in the request.
				}
				else if( ((o as Corpse).Owner as BaseCreature).IsChampionSpawn )
				{
					from.SendMessage("Te zwłoki nie mogą zostać oddane.");
				}
				else if ( !CanBeHunted( from, (o as Corpse) ) )
				{
					from.SendMessage("CBH: Te zwłoki nie mogą zostać oddane.");
				} 
				else
				{ 
					(((Corpse)o).Owner as BaseCreature).IsChampionSpawn = true;
					++AmountCur;

					from.SendLocalizedMessage( 1045170 ); // The item has been combined with the deed.

					from.SendGump( new SmallBODGump( from, this ) );

					if ( AmountCur < AmountMax )
						BeginCombine( from );
				}
			}
			else
			{
				from.SendMessage("Te zwłoki są zbyt stare, żebyś mógł je dodać do zamówienia.");
			}
		}
		
		public bool CanBeHunted( Mobile from, Corpse c )
		{
			if( c == null || c.Owner == null )
				return false;
			
			BaseCreature mob = c.Owner as BaseCreature;
			
			if( from is PlayerMobile && from != null && mob != null && !mob.Summoned )
			{
				if( c.TimeOfDeath + m_HuntProtection > DateTime.Now )
				{
					//from.SendMessage("Czas nie ok!");
					List<DamageStore> rights = mob.GetLootingRights( );
					int maxdmg = -1;
					int idx = -1;
					for(int i = 0; i < rights.Count; i++)
					{
						DamageStore ds = rights[ i ];
						if( ds.m_Damage > maxdmg )
						{
								idx = i;
								maxdmg = ds.m_Damage;
						}
					}
                    if(idx < 0)
                        return false;
					DamageStore ds1 = (DamageStore) rights[ idx ];
					//from.SendMessage(mob.IsQuestMonster.ToString());
					if( from == ds1.m_Mobile && !mob.IsChampionSpawn )
						return true;
					else
						return false;
				}
				else
				{
					//from.SendMessage("Po czasie!");
					if( !mob.IsChampionSpawn )
						return true;
					else
						return false;
				}
			
			}
			return false;
		
		}
	}
}
