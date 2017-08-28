using System;
using System.Collections.Generic;

namespace PullAndBuildAll
{
    [Serializable]
    internal class OperationFailedException : Exception
    {
        public IList<string> Log { get; }

        public OperationFailedException(List<string> log)
            : base(string.Join(Environment.NewLine, log))
        {
            Log = log;
        }
    }
}