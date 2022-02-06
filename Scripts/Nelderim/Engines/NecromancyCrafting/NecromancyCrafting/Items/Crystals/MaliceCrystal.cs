using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class MaliceCrystal : Item
	{
		public override string DefaultName
		{
			get { return "Krysztal Zlego Wymiaru"; }
		}

		[Constructable]
		public MaliceCrystal() : base( 0x1F19 )
		{
			Weight = 1.0;
			Hue = 0x506;
		}

		public MaliceCrystal( Serial serial ) : base( serial )
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

			if ( NecroSkill < 100.0 )
			{
				from.SendMessage( "Musisz mieć przynajmniej 100 umiejętności nekromancji, by stworzyć licza." );
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
					typeof( WrappedMageBod ),
					typeof( WrappedLegs ),
					typeof( Phylacery ),
					typeof( Mind ),
					typeof( NecromancerSpellbook )
				},
				new int[]
				{
					1,
					1,
					1,
					1,
					1
				} );

			switch ( res )
			{
				case 0:
				{
					from.SendMessage( "Musisz mieć Zmumifikowany tułów oznaczony runami." );
					break;
				}
				case 1:
				{
					from.SendMessage( "Musisz mieć zmumifikowane nogi." );
					break;
				}
				case 2:
				{
					from.SendMessage( "Musisz mieć filakterium." );
					break;
				}
				case 3:
				{
					from.SendMessage( "Musisz mieć umysł, by kontrolować tę kreaturę." );
					break;
				}
				case 4:
				{
					from.SendMessage( "Musisz mieć księgę nekromancji.");
					break;
				}
				default:
				{
					Vecna g = new Vecna( true, scalar );

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