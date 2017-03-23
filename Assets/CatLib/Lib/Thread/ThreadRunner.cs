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
using CatLib.API.Thread;
using System.Collections.Generic;
using System.Threading;

namespace CatLib.Thread
{

    /// <summary>
    /// 多线程运行器
    /// </summary>
    public class ThreadRuner : IThread , IUpdate
    {

        [Dependency]
        public IApplication App { get; set; }

        private List<ThreadTask> taskRunner = new List<ThreadTask>();
        private ReaderWriterLockSlim taskRunnerLocker = new ReaderWriterLockSlim();

        public ITask Task(System.Action task)
        {
            var taskRunner = new ThreadTask(this)
            {
                Task = task,
                ReturnResult = false,
            };
            return taskRunner;
        }

        public ITask Task(System.Func<object> task)
        {

            var taskRunner = new ThreadTask(this)
            {
                TaskWithResult = task,
                ReturnResult = true,
            };

            return taskRunner;

        }

        public ITaskHandler AddTask(ThreadTask taskRunner)
        {
            taskRunner.StartTime = App.Time.Time;
            if (taskRunner.DelayTime > 0)
            {
                taskRunnerLocker.EnterWriteLock();
                try
                {
                    this.taskRunner.Add(taskRunner);
                }
                finally
                {
                    taskRunnerLocker.ExitWriteLock();
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecuter), taskRunner);
            }
            return taskRunner;
        }

        public void Cancel(ThreadTask taskRunner)
        {
            taskRunnerLocker.EnterWriteLock();
            try
            {
                this.taskRunner.Remove(taskRunner);
            }finally
            {
                taskRunnerLocker.ExitWriteLock();
            }
        }

        public void Update()
        {
            taskRunnerLocker.EnterReadLock();
            var handlersToRemove = new bool[taskRunner.Count];

            try
            {
                for (var i = 0; i < taskRunner.Count; ++i)
                {
                    var runner = taskRunner[i];
                    if ((runner.StartTime + runner.DelayTime) <= App.Time.Time)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecuter), runner);
                        handlersToRemove[i] = true;
                    }
                }
            }
            finally { taskRunnerLocker.ExitReadLock(); }

            taskRunnerLocker.EnterWriteLock();
            try
            {
                for (var i = handlersToRemove.Length - 1; i > -1; --i)
                {
                    if (handlersToRemove[i])
                    {
                        taskRunner.RemoveAt(i);
                    }
                }
            }
            finally { taskRunnerLocker.ExitWriteLock(); }
        }

        private void ThreadExecuter(object state)
        {
            try
            {
                if (typeof(ThreadTask) == state.GetType()) { RunTaskThread((ThreadTask)state); }
                else
                {
                    App.Trigger(this).SetEventName(ThreadEvents.ON_THREAD_EXECURE_ERROR).Trigger(
                                        new ErrorEventArgs(
                                            new System.Exception(string.Format("type '{0}' not supported!", state.GetType())
                                        )));
                }
            }
            catch (System.Exception exception)
            {
                App.Trigger(this).SetEventName(ThreadEvents.ON_THREAD_EXECURE_ERROR).Trigger(new ErrorEventArgs(exception));
            }
        }

        private void RunTaskThread(ThreadTask taskRunner)
        {
            try
            {
                object result = null;
                if (taskRunner.ReturnResult)
                {
                    result = taskRunner.TaskWithResult();
                }
                else
                {
                    taskRunner.Task();
                }

                if (taskRunner.Complete != null)
                {
                    App.MainThread(() =>
                    {
                        taskRunner.Complete();
                    });
                }

                if (taskRunner.CompleteWithResult != null)
                {
                    App.MainThread(() =>
                    {
                        taskRunner.CompleteWithResult(result);
                    });
                }

            }
            catch (System.Exception exception)
            {
                if (taskRunner.Error != null)
                {
                    App.MainThread(() =>
                    {
                        taskRunner.Error(exception);
                    });
                }
            }
        }

    }


}