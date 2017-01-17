using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Container
{
    /// <summary>依赖标记</summary>
    public class CDependency : Attribute
    {

        /// <summary>别名</summary>
        public string Alias { get; protected set; }

        public CDependency()
        {
            Alias = null;
        }

        public CDependency(string alias)
        {
            Alias = alias;
        }

    }
}