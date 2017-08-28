namespace PullAndBuildAll
{
    public class NuGetHash : BaseHash
    {
        public NuGetHash(string name)
            : base($"NuGet|{name}")
        { }
    }
}