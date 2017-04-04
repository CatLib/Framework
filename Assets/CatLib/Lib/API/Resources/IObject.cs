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
 
using UnityEngine;

namespace CatLib.API.Resources
{
    /// <summary>对象信息</summary>
    public interface IObject
    {

        GameObject Instantiate();

        T Get<T>(object hostedObject) where T : Object;

        Object Get(object hostedObject);

        Object UnHostedGet();

        void Retain();

        void Release();

    }

}