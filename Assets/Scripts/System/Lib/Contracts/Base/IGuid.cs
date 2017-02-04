using UnityEngine;
using System.Collections;

namespace CatLib.Contracts.Base
{

    public interface IGuid
    {
        long Guid { get; }
        string TypeGuid { get; }
    }

}