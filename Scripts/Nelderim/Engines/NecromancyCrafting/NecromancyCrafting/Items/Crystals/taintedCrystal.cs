using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class   taintedCrystal : Item
	{
		public override string DefaultName
		{
			get { return "zepsuty krysztal"; }
		}

		[Constructable]
		public  taintedCrystal() : base( 0x1F19 )
		{
			Weight = 1.0;
			Hue = 0x503;
		}

		public  taintedCrystal( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			double NecroSkill = from.Skills[SkillName.Necromancy].Value;

			if ( NecroSkill < 20.0 )
			{
				from.SendMessage( "Musisz miec przynajmniej 20 umiejetnosci nekromacji, by stworzyc zombie." );
				return;
			}

			double scalar;

			if ( NecroSkill >= 100.0 )
				scalar = 1.5;
			else if ( NecroSkill >= 90.0 )
				scalar = 1.3;
			else if ( NecroSkill >= 80.0 )
				scalar = 1.1;
			else if ( NecroSkill >= 70.0 )
				scalar = 1.0;
			else
				scalar = 1.0;

			Container pack = from.Backpack;

			if ( pack == null )
				return;

			int res = pack.ConsumeTotal(
				new Type[]
				{
					typeof( RottingBod ),
					typeof( RottingLegs )
				},
				new int[]
				{
					1,
					1
				} );

			switch ( res )
			{
				case 0:
				{
					from.SendMessage( "Musisz miec gnijący tułów." );
					break;
				}
				case 1:
				{
					from.SendMessage( "Musisz miec gnijące nogi." );
					break;
				}
				default:
				{
					ZombieMinion g = new ZombieMinion( true, scalar );

					if ( g.SetControlMaster( from ) )
					{
						Delete();

						g.MoveToWorld( from.Location, from.Map );
						from.PlaySound( 0x241 );
					}

					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}