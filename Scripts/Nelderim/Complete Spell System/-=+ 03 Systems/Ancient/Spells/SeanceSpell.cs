using System;
using System.Collections;
using System.Collections.Generic;
using Nelderim;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSeanceSpell : AncientSpell
    {
        public override double CastDelay { get { return 4.5; } }
        public override double RequiredSkill { get { return 89.0; } }
        public override int RequiredMana { get { return 50; } }
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Seans", "Kal Wis Corp",
                                                        221,
                                                        9002,
                                                        Reagent.Bloodmoss,
                                                        Reagent.SpidersSilk,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }
        
        public AncientSeanceSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (Caster.Mounted)
            {
                Caster.SendLocalizedMessage(1042561); //Please dismount first.
                return false;
            }
            else if (TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do królestwa zmarłych w tej formie.");
                return false;
            }
            else if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do krainy zmarłych będąc ukrytym.");
                return false;
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("Nie możesz wejść do krainy umarłych bez usunięcia farby.");
                return false;
            }
            else if (!Caster.CanBeginAction(typeof(AncientSeanceSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (!CheckSequence())
            {
                return;
            }
            else if (!Caster.CanBeginAction(typeof(AncientSeanceSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do królestwa zmarłych w tej formie.");
            }
            else if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do krainy zmarłych będąc ukrytym.");
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("Nie możesz wejść do krainy umarłych bez usunięcia farby.");
            }
            else if (!Caster.CanBeginAction(typeof(Server.Spells.Fifth.IncognitoSpell)) || Caster.IsBodyMod)
            {
                DoFizzle();
            }
            else if (CheckSequence())
            {

                if (Caster.BeginAction(typeof(AncientSeanceSpell)))
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    Caster.PlaySound(0x379);

                    SeanceSpellExt.Get(Caster).OldBody = Caster.BodyValue;
                    Caster.BodyValue = Caster.Female ? 403 : 402;

                    Caster.SendMessage("Wkraczasz do królestwa zmarłych.");
                    BaseArmor.ValidateMobile(Caster);

                    StopTimer(Caster);

                    Timer t = new InternalTimer(Caster);

                    m_Timers[Caster] = t;

                    t.Start();
                }
                else
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
            }

            FinishSequence();
        }

        private static Hashtable m_Timers = new Hashtable();

        public static bool StopTimer(Mobile m)
        {
            Timer t = (Timer)m_Timers[m];

            if (t != null)
            {
                t.Stop();
                m_Timers.Remove(m);
            }

            return (t != null);
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;

            public InternalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(0))
            {
                m_Owner = owner;

                int val = (int)owner.Skills[SkillName.Magery].Value;

                if (val > 50)
                    val = 50;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(AncientSeanceSpell)))
                {
                    m_Owner.BodyValue = SeanceSpellExt.Get(m_Owner).OldBody;
                    SeanceSpellExt.Delete(m_Owner);
                    m_Owner.EndAction(typeof(AncientSeanceSpell));

                    BaseArmor.ValidateMobile(m_Owner);
                }
            }
        }
        
        public class SeanceSpellExt : NExtension<SeanceSpellExtInfo>
        {
            public static string ModuleName = "SeanceSpell";

            public static void Initialize()
            {
                EventSink.WorldSave += new WorldSaveEventHandler( Save );
                Load( ModuleName );
                m_ExtensionInfo.Clear(); 
            }

            public static void Save( WorldSaveEventArgs args )
            {
                Cleanup();
                Save( args, ModuleName );
            }

            private static void Cleanup()
            {
                List<Serial> toRemove = new List<Serial>();
                foreach ( KeyValuePair<Serial, SeanceSpellExtInfo> kvp in m_ExtensionInfo )
                {
                    if ( World.FindEntity( kvp.Key ) == null )
                        toRemove.Add( kvp.Key );
                }
                foreach ( Serial serial in toRemove )
                {
                    SeanceSpellExtInfo removed;
                    m_ExtensionInfo.TryRemove( serial, out removed );
                }
            }
        }

        public class SeanceSpellExtInfo : NExtensionInfo
        {
            private int m_OldBody;
            public int OldBody { get { return m_OldBody; } set { m_OldBody = value; } }
            
            public override void Deserialize( GenericReader reader )
            {
                m_OldBody = reader.ReadInt();
                World.FindMobile(Serial).BodyValue = m_OldBody;
            }

            public override void Serialize( GenericWriter writer )
            {
                writer.Write( m_OldBody );
            }
        }
    }
}
