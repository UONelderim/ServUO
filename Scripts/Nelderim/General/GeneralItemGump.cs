using Server.Gumps;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Helpers;


// szczaw :: 11.01.2013 :: refaktoryzacja
namespace Server.Gumps
{

    public class ItemGumpInfo
    {
        public readonly Type Type;
        public readonly int ItemID;
        public readonly int LocalizedTooltip;
        public readonly Item Item;

        public ItemGumpInfo( Type type ) : this( type, Default.Item[type].ItemID, Default.Item[type].LabelNumber ) { }

        public ItemGumpInfo( Item item ) : this(item.GetType(), item.ItemID, item.LabelNumber) 
        {
            this.Item = item; // Konkretny przedmiot
        }

        public ItemGumpInfo( Type type, int itemID, int localizedTooltip = -1 )
        {
            this.Type = type;
            this.ItemID = itemID;
            this.LocalizedTooltip = localizedTooltip;
        }
    }

    public class GeneralItemGump : Gump
    {
        private readonly IEnumerable<ItemGumpInfo> _items;
        private readonly Action<NetState, RelayInfo> _onItemChoose;
        private readonly Action _onGumpClosed;

        public override void OnResponse( Network.NetState sender, RelayInfo info )
        {
            if(info.ButtonID == 0)
            {
                if(_onGumpClosed != null)
                    _onGumpClosed();
            }
            else
            {
                RelayInfo ri = new RelayInfo(info.ButtonID - 1, info.Switches, info.TextEntries);
                _onItemChoose(sender, ri);
            }
        }

        /// <summary>
        /// Tworzy gumpa wyswietlajacego podane przedmioty i wywoluje odpowiednie akcje.
        /// <para>Jezeli uzytkownik wybierze przedmiot, to wykona sie akcja onItemChoose(sender, info) gdzie info.ButtonID zawiera indeks wybranego przedmiotu.</para>
        /// <para>Jezeli uzytkownik zdecyduje sie zamknac gumpa np. prawym przyciskiem myszy, to opcjonalnie zostanie wykonana akcja _onGumpClosed.</para>
        /// </summary>
        /// <param name="items">Lista przedmiotow do wyswietlenia.</param>
        /// <param name="onItemChoose">Delegat lub lambda expresion</param>
        /// <param name="onGumpClosed">Opcjonalnie delegat lub lambda expresion</param>
        public GeneralItemGump( IEnumerable<Item> items, Action<NetState, RelayInfo> onItemChoose, Action onGumpClosed = null )
            : this
            (
                GeneralItemGump.SelectItem(items),
                onItemChoose,
                onGumpClosed
            )
        {
        }
        
        /// <summary>
        /// Tworzy gumpa wyswietlajacego podane przedmioty i wywoluje odpowiednie akcje.
        /// <para>Jezeli uzytkownik wybierze przedmiot, to wykona sie akcja onItemChoose(sender, info) gdzie info.ButtonID zawiera indeks wybranego przedmiotu.</para>
        /// <para>Jezeli uzytkownik zdecyduje sie zamknac gumpa np. prawym przyciskiem myszy, to opcjonalnie zostanie wykonana akcja _onGumpClosed.</para>
        /// </summary>
        /// <param name="items">Lista przedmiotow do wyswietlenia.</param>
        /// <param name="onItemChoose">Delegat lub lambda expresion</param>
        /// <param name="onGumpClosed">Opcjonalnie delegat lub lambda expresion</param>
        public GeneralItemGump( IEnumerable<Type> types, Action<NetState, RelayInfo> onItemChoose, Action onGumpClosed = null )
            : this
            (
                GeneralItemGump.SelectType(types),
                onItemChoose,
                onGumpClosed
            )
        {
        }

        /// <summary>
        /// Tworzy gumpa wyswietlajacego podane przedmioty i wywoluje odpowiednie akcje.
        /// <para>Jezeli uzytkownik wybierze przedmiot, to wykona sie akcja onItemChoose(sender, info) gdzie info.ButtonID zawiera indeks wybranego przedmiotu.</para>
        /// <para>Jezeli uzytkownik zdecyduje sie zamknac gumpa np. prawym przyciskiem myszy, to opcjonalnie zostanie wykonana akcja _onGumpClosed.</para>
        /// </summary>
        /// <param name="items">Lista przedmiotow do wyswietlenia.</param>
        /// <param name="onItemChoose">Delegat lub lambda expresion</param>
        /// <param name="onGumpClosed">Opcjonalnie delegat lub lambda expresion</param>
        public GeneralItemGump( IEnumerable<ItemGumpInfo> gumpInfo, Action<NetState, RelayInfo> onItemChoose, Action onGumpClosed = null ) : this(gumpInfo) 
        {
            _onItemChoose = onItemChoose;
            _onGumpClosed = onGumpClosed;
        }

        #region CreateGump
        private Point2D _itemPosition;
        public Point2D ItemPosition
        {
            get { return _itemPosition; }
            set { _itemPosition = value; CreateGump(); }
        }

        private int _columnsCount;
        public int ColumnsCount
        {
            get { return _columnsCount; }
            set { _columnsCount = value; CreateGump(); }
        }

        const int _horizontalMargin = 5;
        const int _verticalMargin = 7;
        const int _buttonWidth = 80;
        const int _buttonHeight = 60;
        const int _buttonArt = 0x919;

        private GeneralItemGump( IEnumerable<ItemGumpInfo> items)
            : base(50, 50)
        {
            _items = items;
            ColumnsCount = 3;
        }

        private void CreateGump()
        {
            this.Entries.Clear();

            this.X = _buttonWidth * ColumnsCount + _verticalMargin * 2;
            this.Y = (Count(_items) / ColumnsCount + (Count(_items) % ColumnsCount == 0 ? 0 : 1)) * _buttonHeight + _horizontalMargin * 2;

            AddPage(0);
            AddBackground(0, 0, this.X, this.Y, 3000);
            List<GumpHelper> entries = SelectGump(_items, ColumnsCount);


            foreach(var e in entries)
                AddImageTiledButton(e.x, e.y, _buttonArt, _buttonArt, e.index, GumpButtonType.Reply, 0, e.info.ItemID, 0x0, ItemPosition.X, ItemPosition.Y, e.info.LocalizedTooltip);

        }
        #endregion

        private static int Count(IEnumerable<ItemGumpInfo> items)
        {
            int counter = 0;
            foreach(ItemGumpInfo it in items)
            {
                counter ++;
            }
            return counter;
        }
        private static List<ItemGumpInfo> SelectItem(IEnumerable<Item> items)
        {
            List<ItemGumpInfo> list = new List<ItemGumpInfo>();
            foreach (Item it in items)
            {
                list.Add(new ItemGumpInfo(it));
            }
            return list;
        }
        private static List<ItemGumpInfo> SelectType(IEnumerable<Type> types)
        {
            List<ItemGumpInfo> list = new List<ItemGumpInfo>();
            foreach (Type ty in types)
            {
                list.Add(new ItemGumpInfo(ty));
            }
            return list;
        }

        private static List<GumpHelper> SelectGump(IEnumerable<ItemGumpInfo> items, int columns)
        {
            int i = 0;
            List<GumpHelper> list = new List<GumpHelper>();
            foreach (ItemGumpInfo it in items)
            {
                list.Add(new GumpHelper() { index = i + 1, x = _verticalMargin + _buttonWidth * (i % columns), y = _horizontalMargin + (i / columns) * _buttonHeight, info = it });
                i++;
            }
            return list;
        }
        protected class GumpHelper
        {
            public int index, x, y;
            public ItemGumpInfo info;
            public GumpHelper() { }
        }
        
    }
    
}