using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;

namespace Nelderim
{
    public class Decor
    {
        public static void Initialize()
        {
            TargetCommands.Register(new AddQuestItem());
            TargetCommands.Register(new RemoveQuestItem());

            TargetCommands.Register(new AddDecor());
            TargetCommands.Register(new RemoveDecor());
        }

        public class AddQuestItem : BaseCommand
        {
            public AddQuestItem()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new[] { "AddQuestItem" };
                ObjectTypes = ObjectTypes.All;
                Usage = "AddQuestItem [itemID]";
                Description = "";
            }

            public override bool ValidateArgs(BaseCommandImplementor impl, CommandEventArgs e)
            {
                return e.Length >= 0;
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                Mobile from = e.Mobile;

                IPoint3D p = obj as IPoint3D;

                if (p == null)
                    return;

                QuestDecorItem questItem;
                int itemId;
                if (e.Length >= 1 && int.TryParse(e.GetString(0), out itemId))
                    questItem = new QuestDecorItem(itemId);
                else
                    questItem = new QuestDecorItem();

                // questItem.LabelOfCreator = CommandLogging.Format(from) as string;
                questItem.MoveToWorld(new Point3D(p), from.Map);
            }
        }

        public class RemoveQuestItem : BaseCommand
        {
            public RemoveQuestItem()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.AllItems;
                Commands = new[] { "RemoveQuestItem" };
                ObjectTypes = ObjectTypes.Both;
                Usage = "RemoveQuestItem";
                Description = "";
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                QuestDecorItem p = obj as QuestDecorItem;
                if (p == null)
                {
                    e.Mobile.SendMessage("This is not a quest item");
                    return;
                }

                p.Delete();
            }
        }

        public class AddDecor : BaseCommand
        {
            public AddDecor()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new[] { "AddDecor" };
                ObjectTypes = ObjectTypes.All;
                Usage = "AddDecor itemID [hue]";
                Description = "";
            }

            public override bool ValidateArgs(BaseCommandImplementor impl, CommandEventArgs e)
            {
                return e.Length >= 1;
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                Mobile from = e.Mobile;

                IPoint3D p = obj as IPoint3D;

                if (p == null)
                    return;

                int itemId;
                if (!int.TryParse(e.GetString(0), out itemId))
                {
                    from.SendMessage("Invalid itemid");
                    return;
                }

                int hue = e.GetInt32(1);

                Static decor = new Static(itemId);
                decor.Hue = hue;
                decor.MoveToWorld(new Point3D(p), from.Map);
            }
        }

        public class RemoveDecor : BaseCommand
        {
            public RemoveDecor()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.AllNPCs | CommandSupport.AllItems;
                Commands = new[] { "RemoveDecor" };
                ObjectTypes = ObjectTypes.Both;
                Usage = "RemoveDecor";
                Description = "";
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                Static p = obj as Static;
                if (p == null)
                {
                    e.Mobile.SendMessage("This is not a decor");
                    return;
                }

                p.Delete();
            }
        }
    }
}
