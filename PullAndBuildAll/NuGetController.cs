using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PullAndBuildAll
{
    /// <summary>
    /// The controller for a git pull task.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("NuGet {HashString}")]
    public class NuGetController : RepositoryHash, IController
    {
        /// <summary>
        /// The task factory used for all tasks of this type.
        /// </summary>
        private static readonly TaskFactory TaskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        /// <summary>
        /// The instance used to operate git.
        /// </summary>
        private readonly NuGetService _nugetService;

        /// <summary>
        /// The control managing the asynchronous task.
        /// </summary>
        public TaskControl Control { get; } = new TaskControl {
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            TaskFactory = TaskFactory,
        };

        /// <summary>
        /// The names of the repositories upon which this task is dependent.
        /// </summary>
        public RepositoryHash[] DependencyHashes { get; }

        /// <summary>
        /// The <see cref="Task"/> objects upon which this task is dependant.
        /// </summary>
        public TaskControl[] DependencyTaskControls { get; set; }

        /// <summary>
        /// The total number of tasks depeneant on this task.
        /// </summary>
        public int Dependents { get; set; } = 0;

        /// <summary>
        /// The repository's directory.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PullController"/> class.
        /// </summary>
        /// <param name="gitService">The instance used to operate git.</param>
        /// <param name="directory">The repository's directory.</param>
        /// <param name="name">The repository's name.</param>
        /// <param name="dependencies">The name of repositories this repository is dependant upon.</param>
        public NuGetController(NuGetService nugetService, string directory, string name, string[] dependencies)
            : base(name)
        {
            _nugetService = nugetService;
            DependencyHashes = dependencies
                .Select(dependency => new RepositoryHash(dependency))
                .ToArray();
            Directory = directory;

            Control.Name = $"NuGet_{name}";
        }

        /// <summary>
        /// Schedules the task for execution.
        /// </summary>
        public void ScheduleTask() =>
            Control.ContinueWhenAll(DependencyTaskControls.Select(control => control.Task).ToArray(), ExecuteTask);

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="tasks">The finished dependency tasks.</param>
        private void ExecuteTask(Task[] tasks)
        {
            if (tasks.Any(task => task.IsCanceled || task.IsFaulted))
                Control.Cancel().ThrowIfCancellationRequested();

            var log = new List<string>();
            foreach (var solutionPath in System.IO.Directory.GetFiles(Directory, "*.sln", SearchOption.AllDirectories))
            {
                Control.CancellationToken.ThrowIfCancellationRequested();
                log.AddRange(_nugetService.Restore(solutionPath));
            }
            Control.Log = string.Join(Environment.NewLine, log);
        }
    }
}
