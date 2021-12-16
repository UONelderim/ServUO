using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Chivalry;
using Server.Spells.Fourth;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarCurseRemovalSpell : AvatarSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Reka Mnicha", "Haures Manibus Omnus",
				227,
				9031
			);
		
		        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }
		
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1 ); } }
		public override double RequiredSkill{ get{ return 50.0; } }
		public override int RequiredMana{ get{ return 8; } }
		public override int RequiredTithing{ get{ return 20; } }

		public AvatarCurseRemovalSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( CheckBSequence( m ) && CheckFizzle() )
			{
				SpellHelper.Turn( Caster, m );

				int chance = 0;
				int karma = Caster.Karma * -1;

				if ( karma < -5000 )
					chance = 0;
				else if ( karma < 0 )
					chance = (int) Math.Sqrt( 20000 + karma ) - 122;
				else if ( karma < 5625 )
					chance = (int) Math.Sqrt( karma ) + 25;
				else
					chance = 100;

				if ( chance > Utility.Random( 100 ) )
				{
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

					EvilOmenSpell.CheckEffect( m );
					StrangleSpell.RemoveCurse( m );
					CorpseSkinSpell.RemoveCurse( m );
					CurseSpell.RemoveEffect( m );
					MortalStrike.EndWound( m );
					BloodOathSpell.RemoveCurse( m );
					MindRotSpell.ClearMindRotScalar( m );

					BuffInfo.RemoveBuff( m, BuffIcon.Clumsy );
					BuffInfo.RemoveBuff( m, BuffIcon.FeebleMind );
					BuffInfo.RemoveBuff( m, BuffIcon.Weaken );
					BuffInfo.RemoveBuff( m, BuffIcon.Curse );
					BuffInfo.RemoveBuff( m, BuffIcon.MassCurse );
					BuffInfo.RemoveBuff( m, BuffIcon.MortalStrike );
					BuffInfo.RemoveBuff( m, BuffIcon.Mindrot );

				}
				else
				{
					m.PlaySound( 0x1DF );
				}
			}

			FinishSequence();
		}

		public void TargetItem( Item o, Mobile caster )
		{
			if ( caster.CheckSkill( SkillName.Chivalry, 0, 100 ) && ( caster.Karma * -1 ) > 0 )
			{
				if ( o is WoodenChest )
				{
					Container pack = (Container)o;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							caster.AddToBackpack ( item );
						}
					caster.PrivateOverheadMessage(MessageType.Regular, 1153, false, "The curse has been lifted from the books.", caster.NetState);
					o.Delete();
				}
				else if ( o is MetalChest )
				{
					Container pack = (Container)o;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							caster.AddToBackpack ( item );
						}
					string curseName = o.Name;
						if ( curseName == ""){ curseName = "item"; }
					caster.PrivateOverheadMessage(MessageType.Regular, 1153, false, "The curse has been lifted from the " + curseName + ".", caster.NetState);
					o.Delete();
				}

				caster.PlaySound( 0xF6 );
				caster.PlaySound( 0x1F7 );
				caster.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );
			}
			else
			{
				caster.PlaySound( 0x1DF );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private AvatarCurseRemovalSpell m_Owner;

			public InternalTarget( AvatarCurseRemovalSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );

				else if ( o is WoodenChest || o is MetalChest )
					m_Owner.TargetItem( (Item)o, from );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}