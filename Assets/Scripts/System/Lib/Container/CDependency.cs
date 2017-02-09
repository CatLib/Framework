using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Container
{
    /// <summary>依赖标记</summary>
    public class CDependency : Attribute
    {

        public string Alias { get; protected set; } 

        public CDependency(string alias)
        {
            Alias = alias;
        }

        public CDependency() { }

    }
}