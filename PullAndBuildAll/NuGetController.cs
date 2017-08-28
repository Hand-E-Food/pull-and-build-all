using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PullAndBuildAll
{
    /// <summary>
    /// The controller for a git pull task.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("NuGet {HashString}")]
    public class NuGetController : BaseController<NuGetHash>
    {
        /// <summary>
        /// The task factory used for all tasks of this type.
        /// </summary>
        private static readonly TaskFactory _taskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        /// <summary>
        /// The instance used to operate git.
        /// </summary>
        private readonly NuGetService NugetService;

        /// <summary>
        /// The repository's directory.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// The repository's name.
        /// </summary>
        public string Name { get; }

        protected override TaskFactory TaskFactory => _taskFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="PullController"/> class.
        /// </summary>
        /// <param name="gitService">The instance used to operate git.</param>
        /// <param name="directory">The repository's directory.</param>
        /// <param name="name">The repository's name.</param>
        /// <param name="dependencies">The name of repositories this repository is dependant upon.</param>
        public NuGetController(NuGetService nugetService, string directory, string name, string[] dependencies)
            : base(new NuGetHash(name), dependencies)
        {
            NugetService = nugetService;
            Directory = directory;
            Name = name;
        }

        protected override void ExecuteTask()
        {
            var anyDependencyFailed = DependencyControls
                .Select(control => control.Task)
                .Any(task => task.IsCanceled || task.IsFaulted);

            if (anyDependencyFailed)
                Control.Cancel().ThrowIfCancellationRequested();

            var log = new List<string>();
            foreach (var solutionPath in System.IO.Directory.GetFiles(Directory, "*.sln", SearchOption.AllDirectories))
            {
                Control.CancellationToken.ThrowIfCancellationRequested();
                log.AddRange(NugetService.Restore(solutionPath));
            }
            Control.Log = string.Join(Environment.NewLine, log);
        }
    }
}
