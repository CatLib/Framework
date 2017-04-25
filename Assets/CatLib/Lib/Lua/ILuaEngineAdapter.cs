/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
using UnityEngine;
using System.Collections;
using System;
using CatLib.API.Lua;

namespace CatLib.Lua
{
    // ===============================================================================
    // File Name           :    ILuaEngineAdapter.cs
    // Class Description   :    Lua引擎适配器
    // Author              :    Mingming
    // Create Time         :    2017-04-22 15:54:55
    // ===============================================================================
    // Copyright © Mingming . All rights reserved.
    // ===============================================================================
    public interface ILuaEngineAdapter : IDisposable
    {
        /// <summary>
        /// 初始化函数
        /// </summary>
        IEnumerator Init();
        /// <summary>
        /// 执行Lua脚本
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="scriptCode">Script code.</param>
        object ExecuteScript(byte[] scriptCode);
        /// <summary>
        /// 执行Lua脚本
        /// </summary>
        /// <returns><c>true</c>, if script was executed, <c>false</c> otherwise.</returns>
        /// <param name="scriptCode">Script code.</param>
        /// <param name="retObj">Ret object.</param>
        bool ExecuteScript(byte[] scriptCode, out object retObj);
        /// <summary>
        /// 执行Lua脚本
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="script">Script.</param>
        object ExecuteScript(string scriptCode);

        /// <summary>
        /// 执行Lua脚本
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="script">Script.</param>
        bool ExecuteScript(string scriptCode, out object retObj);
        /// <summary>
        /// 设置热修复的脚本路径
        /// </summary>
        /// <param name="path">Path.</param>
        void SetHotfixPath(string[] path);
        /// <summary>
        /// 添加自定义lua加载器
        /// </summary>
        /// <param name="callback">Callback.</param>
        ///
        void AddCustomLoader(Func<string,byte[]> callback);
        /// <summary>
        /// 删除自定义lua加载器
        /// </summary>
        /// <returns><c>true</c>, if custom loader was removed, <c>false</c> otherwise.</returns>
        /// <param name="callback">Callback.</param>
        bool RemoveCustomLoader(Func<string,byte[]> callback);
    }
}

