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
 
using CatLib.API;
using CatLib.API.TimeQueue;
using System.Collections.Generic;
using System.Threading;

namespace CatLib.TimeQueue {

    public class TimeRunner : IUpdate{

        [Dependency]
        public IApplication App { get; set; }

        private List<TimeQueue> timeRuner = new List<TimeQueue>();
        private ReaderWriterLockSlim timeRunnerLocker = new ReaderWriterLockSlim();

        public ITimeQueue CreateQueue()
        {
            return new TimeQueue(){ Runner = this , App = App };
        }

        public bool Runner(TimeQueue runner)
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

        public bool StopRunner(TimeQueue runner)
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
            if (timeRuner.Count <= 0){ return; }
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