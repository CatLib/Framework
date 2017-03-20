/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using System;

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
