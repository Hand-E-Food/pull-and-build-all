namespace PullAndBuildAll
{
    public class RepositoryHash : IHash
    {

        public string HashString { get; }

        /// <summary>
        /// The repository's name.
        /// </summary>
        public string Name { get; }

        public RepositoryHash(string name)
        {
            HashString = name;
            Name = name;
        }

        public override bool Equals(object obj) => HashString == (obj as RepositoryHash)?.HashString;

        public override int GetHashCode() => HashString.GetHashCode();
    }
}
