using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells.Necromancy;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericPurgeSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Czystka", "Repurgo",
		                                                //SpellCircle.Second,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

		public override int RequiredTithing{ get{ return 5; } }
		public override double RequiredSkill{ get{ return 70.0; } }
		public override int RequiredMana{ get{ return 20; } }

		public ClericPurgeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckBSequence( m, false ) )
			{
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0xF6 );
				m.PlaySound( 0x1F7 );
				m.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );

				IEntity from = new Entity( Serial.Zero, new Point3D( m.X, m.Y, m.Z - 10 ), Caster.Map );
				IEntity to = new Entity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 50 ), Caster.Map );
				Effects.SendMovingParticles( from, to, 0x2255, 1, 0, false, false, 13, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

				StatMod mod;

				mod = m.GetStatMod( "[Magic] Str Curse" );
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod("[Magic] Str Curse");

					mod = m.GetStatMod("[Magic] Dex Curse");
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod("[Magic] Dex Curse");

					mod = m.GetStatMod("[Magic] Int Curse");
					if ( mod != null && mod.Offset < 0 )
						m.RemoveStatMod("[Magic] Int Curse");


				m.Paralyzed = false;
				m.CurePoison( Caster );

				EvilOmenSpell.CheckEffect( m );
				StrangleSpell.RemoveCurse( m );
				BloodOathSpell.RemoveCurse( m );
				MindRotSpell.ClearMindRotScalar( m );
				CorpseSkinSpell.RemoveCurse( m );

				BuffInfo.RemoveBuff( m, BuffIcon.Clumsy );
					BuffInfo.RemoveBuff( m, BuffIcon.FeebleMind );
					BuffInfo.RemoveBuff( m, BuffIcon.Weaken );
					BuffInfo.RemoveBuff( m, BuffIcon.Curse );
					BuffInfo.RemoveBuff( m, BuffIcon.MassCurse );
					BuffInfo.RemoveBuff( m, BuffIcon.Mindrot );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ClericPurgeSpell m_Owner;

			public InternalTarget( ClericPurgeSpell owner ) : base( 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
