namespace PullAndBuildAll
{
    public abstract class BaseHash : IHash
    {

        public string HashString { get; }

        public BaseHash(string hashString)
        {
            HashString = hashString;
        }

        public override bool Equals(object obj) => HashString == (obj as IHash)?.HashString;

        public override int GetHashCode() => HashString.GetHashCode();
    }
}