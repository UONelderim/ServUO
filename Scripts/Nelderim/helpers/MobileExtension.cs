using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

namespace Server
{
    public static class MobileExtension
    {
        public static bool IsNearBy( this Mobile m, Type type, int range = 3  )
        {
            foreach(var o in m.GetObjectsInRange(range))
                if(o.GetType() == type)
                    return true;

            return false;
        }

        public static bool IsNearBy( this Mobile m, object searched, int range = 3 )
        {
            foreach(var o in m.GetObjectsInRange(range))
                if(o == searched)
                    return true;

            return false;
        }

        public static bool IsNearByAny( this Mobile m, IEnumerable<Type> types, int range = 3 )
        {
            foreach (var o in m.GetObjectsInRange(range))
            {
                foreach (Type t in types)
                {
                    if(t.Equals(o.GetType()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
