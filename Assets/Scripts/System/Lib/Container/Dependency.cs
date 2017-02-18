using System;

namespace CatLib
{
    /// <summary>依赖标记</summary>
    public class Dependency : Attribute
    {

        public string Alias { get; protected set; } 

        public Dependency(string alias)
        {
            Alias = alias;
        }

        public Dependency() { }

    }
}