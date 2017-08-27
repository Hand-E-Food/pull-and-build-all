namespace PullAndBuildAll
{
    public class PlatformHash : IHash
    {

        public string HashString { get; }

        /// <summary>
        /// The repository's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The platform to build against.
        /// </summary>
        public string Platform { get; }

        public override bool Equals(object obj) => HashString == (obj as PlatformHash)?.HashString;

        public override int GetHashCode() => HashString.GetHashCode();

        public PlatformHash(string name, string platform)
        {
            HashString = $"{name}|{platform}";
            Name = name;
            Platform = platform;
        }
    }
}