using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HGPlugins
{
    public static class CommonExtensions
    {
        // public static bool IsNullOrEmpty(this IEnumerable value)
        // {
        //     return value.IsNullOrEmpty();
        // }
        public static bool IsNullOrEmpty(this IList value)
        {
            return value == null || value.Count == 0;
        }
    }    
}

