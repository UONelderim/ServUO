using System;
using System.Collections.Generic;

// szczaw :: 11.01.2013 :: utworzenie Default zeby latwo wyciagac z typow standardowe wartosci np. 
// Item item = Default.Item[typeof(LeatherCap)];
// int cilloc = item.LabelNumber;
// int itemID = item.itemID; 

namespace Server.Helpers
{
    public static class Default
    {
        public static DefaultValue<Item> Item = new DefaultValue<Item>();
        public static DefaultValue<Mobile> Mobile = new DefaultValue<Mobile>();
    }

    public class DefaultValue<T>
    {
        private static Dictionary<Type, T> _defaulItemsObject;

        public T this[Type type]
        {
            get 
            {
                if(type == null)
                    throw new ArgumentNullException(String.Format( "Proba odczytu z Default.{0} z indexem == null.", typeof(T).Name));
                
                else if(typeof(T).IsAssignableFrom(type))
                {
                    if(! _defaulItemsObject.ContainsKey(type))
                        _defaulItemsObject[type] = (T) Activator.CreateInstance(type);

                    return _defaulItemsObject[type];
                }

                else
                    throw new ArgumentOutOfRangeException(string.Format("Typ {0} nie dziedziczy po {1}.", type.Name, typeof(T).Name));
            }
        }

        public DefaultValue()
        {
            _defaulItemsObject = new Dictionary<Type, T>();
        }
    }

}