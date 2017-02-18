using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts
{

    public interface IGuid
    {
        long Guid { get; }
        string TypeGuid { get; }
    }

}