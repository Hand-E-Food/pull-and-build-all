using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PullAndBuildAll
{
    /// <summary>
    /// The controller for a build task.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Build {HashString}")]
    public class BuildController : PlatformHash, IController
    {
        /// <summary>
        /// The task factory used for all tasks of this type.
        /// </summary>
        private static readonly TaskFactory TaskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        /// <summary>
        /// The instance used to build code.
        /// </summary>
        private readonly BuildService BuildService;

        /// <summary>
        /// The control managing the asynchronous task.
        /// </summary>
        public TaskControl Control { get; } = new TaskControl
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            TaskFactory = TaskFactory,
        };

        /// <summary>
        /// The names of the repositories and platforms upon which this task is dependent.
        /// </summary>
        public PlatformHash[] DependencyHashes { get; }

        /// <summary>
        /// The <see cref="Task"/> objects upon which this task is dependant.
        /// </summary>
        public TaskControl[] DependencyTaskControls { get; set; }

        /// <summary>
        /// The total number of tasks dependent on this task.
        /// </summary>
        public int Dependents { get; set; } = 0;

        /// <summary>
        /// The repository's directory.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PullController"/> class.
        /// </summary>
        /// <param name="buildService">The instance used to build code.</param>
        /// <param name="directory">The repository's directory.</param>
        /// <param name="name">The repository's name.</param>
        /// <param name="platform">The platform to build against.</param>
        /// <param name="dependencies">The name of repositories this repository is dependant upon.</param>
        public BuildController(BuildService buildService, string directory, string name, string platform, string[] dependencies)
            : base(name, platform)
        {
            BuildService = buildService;
            DependencyHashes = dependencies
                .Select(dependency => new PlatformHash(dependency, platform))
                .ToArray();
            Directory = directory;

            Control.Name = $"Build_{name}_{platform}";
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

            foreach (var solutionPath in System.IO.Directory.GetFiles(Directory, "*.sln", SearchOption.AllDirectories))
            {
                Control.CancellationToken.ThrowIfCancellationRequested();
                var log = BuildService.Build(solutionPath, Platform);
                Control.Log = string.Join(Environment.NewLine, log);
            }
        }
    }
}
