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

namespace CatLib.API
{

    public interface IEnv
    {

        DebugLevels DebugLevel { get; }

        string ReleasePath { get; }

        string ResourcesBuildPath { get; }

        string ResourcesNoBuildPath { get; }

        string StreamingAssetsPath { get; }

        string DataPath { get; }

        string PersistentDataPath { get; }

        string AssetPath { get; }

        RuntimePlatform Platform { get; }

        RuntimePlatform SwitchPlatform { get; }

        string PlatformToName(RuntimePlatform? platform = null);

    }

}