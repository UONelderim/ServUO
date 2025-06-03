using System;
using Server;
using Server.Gumps;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public enum PotionUse
    {
        Create
    }

    public class AnimatedVDoll : Item
    {
        private PotionUse m_Use;

        [CommandProperty(AccessLevel.GameMaster)]
        public PotionUse Use
        {
            get => m_Use;
            set { m_Use = value; InvalidateProperties(); }
        }

        [Constructable]
        public AnimatedVDoll() : base(3848)
        {
            Name      = "mikstura animowania";
            Weight    = 1;
            Hue       = 2992;
            Stackable = true;
        }

        public AnimatedVDoll(Serial serial) : base(serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("<basefont color=#cc0000>*mikstura wymagająca wiedzy o Guślarstwie");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            if (!Movable)
                return;

            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private readonly AnimatedVDoll _doll;

            public InternalTarget(AnimatedVDoll doll) : base(1, false, TargetFlags.None)
            {
                _doll = doll;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_doll.Deleted)
                    return;

                if (targeted is VoodooDoll v && v.CursedPerson != null)
                {
                    if (!from.CheckSkill(SkillName.TasteID, 50.0, 100.0))
                    {
                        from.SendMessage("Siły duchowe uniemożliwiają ci skoncentrowanie ich energii. Spróbuj ponownie później.");
                        return;
                    }

                    var m = v.CursedPerson;
                    m.RevealingAction();
                    if (m.Body.IsHuman && !m.Mounted)
                        m.Animate(20, 5, 1, true, false, 0);

                    if (_doll.Use == PotionUse.Create)
                    {
                        from.Karma -= 25;
                        from.SendLocalizedMessage(1019063);
                        _doll.Consume();    // zmniejsza stack
                        _doll.Create(from, m);
                        v.Animated++;

                        // Otwórz gump poprawnie: najpierw lalka, potem gracz
                        from.SendGump(new VoodooSpellGump(v, from));
                    }
                }
                else
                {
                    from.SendMessage("Tego nie ożywisz!");
                }
            }
        }

        public void Create(Mobile from, Mobile target)
        {
            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x202);
            target.SendMessage("Tajemnicze siły, kierowane przez {0}, ożywiły twoją laleczkę!", from.Name);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write((int)m_Use);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Use = (PotionUse)reader.ReadInt();
        }
    }
}
