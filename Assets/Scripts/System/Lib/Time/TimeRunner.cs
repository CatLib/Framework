
using CatLib.API;
using CatLib.API.Time;
using System.Collections.Generic;
using System.Threading;

namespace CatLib.Time {

    public class TimeRunner : Component , IUpdate {

        private List<ITimeRunner> timeRuner = new List<ITimeRunner>();
        private ReaderWriterLockSlim timeRunnerLocker = new ReaderWriterLockSlim();

        public ITimeQueue CreateQueue()
        {
            return App.Make<ITimeQueue>();
        }

        public bool Runner(ITimeRunner runner)
        {
            try
            {
                timeRunnerLocker.EnterWriteLock();
                try
                {
                    timeRuner.Remove(runner);
                    timeRuner.Add(runner);
                }
                finally { timeRunnerLocker.ExitWriteLock(); }
            }
            catch { return false; }
            return true;
        }

        public bool StopRunner(ITimeRunner runner)
        {
            try
            {
                timeRunnerLocker.EnterWriteLock();
                try
                {
                    timeRuner.Remove(runner);
                }
                finally { timeRunnerLocker.ExitWriteLock(); }
            }
            catch { return false; }
            return true;
        }

        public void Update()
        {
            timeRunnerLocker.EnterWriteLock();
            try
            {
                if (timeRuner.Count > 0)
                {
                    var handlersToRemove = new bool[timeRuner.Count];
                    for (var i = 0; i < timeRuner.Count; ++i)
                    {
                        var runner = timeRuner[i];
                        runner.Update();
                        if (runner.IsComplete)
                        {
                            handlersToRemove[i] = true;
                        }
                    }

                    for (var i = handlersToRemove.Length - 1; i > -1; --i)
                    {
                        if (handlersToRemove[i])
                        {
                            timeRuner.RemoveAt(i);
                        }
                    }
                }
            }
            finally { timeRunnerLocker.ExitWriteLock(); }
        }
    }

}