using System;

// szczaw :: 2013.01.13 :: uogolnione menu entry
namespace Server.ContextMenus
{

    public class GeneralContextMenuEntry : ContextMenuEntry
    {
        private readonly Action _onClick;

        public override void OnClick()
        {
            _onClick();
        }

        public GeneralContextMenuEntry( int cilloc, Action action )
            : base(cilloc)
        {
            _onClick = action;
        }
    }
}
