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

using System.Collections;
using System;

namespace CatLib.API
{

    /// <summary>
    /// 驱动器
    /// </summary>
    public interface IDriver
    {

        bool IsMainThread { get; }

        void MainThread(IEnumerator action);

        void MainThread(Action action);

        IGlobalEvent Trigger(object score);

        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

    }

}