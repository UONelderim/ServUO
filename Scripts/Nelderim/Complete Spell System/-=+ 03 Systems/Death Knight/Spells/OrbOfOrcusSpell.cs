using System;
using System.Collections;

namespace Server.Spells.DeathKnight
{
	public class OrbOfOrcusSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new(
				"Kula Smierci", "Orcus Arma",
				218,
				9031
			);

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds( 1 );
		public override int RequiredTithing => 200;
		public override double RequiredSkill => 80.0;
		public override int RequiredMana => 26;

		public OrbOfOrcusSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

		private static Hashtable m_Table = new();

		public override void OnCast()
		{
			if ( Caster.MagicDamageAbsorb > 0 )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( !Caster.CanBeginAction( typeof( OrbOfOrcusSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005385 ); // The spell will not adhere to you at this time.
			}
			else if ( CheckSequence() )
			{
				if ( Caster.BeginAction( typeof( OrbOfOrcusSpell ) ) /*&& CheckFizzle()*/ )
				{
					int value = (int)( GetKarmaPower( Caster ) / 4 );

					Caster.MagicDamageAbsorb = value;

					Caster.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );
					Caster.PlaySound( 0x1E9 );
				}
				else
				{
					Caster.SendLocalizedMessage( 1005385 ); // The spell will not adhere to you at this time.
				}

				FinishSequence();
			}
		}
	}
}
