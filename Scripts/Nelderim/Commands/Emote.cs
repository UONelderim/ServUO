using System;
using Server.Items;
using Server.Network;
using Server.Commands.Generic;

namespace Server.Commands
{
    public class emote
    {
        private static int m_Color;

        public static void Initialize()
        {
            m_Color = 99;
            TargetCommands.Register(new ShowEmote());
            CommandSystem.Register( "SetEmoteColor", AccessLevel.Counselor, SetEmoteColorCommand );
        }
        
        public class ShowEmote : BaseCommand
        {
            public ShowEmote()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new[] { "Em" };
                ObjectTypes = ObjectTypes.All;
                Usage = "Em <emote text to display>";
                Description = "Force target to emote <text>.";
            }

            public override bool ValidateArgs(BaseCommandImplementor impl, CommandEventArgs e)
            {
                return e.Length >= 1;
            }

            public override void Execute(CommandEventArgs e, object targeted)
            {
                var from = e.Mobile;
                var emoteText = $"*{e.ArgString}*";

                if (targeted is Mobile m)
                {
                    if (from != m && from.TrueAccessLevel > m.TrueAccessLevel)
                    {
                        CommandLogging.WriteLine(from, $"{from.TrueAccessLevel} { CommandLogging.Format(from)} forcing speech on {CommandLogging.Format(m)}");
                        m.Emote(emoteText);
                    }
                }
                else if (targeted is Item item)
                {
                    item.PublicOverheadMessage(MessageType.Regular, Say.Color, false, emoteText);
                }
                else if (targeted is IPoint3D p)
                {
                    Static emoteHolder = new EmoteItem();
                    emoteHolder.MoveToWorld(new Point3D(p), from.Map);
                    emoteHolder.PublicOverheadMessage(MessageType.Regular, Say.Color, false, emoteText);
                }
                else
                {
                    from.SendMessage("Invalid target");
                }
            }
        }

        public static int Color => m_Color;

        [Usage( "SetEmoteColor <int>" )]
        [Description( "Defines the color for items speech with the emote command" )]
        public static void SetEmoteColorCommand( CommandEventArgs e )
        {
            if ( e.Length <= 0 )
                e.Mobile.SendMessage( "Format: SetEmoteColor \"<int>\"" );
            else
            {
                m_Color = e.GetInt32(0);
                e.Mobile.SendMessage( m_Color, "You chose this color." );
            }
        }
    }

    class EmoteItem : Static
    {
        public EmoteItem() : base(0x1)
        {
            Name = "";
            Timer.DelayCall(TimeSpan.FromSeconds(10), Delete);
        }

        public EmoteItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }
    };
}
