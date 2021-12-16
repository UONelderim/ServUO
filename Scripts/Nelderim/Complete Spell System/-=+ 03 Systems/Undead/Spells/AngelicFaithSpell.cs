using System;
using System.Collections;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadAngelicFaithSpell : UndeadSpell, ITransformationSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Demoniczny Awatar", "Deus Vox Uus Terum",
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

		public int Body
		{
			get { return 130; }
		}
		public int Hue
		{
			get { return 2874; }
		}
		public virtual int PhysResistOffset { get{ return 0; } }
		public virtual int FireResistOffset { get{ return 0; } }
		public virtual int ColdResistOffset { get{ return 0; } }
		public virtual int PoisResistOffset { get{ return 0; } }
		public virtual int NrgyResistOffset { get{ return 0; } }
		public double TickRate
		{
			get { return 600; }
		}
		public void OnTick(Mobile m)
		{
			RemoveEffect(m);
		}

		public void DoEffect(Mobile m)
		{
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
			
			Caster.PlaySound( 0x165 );
			Caster.FixedParticles( 0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head );
		}

		public void RemoveEffect(Mobile m)
		{
			object[] mods = (object[])m_Table[m];

			if ( mods != null )
			{
				m.RemoveStatMod( ((StatMod)mods[0]).Name );
				m.RemoveStatMod( ((StatMod)mods[1]).Name );
				m.RemoveStatMod( ((StatMod)mods[2]).Name );
			}
			m_Table.Remove( m );
		}
		
		private static Hashtable m_Table = new Hashtable();

		public UndeadAngelicFaithSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			if (this.Scroll != null)
                Scroll.Consume();
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

            FinishSequence();
		}
	}
}
