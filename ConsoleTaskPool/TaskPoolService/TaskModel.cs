using System;

namespace ConsoleTaskPool.TaskPoolService
{
    public class TaskModel
    {

        private bool _isFailed;
        private bool _isCompleted;
        private Exception _taskException;

        public bool IsFailed => _isFailed;

        public bool IsCompleted => _isCompleted;

        public Exception TaskException => _taskException;

        public string TaskName { get; set; }

        public Action TaskAction { get; set; }

        public void ExecuteFailed(Exception exception)
        {
            _taskException = exception;
            _isFailed = true;
        }

        public void HasCompleted()
        {
            _isCompleted = true;
        }

        public TaskModel()
        {
        }

        public TaskModel(Action taskAction)
        {
            TaskAction = taskAction;
        }

        public TaskModel(String taskName,Action taskAction)
        {
            TaskName = taskName;
            TaskAction = taskAction;
        }
    }
}
