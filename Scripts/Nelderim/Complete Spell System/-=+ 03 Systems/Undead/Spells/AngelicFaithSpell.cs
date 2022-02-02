using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadAngelicFaithSpell : UndeadSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Demoniczny Awatar", "Deus Vox Uus Terum",
		                                                //SpellCircle.Eighth,
		                                                 269,
		                                                9020,
														Reagent.BlackPearl,
                                                        Reagent.Nightshade,
                                                        Reagent.PigIron
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }
		public override double RequiredSkill{ get{ return 80.0; } }

		public override int RequiredMana{ get{ return 50; } }

		public virtual int PhysResistOffset { get { return -10; } }
		public virtual int FireResistOffset { get { return -10; } }
		public virtual int ColdResistOffset { get { return -10; } }
		public virtual int PoisResistOffset { get { return 10; } }
		public virtual int NrgyResistOffset { get { return 0; } }
		public virtual int FasterCastingRecoveryBonus { get { return 2; } }
		public virtual int LowerManaCostBonus { get { return 10; } }

		private static Hashtable m_Table = new Hashtable();

		public UndeadAngelicFaithSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			                    if (this.Scroll != null)
                        Scroll.Consume();
		}

		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			object[] mods = (object[])m_Table[m];

			if ( mods != null )
			{
				m.RemoveStatMod( ((StatMod)mods[0]).Name );
				m.RemoveStatMod( ((StatMod)mods[1]).Name );
				m.RemoveStatMod( ((StatMod)mods[2]).Name );
				
				
			}

			m_Table.Remove( m );

			m.EndAction( typeof( UndeadAngelicFaithSpell ) );

			m.BodyMod = 0;
			m.HueMod = -1;
		}

		public override bool CheckCast()
		{
            if (!TransformationSpellHelper.CheckCast(Caster, this))
                return false;

            return base.CheckCast();
		}

		public override void OnCast()
		{
            TransformationSpellHelper.OnCast(Caster, this);

            if ( CheckSequence() )
			{
                RemoveEffect(Caster);

				object[] mods = new object[]
				{
					new StatMod( StatType.Str, "[Undead] Str Offset", 10, TimeSpan.Zero ),
					new StatMod( StatType.Dex, "[Undead] Dex Offset", 10, TimeSpan.Zero ),
					new StatMod( StatType.Int, "[Undead] Int Offset", 10, TimeSpan.Zero ),
					
					
				};

				m_Table[Caster] = mods;

				Caster.AddStatMod( (StatMod)mods[0] );
				Caster.AddStatMod( (StatMod)mods[1] );
				Caster.AddStatMod( (StatMod)mods[2] );
			
				
			

				double span = 10.0 /* ClericDivineFocusSpell.GetScalar( Caster )*/;
				new InternalTimer( Caster, TimeSpan.FromMinutes( (int)span ) ).Start();

				IMount mount = Caster.Mount;

				if ( mount != null )
					mount.Rider = null;

				Caster.BodyMod = 130;
				Caster.HueMod = 2874;
				Caster.BeginAction( typeof( UndeadAngelicFaithSpell ) );
				Caster.PlaySound( 0x165 );
				Caster.FixedParticles( 0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head );
			}

            FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_Expire;

			public InternalTimer( Mobile owner, TimeSpan duration ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Owner = owner;
				m_Expire = DateTime.Now + duration;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					UndeadAngelicFaithSpell.RemoveEffect( m_Owner );
					Stop();
				}
			}
		}
	}
}
