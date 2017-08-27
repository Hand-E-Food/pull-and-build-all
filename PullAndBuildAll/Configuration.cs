namespace PullAndBuildAll
{
    /// <summary>
    /// The configuration detailing what requires a refresh.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The root directory containing the repositories.
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// The code repositories to refresh.
        /// </summary>
        public RepositoryConfiguration[] Repositories { get; set; }
    }
}
