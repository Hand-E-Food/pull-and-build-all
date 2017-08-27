using System;

namespace PullAndBuildAll
{
    /// <summary>
    /// Thrown when a git merge conflit occurs.
    /// </summary>
    public class MergeConflictException : Exception
    {
        public MergeConflictException()
            : base("The local branch is in conflict with the origin branch.")
        { }

        public MergeConflictException(string message)
            : base(message)
        { }
    }
}