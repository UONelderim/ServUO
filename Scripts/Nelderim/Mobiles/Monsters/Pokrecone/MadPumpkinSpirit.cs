using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("zwloki szalenca")]
    public class MadPumpkinSpirit : BaseCreature
    {
        private Timer m_Timer;

        [Constructable]
        public MadPumpkinSpirit() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.175, 0.350)
        {
            Name = "Szaleniec";
            Body = 153;

            SetStr(115, 137);
            SetDex(52, 71);
            SetInt(25, 39);

            SetDamage(1, 3);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25);
            SetResistance(ResistanceType.Fire, -25);
            SetResistance(ResistanceType.Cold, 10);
            SetResistance(ResistanceType.Poison, 15);
            SetResistance(ResistanceType.Energy, 25);

            SetSkill(SkillName.MagicResist, 25.9, 34.4);
            SetSkill(SkillName.Tactics, 45.6, 54.4);
            SetSkill(SkillName.Wrestling, 50.7, 59.6);

            SetSpecialAbility(SpecialAbility.DragonBreath); //Custom definition?
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from == null || from == this) return;

            PlayerMobile pm = from as PlayerMobile;
            if (pm == null) return;
            
            Item weapon1 = pm.FindItemOnLayer(Layer.OneHanded);
            Item weapon2 = pm.FindItemOnLayer(Layer.TwoHanded);

            if (weapon1 is BaseAxe || weapon2 is BaseAxe || weapon1 is BasePoleArm || weapon2 is BasePoleArm)
            {
                damage *= 4;
            }
            else if (weapon1 is BaseSword || weapon2 is BaseSword)
            {
                damage *= 2;
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m.Alive)
            {
                if (InRange(m.Location, 10) && !InRange(oldLocation, 10) && m is PlayerMobile)
                {
                    RangePerception = Core.GlobalMaxUpdateRange;
                    Combatant = m;
                }
            }
        }

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            if (corpse.Carved == false)
            {
                base.OnCarve(from, corpse, with);

                corpse.AddCarvedItem(new Gold(Utility.RandomMinMax(26, 100)), from);

                from.SendMessage("You carve up gold.");
                corpse.Carved = true;
            }
        }

        public override bool CanRummageCorpses => true;

        public override int GetAttackSound()
        {
            return 1429;
        }

        public override int GetIdleSound()
        {
            return 383;
        }

        public override int GetAngerSound()
        {
            return 895;
        }

        public override int GetHurtSound()
        {
            return 384;
        }

        public override int GetDeathSound()
        {
            return 897;
        }

        public MadPumpkinSpirit(Serial serial) : base(serial)
        {
        }

        public override void OnDelete()
        {
	        if (m_Timer != null)
	        {
		        m_Timer.Stop();
	        }

	        base.OnDelete();
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

        public override void OnSectorDeactivate()
        {
	        m_Timer?.Stop();
	        base.OnSectorDeactivate();
        }

        public override void OnSectorActivate()
        {
	        m_Timer ??= new InternalTimer(this);
		    m_Timer.Start();
		    
	        base.OnSectorActivate();
        }

        private class InternalTimer : Timer
        {
            private MadPumpkinSpirit m_Owner;
            private int m_Count;

            public InternalTimer(MadPumpkinSpirit owner) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if ((m_Count++ & 48) == 0)
                {
                    m_Owner.Direction = (Direction)(Utility.Random(8) | 0x80);
                }

                m_Owner.Move(m_Owner.Direction);
            }
        }
    }
}
