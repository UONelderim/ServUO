using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericHammerOfFaithSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Topór Wiary", "Malleus Terum",
		                                                //SpellCircle.Fifth,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

		public override int RequiredTithing{ get{ return 20; } }
		public override double RequiredSkill{ get{ return 40.0; } }

		public override int RequiredMana{ get{ return 14; } }

		public ClericHammerOfFaithSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Item weap = new HammerOfFaith( Caster );

				Caster.AddToBackpack( weap );
				Caster.SendMessage( "W Twym plecaku pojawia się magiczna broń." );

				Caster.PlaySound( 0x212 );
				Caster.PlaySound( 0x206 );

				Effects.SendLocationParticles( EffectItem.Create( Caster.Location, Caster.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 0x47D, 2, 9962, 0 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( Caster.X, Caster.Y, Caster.Z - 7 ), Caster.Map, EffectItem.DefaultDuration ), 0x37C4, 1, 29, 0x47D, 2, 9502, 0 );
			}
		}

		[FlipableAttribute( 0x13B0, 0x13B0 )]
		private class HammerOfFaith : BaseBashing
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.CrushingBlow; } }

			public override int AosStrengthReq{ get{ return 10; } }
			public override int AosMinDamage{ get{ return 17; } }
			public override int AosMaxDamage{ get{ return 18; } }
			public override int AosSpeed{ get{ return 28; } }

			public override int OldStrengthReq{ get{ return 40; } }
			public override int OldMinDamage{ get{ return 8; } }
			public override int OldMaxDamage{ get{ return 36; } }
			public override int OldSpeed{ get{ return 31; } }

			public override int InitMinHits{ get{ return 255; } }
			public override int InitMaxHits{ get{ return 255; } }

			public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash2H; } }

			[Constructable]
			public HammerOfFaith( Mobile owner ) : base( 0x13B0 )
			{
				m_Owner = owner;
				Weight = 10.0;
				Layer = Layer.TwoHanded;
				Hue = 0x481;
				BlessedFor = owner;
				Slayer = SlayerName.Silver;
				Attributes.WeaponDamage = 30;
				WeaponAttributes.UseBestSkill = 1;
				Name = "Topór Wiary";

				double time = ( owner.Skills[SkillName.Anatomy].Value / 20.0 ) * ClericDivineFocusSpell.GetScalar( owner );
				m_Expire = DateTime.Now + TimeSpan.FromMinutes( (int)time );
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
			{
				fire = cold = pois = nrgy = 0;
				phys = 100;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return true;
			}

			public void Remove()
			{
				m_Owner.SendMessage( "Broń powoli się rozpływa." );
				Delete();
			}

			public HammerOfFaith( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}

		private class InternalTimer : Timer
		{
			private HammerOfFaith m_Hammer;
			private DateTime m_Expire;

			public InternalTimer( HammerOfFaith hammer, DateTime expire ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Hammer = hammer;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					m_Hammer.Remove();
					Stop();
				}
			}
		}
	}
}
