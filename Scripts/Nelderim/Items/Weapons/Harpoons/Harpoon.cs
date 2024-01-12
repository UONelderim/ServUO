using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
	public class Harpoon : BaseThrown
	{
		public override int EffectID{ get{ return 0x528A; } }
		
		public override Type AmmoType => null;

		public override Item Ammo => null;

		public override int DefHitSound{ get{ return 0x5D2; } }
		public override int DefMissSound{ get{ return 0x5D3; } }

		public override SkillName DefSkill{ get{ return SkillName.Throwing; } }
		public override WeaponType DefType{ get{ return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Throwing; } }

		//public SkillName AccuracySkill{ get{ return SkillName.Tactics; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }


		public override int StrengthReq => 25;
		public override int MinDamage => 13;
		public override int MaxDamage => 17;
		public override float Speed => 3.0f;

		public int OldStrengthReq{ get{ return 15; } }
		public int OldMinDamage{ get{ return 9; } }
		public int OldMaxDamage{ get{ return 41; } }
		public int OldSpeed{ get{ return 20; } }

		public override int MinThrowRange => 5;
		//public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 90; } }

		[Constructable]
		public Harpoon() : base( 0xF63 )
		{
			Name = "harpun";
			Weight = 7.0;
			Layer = Layer.OneHanded;
		}

		public override bool OnEquip( Mobile from )
		{
			from.SendMessage( "To jest bron miotana." );
			return base.OnEquip( from );
		}

		public TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			WeaponAbility a = WeaponAbility.GetCurrentAbility( attacker );

			// Make sure we've been standing still for .25/.5/1 second depending on Era
			if (Core.TickCount > (attacker.LastMoveTime + (Core.SE ? 250 : (Core.AOS ? 500 : 1000) )) || (Core.AOS && WeaponAbility.GetCurrentAbility( attacker ) is MovingShot) )
			{
				bool canSwing = true;

				if ( Core.AOS )
				{
					canSwing = ( !attacker.Paralyzed && !attacker.Frozen );

					if ( canSwing )
					{
						Spell sp = attacker.Spell as Spell;

						canSwing = ( sp == null || !sp.IsCasting || !sp.BlocksMovement );
					}
				}

				if ( canSwing && attacker.HarmfulCheck( defender ) )
				{
					attacker.DisruptiveAction();
					attacker.Send( new Swing( 0, attacker, defender ) );

					if ( OnFired( attacker, defender ) )
					{
						if ( CheckHit( attacker, defender ) )
							OnHit( attacker, defender );
						else
							OnMiss( attacker, defender );
					}
				}

				attacker.RevealingAction();

				return GetDelay( attacker );
			}
			else
			{
				attacker.RevealingAction();

				return TimeSpan.FromSeconds( 0.25 );
			}
		}

	/*	public void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );
		}

		public void OnMiss( Mobile attacker, Mobile defender )
		{
			base.OnMiss( attacker, defender );
		}*/

		/*public virtual bool OnFired(Mobile attacker, Mobile defender)
		{
			BasePoon quiver = attacker.FindItemOnLayer(Layer.Cloak) as BasePoon;
			Container pack = attacker.Backpack;

			if (attacker.Player)
			{
				if (quiver == null || quiver.LowerAmmoCost == 0 || quiver.LowerAmmoCost > Utility.Random(100))
				{
					if (quiver != null)
					{
						Item harpoonrope = quiver.FindItemByType(typeof(HarpoonRope));
						if (harpoonrope != null && quiver.ConsumeTotal(harpoonrope.GetType(), 1))
							quiver.InvalidateWeight();
					}
					else if (pack == null || !pack.ConsumeTotal(typeof(HarpoonRope), 1))
						return false;
				}
			}

			attacker.MovingEffect(defender, EffectID, 18, 1, false, false);

			return true;
		}*/


		public Harpoon( Serial serial ) : base( serial )
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
	}

	/*public class HarpoonRope : Item
	{
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public HarpoonRope() : this( 1 )
		{
		}

		[Constructable]
		public HarpoonRope( int amount ) : base( 0x14F8 )
		{
			Name = "lina do harpunow";
			Stackable = true;
			Amount = amount;
		}

		public HarpoonRope( Serial serial ) : base( serial )
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
	}*/
}
