namespace PullAndBuildAll
{

    /// <summary>
    /// Details a code repository to be refreshed.
    /// </summary>
    public class RepositoryConfiguration
    {
        /// <summary>
        /// This repository's folder name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The platforms to build for this repository.
        /// </summary>
        public string[] Platforms
        {
            get => _platforms;
            set => _platforms = value ?? new string[0];
        }
        private string[] _platforms = new string[0];

        /// <summary>
        /// The names of repositories this repository is dependant upon.
        /// </summary>
        public string[] Dependencies
        {
            get => _dependencies;
            set => _dependencies = value ?? new string[0];
        }
        private string[] _dependencies = new string[0];

        /// <summary>
        /// The repository's folder.
        /// </summary>
        public string Directory { get; set; }
    }
}