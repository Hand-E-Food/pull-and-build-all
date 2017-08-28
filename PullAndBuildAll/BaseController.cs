using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PullAndBuildAll
{
    public abstract class BaseController<THash> : IController
        where THash : IHash
    {

        /// <summary>
        /// The control managing the asynchronous task.
        /// </summary>
        public TaskControl Control { get; } = new TaskControl {
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
        };

        /// <summary>
        /// The <see cref="TaskControl"/> objects upon which this task is dependant.
        /// </summary>
        public TaskControl[] DependencyControls { get; set; }

        /// <summary>
        /// The names of the repositories upon which this controller is dependent.
        /// </summary>
        public ICollection<string> DependencyNames { get; }

        /// <summary>
        /// The total number of tasks dependent on this task.
        /// </summary>
        public int Dependents { get; set; } = 0;

        /// <summary>
        /// This controller's hash.
        /// </summary>
        public IHash Hash { get; }

        /// <summary>
        /// The task factory used to create tasks.
        /// </summary>
        protected virtual TaskFactory TaskFactory => Task.Factory;

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="hash">This controller's hash.</param>
        /// <param name="dependencies">The names of the repositories upon which this controller is dependent.</param>
        public BaseController(IHash hash, string[] dependencies)
        {
            Hash = hash;
            DependencyNames = dependencies ?? new string[0];
            Control.Name = Hash.HashString;
            Control.TaskFactory = TaskFactory;
        }

        /// <summary>
        /// Schedules the task for execution.
        /// </summary>
        public void ScheduleTask(Task[] dependencyTasks)
        {
            var tasks = DependencyControls
                .Select(control => control.Task)
                .Concat(dependencyTasks ?? new Task[0])
                .ToArray();

            Control.ContinueWhenAll(tasks, ExecuteTask);
        }

        private void ExecuteTask(Task[] tasks) => ExecuteTask();

        protected abstract void ExecuteTask();
    }
}
