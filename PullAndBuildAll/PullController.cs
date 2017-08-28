using System.Threading.Tasks;

namespace PullAndBuildAll
{
    /// <summary>
    /// The controller for a git pull task.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Pull {HashString}")]
    public class PullController : BaseController<PullHash>
    {
        /// <summary>
        /// The task factory used for all tasks of this type.
        /// </summary>
        private static readonly TaskFactory _taskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        /// <summary>
        /// The instance used to operate git.
        /// </summary>
        private readonly GitService GitService;

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
        /// <param name="name">The repository's name.</param>
        /// <param name="dependencies">The name of repositories this repository is dependant upon.</param>
        public PullController(GitService gitService, string directory, string name, string[] dependencies)
            : base(new PullHash(name), dependencies)
        {
            GitService = gitService;
            Directory = directory;
            Name = name;
        }

        protected override void ExecuteTask()
        {
            Control.CancellationToken.ThrowIfCancellationRequested();
            Control.Log = GitService.PullRepository(Directory);
        }
    }
}
