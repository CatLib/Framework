using UnityEngine;
using System.Collections;
using System;

namespace CatLib.Contracts.Container
{

    public interface IBindData
    {

        string Service { get; }

        Func<IContainer, object[], object> Concrete { get; }

        bool IsStatic { get; }

        ITmpData Needs(string service);

        ITmpData Needs<T>();

        IBindData Alias<T>();

        IBindData Alias(string alias);

        IBindData Decorator(Func<IContainer, IBindData, object, object> func);

    }
}
