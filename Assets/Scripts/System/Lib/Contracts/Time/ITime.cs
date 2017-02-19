
namespace CatLib.Contracts.Time
{
    /// <summary>
    /// 时间
    /// </summary>
    public interface ITime : ITimeRunnerStore
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