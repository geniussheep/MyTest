using System;

namespace ConsoleTaskPool.TaskPoolService
{
    public class TaskModel
    {

        private bool _isFailed;
        private bool _isCompleted;
        private Exception _taskException;
        private readonly Action _taskAction;

        public bool IsFailed => _isFailed;

        public bool IsCompleted => _isCompleted;

        public Exception TaskException => _taskException;

        public string TaskName { get; set; }

        public TaskModel(Action taskAction)
        {
            _taskAction = taskAction;
        }

        public void ExecuteFailed(Exception exception)
        {
            _taskException = exception;
            _isFailed = true;
        }

        public void HasCompleted()
        {
            _isCompleted = true;
        }

        public void ExecuteAction()
        {
            _taskAction?.Invoke();
        }
    }
}
