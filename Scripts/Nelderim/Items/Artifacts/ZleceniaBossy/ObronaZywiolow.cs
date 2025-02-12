using System;
using Server.ACC.CSS.Systems.Cleric;


namespace Server.Items
{
    public class ObronaZywiolow : ChaosShield
    {
	    public static void Initialize()
        {
            PlayerEvent.HitByWeapon += OnHitByWeapon;
        }

	    public static void OnHitByWeapon(Mobile attacker, Mobile defender, int damage, WeaponAbility a)
	    {
		    if (attacker == null || defender == null || defender.Weapon is not BaseMeleeWeapon || damage <= 20) return;
		    
		    if (defender.FindItemOnLayer(Layer.TwoHanded) is ObronaZywiolow && defender.BeginAction(typeof(ObronaZywiolow)))
		    {
			    attacker.Damage(10);
			    attacker.SendMessage("Lodowa tarcza zmraza krew w Twych zylach");
			    defender.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
			    Timer.DelayCall(TimeSpan.FromSeconds(10), () => defender.EndAction(typeof(ObronaZywiolow)));
		    }
	    }

	    public override int InitMinHits => 255;
	    public override int InitMaxHits => 255;

	    [Constructable]
        public ObronaZywiolow()
        {
            Hue = 2613;
            Name = "Obrona Zywiolow - Zimno";
            Attributes.DefendChance = 25;
            Attributes.EnhancePotions = 15;
        }
        
	    public override void AddNameProperties(ObjectPropertyList list)
	    {
		    base.AddNameProperties(list);
		    list.Add("okruchy magicznego lodu pokrywaja tarcze");
	    }

        public ObronaZywiolow(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
