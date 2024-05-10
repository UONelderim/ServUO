using System;
using Server.Items;
using Server.Spells.Chivalry;

namespace Server.Spells.DeathKnight
{
	//ConsecrateWeapon
	public class WeakSpotSpell : DeathKnightSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Slaby Punkt", "Infirma Macula",
				-1,
				9002
			);

		public WeakSpotSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(0.5);
        public override double RequiredSkill => 15.0;
        public override int RequiredMana => 10;
        public override int RequiredTithing => 10;
        public override bool BlocksMovement => false;
        public override void OnCast()
        {
            BaseWeapon weapon = Caster.Weapon as BaseWeapon;

            if (Caster.Player && (weapon == null || weapon is Fists))
            {
                Caster.SendLocalizedMessage(501078); // You must be holding a weapon.
            }
            else if (CheckSequence())
            {
                /* Temporarily enchants the weapon the caster is currently wielding.
                * The type of damage the weapon inflicts when hitting a target will
                * be converted to the target's worst Resistance type.
                * Duration of the effect is affected by the caster's Karma and lasts for 3 to 11 seconds.
                */
                int itemID, soundID;

                switch (weapon.Skill)
                {
                    case SkillName.Macing:
                        itemID = 0xFB4;
                        soundID = 0x232;
                        break;
                    case SkillName.Archery:
                        itemID = 0x13B1;
                        soundID = 0x145;
                        break;
                    default:
                        itemID = 0xF5F;
                        soundID = 0x56;
                        break;
                }

                Caster.PlaySound(0x20C);
                Caster.PlaySound(soundID);
                Caster.FixedParticles(0x3779, 1, 30, 9964, 3, 3, EffectLayer.Waist);

                IEntity from = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z), Caster.Map);
                IEntity to = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z + 50), Caster.Map);
                Effects.SendMovingParticles(from, to, itemID, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100);

                double seconds = ComputePowerValue(20);

                // TODO: Should caps be applied?

                int pkarma = Caster.Karma;

                if (pkarma < -5000)
                    seconds = 11.0;
                else if (pkarma <= -4999)
                    seconds = 10.0;
                else if (pkarma <= -3999)
                    seconds = 9.00;
                else if (pkarma <= -2999)
                    seconds = 8.0;
                else if (pkarma <= -1999)
                    seconds = 7.0;
                else if (pkarma <= -999)
                    seconds = 6.0;
                else
                    seconds = 5.0;

                TimeSpan duration = TimeSpan.FromSeconds(seconds);
                ConsecratedWeaponContext context;

                if (ConsecrateWeaponSpell.IsUnderEffects(Caster))
                {
                    context = ConsecrateWeaponSpell.GetEffect(Caster);

                    if (context.Timer != null)
                    {
                        context.Timer.Stop();
                        context.Timer = null;
                    }

                    context.Weapon = weapon;
                }
                else
                {
                    context = new ConsecratedWeaponContext(Caster, weapon);
                }

                weapon.ConsecratedContext = context;
                context.Timer = Timer.DelayCall(duration, ConsecrateWeaponSpell.RemoveEffects, Caster);

                ConsecrateWeaponSpell.AddEffect(Caster, context);

                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.ConsecrateWeapon, 1151385, 1151386, duration, Caster,
	                $"{context.ConsecrateProcChance}\t{context.ConsecrateDamageBonus}"));
            }

            FinishSequence();
        }
	}
}
