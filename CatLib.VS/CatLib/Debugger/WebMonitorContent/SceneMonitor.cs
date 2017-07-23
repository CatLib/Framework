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

using CatLib.Debugger.WebMonitor.Handler;
using UnityEngine.SceneManagement;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 场景监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SceneMonitor
    {
        /// <summary>
        /// 构建一个场景监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public SceneMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.sceneCount", "unit.number", new[] { "tags.scene" },
                () => SceneManager.sceneCount));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.sceneCountInBuildSettings", "unit.number", new[] { "tags.scene" },
                () => SceneManager.sceneCountInBuildSettings));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.name", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().name));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.path", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().path));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.buildIndex", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().buildIndex));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.isDirty", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().isDirty));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.isLoaded", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().isLoaded));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.sceneManager.getActiveScene.rootCount", string.Empty, new[] { "tags.scene" },
                () => SceneManager.GetActiveScene().rootCount));
        }
    }
}
