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
 
namespace CatLib.API.Time
{
    /// <summary>
    /// 时间
    /// </summary>
    public interface ITime
    {

        float Time { get; }

        float DeltaTime { get; }

        float FixedTime { get; }

        float TimeSinceLevelLoad { get; }

        float FixedDeltaTime { get; }

        float MaximumDeltaTime { get; }

        float SmoothDeltaTime { get; }

        float TimeScale { get; }

        float FrameCount { get; }

        float RealtimeSinceStartup { get; }

        float CaptureFramerate { get; }

        float UnscaledDeltaTime { get; }

        float UnscaledTime { get; }

    }

}