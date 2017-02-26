using UnityEngine;
using System.Collections;
using System;
using XLua;

namespace CatLib.API.Container
{

    public interface IBindData
    {

        string Service { get; }

        Func<IContainer, object[], object> Concrete { get; }

        bool IsStatic { get; }

        IGivenData Needs(string service);

        IGivenData Needs<T>();

        IBindData Alias<T>();

        IBindData Alias(string alias);

        IBindData Resolving(Func<IContainer, IBindData, object, object> func);

    }
}
