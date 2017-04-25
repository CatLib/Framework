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
using CatLib.Lua;
using System;

namespace CatLib.API.Lua
{
    // ===============================================================================
    // File Name           :    ILua.cs
    // Class Description   :    Lua调用的设计接口
    // Author              :    Mingming
    // Create Time         :    2017-04-22 15:26:23
    // ===============================================================================
    // Copyright © Mingming . All rights reserved.
    // ===============================================================================
    public interface ILua : IDisposable
    {
        /// <summary>
        /// 初始化是否完毕
        /// </summary>
        /// <value><c>true</c> if this instance is inited; otherwise, <c>false</c>.</value>
        bool IsInited { get; set; }
        /// <summary>
        /// 获取LuaEngine
        /// </summary>
        /// <value>The get lua engine.</value>
        ILuaEngineAdapter LuaEngineAdapter { get; set; }
        /// <summary>
        /// 执行Lua脚本，鉴于lua返回值可以为多个
        /// 故object可能为object或者object[]两种类型，注意鉴别
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="scriptCode">Script code.</param>
        object ExecuteScript(byte[] scriptCode);
        /// <summary>
        /// 执行Lua脚本，鉴于lua返回值可以为多个
        /// 故object可能为object或者object[]两种类型，注意鉴别
        /// </summary>
        /// <returns><c>true</c>, if script was executed, <c>false</c> otherwise.</returns>
        /// <param name="scriptCode">Script code.</param>
        /// <param name="retObj">Ret object.</param>
        bool ExecuteScript(byte[] scriptCode, out object retObj);
        /// <summary>
        /// 执行Lua脚本，鉴于lua返回值可以为多个
        /// 故object可能为object或者object[]两种类型，注意鉴别
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="script">Script.</param>
        object ExecuteScript(string scriptCode);
        /// <summary>
        /// 通过加载Lua文件执行Lua脚本
        /// </summary>
        /// <returns>The script by file.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        object ExecuteScriptByFile(string filePath, string fileName);
        /// <summary>
        /// 执行Lua脚本，鉴于lua返回值可以为多个
        /// 故object可能为object或者object[]两种类型，注意鉴别
        /// </summary>
        /// <returns><c>true</c>, if script by file was executed, <c>false</c> otherwise.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="retObj">Ret object.</param>
        bool ExecuteScriptByFile(string filePath, string fileName, out object retObj);
        /// <summary>
        /// 判断脚本在该路径下是否存在
        /// </summary>
        /// <returns><c>true</c> if this instance has script the specified filePath fileName; otherwise, <c>false</c>.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        bool HasScript(string filePath, string fileName, out byte[] scriptCode);
        /// <summary>
        /// 设置热修复脚本路径
        /// </summary>
        /// <param name="path">Path.</param>
        void SetHotfixPath(string[] path);

        /// <summary>
        /// 添加自定义lua加载器
        /// </summary>
        /// <param name="callback">Callback.</param>
        void AddCustomLoader(Func<string,byte[]> callback);
        /// <summary>
        /// 删除自定义lua加载器
        /// </summary>
        /// <returns><c>true</c>, if custom loader was removed, <c>false</c> otherwise.</returns>
        /// <param name="callback">Callback.</param>
        bool RemoveCustomLoader(Func<string,byte[]> callback);
    }
}

