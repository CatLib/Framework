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
 
using CatLib.API.TimeQueue;
using System;
using System.Collections.Generic;

namespace CatLib.TimeQueue
{

    public class TimeQueue : Component , ITimeQueue
    {

        private Action queueOnComplete;
        private Action<object> queueOnCompleteWithContext;
        private object context;

        private List<TimeTask> queueTasks = new List<TimeTask>();
        public bool IsComplete { get; set; }

        public TimeRunner Runner{ get; set; }

        public ITimeTaskHandler Push(TimeTask task)
        {
            queueTasks.Add(task);
            return task;
        }

        public void Cancel(TimeTask task)
        {
            queueTasks.Remove(task);
        }

        public ITimeTask Task(Action task)
        {
            TimeTask timeTask = new TimeTask(this)
            {
                TaskCall = task
            };
            return timeTask;
        }

        public ITimeTask Task(Action<object> task)
        {
            TimeTask timeTask = new TimeTask(this)
            {
                TaskCallWithContext = task
            };
            return timeTask;
        }

        public ITimeQueue OnComplete(Action<object> onComplete)
        {
            queueOnCompleteWithContext = onComplete;
            return this;
        }

        public ITimeQueue OnComplete(Action onComplete)
        {
            queueOnComplete = onComplete;
            return this;
        }

        public ITimeQueue SetContext(object context)
        {
            this.context = context;
            return this;
        }

        public bool Pause()
        {
            return Runner.StopRunner(this);
        }

        public bool Play()
        {
            IsComplete = false;
            return Runner.Runner(this);
        }

        public bool Stop()
        {
            bool statu = Runner.StopRunner(this);
            if (statu)
            {
                Reset();
            }
            return statu;
        }

        public bool Replay()
        {
            var statu = Stop();
            if (statu)
            {
                statu = Play();
            }
            return statu;
        }

        private void Reset()
        {
            TimeTask task;
            for (int i = 0; i < queueTasks.Count; ++i)
            {
                task = queueTasks[i];
                task.IsComplete = false;
                task.Reset();
            }
        }

        public void Update()
        {

            bool isAllComplete = true;
            float deltaTime = App.Time.DeltaTime;
            bool jumpFlag;
            for (int i = 0; i < queueTasks.Count; ++i)
            {

                if (queueTasks[i].IsComplete) { continue; }
                
                isAllComplete = false;
                jumpFlag = false;
                
                while(queueTasks[i].TimeLineIndex < queueTasks[i].TimeLine.Count){

                    if(!RunTask(queueTasks[i] , ref deltaTime)){
                        
                        jumpFlag = true;
                        break;

                    }

                }

                if(jumpFlag){ break; }
                if(deltaTime <= 0){ break; }

                CallTaskComplete(queueTasks[i]);

            }

            if (isAllComplete){ QueueComplete(); }

        }

        protected bool RunTask(TimeTask task , ref float deltaTime){

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];

            switch(action.Type){

                case TimeTaskActionTypes.DelayFrame: return TaskDelayFrame(task , ref deltaTime);
                case TimeTaskActionTypes.DelayTime: return TaskDelayTime(task , ref deltaTime);
                case TimeTaskActionTypes.LoopFunc:  return TaskLoopFunc(task , ref deltaTime);
                case TimeTaskActionTypes.LoopTime:  return TaskLoopTime(task , ref deltaTime);
                case TimeTaskActionTypes.LoopFrame: return TaskLoopFrame(task , ref deltaTime);
                default: return true;

            }

        }

        protected bool TaskDelayFrame(TimeTask task , ref float deltaTime){

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];
            if (action.IntArgs[0] >= 0 && action.IntArgs[1] < action.IntArgs[0])
            {
                action.IntArgs[1] += 1;
                deltaTime = 0;
                if(action.IntArgs[1] >= action.IntArgs[0]){
                    
                    task.TimeLineIndex++;
                    CallTask(task);
                    return true;

                }
            }
            return false;

        }

        protected bool TaskDelayTime(TimeTask task , ref float deltaTime){

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];

            if (action.FloatArgs[0] >= 0 && action.FloatArgs[1] < action.FloatArgs[0])
            {
                action.FloatArgs[1] += deltaTime;

                if(action.FloatArgs[1] >= action.FloatArgs[0]){
                    
                    deltaTime = action.FloatArgs[1] - action.FloatArgs[0];
                    task.TimeLineIndex++;
                    CallTask(task);
                    return true;

                }
            }

            return false;

        }

        protected bool TaskLoopFunc(TimeTask task , ref float deltaTime){

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];

            if(!action.FuncBoolArg()){

                task.TimeLineIndex++;
                return true;

            }

            CallTask(task);
            return false;

        }

        protected bool TaskLoopTime(TimeTask task , ref float deltaTime)
        {

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];

            if (action.FloatArgs[0] >= 0 && action.FloatArgs[1] <= action.FloatArgs[0])
            {
                action.FloatArgs[1] += deltaTime;

                if(action.FloatArgs[1] > action.FloatArgs[0]){
                    
                    deltaTime = action.FloatArgs[1] - action.FloatArgs[0];
                    task.TimeLineIndex++;
                    return true;

                }
            }

            CallTask(task);
            return false;

        }

        protected bool TaskLoopFrame(TimeTask task , ref float deltaTime){

            TimeTaskAction action = task.TimeLine[task.TimeLineIndex];
            if (action.IntArgs[0] >= 0 && action.IntArgs[1] <= action.IntArgs[0])
            {
                action.IntArgs[1] += 1;
                deltaTime = 0;
                if(action.IntArgs[1] > action.IntArgs[0]){
                    
                    task.TimeLineIndex++;
                    return true;

                }
            }
            
            CallTask(task);
            return false;

        }

        protected void CallTaskComplete(TimeTask task){

            task.IsComplete = true;
            if (task.OnCompleteTask != null)
            {
                task.OnCompleteTask.Invoke();
            }
            if (task.OnCompleteTaskWithContext != null)
            {
                task.OnCompleteTaskWithContext.Invoke(context);
            }

        }

        protected void CallTask(TimeTask task)
        {
            if (task.TaskCall != null)
            {
                task.TaskCall.Invoke();
            }
            if (task.TaskCallWithContext != null)
            {
                task.TaskCallWithContext.Invoke(context);
            }
        }

        protected void QueueComplete(){

            if(queueOnComplete != null)
            {
                queueOnComplete.Invoke();
            }
            if(queueOnCompleteWithContext != null)
            {
                queueOnCompleteWithContext.Invoke(context);
            }
            IsComplete = true;

        }

    }

}