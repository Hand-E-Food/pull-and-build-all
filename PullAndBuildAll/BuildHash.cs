namespace PullAndBuildAll
{
    public class BuildHash : BaseHash
    {
        public BuildHash(string name, string platform)
            : base($"Build|{name}|{platform}")
        { }
    }
}