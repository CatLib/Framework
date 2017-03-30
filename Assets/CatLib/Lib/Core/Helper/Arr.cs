
using System;
using System.Collections.Generic;

namespace CatLib
{


    public static class Arr
    {

        public static T First<T>(IEnumerable<T> array , Func<T, bool> func)
        {
            foreach (var data in array)
            {
                if (func(data))
                {
                    return data;
                }
            }
            return default(T);
        }

    }
     
}