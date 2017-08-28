using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PullAndBuildAll
{
    /// <summary>
    /// The controller for a build task.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Build {HashString}")]
    public class BuildController : BaseController<BuildHash>
    {
        /// <summary>
        /// The task factory used for all tasks of this type.
        /// </summary>
        private static readonly TaskFactory _taskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        /// <summary>
        /// The instance used to build code.
        /// </summary>
        private readonly BuildService BuildService;

        /// <summary>
        /// The repository's directory.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// The repository's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The platform to build against.
        /// </summary>
        public string Platform { get; }

        protected override TaskFactory TaskFactory => _taskFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="PullController"/> class.
        /// </summary>
        /// <param name="buildService">The instance used to build code.</param>
        /// <param name="directory">The repository's directory.</param>
        /// <param name="name">The repository's name.</param>
        /// <param name="platform">The platform to build against.</param>
        /// <param name="dependencies">The name of repositories this repository is dependant upon.</param>
        public BuildController(BuildService buildService, string directory, string name, string platform, string[] dependencies)
            : base(new BuildHash(name, platform), dependencies)
        {
            BuildService = buildService;
            Directory = directory;
            Name = name;
            Platform = platform;
        }

        protected override void ExecuteTask()
        {
            var anyDependencyFailed = DependencyControls
                .Select(control => control.Task)
                .Any(task => task.IsCanceled || task.IsFaulted);

            if (anyDependencyFailed)
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
